using Microsoft.Maui;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class Border
{
    /// <summary>
    /// Sets <see cref="StrokeShape"/> to a RoundRectangle with specified CornerRadius.
    /// This property is not allowed if <see cref="StrokeShape"/> is already set explicitly.
    /// </summary>
    [Parameter] public CornerRadius? CornerRadius { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(CornerRadius))
        {
            if (!Equals(CornerRadius, value))
            {
                if (StrokeShape != null)
                    throw new InvalidOperationException("It is not allowed to set both CornerRadius and StrokeShape properties.");

                CornerRadius = CastParameter<CornerRadius?>(value, name);

                if (NativeControl.StrokeShape is not MC.Shapes.RoundRectangle roundRectangle)
                {
                    roundRectangle = new MC.Shapes.RoundRectangle();
                    NativeControl.StrokeShape = roundRectangle;
                }

                roundRectangle.CornerRadius = CornerRadius ?? (CornerRadius)MC.Shapes.RoundRectangle.CornerRadiusProperty.DefaultValue;
            }
            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }
}
