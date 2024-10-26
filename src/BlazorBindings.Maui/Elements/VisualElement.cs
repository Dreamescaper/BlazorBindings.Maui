using Microsoft.Maui.Graphics;

namespace BlazorBindings.Maui.Elements;

public abstract partial class VisualElement
{
    // This property is defined manually to allow to override it's behavior (see Shell).
    /// <summary>
    /// Gets or sets the color which will fill the background of a VisualElement.
    /// </summary>
    [Parameter] public Color BackgroundColor { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(BackgroundColor))
        {
            if (!Equals(BackgroundColor, value))
            {
                BackgroundColor = (Color)value;
                NativeControl.BackgroundColor = BackgroundColor;
            }
            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }
}
