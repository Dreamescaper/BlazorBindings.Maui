// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Maui.Elements.Shapes;

public partial class Polygon : Shape
{
    [Parameter] public string Points { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(Points))
        {
            if (!Equals(value, Points))
            {
                Points = (string)value;
                NativeControl.Points = AttributeHelper.StringToPointCollection(Points);
            }
            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }
}
