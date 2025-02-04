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
    /// The CategoryAxis is an indexed based axis that plots values based on the index of the data point collection. It displays string values in axis labels.
    /// </summary>
    public partial class CategoryAxis : ChartAxis
    {
        static CategoryAxis()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a value that determines whether to arrange the axis labels by index or by value.
        /// </summary>
        /// <value>
        /// This property takes the <c>bool</c> as its value and its default is <c>True</c>.
        /// </value>
        [Parameter] public bool? ArrangeByIndex { get; set; }
        /// <summary>
        /// Gets or sets a value that can be used to customize the interval between the axis labels.
        /// </summary>
        /// <value>
        /// It accepts double values and the default value is double.NaN.
        /// </value>
        [Parameter] public double? Interval { get; set; }
        /// <summary>
        /// Gets or sets a value that determines whether to place the axis label in between or on the tick lines.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Syncfusion.Maui.Toolkit.Charts.LabelPlacement" /> values and the default value is <c>OnTicks</c>.
        /// </value>
        [Parameter] public SMTC.LabelPlacement? LabelPlacement { get; set; }
        /// <summary>
        /// Gets or sets the collection of plot bands to be added to the chart axis.
        /// </summary>
        [Parameter] public RenderFragment PlotBands { get; set; }

        public new SMTC.CategoryAxis NativeControl => (SMTC.CategoryAxis)((BindableObject)this).NativeControl;

        protected override SMTC.CategoryAxis CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ArrangeByIndex):
                    if (!Equals(ArrangeByIndex, value))
                    {
                        ArrangeByIndex = (bool?)value;
                        NativeControl.ArrangeByIndex = ArrangeByIndex ?? (bool)SMTC.CategoryAxis.ArrangeByIndexProperty.DefaultValue;
                    }
                    break;
                case nameof(Interval):
                    if (!Equals(Interval, value))
                    {
                        Interval = (double?)value;
                        NativeControl.Interval = Interval ?? (double)SMTC.CategoryAxis.IntervalProperty.DefaultValue;
                    }
                    break;
                case nameof(LabelPlacement):
                    if (!Equals(LabelPlacement, value))
                    {
                        LabelPlacement = (SMTC.LabelPlacement?)value;
                        NativeControl.LabelPlacement = LabelPlacement ?? (SMTC.LabelPlacement)SMTC.CategoryAxis.LabelPlacementProperty.DefaultValue;
                    }
                    break;
                case nameof(PlotBands):
                    PlotBands = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddListContentProperty<SMTC.CategoryAxis, SMTC.NumericalPlotBand>(builder, sequence++, PlotBands, x => x.PlotBands);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
