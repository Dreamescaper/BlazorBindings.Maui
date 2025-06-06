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
    /// Represents the base class for all circular chart series, such as <see cref="T:Syncfusion.Maui.Toolkit.Charts.PieSeries" />, <see cref="T:Syncfusion.Maui.Toolkit.Charts.DoughnutSeries" />, and <see cref="T:Syncfusion.Maui.Toolkit.Charts.RadialBarSeries" /> series.
    /// </summary>
    public abstract partial class CircularSeries : ChartSeries
    {
        static CircularSeries()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a value that can be used to modify the series end rendering position.
        /// </summary>
        /// <value>
        /// It accepts <c>double</c> values, and the default value is 360.
        /// </value>
        [Parameter] public double? EndAngle { get; set; }
        /// <summary>
        /// Gets or sets a value that can be used to render the series size.
        /// </summary>
        /// <value>
        /// It accepts <c>double</c> values, and the default value is 0.8. Here, the value is between 0 and 1.
        /// </value>
        [Parameter] public double? Radius { get; set; }
        /// <summary>
        /// Gets or sets a value that can be used to modify the series start rendering position.
        /// </summary>
        /// <value>
        /// It accepts <c>double</c> values, and the default value is 0.
        /// </value>
        [Parameter] public double? StartAngle { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of the series.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Controls.Brush" /> values and its default value is <c>Transparent</c>.
        /// </value>
        [Parameter] public Color StrokeColor { get; set; }
        /// <summary>
        /// Gets or sets a value to specify the stroke width of a chart series.
        /// </summary>
        /// <value>
        /// It accepts <c>double</c> values and its default value is 2.
        /// </value>
        [Parameter] public double? StrokeWidth { get; set; }
        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value>
        /// The <c>string</c> that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        [Parameter] public string YBindingPath { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the circular series.
        /// </summary>
        /// <value>
        /// This property takes the <see cref="T:Syncfusion.Maui.Toolkit.Charts.CircularDataLabelSettings" />.
        /// </value>
        /// <remarks>
        /// Accepts single CircularDataLabelSettings element.
        /// </remarks>
        [Parameter] public RenderFragment DataLabelSettings { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of the series.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Controls.Brush" /> values and its default value is <c>Transparent</c>.
        /// </value>
        /// <remarks>
        /// Accepts single Brush element.
        /// </remarks>
        [Parameter] public RenderFragment Stroke { get; set; }

        public new SMTC.CircularSeries NativeControl => (SMTC.CircularSeries)((BindableObject)this).NativeControl;


        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(EndAngle):
                    if (!Equals(EndAngle, value))
                    {
                        EndAngle = (double?)value;
                        NativeControl.EndAngle = EndAngle ?? (double)SMTC.CircularSeries.EndAngleProperty.DefaultValue;
                    }
                    break;
                case nameof(Radius):
                    if (!Equals(Radius, value))
                    {
                        Radius = (double?)value;
                        NativeControl.Radius = Radius ?? (double)SMTC.CircularSeries.RadiusProperty.DefaultValue;
                    }
                    break;
                case nameof(StartAngle):
                    if (!Equals(StartAngle, value))
                    {
                        StartAngle = (double?)value;
                        NativeControl.StartAngle = StartAngle ?? (double)SMTC.CircularSeries.StartAngleProperty.DefaultValue;
                    }
                    break;
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
                        NativeControl.StrokeWidth = StrokeWidth ?? (double)SMTC.CircularSeries.StrokeWidthProperty.DefaultValue;
                    }
                    break;
                case nameof(YBindingPath):
                    if (!Equals(YBindingPath, value))
                    {
                        YBindingPath = (string)value;
                        NativeControl.YBindingPath = YBindingPath;
                    }
                    break;
                case nameof(DataLabelSettings):
                    DataLabelSettings = (RenderFragment)value;
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
            RenderTreeBuilderHelper.AddContentProperty<SMTC.CircularSeries>(builder, sequence++, DataLabelSettings, (x, value) => x.DataLabelSettings = (SMTC.CircularDataLabelSettings)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.CircularSeries>(builder, sequence++, Stroke, (x, value) => x.Stroke = (MC.Brush)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
