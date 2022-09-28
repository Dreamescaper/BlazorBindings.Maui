// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using System;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements
{
    public partial class Picker<T> : View
    {
        [Parameter] public Func<T, string> ItemDisplayBinding { get; set; }

        protected override bool HandleAdditionalParameter(string name, object value)
        {
            if (name == nameof(ItemDisplayBinding))
            {
                if (!Equals(ItemDisplayBinding, value))
                {
                    ItemDisplayBinding = (Func<T, string>)value;
                    NativeControl.ItemDisplayBinding = new MC.Internals.TypedBinding<T, string>((item) => (ItemDisplayBinding(item), true), null, null);
                }
                return true;
            }
            return base.HandleAdditionalParameter(name, value);
        }
    }
}
