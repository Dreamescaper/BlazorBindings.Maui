// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    /// <summary>
    /// A <see cref="T:Microsoft.Maui.Controls.View" /> control that provides a toggled value.
    /// </summary>
    public partial class Switch : View
    {
        static Switch()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether this <see cref="T:Microsoft.Maui.Controls.Switch" /> element is toggled.
        /// </summary>
        [Parameter] public bool? IsToggled { get; set; }
        /// <summary>
        /// Gets or sets the color of the switch when it is in the "On" position.
        /// </summary>
        /// <value>
        /// The color of the switch when it is in the "On" position.
        /// </value>
        [Parameter] public Color OnColor { get; set; }
        [Parameter] public Color ThumbColor { get; set; }
        [Parameter] public EventCallback<bool> IsToggledChanged { get; set; }

        public new MC.Switch NativeControl => (MC.Switch)((BindableObject)this).NativeControl;

        protected override MC.Switch CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(IsToggled):
                    if (!Equals(IsToggled, value))
                    {
                        IsToggled = (bool?)value;
                        NativeControl.IsToggled = IsToggled ?? (bool)MC.Switch.IsToggledProperty.DefaultValue;
                    }
                    break;
                case nameof(OnColor):
                    if (!Equals(OnColor, value))
                    {
                        OnColor = (Color)value;
                        NativeControl.OnColor = OnColor;
                    }
                    break;
                case nameof(ThumbColor):
                    if (!Equals(ThumbColor, value))
                    {
                        ThumbColor = (Color)value;
                        NativeControl.ThumbColor = ThumbColor;
                    }
                    break;
                case nameof(IsToggledChanged):
                    if (!Equals(IsToggledChanged, value))
                    {
                        void NativeControlToggled(object sender, MC.ToggledEventArgs e)
                        {
                            var value = NativeControl.IsToggled;
                            IsToggled = value;
                            InvokeEventCallback(IsToggledChanged, value);
                        }

                        IsToggledChanged = (EventCallback<bool>)value;
                        NativeControl.Toggled -= NativeControlToggled;
                        NativeControl.Toggled += NativeControlToggled;
                    }
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        static partial void RegisterAdditionalHandlers();
    }
}
