// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Maui.Controls;

namespace BlazorBindings.Maui;

/// <remarks>Experimental API, subject to change.</remarks>
[Experimental("MBB001")]
public static class AttachedPropertyRegistry
{
    internal static readonly Dictionary<string, Action<BindableObject, object>> AttachedPropertyHandlers = [];

    public static void RegisterAttachedPropertyHandler(string propertyName, Action<BindableObject, object> handler)
    {
        AttachedPropertyHandlers[propertyName] = handler;
    }
}
