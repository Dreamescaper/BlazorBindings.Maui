using BlazorBindings.Maui.Extensions;

namespace BlazorBindings.Maui.Elements.Internal;

internal class ListContentPropertyComponent<TControl, TItem> : NativeControlComponentBase, IContainerElementHandler, INonPhysicalChild
    where TItem : class
{
    private TControl _parent;
    private IList<TItem> _propertyItems;

    [Parameter] public Func<TControl, IList<TItem>> ListPropertyAccessor { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override RenderFragment GetChildContent() => ChildContent;

    public void SetParent(object parentElement)
    {
        _parent = parentElement.Cast<TControl>();
        _propertyItems = ListPropertyAccessor(_parent);
    }

    public void RemoveFromParent(object parentElement)
    {
        // Because this Handler is used internally only, this method is no-op.
    }

    object IElementHandler.TargetElement => _parent;

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        _propertyItems.Insert(physicalSiblingIndex, child.Cast<TItem>());
    }

    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex)
    {
        _propertyItems.Remove(child.Cast<TItem>());
    }
}
