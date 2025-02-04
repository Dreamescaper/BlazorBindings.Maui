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
    /// The <see cref="T:Syncfusion.Maui.Toolkit.Charts.CandleSeries" /> displays a set of candle used in financial analysis to represent open, high, low, and close values of an asset or security.
    /// </summary>
    public partial class CandleSeries : HiLoOpenCloseSeries
    {
        static CandleSeries()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a value indicating a whether enable solid candles.
        /// </summary>
        /// <value>
        /// It accepts the <c>bool</c> values and its default value is <c>false</c>.
        /// </value>
        [Parameter] public bool? EnableSolidCandle { get; set; }
        /// <summary>
        /// Gets or sets the stroke to the candle data point.
        /// </summary>
        /// <value>
        /// It accept the <see cref="T:Microsoft.Maui.Controls.Brush" /> values and its default value is null
        /// </value>
        [Parameter] public Color StrokeColor { get; set; }
        /// <summary>
        /// Gets or sets the stroke to the candle data point.
        /// </summary>
        /// <value>
        /// It accept the <see cref="T:Microsoft.Maui.Controls.Brush" /> values and its default value is null
        /// </value>
        [Parameter] public RenderFragment Stroke { get; set; }

        public new SMTC.CandleSeries NativeControl => (SMTC.CandleSeries)((BindableObject)this).NativeControl;

        protected override SMTC.CandleSeries CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(EnableSolidCandle):
                    if (!Equals(EnableSolidCandle, value))
                    {
                        EnableSolidCandle = (bool?)value;
                        NativeControl.EnableSolidCandle = EnableSolidCandle ?? (bool)SMTC.CandleSeries.EnableSolidCandleProperty.DefaultValue;
                    }
                    break;
                case nameof(StrokeColor):
                    if (!Equals(StrokeColor, value))
                    {
                        StrokeColor = (Color)value;
                        NativeControl.Stroke = StrokeColor;
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
            RenderTreeBuilderHelper.AddContentProperty<SMTC.CandleSeries>(builder, sequence++, Stroke, (x, value) => x.Stroke = (MC.Brush)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
