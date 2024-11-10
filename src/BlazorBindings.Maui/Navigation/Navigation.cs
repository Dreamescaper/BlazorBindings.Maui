using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Maui.Controls;
using IComponent = Microsoft.AspNetCore.Components.IComponent;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui;

public partial class Navigation : INavigation
{
    private readonly MauiBlazorBindingsRenderer _renderer;
    private readonly NavigationManager _navigationManager;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    private Type _wrapperComponentType;

    internal Navigation(MauiBlazorBindingsServiceProvider services)
    {
        _renderer = services.GetRequiredService<MauiBlazorBindingsRenderer>();
        _navigationManager = services.GetRequiredService<NavigationManager>();
    }

    internal void SetWrapperComponentType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type wrapperComponentType)
    {
        _wrapperComponentType = wrapperComponentType;
    }

    protected MC.INavigation MauiNavigation => Application.Current.MainPage.Navigation;

    /// <summary>
    /// Push page component <typeparamref name="T"/> to the Navigation Stack.
    /// </summary>
    public Task PushAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
        Dictionary<string, object> arguments = null, bool animated = true) where T : IComponent
    {
        return Navigate(typeof(T), arguments, NavigationTarget.Navigation, animated);
    }

    /// <summary>
    /// Push page component <typeparamref name="T"/> to the Modal Stack.
    /// </summary>
    public Task PushModalAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
        Dictionary<string, object> arguments = null, bool animated = true) where T : IComponent
    {
        return Navigate(typeof(T), arguments, NavigationTarget.Modal, animated);
    }

    /// <summary>
    /// Push page component from the <paramref name="renderFragment"/> to the Modal Stack.
    /// </summary>
    public Task PushModalAsync(RenderFragment renderFragment, bool animated = true)
    {
        return Navigate(renderFragment, NavigationTarget.Modal, animated);
    }

    /// <summary>
    /// Push page component from the <paramref name="renderFragment"/> to the Navigation Stack.
    /// </summary>
    public Task PushAsync(RenderFragment renderFragment, bool animated = true)
    {
        return Navigate(renderFragment, NavigationTarget.Navigation, animated);
    }

    public async Task PopModalAsync(bool animated = true)
    {
        await NavigationAction(() => MauiNavigation.PopModalAsync(animated));
    }

    public async Task PopAsync(bool animated = true)
    {
        await NavigationAction(() => MauiNavigation.PopAsync(animated));
    }

    public async Task PopToRootAsync(bool animated = true)
    {
        await MauiNavigation.PopToRootAsync(animated);
    }

    public async Task OpenWindow<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>
        (Dictionary<string, object> arguments = null) where T : IComponent
    {
        var window = await BuildElement<MC.Window>(typeof(T), arguments);
        Application.Current.OpenWindow(window);
    }

    /// <summary>
    /// Returns rendered MAUI element from component <typeparamref name="T"/>.
    /// This method is exposed for extensibility purposes, and shouldn't be used directly.
    /// </summary>
    /// <remarks>Experimental API, subject to change.</remarks>
    [Experimental("MBB001")]
    public async Task<T> BuildElement<T>(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType,
        Dictionary<string, object> arguments) where T : Element
    {
        var (bindableObject, componentTask) = await _renderer.GetElementFromRenderedComponent(componentType, arguments);
        var element = (Element)bindableObject;

        element.ParentChanged += DisposeScopeWhenParentRemoved;

        return element as T
            ?? throw new InvalidOperationException($"The target component of a navigation must derive from the {typeof(T).Name} component.");

        async void DisposeScopeWhenParentRemoved(object _, EventArgs __)
        {
            if (element.Parent is null)
            {
                element.ParentChanged -= DisposeScopeWhenParentRemoved;

                var component = await componentTask;
                _renderer.RemoveRootComponent(component);
            }
        }
    }

    private Task Navigate(RenderFragment renderFragment, NavigationTarget target, bool animated)
    {
        return Navigate(typeof(RenderFragmentComponent), new()
        {
            [nameof(RenderFragmentComponent.RenderFragment)] = renderFragment
        }, target, animated);
    }

    private async Task Navigate(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType,
        Dictionary<string, object> arguments, NavigationTarget target, bool animated)
    {
        if (_wrapperComponentType != null)
        {
            arguments = new()
            {
                ["ChildContent"] = RenderFragments.FromComponentType(componentType, arguments)
            };
            componentType = _wrapperComponentType;
        }

        await NavigationAction(() =>
        {
            var navigationHandler = new NavigationHandler(MauiNavigation, target, animated);
            var renderTask = _renderer.AddComponent(componentType, navigationHandler, arguments);

            navigationHandler.PageClosed += async () =>
            {
                _renderer.RemoveRootComponent(await renderTask);
            };

            return Task.WhenAny(renderTask, navigationHandler.WaitForNavigation());
        });
    }

    static bool _navigationInProgress;
    static async Task NavigationAction(Func<Task> action)
    {
        // Do not allow multiple navigations at the same time.
        if (_navigationInProgress)
            return;

        try
        {
            _navigationInProgress = true;
            await action();
        }
        finally
        {
            // Small delay for animation.
            await Task.Yield();
            _navigationInProgress = false;
        }
    }

    private class RenderFragmentComponent : ComponentBase
    {
        [Parameter] public RenderFragment RenderFragment { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            RenderFragment(builder);
        }
    }
}
