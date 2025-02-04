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
using SMTC = Syncfusion.Maui.Toolkit.Charts;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.Charts
{
    /// <summary>
    /// The <see cref="T:Syncfusion.Maui.Toolkit.Charts.SplineRangeAreaSeries" /> is a set of data points represented by smooth bezier curves, with the area between the curves filled to illustrate the range between two values.
    /// </summary>
    public partial class SplineRangeAreaSeries : RangeSeriesBase
    {
        static SplineRangeAreaSeries()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the value indicating whether to show markers for the series data point.
        /// </summary>
        /// <value>
        /// It accepts <c>bool</c> values and its default value is false.
        /// </value>
        [Parameter] public bool? ShowMarkers { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates the shape of the spline range area series.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Syncfusion.Maui.Toolkit.Charts.SplineType" /> values and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.SplineType.Natural" />.
        /// </value>
        [Parameter] public SMTC.SplineType? Type { get; set; }
        /// <summary>
        /// Gets or sets the option for customize the series markers.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartMarkerSettings" />.
        /// </value>
        [Parameter] public RenderFragment MarkerSettings { get; set; }

        public new SMTC.SplineRangeAreaSeries NativeControl => (SMTC.SplineRangeAreaSeries)((BindableObject)this).NativeControl;

        protected override SMTC.SplineRangeAreaSeries CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ShowMarkers):
                    if (!Equals(ShowMarkers, value))
                    {
                        ShowMarkers = (bool?)value;
                        NativeControl.ShowMarkers = ShowMarkers ?? (bool)SMTC.SplineRangeAreaSeries.ShowMarkersProperty.DefaultValue;
                    }
                    break;
                case nameof(Type):
                    if (!Equals(Type, value))
                    {
                        Type = (SMTC.SplineType?)value;
                        NativeControl.Type = Type ?? (SMTC.SplineType)SMTC.SplineRangeAreaSeries.TypeProperty.DefaultValue;
                    }
                    break;
                case nameof(MarkerSettings):
                    MarkerSettings = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.SplineRangeAreaSeries>(builder, sequence++, MarkerSettings, (x, value) => x.MarkerSettings = (SMTC.ChartMarkerSettings)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
