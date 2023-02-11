﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBindings.Core
{
    public abstract class NativeComponentRenderer : Renderer
    {
        private readonly Dictionary<int, NativeComponentAdapter> _componentIdToAdapter = new Dictionary<int, NativeComponentAdapter>();
        private ElementManager _elementManager;
        private readonly Dictionary<ulong, Action<ulong>> _eventRegistrations = new Dictionary<ulong, Action<ulong>>();
        private readonly List<(int Id, IComponent Component)> _rootComponents = new();


        public NativeComponentRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        protected abstract ElementManager CreateNativeControlManager();

        internal ElementManager ElementManager
        {
            get
            {
                return _elementManager ??= CreateNativeControlManager();
            }
        }

        public override Dispatcher Dispatcher { get; }
             = Dispatcher.CreateDefault();

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

                    var rootAdapter = new NativeComponentAdapter(this, closestPhysicalParent: parent, knownTargetElement: parent)
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
            HashSet<int> processedComponentIds = new HashSet<int>();

            var numUpdatedComponents = renderBatch.UpdatedComponents.Count;
            for (var componentIndex = 0; componentIndex < numUpdatedComponents; componentIndex++)
            {
                var updatedComponent = renderBatch.UpdatedComponents.Array[componentIndex];

                // If UpdatedComponent is already processed (due to recursive ApplyEdits) - skip it.
                if (updatedComponent.Edits.Count > 0 && !processedComponentIds.Contains(updatedComponent.ComponentId))
                {
                    var adapter = _componentIdToAdapter[updatedComponent.ComponentId];
                    adapter.ApplyEdits(updatedComponent.ComponentId, updatedComponent.Edits, renderBatch.ReferenceFrames, renderBatch, processedComponentIds);
                }
            }

            var numDisposedComponents = renderBatch.DisposedComponentIDs.Count;
            for (var i = 0; i < numDisposedComponents; i++)
            {
                var disposedComponentId = renderBatch.DisposedComponentIDs.Array[i];
                if (_componentIdToAdapter.Remove(disposedComponentId, out var adapter))
                {
                    (adapter as IDisposable)?.Dispose();
                }
            }

            var numDisposeEventHandlers = renderBatch.DisposedEventHandlerIDs.Count;
            for (var i = 0; i < numDisposeEventHandlers; i++)
            {
                DisposeEvent(renderBatch.DisposedEventHandlerIDs.Array[i]);
            }

            return Task.CompletedTask;
        }

        public void RegisterEvent(ulong eventHandlerId, Action<ulong> unregisterCallback)
        {
            if (eventHandlerId == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(eventHandlerId), "Event handler ID must not be 0.");
            }
            if (unregisterCallback == null)
            {
                throw new ArgumentNullException(nameof(unregisterCallback));
            }
            _eventRegistrations.Add(eventHandlerId, unregisterCallback);
        }

        private void DisposeEvent(ulong eventHandlerId)
        {
            if (!_eventRegistrations.TryGetValue(eventHandlerId, out var unregisterCallback))
            {
                throw new InvalidOperationException($"Attempting to dispose unknown event handler id '{eventHandlerId}'.");
            }
            unregisterCallback(eventHandlerId);
        }

        internal NativeComponentAdapter CreateAdapterForChildComponent(IElementHandler physicalParent, int componentId)
        {
            var result = new NativeComponentAdapter(this, physicalParent);
            _componentIdToAdapter[componentId] = result;
            return result;
        }
    }
}
