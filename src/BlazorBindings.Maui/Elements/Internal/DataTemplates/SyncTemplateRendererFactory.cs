// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.Elements.Handlers;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;
using System.Runtime.ExceptionServices;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Internal.DataTemplates;

/// <summary>
/// Creates isolated renderers for MAUI templates that must synchronously return their native root.
/// </summary>
internal interface ISyncTemplateRendererFactory
{
    ISyncTemplateRootHandle<RenderFragment> Render(RenderFragment template);
    ISyncTemplateRootHandle<RenderFragment<TItem>> Render<TItem>(RenderFragment<TItem> template, TItem initialItem);
}

/// <summary>
/// Owns a native template root and the child renderer that keeps it alive.
/// </summary>
internal interface ISyncTemplateRootHandle : IDisposable
{
    object RootElement { get; }
}

/// <summary>
/// Updates a previously-created synchronous template root when its holder receives a new template fragment.
/// </summary>
internal interface ISyncTemplateRootHandle<in TTemplate> : ISyncTemplateRootHandle
{
    void UpdateTemplate(TTemplate template);
}

/// <summary>
/// Renders synchronous MAUI template roots in child renderers to avoid reentrant renders in the main renderer batch.
/// </summary>
internal class SyncTemplateRendererFactory(
    MauiBlazorBindingsServiceProvider serviceProvider,
    ILoggerFactory loggerFactory)
    : ISyncTemplateRendererFactory
{
    public ISyncTemplateRootHandle<RenderFragment> Render(RenderFragment template)
    {
        var renderer = CreateRenderer();
        var container = new RootContainerHandler();

        var (component, renderTask) = renderer.StartRenderingComponent(
            typeof(SyncTemplateRootComponent),
            container,
            new Dictionary<string, object>
            {
                [nameof(SyncTemplateRootComponent.Template)] = template
            });

        ThrowIfFaulted(renderTask);

        return new SyncTemplateRootHandle<RenderFragment>(
            renderer,
            component,
            GetRootElement(container, renderTask),
            nameof(SyncTemplateRootComponent.Template));
    }

    public ISyncTemplateRootHandle<RenderFragment<TItem>> Render<TItem>(
        RenderFragment<TItem> template,
        TItem initialItem)
    {
        var renderer = CreateRenderer();
        var container = new RootContainerHandler();

        var (component, renderTask) = renderer.StartRenderingComponent(
            typeof(SyncDataTemplateItemComponent<TItem>),
            container,
            new Dictionary<string, object>
            {
                [nameof(SyncDataTemplateItemComponent<TItem>.Template)] = template,
                [nameof(SyncDataTemplateItemComponent<TItem>.InitialItem)] = initialItem
            });

        ThrowIfFaulted(renderTask);

        var itemComponent = (SyncDataTemplateItemComponent<TItem>)component;
        var rootElement = itemComponent.RootControl ?? GetMissingRootElement(renderTask);

        return new SyncTemplateRootHandle<RenderFragment<TItem>>(
            renderer,
            component,
            rootElement,
            nameof(SyncDataTemplateItemComponent<TItem>.Template));
    }

    private MauiBlazorBindingsRenderer CreateRenderer()
    {
        var renderer = new MauiBlazorBindingsRenderer(serviceProvider, loggerFactory);
        if (!renderer.Dispatcher.CheckAccess())
        {
            throw new InvalidOperationException(
                "Synchronous templates must be created on the renderer dispatcher thread.");
        }

        return renderer;
    }

    private static object GetRootElement(RootContainerHandler container, Task renderTask)
    {
        if (container.Elements.Count == 1)
        {
            return container.Elements[0];
        }

        if (container.Elements.Count > 1)
        {
            throw new InvalidOperationException("Synchronous templates must have exactly one root element.");
        }

        return GetMissingRootElement(renderTask);
    }

    private static MC.BindableObject GetMissingRootElement(Task renderTask)
    {
        ThrowIfFaulted(renderTask);

        if (!renderTask.IsCompleted)
        {
            throw new InvalidOperationException(
                "Synchronous templates must render a root element before awaiting asynchronous work.");
        }

        throw new InvalidOperationException("Synchronous templates must render exactly one root element.");
    }

    private static void ThrowIfFaulted(Task task)
    {
        if (task.IsFaulted)
        {
            ExceptionDispatchInfo.Throw(task.Exception.InnerException ?? task.Exception);
        }

        if (task.IsCanceled)
        {
            throw new TaskCanceledException(task);
        }
    }
}

/// <summary>
/// Tracks one child-rendered template root and disposes its renderer with the root.
/// </summary>
internal sealed class SyncTemplateRootHandle<TTemplate>(
    MauiBlazorBindingsRenderer renderer,
    IComponent component,
    object rootElement,
    string templateParameterName)
    : ISyncTemplateRootHandle<TTemplate>
{
    private bool _disposed;

    public object RootElement { get; } = rootElement;

    public void UpdateTemplate(TTemplate template)
    {
        if (_disposed)
        {
            return;
        }

        var renderTask = renderer.UpdateRootComponent(
            component,
            new Dictionary<string, object>
            {
                [templateParameterName] = template
            });

        if (renderTask.IsFaulted)
        {
            ExceptionDispatchInfo.Throw(renderTask.Exception.InnerException ?? renderTask.Exception);
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        renderer.RemoveRootComponent(component);
        (renderer as IDisposable)?.Dispose();
    }
}

/// <summary>
/// Root component used by child renderers for untyped synchronous templates.
/// </summary>
internal class SyncTemplateRootComponent : NativeControlComponentBase
{
    [Parameter] public RenderFragment Template { get; set; }

    protected override RenderFragment GetChildContent() => Template;
}
