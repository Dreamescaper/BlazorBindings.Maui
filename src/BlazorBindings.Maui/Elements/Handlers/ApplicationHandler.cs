using BlazorBindings.Maui.Extensions;
using Microsoft.Maui.Controls;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Handlers;

internal class ApplicationHandler : IContainerElementHandler
{
    private readonly Application _application;

    public ApplicationHandler(Application application)
    {
        _application = application;
    }

    public void AddChild(object child, int physicalSiblingIndex)
    {
        _application.MainPage = child.Cast<MC.Page>();
    }

    public void RemoveChild(object child, int physicalSiblingIndex)
    {
        // It is not allowed to have no MainPage.
    }

    public object TargetElement => _application;
}
