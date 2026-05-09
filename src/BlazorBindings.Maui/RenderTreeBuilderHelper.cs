// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.Elements.Internal;
using BlazorBindings.Maui.Elements.Internal.DataTemplates;
using Microsoft.AspNetCore.Components.Rendering;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui;

/// <remarks>Experimental API, subject to change.</remarks>
[Experimental("MBB001")]
public static class RenderTreeBuilderHelper
{
    /// <summary>
    /// Adds a single-value content property to the render tree by rendering <paramref name="content"/> inside
    /// a <see cref="ContentPropertyComponent{TControl}"/> that calls <paramref name="setPropertyAction"/> when
    /// the rendered child element is ready.
    /// </summary>
    /// <typeparam name="TControl">The MAUI control type that owns the property.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to write to.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="content">The <see cref="RenderFragment"/> that produces the child content. Nothing is rendered when <see langword="null"/>.</param>
    /// <param name="setPropertyAction">A callback invoked with the owning control and the rendered child object to assign the property.</param>
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
            builder.AddAttribute(1, nameof(ContentPropertyComponent<>.ChildContent), content);
            builder.AddAttribute(2, nameof(ContentPropertyComponent<>.SetPropertyAction), setPropertyAction);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    /// <summary>
    /// Adds a list content property to the render tree by rendering <paramref name="content"/> inside
    /// a <see cref="ListContentPropertyComponent{TControl, TItem}"/> that appends rendered child elements
    /// to the list returned by <paramref name="listPropertyAccessor"/>.
    /// </summary>
    /// <typeparam name="TControl">The MAUI control type that owns the list property.</typeparam>
    /// <typeparam name="TItem">The element type of the list.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to write to.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="content">The <see cref="RenderFragment"/> that produces the child items. Nothing is rendered when <see langword="null"/>.</param>
    /// <param name="listPropertyAccessor">A function that retrieves the target <see cref="IList{TItem}"/> from the owning control.</param>
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
            builder.AddAttribute(1, nameof(ListContentPropertyComponent<,>.ChildContent), content);
            builder.AddAttribute(2, nameof(ListContentPropertyComponent<,>.ListPropertyAccessor), listPropertyAccessor);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    /// <summary>
    /// Adds a typed data-template property to the render tree by wrapping <paramref name="template"/> in a
    /// <see cref="MC.DataTemplate"/> and passing it to <paramref name="setDataTemplateAction"/>.
    /// Each item of type <typeparamref name="TItem"/> is rendered using the supplied <see cref="RenderFragment{TItem}"/>.
    /// </summary>
    /// <typeparam name="TControl">The MAUI control type that owns the data-template property.</typeparam>
    /// <typeparam name="TItem">The type of the data item passed to the template.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to write to.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="template">The typed <see cref="RenderFragment{TItem}"/> used to render each item. Nothing is rendered when <see langword="null"/>.</param>
    /// <param name="setDataTemplateAction">A callback invoked with the owning control and the constructed <see cref="MC.DataTemplate"/>.</param>
    public static void AddDataTemplateProperty<TControl, TItem>(
        RenderTreeBuilder builder,
        int sequence,
        RenderFragment<TItem> template,
        Action<TControl, MC.DataTemplate> setDataTemplateAction)
    {
        AddDataTemplateProperty<TControl, TItem, MC.ContentView>(builder, sequence, template, setDataTemplateAction);
    }

    /// <summary>
    /// Adds a typed data-template property to the render tree by wrapping <paramref name="template"/> in a
    /// <see cref="MC.DataTemplate"/> and passing it to <paramref name="setDataTemplateAction"/>.
    /// Each item of type <typeparamref name="TItem"/> is rendered using the supplied <see cref="RenderFragment{TItem}"/>.
    /// </summary>
    /// <typeparam name="TControl">The MAUI control type that owns the data-template property.</typeparam>
    /// <typeparam name="TItem">The type of the data item passed to the template.</typeparam>
    /// <typeparam name="TTemplateRoot">Due to async nature, BlazorBinding require wrapper control for data template. This type parameter allows to set type of this wrapper control.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to write to.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="template">The typed <see cref="RenderFragment{TItem}"/> used to render each item. Nothing is rendered when <see langword="null"/>.</param>
    /// <param name="setDataTemplateAction">A callback invoked with the owning control and the constructed <see cref="MC.DataTemplate"/>.</param>
    public static void AddDataTemplateProperty<TControl, TItem, TTemplateRoot>(
        RenderTreeBuilder builder,
        int sequence,
        RenderFragment<TItem> template,
        Action<TControl, MC.DataTemplate> setDataTemplateAction)
        where TTemplateRoot : MC.ContentView, new()
    {
        if (template != null)
        {
            builder.OpenRegion(sequence);

            builder.OpenComponent<DataTemplateItemsComponent<TControl, TItem, TTemplateRoot>>(0);
            builder.AddAttribute(1, nameof(DataTemplateItemsComponent<,,>.SetDataTemplateAction), setDataTemplateAction);
            builder.AddAttribute(2, nameof(DataTemplateItemsComponent<,,>.Template), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    /// <summary>
    /// Adds a <see cref="MC.DataTemplateSelector"/> property to the render tree by wrapping
    /// <paramref name="template"/> in a selector and passing it to <paramref name="setDataTemplateSelectorAction"/>.
    /// </summary>
    /// <typeparam name="TControl">The MAUI control type that owns the data-template-selector property.</typeparam>
    /// <typeparam name="TItem">The type of the data item passed to the template.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to write to.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="template">The typed <see cref="RenderFragment{TItem}"/> used to render each item. Nothing is rendered when <see langword="null"/>.</param>
    /// <param name="setDataTemplateSelectorAction">A callback invoked with the owning control and the constructed <see cref="MC.DataTemplateSelector"/>.</param>
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
            builder.AddAttribute(1, nameof(DataTemplateSelectorComponent<,>.SetDataTemplateSelectorAction), setDataTemplateSelectorAction);
            builder.AddAttribute(2, nameof(DataTemplateSelectorComponent<,>.TemplateSelector), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    /// <summary>
    /// Adds an untyped data-template property to the render tree by wrapping the non-generic
    /// <paramref name="template"/> in a <see cref="MC.DataTemplate"/> and passing it to
    /// <paramref name="setDataTemplateAction"/>. Use this overload when the template does not
    /// depend on a typed item (e.g. control templates on a <see cref="MC.BindableObject"/>).
    /// </summary>
    /// <typeparam name="T">A <see cref="MC.BindableObject"/> type that owns the data-template property.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to write to.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="template">The non-generic <see cref="RenderFragment"/> used as the template. Nothing is rendered when <see langword="null"/>.</param>
    /// <param name="setDataTemplateAction">A callback invoked with the owning control and the constructed <see cref="MC.DataTemplate"/>.</param>
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
            builder.AddAttribute(1, nameof(ControlTemplateItemsComponent<>.SetDataTemplateAction), setDataTemplateAction);
            builder.AddAttribute(2, nameof(ControlTemplateItemsComponent<>.Template), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    /// <summary>
    /// Adds a <see cref="MC.ControlTemplate"/> property to the render tree by wrapping
    /// <paramref name="template"/> in a <see cref="MC.ControlTemplate"/> and passing it to
    /// <paramref name="setControlTemplateAction"/>.
    /// </summary>
    /// <typeparam name="T">A <see cref="MC.BindableObject"/> type that owns the control-template property.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to write to.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="template">The <see cref="RenderFragment"/> used as the control template. Nothing is rendered when <see langword="null"/>.</param>
    /// <param name="setControlTemplateAction">A callback invoked with the owning control and the constructed <see cref="MC.ControlTemplate"/>.</param>
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
            builder.AddAttribute(1, nameof(ControlTemplateItemsComponent<>.SetControlTemplateAction), setControlTemplateAction);
            builder.AddAttribute(2, nameof(ControlTemplateItemsComponent<>.Template), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    /// <summary>
    /// Adds a synchronous typed data-template property to the render tree.
    /// Unlike <see cref="AddDataTemplateProperty{TControl, TItem}(RenderTreeBuilder, int, RenderFragment{TItem}, Action{TControl, MC.DataTemplate})"/>,
    /// the template rendering is performed synchronously, which may be required for certain MAUI controls.
    /// </summary>
    /// <typeparam name="TControl">The MAUI control type that owns the data-template property.</typeparam>
    /// <typeparam name="TItem">The type of the data item passed to the template.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to write to.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="template">The typed <see cref="RenderFragment{TItem}"/> used to render each item. Nothing is rendered when <see langword="null"/>.</param>
    /// <param name="setDataTemplateAction">A callback invoked with the owning control and the constructed <see cref="MC.DataTemplate"/>.</param>
    [Experimental("MBB001")]
    public static void AddSyncDataTemplateProperty<TControl, TItem>(
        RenderTreeBuilder builder,
        int sequence,
        RenderFragment<TItem> template,
        Action<TControl, MC.DataTemplate> setDataTemplateAction)
    {
        if (template != null)
        {
            builder.OpenRegion(sequence);

            builder.OpenComponent<SyncDataTemplateItemsComponent<TControl, TItem>>(0);
            builder.AddAttribute(1, nameof(SyncDataTemplateItemsComponent<,>.SetDataTemplateAction), setDataTemplateAction);
            builder.AddAttribute(2, nameof(SyncDataTemplateItemsComponent<,>.Template), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    /// <summary>
    /// Adds a synchronous untyped data-template property to the render tree.
    /// Unlike <see cref="AddDataTemplateProperty{T}(RenderTreeBuilder, int, RenderFragment, Action{T, MC.DataTemplate})"/>,
    /// the template rendering is performed synchronously, which may be required for certain MAUI controls.
    /// </summary>
    /// <typeparam name="T">A <see cref="MC.BindableObject"/> type that owns the data-template property.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to write to.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="template">The non-generic <see cref="RenderFragment"/> used as the template. Nothing is rendered when <see langword="null"/>.</param>
    /// <param name="setDataTemplateAction">A callback invoked with the owning control and the constructed <see cref="MC.DataTemplate"/>.</param>
    [Experimental("MBB001")]
    public static void AddSyncDataTemplateProperty<T>(
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
            builder.AddAttribute(1, nameof(SyncControlTemplateItemsComponent<>.SetDataTemplateAction), setDataTemplateAction);
            builder.AddAttribute(2, nameof(SyncControlTemplateItemsComponent<>.Template), template);
            builder.CloseComponent();

            builder.CloseRegion();
        }
    }

    /// <summary>
    /// Adds an items-source property to the render tree, binding <paramref name="items"/> to the
    /// owning control via <paramref name="collectionSetter"/>. The component monitors the enumerable
    /// for changes and reconciles the target collection, using <paramref name="keySelector"/> (when
    /// provided) to match items across renders for efficient updates.
    /// </summary>
    /// <typeparam name="TControl">The MAUI control type that owns the items-source property.</typeparam>
    /// <typeparam name="TItem">The type of each item in the source sequence.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to write to.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="items">The source items to bind. Nothing is rendered when <see langword="null"/>.</param>
    /// <param name="keySelector">An optional function that returns a unique key for each item, used for efficient diffing. Pass <see langword="null"/> to disable keyed diffing.</param>
    /// <param name="collectionSetter">A callback invoked with the owning control and the managed <see cref="ICollection{TItem}"/> that stays in sync with <paramref name="items"/>.</param>
    [Experimental("MBB001")]
    public static void AddItemsSourceProperty<TControl, TItem>(
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
        builder.AddAttribute(1, nameof(ItemsSourceComponent<,>.Items), items);
        builder.AddAttribute(2, nameof(ItemsSourceComponent<,>.CollectionSetter), collectionSetter);

        if (keySelector != null)
            builder.AddAttribute(3, nameof(ItemsSourceComponent<,>.KeySelector), keySelector);

        builder.CloseComponent();

        builder.CloseRegion();
    }
}
