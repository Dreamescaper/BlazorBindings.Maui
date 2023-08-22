using System.Collections.ObjectModel;

namespace BlazorBindings.Maui.Elements.Internal;

/// <summary>
/// This component creates an observable collection, which is updated by blazor renderer.
/// This allows to use it for cases, when MAUI expects an ObservableCollection to handle the updates,
/// but instead of forcing the user to use ObservableCollection on their side, we manage the updates by Blazor.
/// Probably not the most performant way, is there any other option?
/// </summary>
internal class ItemSourceComponent<TControl, TItem> : NativeControlComponentBase, IElementHandler, IContainerElementHandler, INonChildContainerElement
{
    private readonly ObservableCollection<TItem> _observableCollection = new();

    [Parameter]
    public IEnumerable<TItem> Items { get; set; }

    [Parameter]
    public Action<TControl, ObservableCollection<TItem>> CollectionSetter { get; set; }


    private TControl _parent;
    public object TargetElement => _parent;

    protected override RenderFragment GetChildContent() => builder =>
    {
        foreach (var item in Items)
        {
            builder.OpenComponent<ItemHolderComponent>(1);
            builder.SetKey(item);
            builder.AddAttribute(2, nameof(ItemHolderComponent.Item), item);
            builder.CloseComponent();
        }
    };

    public void AddChild(object child, int physicalSiblingIndex)
    {
        _observableCollection.Insert(physicalSiblingIndex, (TItem)child);
    }

    public int GetChildIndex(object child)
    {
        return _observableCollection.IndexOf((TItem)child);
    }

    public void RemoveChild(object child, int physicalSiblingIndex)
    {
        _observableCollection.RemoveAt(physicalSiblingIndex);
    }

    public void RemoveFromParent(object parentElement)
    {
    }

    public void SetParent(object parentElement)
    {
        _parent = (TControl)parentElement;
        CollectionSetter(_parent, _observableCollection);

    }
    public void ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName) { }

    private class ItemHolderComponent : NativeControlComponentBase, IElementHandler
    {
        [Parameter]
        public TItem Item { get; set; }

        public object TargetElement => Item;

        public void ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName)
        {
        }
    }
}
