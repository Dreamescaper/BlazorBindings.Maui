﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Internal.DataTemplates;

internal class DataTemplateSelectorComponent<TControl, TItem> : NativeControlComponentBase, IContainerElementHandler, INonPhysicalChild
{
    protected override RenderFragment GetChildContent() => builder =>
    {
        for (var groupIndex = 0; groupIndex < _itemRootsGroups.Count; groupIndex++)
        {
            builder.OpenRegion(groupIndex);

            foreach (var itemRoot in _itemRootsGroups[groupIndex])
            {
                builder.OpenComponent<InitializedContentView>(1);

                builder.AddAttribute(2, nameof(InitializedContentView.NativeControl), itemRoot);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(builder =>
                {
                    builder.OpenComponent<DataTemplateItemComponent<TItem>>(4);
                    builder.AddAttribute(5, nameof(DataTemplateItemComponent<TItem>.ContentView), itemRoot);
                    builder.AddAttribute(6, nameof(DataTemplateItemComponent<TItem>.Template), TemplateSelector);
                    builder.CloseComponent();
                }));

                builder.CloseComponent();
            }

            builder.CloseRegion();
        }
    };

    [Parameter] public Action<TControl, MC.DataTemplateSelector> SetDataTemplateSelectorAction { get; set; }
    [Parameter] public RenderFragment<TItem> TemplateSelector { get; set; }

    private readonly List<List<MC.ContentView>> _itemRootsGroups = [];

    private MC.View AddTemplateRootToGroup(int groupIndex)
    {
        var templateRoot = new MC.ContentView();
        _itemRootsGroups[groupIndex].Add(templateRoot);
        StateHasChanged();
        return templateRoot;
    }

    private int AddTemplateGroup()
    {
        _itemRootsGroups.Add([]);
        return _itemRootsGroups.Count - 1;
    }

    void INonPhysicalChild.SetParent(object parentElement)
    {
        var parent = (TControl)parentElement;
        var dataTemplate = new DataTemplateSelector<TItem>(TemplateSelector, AddTemplateGroup, AddTemplateRootToGroup);
        SetDataTemplateSelectorAction(parent, dataTemplate);
    }

    void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    object IElementHandler.TargetElement => null;
    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex) { }
    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex) { }

}

file class DataTemplateSelector<TItem>(
    RenderFragment<TItem> renderFragment,
    Func<int> addGroupFunc,
    Func<int, MC.View> addRootToGroupFunc)
    : MC.DataTemplateSelector
{
    private readonly Dictionary<Type, MC.DataTemplate> _dataTemplates = [];

    protected override MC.DataTemplate OnSelectTemplate(object item, MC.BindableObject container)
    {
        var componentType = GetComponentType(renderFragment((TItem)item));

        if (!_dataTemplates.TryGetValue(componentType, out var dataTemplate))
        {
            var groupIndex = addGroupFunc();
            dataTemplate = new MC.DataTemplate(() => addRootToGroupFunc(groupIndex));
            _dataTemplates[componentType] = dataTemplate;
        }

        return dataTemplate;
    }

    static readonly RenderTreeBuilder InspectRenderTreeBuilder = new();
    private static Type GetComponentType(RenderFragment renderFragment)
    {
        // Consider reworking if https://github.com/dotnet/aspnetcore/issues/17200 is implemented.
        try
        {
            renderFragment(InspectRenderTreeBuilder);
            var frames = InspectRenderTreeBuilder.GetFrames();

            for (var i = 0; i < frames.Count; i++)
            {
                if (frames.Array[i].FrameType == RenderTreeFrameType.Component)
                {
                    return frames.Array[i].ComponentType;
                }
            }

            throw new InvalidOperationException("RenderFragment does not contain any components.");
        }
        finally
        {
            InspectRenderTreeBuilder.Clear();
        }
    }
}