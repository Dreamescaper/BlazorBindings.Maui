// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace BlazorBindings.Core;

/// <summary>
/// The result of a synchronous render started by
/// <see cref="NativeComponentRenderer.Render{TComponent}(IElementHandler, Dictionary{string, object})"/>.
/// <para>
/// By the time this value exists, the component has already produced its first render batch, so any
/// native elements it creates are attached to the parent handler. Awaiting it additionally waits for
/// quiescence - i.e. for asynchronous lifecycle work such as <c>OnInitializedAsync</c> to complete.
/// </para>
/// </summary>
/// <remarks>Experimental API, subject to change.</remarks>
public readonly struct NativeRender<TComponent> where TComponent : IComponent
{
    private readonly Task _quiescence;

    internal NativeRender(TComponent component, Task quiescence)
    {
        Component = component;
        _quiescence = quiescence;
    }

    /// <summary>
    /// The rendered component. Its first render has already been applied to the native tree.
    /// </summary>
    public TComponent Component { get; }

    /// <summary>
    /// Completes when the component and all of its descendants have finished any asynchronous
    /// lifecycle work. Note that the native elements are available before this task completes.
    /// </summary>
    public Task Quiescence => _quiescence ?? Task.CompletedTask;

    /// <summary>
    /// Allows the result to be awaited directly, which waits for <see cref="Quiescence"/>.
    /// </summary>
    public TaskAwaiter GetAwaiter() => Quiescence.GetAwaiter();
}
