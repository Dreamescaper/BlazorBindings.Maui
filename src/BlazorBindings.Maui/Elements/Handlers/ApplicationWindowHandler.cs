using BlazorBindings.Maui.Extensions;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Handlers;

// We allow root component to be either a window, or a page. Therefore, this handler supports both.
internal class ApplicationWindowHandler : IContainerElementHandler
{
    private TaskCompletionSource<object> _taskCompletionSource;

    public void AddChild(object child, int physicalSiblingIndex)
    {
        TargetElement = child switch
        {
            MC.Window window => window,
            MC.Page page => new MC.Window(page),
            _ => throw new InvalidOperationException($"Element '{child.GetType().FullName}' is not supported as an application root.")
        };
    }

    public void ReplaceChild(int physicalSiblingIndex, object oldChild, object newChild)
    {
        if (oldChild is MC.Window)
            throw new InvalidOperationException("Removing application root window is not supported.");

        TargetElement.Page = newChild.Cast<MC.Page>();
    }

    public void RemoveChild(object child, int physicalSiblingIndex)
    {
        if (child is MC.Window)
            throw new InvalidOperationException("Removing application root window is not supported.");

        TargetElement.Page = null;
    }

    public Task WaitForWindowAsync()
    {
        if (TargetElement is not null)
        {
            return Task.CompletedTask;
        }

        _taskCompletionSource ??= new();
        return _taskCompletionSource.Task;
    }

    public MC.Window TargetElement { get; private set; }

    object IElementHandler.TargetElement => TargetElement;
}
