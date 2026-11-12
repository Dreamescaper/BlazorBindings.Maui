using BlazorBindings.Maui.Elements.Handlers;
using Microsoft.Maui.Controls;

namespace BlazorBindings.Maui;

public class BlazorBindingsApplication<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>
    : Application where T : IComponent
{
    public BlazorBindingsApplication()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Configure();
#pragma warning restore CS0618 // Type or member is obsolete
    }

    [Obsolete("Use parameterless constructor instead.")]
    public BlazorBindingsApplication(IServiceProvider _) { }

    protected override Window CreateWindow(Microsoft.Maui.IActivationState activationState)
    {
        var services = activationState?.Context?.Services ?? Handler.MauiContext.Services;
        var renderer = services.GetRequiredService<MauiBlazorBindingsRenderer>();

        if (WrapperComponentType != null)
        {
            var navigation = services.GetService<INavigation>();
            (navigation as Navigation)?.SetWrapperComponentType(WrapperComponentType);
        }

        var (componentType, parameters) = GetComponentToRender();


        var handler = new ApplicationWindowHandler();

        // CreateWindow must return a Window synchronously, so the component's first render has to
        // have been applied by the time we read TargetElement. Render guarantees that, or throws.
        var render = renderer.Render(componentType, handler, parameters);

        AwaitVoid(render.Quiescence);

        // Render guarantees the first batch was applied, not that it produced a window - a root
        // whose content is, say, only a CascadingValue renders without creating any native element.
        return handler.TargetElement
            ?? throw new InvalidOperationException(
                $"The root component {componentType.FullName} did not produce a Window or a Page. "
                + "The application's root component must render one of them.");

        // async void is used here to crash the application if rendering fails.
        static async void AwaitVoid(Task task) => await task;
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public virtual Type WrapperComponentType { get; }


    /// <summary>
    /// This method is executed before the rendering. It can be used to set resources, for example.
    /// </summary>
    [Obsolete("Configure the application in the constructor instead.")]
    protected virtual void Configure() { }

    private (Type ComponentType, Dictionary<string, object> Parameters) GetComponentToRender()
    {
        if (WrapperComponentType is null)
        {
            return (typeof(T), null);
        }
        else
        {
            var parameters = new Dictionary<string, object>
            {
                ["ChildContent"] = RenderFragments.FromComponentType(typeof(T))
            };
            return (WrapperComponentType, parameters);
        }
    }
}
