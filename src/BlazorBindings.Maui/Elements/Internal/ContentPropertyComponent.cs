using BlazorBindings.Maui.Extensions;

namespace BlazorBindings.Maui.Elements.Internal;

internal class ContentPropertyComponent<TControl> : NativeControlComponentBase, IContainerElementHandler, INonChildContainerElement
{
    private TControl _parent;

    [Parameter] public Action<TControl, object> SetPropertyAction { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override RenderFragment GetChildContent() => ChildContent;

    public void SetParent(object parentElement)
    {
        _parent = parentElement.Cast<TControl>();
    }

    public void RemoveFromParent(object parentElement)
    {
        // Because this Handler is used internally only, this method is no-op.
    }

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        SetPropertyAction(_parent, child);
    }

    int IContainerElementHandler.GetChildIndex(object child)
    {
        return -1;
    }

    void IContainerElementHandler.RemoveChild(object child)
    {
        SetPropertyAction(_parent, null);
    }

    // Because this is a 'fake' element, all matters related to physical trees
    // should be no-ops.
    object IElementHandler.TargetElement => null;
    void IElementHandler.ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName) { }
}
