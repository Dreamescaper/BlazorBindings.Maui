// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Core;

/// <summary>
/// Represents a container for native element.
/// </summary>
public interface IElementHandler
{
    /// <summary>
    /// The native element represented by this handler. This is often a native UI component, but can be any type
    /// of component used by the native system.
    /// </summary>
    object TargetElement { get; }
}
