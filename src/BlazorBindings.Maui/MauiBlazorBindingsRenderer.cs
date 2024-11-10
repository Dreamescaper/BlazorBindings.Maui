// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.Elements.Handlers;
using Microsoft.Extensions.Logging;
using System.Runtime.ExceptionServices;

namespace BlazorBindings.Maui;

public class MauiBlazorBindingsRenderer : NativeComponentRenderer
{
    public MauiBlazorBindingsRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        : base(serviceProvider, loggerFactory)
    {
    }

    internal MauiBlazorBindingsRenderer(MauiBlazorBindingsServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        : base(serviceProvider, loggerFactory)
    {
    }

    public override Dispatcher Dispatcher { get; } = new MauiDeviceDispatcher();

    protected override void HandleException(Exception exception)
    {
        ExceptionDispatchInfo.Throw(exception);
    }

    // It tries to return the Element as soon as it is available, therefore Component task might still be in progress.
    internal async Task<(object Element, Task<IComponent> Component)> GetElementFromRenderedComponent(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType,
        Dictionary<string, object> parameters = null)
    {
        var (elements, addComponentTask) = await GetElementsFromRenderedComponent(componentType, parameters);

        if (elements.Count != 1)
        {
            throw new InvalidOperationException("The target component must have exactly one root element.");
        }

        return (elements[0], addComponentTask);
    }

    internal async Task<(T Component, int ComponentId)> AddRootComponent<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
        Dictionary<string, object> initialParameters)
        where T : IComponent
    {
        var component = (T)InstantiateComponent(typeof(T));
        var componentId = AssignRootComponentId(component);
        await RenderRootComponentAsync(componentId, ParameterView.FromDictionary(initialParameters));
        return (component, componentId);
    }

    private async Task<(List<object> Elements, Task<IComponent> Component)> GetElementsFromRenderedComponent(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType,
        Dictionary<string, object> parameters)
    {
        var container = new RootContainerHandler();

        var addComponentTask = AddComponent(componentType, container, parameters);
        var elementAddedTask = container.WaitForElementAsync();

        await Task.WhenAny(addComponentTask, elementAddedTask);

        if (addComponentTask.Exception != null)
        {
            var exception = addComponentTask.Exception.InnerException;
            ExceptionDispatchInfo.Throw(exception);
        }

        return (container.Elements, addComponentTask);
    }
}
