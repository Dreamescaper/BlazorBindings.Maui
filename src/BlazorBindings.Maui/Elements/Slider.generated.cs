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
using System;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    /// <summary>
    /// A <see cref="T:Microsoft.Maui.Controls.View" /> control that inputs a linear value.
    /// </summary>
    public partial class Slider : View
    {
        static Slider()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the maximum selectable value for the Slider.
        /// </summary>
        /// <value>
        /// A double.
        /// </value>
        [Parameter] public double? Maximum { get; set; }
        /// <summary>
        /// Gets or sets the color of the portion of the slider track that contains the maximum value of the slider.
        /// </summary>
        /// <value>
        /// Thhe color of the portion of the slider track that contains the maximum value of the slider.
        /// </value>
        [Parameter] public Color MaximumTrackColor { get; set; }
        /// <summary>
        /// Gets or sets the minimum selectable value for the Slider.
        /// </summary>
        /// <value>
        /// A double.
        /// </value>
        [Parameter] public double? Minimum { get; set; }
        /// <summary>
        /// Gets or sets the color of the portion of the slider track that contains the minimum value of the slider.
        /// </summary>
        /// <value>
        /// Thhe color of the portion of the slider track that contains the minimum value of the slider.
        /// </value>
        [Parameter] public Color MinimumTrackColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the slider thumb button.
        /// </summary>
        /// <value>
        /// The color of the slider thumb button.
        /// </value>
        [Parameter] public Color ThumbColor { get; set; }
        [Parameter] public MC.ImageSource ThumbImageSource { get; set; }
        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        /// <value>
        /// A double.
        /// </value>
        [Parameter] public double? Value { get; set; }
        [Parameter] public EventCallback<double> ValueChanged { get; set; }
        [Parameter] public EventCallback OnDragStarted { get; set; }
        [Parameter] public EventCallback OnDragCompleted { get; set; }

        public new MC.Slider NativeControl => (MC.Slider)((BindableObject)this).NativeControl;

        protected override MC.Slider CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Maximum):
                    if (!Equals(Maximum, value))
                    {
                        Maximum = (double?)value;
                        NativeControl.Maximum = Maximum ?? (double)MC.Slider.MaximumProperty.DefaultValue;
                    }
                    break;
                case nameof(MaximumTrackColor):
                    if (!Equals(MaximumTrackColor, value))
                    {
                        MaximumTrackColor = (Color)value;
                        NativeControl.MaximumTrackColor = MaximumTrackColor;
                    }
                    break;
                case nameof(Minimum):
                    if (!Equals(Minimum, value))
                    {
                        Minimum = (double?)value;
                        NativeControl.Minimum = Minimum ?? (double)MC.Slider.MinimumProperty.DefaultValue;
                    }
                    break;
                case nameof(MinimumTrackColor):
                    if (!Equals(MinimumTrackColor, value))
                    {
                        MinimumTrackColor = (Color)value;
                        NativeControl.MinimumTrackColor = MinimumTrackColor;
                    }
                    break;
                case nameof(ThumbColor):
                    if (!Equals(ThumbColor, value))
                    {
                        ThumbColor = (Color)value;
                        NativeControl.ThumbColor = ThumbColor;
                    }
                    break;
                case nameof(ThumbImageSource):
                    if (!Equals(ThumbImageSource, value))
                    {
                        ThumbImageSource = (MC.ImageSource)value;
                        NativeControl.ThumbImageSource = ThumbImageSource;
                    }
                    break;
                case nameof(Value):
                    if (!Equals(Value, value))
                    {
                        Value = (double?)value;
                        NativeControl.Value = Value ?? (double)MC.Slider.ValueProperty.DefaultValue;
                    }
                    break;
                case nameof(ValueChanged):
                    if (!Equals(ValueChanged, value))
                    {
                        void NativeControlValueChanged(object sender, MC.ValueChangedEventArgs e)
                        {
                            var value = NativeControl.Value;
                            Value = value;
                            InvokeEventCallback(ValueChanged, value);
                        }

                        ValueChanged = (EventCallback<double>)value;
                        NativeControl.ValueChanged -= NativeControlValueChanged;
                        NativeControl.ValueChanged += NativeControlValueChanged;
                    }
                    break;
                case nameof(OnDragStarted):
                    if (!Equals(OnDragStarted, value))
                    {
                        void NativeControlDragStarted(object sender, EventArgs e) => InvokeEventCallback(OnDragStarted);

                        OnDragStarted = (EventCallback)value;
                        NativeControl.DragStarted -= NativeControlDragStarted;
                        NativeControl.DragStarted += NativeControlDragStarted;
                    }
                    break;
                case nameof(OnDragCompleted):
                    if (!Equals(OnDragCompleted, value))
                    {
                        void NativeControlDragCompleted(object sender, EventArgs e) => InvokeEventCallback(OnDragCompleted);

                        OnDragCompleted = (EventCallback)value;
                        NativeControl.DragCompleted -= NativeControlDragCompleted;
                        NativeControl.DragCompleted += NativeControlDragCompleted;
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
