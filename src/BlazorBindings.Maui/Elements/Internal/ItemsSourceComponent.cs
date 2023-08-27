using System.Collections.ObjectModel;

namespace BlazorBindings.Maui.Elements.Internal;

/// <summary>
/// This component creates an observable collection, which is updated by blazor renderer.
/// This allows to use it for cases, when MAUI expects an ObservableCollection to handle the updates,
/// but instead of forcing the user to use ObservableCollection on their side, we manage the updates by Blazor.
/// Probably not the most performant way, is there any other option?
/// </summary>
internal class ItemsSourceComponent<TControl, TItem> : NativeControlComponentBase, IElementHandler, IContainerElementHandler, INonPhysicalChild
{
    private readonly ObservableCollection<TItem> _observableCollection = new();

    [Parameter]
    public IEnumerable<TItem> Items { get; set; }

    [Parameter]
    public Action<TControl, ObservableCollection<TItem>> CollectionSetter { get; set; }

    private TControl _parent;
    public object TargetElement => _parent;

    private readonly HashSet<object> _keys = new();

    protected override RenderFragment GetChildContent() => builder =>
    {
        _keys?.Clear();
        bool shouldAddKey = true;

        int index = 0;
        foreach (var item in Items)
        {
            object key = item;
            // Blazor doesn't allow duplicate keys. Therefore we add keys only until the first duplicate.
            shouldAddKey &= _keys.Add(key);
            if (!shouldAddKey)
                key = null;

            builder.OpenComponent<ItemHolderComponent>(1);
            builder.SetKey(key);
            builder.AddAttribute(2, nameof(ItemHolderComponent.Item), item);
            builder.AddAttribute(3, nameof(ItemHolderComponent.Index), index);
            builder.AddAttribute(4, nameof(ItemHolderComponent.ObservableCollection), _observableCollection);
            builder.CloseComponent();

            index++;
        }
    };

    public void AddChild(object child, int physicalSiblingIndex)
    {
        _observableCollection.Insert(physicalSiblingIndex, (TItem)child);
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

    private class ItemHolderComponent : NativeControlComponentBase, IElementHandler
    {
        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public ObservableCollection<TItem> ObservableCollection { get; set; }

        [Parameter]
        public int? Index { get; set; }

        public object TargetElement => Item;

        public override Task SetParametersAsync(ParameterView parameters)
        {
            var previousIndex = Index;
            var previousItem = Item;

            // Task should be completed immediately
            var task = base.SetParametersAsync(parameters);

            if (previousIndex == null)
                return task;

            if (previousIndex == Index && Equals(previousItem, Item))
                return task;

            // Generally it will not be invoked, but it is needed when Source has duplicate items, and component has no key.
            if (previousIndex == Index && !Equals(ObservableCollection[Index.Value], Item))
                ObservableCollection[Index.Value] = Item;

            return task;
        }
    }
}
