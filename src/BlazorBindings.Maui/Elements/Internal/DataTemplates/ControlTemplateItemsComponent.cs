﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.DataTemplates;

/// <summary>
/// This ControlTemplate implementation wraps the content in an additional View, therefore it is not suitable in cases when non-View content
/// is expected from template.
/// </summary>
internal class ControlTemplateItemsComponent<T> : NativeControlComponentBase, IMauiContainerElementHandler, INonChildContainerElement
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
    void IElementHandler.ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName) { }
    void IMauiContainerElementHandler.AddChild(MC.BindableObject child, int physicalSiblingIndex) { }
    void IMauiContainerElementHandler.RemoveChild(MC.BindableObject child) { }
    int IMauiContainerElementHandler.GetChildIndex(MC.BindableObject child) => _itemRoots.IndexOf((MC.ContentView)child);
    MC.BindableObject IMauiElementHandler.ElementControl => null;
    object IElementHandler.TargetElement => null;
}
