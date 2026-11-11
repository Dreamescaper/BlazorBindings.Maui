// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;

namespace BlazorBindings.Maui.Elements;

public static partial class AttributeHelper
{
    public static BindingBase GetBinding<T, TResult>(Func<T, TResult> bindingFunc)
    {
        if (bindingFunc is null)
            return null;

        return new TypedBinding<T, TResult>((item) => (bindingFunc(item), true), null, null)
        {
            // MAUI applies such bindings for null items as well (e.g. Picker.ItemDisplayBinding for a
            // nullable items source), but TypedBinding skips its getter whenever the source is not an
            // instance of TSource - which includes null - and uses FallbackValue instead. Therefore the
            // value for a null item is evaluated upfront.
            FallbackValue = GetValueForNull(bindingFunc)
        };
    }

    private static object GetValueForNull<T, TResult>(Func<T, TResult> bindingFunc)
    {
        try
        {
            return bindingFunc(default);
        }
        catch
        {
            // The function is not required to handle null items, in which case the default value is used.
            return null;
        }
    }
}
