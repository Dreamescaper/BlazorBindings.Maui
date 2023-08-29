// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.Elements.DataTemplates;
using BlazorBindings.Maui.Elements.Internal;
using Microsoft.AspNetCore.Components.Rendering;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui;

/// <remarks>Experimental API, subject to change.</remarks>
[RequiresPreviewFeatures]
public static class RenderTreeBuilderHelper
{
    public static void AddContentProperty<TControl>(
        RenderTreeBuilder builder,
        int sequence,
        RenderFragment content,
        Action<TControl, object> setPropertyAction)
    {
        if (content != null)
        {
            builder.OpenRegion(sequence);

            builder.OpenComponent<ContentPropertyComponent<TControl>>(0);
            builder.AddAttribute(1, nameof(ContentPropertyComponent<TControl>.ChildContent), content);
            builder.AddAttribute(2, nameof(ContentPropertyComponent<TControl>.SetPropertyAction), setPropertyAction);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    public static void AddListContentProperty<TControl, TItem>(
        RenderTreeBuilder builder,
        int sequence,
        RenderFragment content,
        Func<TControl, IList<TItem>> listPropertyAccessor)
        where TItem : class
    {
        if (content != null)
        {
            builder.OpenRegion(sequence);

            builder.OpenComponent<ListContentPropertyComponent<TControl, TItem>>(0);
            builder.AddAttribute(1, nameof(ListContentPropertyComponent<TControl, TItem>.ChildContent), content);
            builder.AddAttribute(2, nameof(ListContentPropertyComponent<TControl, TItem>.ListPropertyAccessor), listPropertyAccessor);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    public static void AddDataTemplateProperty<TControl, TItem>(
        RenderTreeBuilder builder,
        int sequence,
        RenderFragment<TItem> template,
        Action<TControl, MC.DataTemplate> setDataTemplateAction)
    {
        if (template != null)
        {
            builder.OpenRegion(sequence);

            builder.OpenComponent<DataTemplateItemsComponent<TControl, TItem>>(0);
            builder.AddAttribute(1, nameof(DataTemplateItemsComponent<TControl, TItem>.SetDataTemplateAction), setDataTemplateAction);
            builder.AddAttribute(2, nameof(DataTemplateItemsComponent<TControl, TItem>.Template), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    public static void AddDataTemplateSelectorProperty<TControl, TItem>(
        RenderTreeBuilder builder,
        int sequence,
        RenderFragment<TItem> template,
        Action<TControl, MC.DataTemplateSelector> setDataTemplateSelectorAction)
    {
        if (template != null)
        {
            builder.OpenRegion(sequence);

            builder.OpenComponent<DataTemplateSelectorComponent<TControl, TItem>>(0);
            builder.AddAttribute(1, nameof(DataTemplateSelectorComponent<TControl, TItem>.SetDataTemplateSelectorAction), setDataTemplateSelectorAction);
            builder.AddAttribute(2, nameof(DataTemplateSelectorComponent<TControl, TItem>.TemplateSelector), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    public static void AddDataTemplateProperty<T>(
        RenderTreeBuilder builder,
        int sequence,
        RenderFragment template,
        Action<T, MC.DataTemplate> setDataTemplateAction)
        where T : MC.BindableObject
    {
        if (template != null)
        {
            builder.OpenRegion(sequence);

            builder.OpenComponent<ControlTemplateItemsComponent<T>>(0);
            builder.AddAttribute(1, nameof(ControlTemplateItemsComponent<T>.SetDataTemplateAction), setDataTemplateAction);
            builder.AddAttribute(2, nameof(ControlTemplateItemsComponent<T>.Template), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    public static void AddControlTemplateProperty<T>(
        RenderTreeBuilder builder,
         int sequence,
        RenderFragment template,
        Action<T, MC.ControlTemplate> setControlTemplateAction)
        where T : MC.BindableObject
    {
        if (template != null)
        {
            builder.OpenRegion(sequence);

            builder.OpenComponent<ControlTemplateItemsComponent<T>>(0);
            builder.AddAttribute(1, nameof(ControlTemplateItemsComponent<T>.SetControlTemplateAction), setControlTemplateAction);
            builder.AddAttribute(2, nameof(ControlTemplateItemsComponent<T>.Template), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    internal static void AddSyncDataTemplateProperty<TControl, TItem>(
        RenderTreeBuilder builder,
        int sequence,
        RenderFragment<TItem> template,
        Action<TControl, MC.DataTemplate> setDataTemplateAction)
    {
        if (template != null)
        {
            builder.OpenRegion(sequence);

            builder.OpenComponent<SyncDataTemplateItemsComponent<TControl, TItem>>(0);
            builder.AddAttribute(1, nameof(SyncDataTemplateItemsComponent<TControl, TItem>.SetDataTemplateAction), setDataTemplateAction);
            builder.AddAttribute(2, nameof(SyncDataTemplateItemsComponent<TControl, TItem>.Template), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    internal static void AddSyncDataTemplateProperty<T>(
        RenderTreeBuilder builder,
        int sequence,
        RenderFragment template,
        Action<T, MC.DataTemplate> setDataTemplateAction)
        where T : MC.BindableObject
    {
        if (template != null)
        {
            builder.OpenRegion(sequence);

            builder.OpenComponent<SyncControlTemplateItemsComponent<T>>(0);
            builder.AddAttribute(1, nameof(SyncControlTemplateItemsComponent<T>.SetDataTemplateAction), setDataTemplateAction);
            builder.AddAttribute(2, nameof(SyncControlTemplateItemsComponent<T>.Template), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    internal static void AddItemsSourceProperty<TControl, TItem>(
        RenderTreeBuilder builder,
        int sequence,
        IEnumerable<TItem> items,
        Func<TItem, object> keySelector,
        Action<TControl, ICollection<TItem>> collectionSetter)
    {
        if (items is null)
            return;

        builder.OpenRegion(sequence);

        builder.OpenComponent<ItemsSourceComponent<TControl, TItem>>(0);
        builder.AddAttribute(1, nameof(ItemsSourceComponent<TControl, TItem>.Items), items);
        builder.AddAttribute(2, nameof(ItemsSourceComponent<TControl, TItem>.CollectionSetter), collectionSetter);

        if (keySelector != null)
            builder.AddAttribute(3, nameof(ItemsSourceComponent<TControl, TItem>.KeySelector), keySelector);

        builder.CloseComponent();

        builder.CloseRegion();
    }
}
