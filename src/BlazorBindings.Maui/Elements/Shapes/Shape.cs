// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;

namespace BlazorBindings.Maui.Elements.Shapes
{
    public abstract partial class Shape : BlazorBindings.Maui.Elements.View
    {
        [Parameter] public string StrokeDashArray { get; set; }

        protected override bool HandleAdditionalParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(StrokeDashArray):
                    if (!Equals(StrokeDashArray, value))
                    {
                        NativeControl.StrokeDashArray = AttributeHelper.GetDoubleCollection((string)value);
                        StrokeDashArray = (string)value;
                    }
                    return true;
                default:
                    return base.HandleAdditionalParameter(name, value);
            }
        }
    }
}
