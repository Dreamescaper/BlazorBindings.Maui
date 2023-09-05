using System.Collections.ObjectModel;
using System.Diagnostics;

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

    [Parameter]
    public Func<TItem, object> KeySelector { get; set; }


    private TControl _parent;
    public object TargetElement => _parent;

    private HashSet<object> _keys;

    protected override RenderFragment GetChildContent() => builder =>
    {
        _keys?.Clear();
        bool shouldAddKey = true;

        int index = 0;
        foreach (var item in Items)
        {
            var key = KeySelector == null ? item : KeySelector(item);
            if (KeySelector == null)
            {
                // Blazor doesn't allow duplicate keys. Therefore we add keys until the first duplicate.
                // In case KeySelector is provided, we don't check for that here, since it's user's responsibility now.
                _keys ??= new();
                shouldAddKey &= _keys.Add(key);
                if (!shouldAddKey)
                    key = null;
            }

            builder.OpenComponent<ItemHolderComponent>(1);
            builder.SetKey(key);
            builder.AddAttribute(2, nameof(ItemHolderComponent.Item), item);
            builder.AddAttribute(3, nameof(ItemHolderComponent.Index), index);
            builder.AddAttribute(4, nameof(ItemHolderComponent.ObservableCollection), _observableCollection);

            if (key != null)
                builder.AddAttribute(5, nameof(ItemHolderComponent.HasKey), true);

            builder.CloseComponent();

            index++;
        }
    };

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        _observableCollection.Insert(physicalSiblingIndex, (TItem)child);
    }

    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex)
    {
        Debug.Assert(Equals(_observableCollection[physicalSiblingIndex], child));
        _observableCollection.RemoveAt(physicalSiblingIndex);
    }

    void IContainerElementHandler.ReplaceChild(int physicalSiblingIndex, object oldChild, object newChild)
    {
        Debug.Assert(Equals(_observableCollection[physicalSiblingIndex], oldChild));
        if (!Equals(_observableCollection[physicalSiblingIndex], newChild))
            _observableCollection[physicalSiblingIndex] = (TItem)newChild;
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

        [Parameter]
        public bool HasKey { get; set; }

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

            // Generally it will not be invoked, but it is needed when Source has duplicate items, or component has key.
            // The problem here is that we don't know whether previous items are going to be removed or added.
            // We use previousIndex here, because this part of the code is executed before items are actually added/removed to ObservableCollection.
            if ((HasKey || previousIndex == Index) && !Equals(ObservableCollection[previousIndex.Value], Item))
                ObservableCollection[previousIndex.Value] = Item;

            return task;
        }
    }
}
