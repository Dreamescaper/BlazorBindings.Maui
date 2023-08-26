// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Maui.Graphics;

namespace BlazorBindings.Maui.Elements;

public static partial class AttributeHelper
{
    /// <summary>
    /// Helper method to deserialize <see cref="Color" /> objects.
    /// </summary>
    public static Color GetColor(object obj)
    {
        return obj switch
        {
            null => null,
            Color color => color,
            string colorAsString => Color.FromRgba(colorAsString),
            _ => throw new ArgumentException("Cannot convert parameter to Color.", nameof(obj))
        };
    }
}
