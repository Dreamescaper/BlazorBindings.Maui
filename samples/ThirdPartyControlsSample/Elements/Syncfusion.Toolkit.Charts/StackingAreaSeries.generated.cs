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
    /// The <see cref="T:Syncfusion.Maui.Toolkit.Charts.StackingAreaSeries" /> is a collection of data points, where the areas are stacked on top of each other.
    /// </summary>
    public partial class StackingAreaSeries : StackingSeriesBase
    {
        static StackingAreaSeries()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the value indicating whether to show markers for the series data point.
        /// </summary>
        /// <value>
        /// It accepts <c>bool</c>&gt; values and its default value is false.
        /// </value>
        [Parameter] public bool? ShowMarkers { get; set; }
        /// <summary>
        /// Gets or sets the option to customize the series markers.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartMarkerSettings" />.
        /// </value>
        [Parameter] public RenderFragment MarkerSettings { get; set; }

        public new SMTC.StackingAreaSeries NativeControl => (SMTC.StackingAreaSeries)((BindableObject)this).NativeControl;

        protected override SMTC.StackingAreaSeries CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ShowMarkers):
                    if (!Equals(ShowMarkers, value))
                    {
                        ShowMarkers = (bool?)value;
                        NativeControl.ShowMarkers = ShowMarkers ?? (bool)SMTC.StackingAreaSeries.ShowMarkersProperty.DefaultValue;
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
            RenderTreeBuilderHelper.AddContentProperty<SMTC.StackingAreaSeries>(builder, sequence++, MarkerSettings, (x, value) => x.MarkerSettings = (SMTC.ChartMarkerSettings)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
