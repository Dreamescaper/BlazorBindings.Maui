// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using BlazorBindings.Maui.Elements;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Maui.Graphics;
using SMTC = Syncfusion.Maui.Toolkit.Charts;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.Charts
{
    /// <summary>
    /// Represents a style class that can be used to customize the axis's tick.
    /// </summary>
    public partial class ChartAxisTickStyle : BlazorBindings.Maui.Elements.Element
    {
        static ChartAxisTickStyle()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a value to customize the stroke color of the axis's tick.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Controls.Brush" /> values.
        /// </value>
        [Parameter] public Color StrokeColor { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates the width of the axis's tick.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:System.Double" /> values and the default value is 1.
        /// </value>
        [Parameter] public double? StrokeWidth { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates the length of the axis's tick.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:System.Double" /> values and the default value is 8.
        /// </value>
        [Parameter] public double? TickSize { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the stroke color of the axis's tick.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Controls.Brush" /> values.
        /// </value>
        /// <remarks>
        /// Accepts single Brush element.
        /// </remarks>
        [Parameter] public RenderFragment Stroke { get; set; }

        public new SMTC.ChartAxisTickStyle NativeControl => (SMTC.ChartAxisTickStyle)((BindableObject)this).NativeControl;

        protected override SMTC.ChartAxisTickStyle CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(StrokeColor):
                    if (!Equals(StrokeColor, value))
                    {
                        StrokeColor = (Color)value;
                        NativeControl.Stroke = StrokeColor;
                    }
                    break;
                case nameof(StrokeWidth):
                    if (!Equals(StrokeWidth, value))
                    {
                        StrokeWidth = (double?)value;
                        NativeControl.StrokeWidth = StrokeWidth ?? (double)SMTC.ChartAxisTickStyle.StrokeWidthProperty.DefaultValue;
                    }
                    break;
                case nameof(TickSize):
                    if (!Equals(TickSize, value))
                    {
                        TickSize = (double?)value;
                        NativeControl.TickSize = TickSize ?? (double)SMTC.ChartAxisTickStyle.TickSizeProperty.DefaultValue;
                    }
                    break;
                case nameof(Stroke):
                    Stroke = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.ChartAxisTickStyle>(builder, sequence++, Stroke, (x, value) => x.Stroke = (MC.Brush)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
