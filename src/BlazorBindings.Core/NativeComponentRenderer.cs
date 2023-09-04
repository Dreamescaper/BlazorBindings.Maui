// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;

namespace BlazorBindings.Core;

public abstract class NativeComponentRenderer : Renderer
{
    private readonly Dictionary<int, NativeComponentAdapter> _componentIdToAdapter = new();
    private readonly List<(int Id, IComponent Component)> _rootComponents = new();
    private ElementManager _elementManager;

    public NativeComponentRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        : base(serviceProvider, loggerFactory)
    {
    }

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
    public async Task<TComponent> AddComponent<TComponent>(IElementHandler parent, Dictionary<string, object> parameters = null) where TComponent : IComponent
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
    public async Task<IComponent> AddComponent(Type componentType, IElementHandler parent, Dictionary<string, object> parameters = null)
    {
        try
        {
            return await Dispatcher.InvokeAsync(async () =>
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
                await RenderRootComponentAsync(componentId, parameterView);
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
    /// Removes the specified component from the renderer, causing the component and its
    /// descendants to be disposed.
    /// </summary>
    public void RemoveRootComponent(IComponent component)
    {
        var componentId = _rootComponents.LastOrDefault(c => c.Component == component).Id;
        RemoveRootComponent(componentId);
    }

    protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
    {
        HashSet<NativeComponentAdapter> adaptersWithPendingEdits = new();

        var numUpdatedComponents = renderBatch.UpdatedComponents.Count;
        for (var componentIndex = 0; componentIndex < numUpdatedComponents; componentIndex++)
        {
            var updatedComponent = renderBatch.UpdatedComponents.Array[componentIndex];

            if (updatedComponent.Edits.Count > 0)
            {
                var adapter = _componentIdToAdapter[updatedComponent.ComponentId];
                adapter.ApplyEdits(updatedComponent.ComponentId, updatedComponent.Edits, renderBatch, adaptersWithPendingEdits);
            }
        }

        foreach (var adapter in adaptersWithPendingEdits.OrderByDescending(a => a.DeepLevel))
            adapter.ApplyPendingEdits();

        var numDisposedComponents = renderBatch.DisposedComponentIDs.Count;
        for (var i = 0; i < numDisposedComponents; i++)
        {
            var disposedComponentId = renderBatch.DisposedComponentIDs.Array[i];
            if (_componentIdToAdapter.Remove(disposedComponentId, out var adapter))
            {
                (adapter as IDisposable)?.Dispose();
            }
        }

        return Task.CompletedTask;
    }

    internal void RegisterComponentAdapter(NativeComponentAdapter adapter, int componentId)
    {
        _componentIdToAdapter[componentId] = adapter;
    }
}
