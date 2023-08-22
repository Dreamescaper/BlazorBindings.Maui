// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.DataTemplates;

/// <summary>
/// Unlike <see cref="DataTemplateItemsComponent{TControl, TItem}"/>, this DataTemplate component does not use a wrapping element. 
/// This makes it possible to use when returning a View from template is not an option.
/// However, it requires a DataTemplate to render synchronously, which does not always work with Blazor.
/// </summary>
internal class SyncDataTemplateItemsComponent<TControl, TItem> : NativeControlComponentBase, IContainerElementHandler, INonChildContainerElement
{
    protected override RenderFragment GetChildContent() => builder =>
    {
        foreach (var item in _initialItems)
        {
            builder.OpenComponent<SyncDataTemplateItemComponent<TItem>>(1);
            builder.AddAttribute(2, nameof(SyncDataTemplateItemComponent<TItem>.Template), Template);
            builder.AddAttribute(3, nameof(SyncDataTemplateItemComponent<TItem>.InitialItem), item);
            builder.AddComponentReferenceCapture(4, c => _lastAddedItem = (SyncDataTemplateItemComponent<TItem>)c);
            builder.CloseComponent();
        }
    };

    [Parameter] public Action<TControl, MC.DataTemplate> SetDataTemplateAction { get; set; }
    [Parameter] public RenderFragment<TItem> Template { get; set; }

    private readonly List<TItem> _initialItems = new();

    private SyncDataTemplateItemComponent<TItem> _lastAddedItem;

    public MC.BindableObject AddTemplateRoot(TItem initialItem)
    {
        _initialItems.Add(initialItem);
        StateHasChanged();

        var rootElement = _lastAddedItem?.RootControl
            ?? throw new InvalidOperationException("Template root control is supposed to be rendered at this point.");
        _lastAddedItem = null;

        return rootElement;
    }

    void INonPhysicalChild.SetParent(object parentElement)
    {
        var parent = (TControl)parentElement;
        var dataTemplate = new DataTemplateSelector(AddTemplateRoot);
        SetDataTemplateAction(parent, dataTemplate);
    }

    void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    object IElementHandler.TargetElement => null;
    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex) { }
    void IContainerElementHandler.RemoveChild(object child) { }
    int IContainerElementHandler.GetChildIndex(object child) => -1;
    void IElementHandler.ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName) { }

    // In order to be able to render the item synchronously, we need to have an item upfront, before the render.
    // Unfortunately, regular DataTemplate does not have an access to the item, it is set set BindingContext afterwards.
    // In order to workaround this issue, we use a DataTemplateSelector, which returns the same DataTemplate. But we're able
    // to store the item from OnSelectTemplate method, which is used to render the item in DataTemplate.
    class DataTemplateSelector : MC.DataTemplateSelector
    {
        private readonly MC.DataTemplate _dataTemplate;
        private readonly Func<TItem, MC.BindableObject> _loadTemplate;
        private TItem _initialItem;

        public DataTemplateSelector(Func<TItem, MC.BindableObject> loadTemplate)
        {
            _loadTemplate = loadTemplate;
            _dataTemplate = new MC.DataTemplate(() => _loadTemplate(_initialItem));
        }

        protected override MC.DataTemplate OnSelectTemplate(object item, MC.BindableObject container)
        {
            _initialItem = (TItem)item;
            return _dataTemplate;
        }
    }
}
