// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.DataTemplates;

/// <summary>
/// This ControlTemplate implementation wraps the content in an additional View, therefore it is not suitable in cases when non-View content
/// is expected from template.
/// </summary>
internal class ControlTemplateItemsComponent<T> : NativeControlComponentBase, IContainerElementHandler, INonPhysicalChild
    where T : MC.BindableObject
{
    protected override RenderFragment GetChildContent()
    {
        return builder =>
        {
            foreach (var itemRoot in _itemRoots)
            {
                builder.OpenComponent<InitializedContentView>(1);

                builder.AddAttribute(2, nameof(InitializedContentView.NativeControl), itemRoot);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(builder =>
                {
                    Template.Invoke(builder);
                }));

                builder.CloseComponent();
            }
        };
    }

    [Parameter] public Action<T, MC.ControlTemplate> SetControlTemplateAction { get; set; }
    [Parameter] public Action<T, MC.DataTemplate> SetDataTemplateAction { get; set; }
    [Parameter] public RenderFragment Template { get; set; }

    private readonly List<MC.ContentView> _itemRoots = new();

    private MC.View AddTemplateRoot()
    {
        var templateRoot = new MC.ContentView();
        _itemRoots.Add(templateRoot);
        StateHasChanged();

        return templateRoot;
    }

    void INonPhysicalChild.SetParent(object parentElement)
    {
        var parent = parentElement as T;

        if (SetControlTemplateAction != null)
        {
            var controlTemplate = new MC.ControlTemplate(AddTemplateRoot);
            SetControlTemplateAction(parent, controlTemplate);
        }

        if (SetDataTemplateAction != null)
        {
            var dataTemplate = new MC.DataTemplate(AddTemplateRoot);
            SetDataTemplateAction(parent, dataTemplate);
        }
    }

    void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex) { }
    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex) { }
    object IElementHandler.TargetElement => null;
}
