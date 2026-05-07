// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Maui.Elements.Shapes;

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
                    StrokeDashArray = CastParameter<string>(value, name);
                    NativeControl.StrokeDashArray = AttributeHelper.GetDoubleCollection(StrokeDashArray);
                }
                return true;
            default:
                return base.HandleAdditionalParameter(name, value);
        }
    }
}
