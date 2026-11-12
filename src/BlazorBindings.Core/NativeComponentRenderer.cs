// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace BlazorBindings.Core;

public abstract class NativeComponentRenderer
    (IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    : Renderer(serviceProvider, loggerFactory)
{
    private readonly Dictionary<int, NativeComponentAdapter> _componentIdToAdapter = [];
    private readonly List<(int Id, IComponent Component)> _rootComponents = [];
    private ElementManager _elementManager;
    private int _batchDepth;
    private RenderPhase _phase = RenderPhase.Idle;

    protected virtual ElementManager CreateNativeControlManager() => new();

    internal ElementManager ElementManager => _elementManager ??= CreateNativeControlManager();

    public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

    /// <summary>
    /// Creates a component of type <typeparamref name="TComponent"/> and adds it as a child of <paramref name="parent"/>.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="parent"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<TComponent> AddComponent<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>
        (IElementHandler parent, Dictionary<string, object> parameters = null)
        where TComponent : IComponent
    {
        return (TComponent)await AddComponent(typeof(TComponent), parent, parameters);
    }

    /// <summary>
    /// Creates a component of type <paramref name="componentType"/> and adds it as a child of <paramref name="parent"/>. If parameters are provided they will be set on the component.
    /// </summary>
    /// <param name="componentType"></param>
    /// <param name="parent"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<IComponent> AddComponent(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType,
        IElementHandler parent,
        Dictionary<string, object> parameters = null)
    {
        try
        {
            return await Dispatcher.InvokeAsync(async () =>
            {
                var (component, _, quiescence) = StartRootComponentRender(componentType, parent, parameters);
                await quiescence;
                return component;
            });
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return null;
        }
    }

    /// <summary>
    /// Renders a component of type <typeparamref name="TComponent"/> as a child of <paramref name="parent"/>,
    /// and returns only once its first render batch has been applied - i.e. once the native elements it
    /// creates are attached to <paramref name="parent"/>.
    /// <para>
    /// Unlike <see cref="AddComponent{TComponent}(IElementHandler, Dictionary{string, object})"/>, this does not
    /// wait for asynchronous lifecycle work; await the returned <see cref="NativeRender{TComponent}"/> for that.
    /// This makes it usable from synchronous native callbacks that must return an element immediately.
    /// </para>
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the component could not be materialized synchronously - most commonly because this was
    /// called while a render batch was already being processed.
    /// </exception>
    /// <remarks>Experimental API, subject to change.</remarks>
    public NativeRender<TComponent> Render<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>
        (IElementHandler parent, Dictionary<string, object> parameters = null)
        where TComponent : IComponent
    {
        var (component, quiescence) = RenderSynchronously(typeof(TComponent), parent, parameters);
        return new NativeRender<TComponent>((TComponent)component, quiescence);
    }

    /// <inheritdoc cref="Render{TComponent}(IElementHandler, Dictionary{string, object})"/>
    /// <remarks>Experimental API, subject to change.</remarks>
    public NativeRender<IComponent> Render(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType,
        IElementHandler parent,
        Dictionary<string, object> parameters = null)
    {
        var (component, quiescence) = RenderSynchronously(componentType, parent, parameters);
        return new NativeRender<IComponent>(component, quiescence);
    }

    private (IComponent Component, Task Quiescence) RenderSynchronously(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType,
        IElementHandler parent,
        Dictionary<string, object> parameters)
    {
        Dispatcher.AssertAccess();

        IComponent component = null;
        NativeComponentAdapter adapter = null;
        Task quiescence = null;

        void Start()
        {
            (component, adapter, quiescence) = StartRootComponentRender(componentType, parent, parameters);

            if (quiescence.IsFaulted || adapter.HasRendered)
                return;

            // Nothing materialized - the component never rendered, or deferred its first render
            // past this point. Undo the registration here, while we are still outside the batch,
            // so it cannot attach an element later, after the caller has been told it failed and
            // given no component with which to remove it.
            RemoveRootComponent(component);
            component = null;
        }

        // The root render has to happen outside the current batch, not merely be flushed afterwards:
        // RenderRootComponentAsync waits for quiescence immediately after setting parameters, so if the
        // render were still queued at that point the component's async lifecycle would not be tracked
        // and the returned quiescence task would complete before the component ever rendered.
        //
        // Nothing is registered until we know the render can proceed. A root component created here
        // and then abandoned would still render when the outer batch drains its queue, attaching an
        // element the caller was told had failed and holds no reference to remove.
        if (!TryRunOutsideCurrentBatch(Start))
            throw NotMaterializedSynchronously(componentType);

        // If the first render failed, surface that failure rather than the barrier error it causes.
        if (quiescence.IsFaulted)
        {
            ExceptionDispatchInfo.Throw(quiescence.Exception.InnerException ?? quiescence.Exception);
        }

        // The render ran but produced nothing - Start has already removed the root in that case.
        if (component is null)
            throw NotMaterializedSynchronously(componentType);

        return (component, quiescence);
    }

    private InvalidOperationException NotMaterializedSynchronously(Type componentType)
        => new($"Component {componentType.FullName} did not render synchronously, so its native element is not available yet. "
            + (IsProcessingBatch
                ? "This happened because Render was called while a render batch was being processed - for example from an element "
                  + "handler, a component's Dispose, or OnAfterRender. Use AddComponent and await it instead."
                : "Use AddComponent and await it instead."));

    private (IComponent Component, NativeComponentAdapter Adapter, Task Quiescence) StartRootComponentRender(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType,
        IElementHandler parent,
        Dictionary<string, object> parameters)
    {
        var component = InstantiateComponent(componentType);
        var componentId = AssignRootComponentId(component);

        _rootComponents.Add((componentId, component));

        var rootAdapter = new NativeComponentAdapter(this, null, knownTargetElement: parent)
        {
            Name = $"RootAdapter attached to {parent.GetType().FullName}",
        };

        _componentIdToAdapter[componentId] = rootAdapter;

        var parameterView = parameters?.Count > 0 ? ParameterView.FromDictionary(parameters) : ParameterView.Empty;

        // RenderRootComponentAsync sets the parameters synchronously, which - unless a batch is already
        // being processed - drives the first render batch to completion before it yields.
        var quiescence = RenderRootComponentAsync(componentId, parameterView);

        return (component, rootAdapter, quiescence);
    }

    /// <summary>
    /// Removes the specified component from the renderer, causing the component and its
    /// descendants to be disposed.
    /// </summary>
    public void RemoveRootComponent(IComponent component)
    {
        var componentId = _rootComponents.LastOrDefault(c => c.Component == component).Id;

        // RemoveRootComponent flushes the render queue directly, without going through ProcessPendingRender.
        _batchDepth++;
        try
        {
            RemoveRootComponent(componentId);
        }
        finally
        {
            _batchDepth--;
        }
    }

    /// <summary>
    /// True while a render batch is being processed, i.e. while the render tree is being diffed or
    /// applied to the native tree.
    /// </summary>
    protected bool IsProcessingBatch => _batchDepth > 0;

    /// <summary>
    /// Runs <paramref name="requestRender"/> and applies the renders it triggers before returning,
    /// even when a render batch is already in progress. This is what lets a native control demand an
    /// element during the batch that creates it - e.g. a MAUI DataTemplate realized while we attach it.
    /// <para>
    /// The render must be requested inside the callback rather than beforehand, because the nested
    /// batch is given its own queue. Returns false - without running anything - when this cannot be
    /// done safely, leaving the caller to fall back.
    /// </para>
    /// </summary>
    /// <remarks>Experimental API, subject to change.</remarks>
    public bool TryRenderSynchronously(Action requestRender)
    {
        ArgumentNullException.ThrowIfNull(requestRender);
        Dispatcher.AssertAccess();

        return TryRunOutsideCurrentBatch(() =>
        {
            requestRender();

            // Outside a batch AddToRenderQueue processes the queue itself, so this is normally a
            // no-op; it covers a callback that queued work without triggering that path.
            ProcessPendingRender();
        });
    }

    /// <summary>
    /// Runs <paramref name="action"/> as though no render batch were in progress, so that renders it
    /// queues are processed immediately. Returns false - without running anything - when that cannot
    /// be done safely.
    /// </summary>
    private bool TryRunOutsideCurrentBatch(Action action)
    {
        // Not in a batch: renders already process immediately.
        if (_phase == RenderPhase.Idle && !BatchInProgressFlag(this))
        {
            action();
            return true;
        }

        // Still reading the shared batch buffers - a nested batch would overwrite them underneath us.
        if (_phase != RenderPhase.ApplyingToNativeTree)
            return false;

        ref var batchInProgress = ref BatchInProgressFlag(this);
        var previousPhase = _phase;

        // Hand the nested batch a clean builder, keeping the outer batch's out of its reach.
        var swapped = SwappedBatchBuilder.Swap(this);

        // The outer batch has finished building; only our own pending edits are still in play, so
        // the renderer is free to build and apply another batch on top.
        batchInProgress = false;
        try
        {
            action();
        }
        finally
        {
            // Put the outer batch's builder back, so its own finally returns its arrays to the pool
            // exactly once, as it would have without the nested batch.
            swapped.Restore();
            _phase = previousPhase;
            batchInProgress = true;
        }

        return true;
    }

    protected override void ProcessPendingRender()
    {
        _batchDepth++;
        try
        {
            base.ProcessPendingRender();
        }
        finally
        {
            _batchDepth--;
        }
    }

    protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
    {
        HashSet<NativeComponentAdapter> adaptersWithPendingEdits = [];

        var previousPhase = _phase;

        // Reading the render tree. The frames and edits live in buffers that the renderer reuses for
        // the next batch, so no nested render may run while we are still reading them.
        _phase = RenderPhase.ReadingRenderTree;

        var numUpdatedComponents = renderBatch.UpdatedComponents.Count;
        for (var componentIndex = 0; componentIndex < numUpdatedComponents; componentIndex++)
        {
            var updatedComponent = renderBatch.UpdatedComponents.Array[componentIndex];

            if (updatedComponent.Edits.Count > 0)
            {
                var adapter = _componentIdToAdapter[updatedComponent.ComponentId];
                adapter.HasRendered = true;
                adapter.ApplyEdits(updatedComponent.ComponentId, updatedComponent.Edits, renderBatch, adaptersWithPendingEdits);
            }
        }

        // Copy out the last thing we still need from the shared buffers, so that from here on we only
        // touch state that we own and a nested render becomes safe.
        var disposedComponentIds = new int[renderBatch.DisposedComponentIDs.Count];
        renderBatch.DisposedComponentIDs.Array.AsSpan(0, disposedComponentIds.Length).CopyTo(disposedComponentIds);

        try
        {
            // Applying to the native tree. This only reads the pending edits built above, so a nested
            // render may safely run here - see TryRenderSynchronously.
            _phase = RenderPhase.ApplyingToNativeTree;

            foreach (var adapter in adaptersWithPendingEdits.OrderByDescending(a => a.DeepLevel))
                adapter.ApplyPendingEdits();

            foreach (var disposedComponentId in disposedComponentIds)
            {
                if (_componentIdToAdapter.Remove(disposedComponentId, out var adapter))
                {
                    (adapter as IDisposable)?.Dispose();
                }
            }
        }
        finally
        {
            _phase = previousPhase;
        }

        return Task.CompletedTask;
    }

    internal void RegisterComponentAdapter(NativeComponentAdapter adapter, int componentId)
    {
        _componentIdToAdapter[componentId] = adapter;
    }

    private enum RenderPhase
    {
        /// <summary>No batch is being applied.</summary>
        Idle,

        /// <summary>Reading frames/edits out of the renderer's shared batch buffers.</summary>
        ReadingRenderTree,

        /// <summary>Applying already-read edits to the native tree.</summary>
        ApplyingToNativeTree
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_isBatchInProgress")]
    private static extern ref bool BatchInProgressFlag(Renderer renderer);

    /// <summary>
    /// Swaps the renderer's batch builder for a fresh one, so a nested batch gets clean state and
    /// cannot touch the in-flight outer batch's, then puts the original back.
    /// <para>
    /// The builder owns pool-rented arrays that the outer RenderBatch still points at - and
    /// ProcessRenderQueue reads them after UpdateDisplayAsync returns, via InvokeRenderCompletedCalls.
    /// Letting a nested batch clear that builder would return those arrays to ArrayPool.Shared while
    /// they are still in use.
    /// </para>
    /// </summary>
    private sealed class SwappedBatchBuilder
    {
        private const string RenderBatchBuilderType =
            "Microsoft.AspNetCore.Components.Rendering.RenderBatchBuilder, Microsoft.AspNetCore.Components";

        // Trim/AOT-safe: the type is a literal, so the trimmer resolves the field statically and
        // preserves it. Reading and writing it through FieldInfo needs no annotations, and a Native
        // AOT build of this path produces no trim warnings from this assembly.
        //
        // UnsafeAccessor cannot replace this one. Its field accessors must return byref, and
        // UnsafeAccessorType is rejected there - a writable "ref object" into a field of another
        // reference type would be a type hole - so the inaccessible field type cannot be named.
        // That is deliberate, and tracked by dotnet/runtime#119664; it waits on a usable
        // TypedReference (dotnet/runtime#26186). The documented alternatives are to know the field
        // type or to reach it through a property/method, and Renderer offers neither. Revisit if
        // that issue is implemented.
        //
        // Note this field is declared readonly, so the write below relies on reflection's ability
        // to set a readonly *instance* field. The swap and the restore both complete within the
        // nested render, so the outer ProcessRenderQueue observes only the original value.
        //
        // Constructing the builder does work through UnsafeAccessor, below, which is what avoids
        // the one Activator.CreateInstance(Type) call that trimming warns about (IL2072).
        private static readonly System.Reflection.FieldInfo BatchBuilderField =
            typeof(Renderer).GetField("_batchBuilder", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        [return: UnsafeAccessorType(RenderBatchBuilderType)]
        private static extern object CreateBatchBuilder();

        private readonly Renderer _renderer;
        private readonly object _original;
        private readonly object _temporary;

        private SwappedBatchBuilder(Renderer renderer, object original, object temporary)
        {
            _renderer = renderer;
            _original = original;
            _temporary = temporary;
        }

        public static SwappedBatchBuilder Swap(Renderer renderer)
        {
            var original = BatchBuilderField.GetValue(renderer);
            var temporary = CreateBatchBuilder();

            BatchBuilderField.SetValue(renderer, temporary);
            return new SwappedBatchBuilder(renderer, original, temporary);
        }

        public void Restore()
        {
            BatchBuilderField.SetValue(_renderer, _original);

            // Hand the temporary builder's own rented arrays back.
            (_temporary as IDisposable)?.Dispose();
        }
    }
}
