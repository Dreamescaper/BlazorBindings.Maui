// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public static partial class AttributeHelper
{
    // MAUI applies such bindings for null items as well (e.g. Picker.ItemDisplayBinding for a nullable
    // items source). TypedBinding skips its getter whenever the source is not an instance of TSource,
    // which includes null, so a self-path Binding with a converter is used instead - that one does
    // invoke the converter for null values.
    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "Binding.SelfPath does not resolve any members by name, therefore no reflection is involved.")]
    public static BindingBase GetBinding<T, TResult>(Func<T, TResult> bindingFunc)
    {
        if (bindingFunc is null)
            return null;

        return new Binding(Binding.SelfPath, converter: new FuncConverter<T, TResult>(bindingFunc));
    }

    private sealed class FuncConverter<T, TResult>(Func<T, TResult> bindingFunc) : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                T typedValue => bindingFunc(typedValue),
                null => bindingFunc(default),
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
