using Microsoft.AspNetCore.Components.Routing;
using System.Globalization;
using System.Reflection;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui;

public partial class Navigation
{
    private bool _isRouterRendered;
    private TaskCompletionSource<RouteData> _waitForRouteSource;

    /// <summary>
    /// Performs URI-based navigation.
    /// </summary>
    /// <param name="uri">URI to navigate to.</param>
    /// <param name="parameters">Additional parameters to set for component.</param>
    public async Task NavigateToAsync(string uri, Dictionary<string, object> parameters = null)
    {
        ArgumentNullException.ThrowIfNull(uri);

        // I cannot use Blazor's route discovery directly as it is internal.
        // Instead, I render Router internally with callbacks to get navigated page and parameters.
        if (!_isRouterRendered)
        {
            await RenderRouter();
            _isRouterRendered = true;
        }

        var routeTask = WaitForRoute();
        _navigationManager.NavigateTo(uri);
        var route = await routeTask;

        if (route != null)
        {
            var pars = GetParameters(route, parameters);
            await Navigate(route.PageType, pars, NavigationTarget.Navigation, true);
        }
        else
        {
            throw new InvalidOperationException($"The route '{uri}' is not registered. Register page routes using the '@page' directive in the page.");
        }
    }

    private Dictionary<string, object> GetParameters(RouteData routeData, Dictionary<string, object> additionalParameters)
    {
        if (routeData.RouteValues?.Count is not > 0 && additionalParameters?.Count is not > 0)
            return null;

        var result = new Dictionary<string, object>();

        if (routeData.RouteValues != null)
        {
            foreach (var (key, value) in ConvertParameters(routeData.PageType, routeData.RouteValues))
            {
                result.Add(key, value);
            }
        }

        if (additionalParameters != null)
        {
            foreach (var (key, value) in additionalParameters)
            {
                if (value != null)
                    result.Add(key, value);
            }
        }

        return result;
    }

    // This method is only needed for backward-compatibility - to allow assigning non-string parameters without 
    // specifying Route constraints. It should probably be removed in future versions.
    private static Dictionary<string, object> ConvertParameters(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type componentType, 
        IReadOnlyDictionary<string, object> parameters)
    {
        if (parameters is null)
        {
            return null;
        }

        var convertedParameters = new Dictionary<string, object>();

        foreach (var keyValue in parameters)
        {
            var value = keyValue.Value;

            if (value is string stringValue)
            {
                var propertyType = componentType.GetProperty(keyValue.Key)?.PropertyType ?? typeof(string);
                if (!StringConverter.TryParse(propertyType, stringValue, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    throw new InvalidOperationException($"The value {keyValue.Value} can not be converted to a {propertyType.Name}");
                }

                convertedParameters[keyValue.Key] = parsedValue;
            }
            else
            {
                convertedParameters[keyValue.Key] = value;
            }

        }

        return convertedParameters;
    }

    private async Task<Router> RenderRouter()
    {
        RenderFragment notFound = _ => _waitForRouteSource?.TrySetResult(null);
        RenderFragment<RouteData> found = data => _ => _waitForRouteSource?.TrySetResult(data);

        (var router, _) = await _renderer.AddRootComponent<Router>(new()
        {
            [nameof(Router.AppAssembly)] = GetDefaultAssembly(),
            [nameof(Router.NotFound)] = notFound,
            [nameof(Router.Found)] = found
        });

        return router;
    }

    private async Task<RouteData> WaitForRoute()
    {
        _waitForRouteSource = new();
        var routeData = await _waitForRouteSource.Task;
        _waitForRouteSource = null;
        return routeData;
    }

    private static Assembly GetDefaultAssembly()
    {
        var appType = MC.Application.Current.GetType();
        var assembly = appType.IsGenericType && appType.GetGenericTypeDefinition() == typeof(BlazorBindingsApplication<>)
            ? appType.GenericTypeArguments[0].Assembly
            : appType.Assembly;
        return assembly;
    }

}
