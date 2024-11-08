using BlazorBindings.Maui.Extensions;
using Microsoft.Maui.Controls;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Handlers;

internal class WindowHandler(Window window) : IContainerElementHandler
{
    public void AddChild(object child, int physicalSiblingIndex)
    {
        window.Page = child.Cast<MC.Page>();
    }

    public void ReplaceChild(int physicalSiblingIndex, object oldChild, object newChild)
    {
        window.Page = newChild.Cast<MC.Page>();
    }

    public void RemoveChild(object child, int physicalSiblingIndex)
    {
        window.Page = null;
    }

    public object TargetElement => window;
}
