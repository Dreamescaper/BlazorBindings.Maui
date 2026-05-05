using Microsoft.Maui.Graphics;

namespace BlazorBindings.Maui.Elements;

public partial class Shadow
{
    [Parameter] public Color Color { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(Color))
        {
            if (!Equals(Color, value))
            {
                Color = CastParameter<Color>(value, name);
                NativeControl.Brush = Color;
            }

            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }
}
