using BlazorBindings.Maui.Extensions;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Handlers;

// We allow root component to be either a window, or a page. Therefore, this handler supports both.
internal class ApplicationWindowHandler : IContainerElementHandler
{
    public void AddChild(object child, int physicalSiblingIndex)
    {
        TargetElement = child switch
        {
            MC.Window window => window,
            MC.Page page => new MC.Window(page),
            _ => throw new InvalidOperationException($"Element '{child.GetType().FullName}' is not supported as an application root.")
        };
    }

    public void ReplaceChild(int physicalSiblingIndex, object newChild)
    {
        TargetElement.Page = newChild.Cast<MC.Page>();
    }

    public void RemoveChild(int physicalSiblingIndex)
    {
        TargetElement.Page = null;
    }

    public MC.Window TargetElement { get; private set; }

    object IElementHandler.TargetElement => TargetElement;
}
