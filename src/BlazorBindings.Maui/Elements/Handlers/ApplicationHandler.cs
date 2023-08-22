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

    public int GetChildIndex(object child)
    {
        return Equals(_application.MainPage, child) ? 0 : -1;
    }

    public void RemoveChild(object child)
    {
        // It is not allowed to have no MainPage.
    }

    public object TargetElement => _application;
    public void ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName) { }
}
