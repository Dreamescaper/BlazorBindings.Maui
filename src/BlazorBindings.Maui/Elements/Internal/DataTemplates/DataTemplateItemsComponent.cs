// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.DataTemplates;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Class is used as generic parameter.
internal class DataTemplateItemsComponent<TControl, TItem> : NativeControlComponentBase, IContainerElementHandler, INonPhysicalChild
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
{
    protected override RenderFragment GetChildContent() => builder =>
    {
        foreach (var itemRoot in _itemRoots)
        {
            builder.OpenComponent<InitializedContentView>(1);

            builder.AddAttribute(2, nameof(InitializedContentView.NativeControl), itemRoot);
            builder.AddAttribute(3, "ChildContent", (RenderFragment)(builder =>
            {
                builder.OpenComponent<DataTemplateItemComponent<TItem>>(4);
                builder.AddAttribute(5, nameof(DataTemplateItemComponent<TItem>.ContentView), itemRoot);
                builder.AddAttribute(6, nameof(DataTemplateItemComponent<TItem>.Template), Template);
                builder.CloseComponent();
            }));

            builder.CloseComponent();
        }
    };

    [Parameter] public Action<TControl, MC.DataTemplate> SetDataTemplateAction { get; set; }
    [Parameter] public RenderFragment<TItem> Template { get; set; }

    private readonly List<MC.ContentView> _itemRoots = new();

    public MC.View AddTemplateRoot()
    {
        var templateRoot = new MC.ContentView();
        _itemRoots.Add(templateRoot);
        StateHasChanged();
        return templateRoot;
    }

    void INonPhysicalChild.SetParent(object parentElement)
    {
        var parent = (TControl)parentElement;
        var dataTemplate = new MC.DataTemplate(AddTemplateRoot);
        SetDataTemplateAction(parent, dataTemplate);
    }

    void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    object IElementHandler.TargetElement => null;
    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex) { }
    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex) { }
}
