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
using System;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.Charts
{
    /// <summary>
    /// DateTimeAxis is used to plot <b> DateTime </b> values. The date-time axis uses date time scale and displays date-time values as axis labels in the specified format.
    /// </summary>
    public partial class DateTimeAxis : RangeAxisBase
    {
        static DateTimeAxis()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the date time unit of the <see cref="P:Syncfusion.Maui.Toolkit.Charts.ChartAxis.AutoScrollingDelta" /> value. The value can be set to years, months, days, hours, minutes, seconds and auto.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Syncfusion.Maui.Toolkit.Charts.DateTimeIntervalType" /> values, and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.DateTimeIntervalType.Auto" />, which means that the date time unit of the delta value will be automatically calculated based on the data points.
        /// </value>
        [Parameter] public SMTC.DateTimeIntervalType? AutoScrollingDeltaType { get; set; }
        /// <summary>
        /// Gets or sets a value that can be used to change the interval between labels.
        /// </summary>
        /// <value>
        /// It accepts <c>double</c> values and the default value is double.NaN.
        /// </value>
        [Parameter] public double? Interval { get; set; }
        /// <summary>
        /// Gets or sets the date time unit of the value specified in the <see cref="P:Syncfusion.Maui.Toolkit.Charts.DateTimeAxis.Interval" /> property.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Syncfusion.Maui.Toolkit.Charts.DateTimeIntervalType" /> value and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.DateTimeIntervalType.Auto" />.
        /// </value>
        [Parameter] public SMTC.DateTimeIntervalType? IntervalType { get; set; }
        /// <summary>
        /// Gets or sets the maximum value of the time period to be displayed on the chart axis.
        /// </summary>
        /// <value>
        /// It accepts DateTime values and the default value is null.
        /// </value>
        [Parameter] public Nullable<DateTime> Maximum { get; set; }
        /// <summary>
        /// Gets or sets the minimum value of the time period to be displayed on the chart axis.
        /// </summary>
        /// <value>
        /// It accepts DateTime values and the default value is null.
        /// </value>
        [Parameter] public Nullable<DateTime> Minimum { get; set; }
        /// <summary>
        /// Gets or sets a padding type for the date time axis range.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Syncfusion.Maui.Toolkit.Charts.DateTimeRangePadding" /> value and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.DateTimeRangePadding.Auto" />.
        /// </value>
        [Parameter] public SMTC.DateTimeRangePadding? RangePadding { get; set; }
        /// <summary>
        /// Gets or sets the collection of plot bands to be added to the chart axis.
        /// </summary>
        /// <remarks>
        /// Accepts one or more DateTimePlotBand elements.
        /// </remarks>
        [Parameter] public RenderFragment PlotBands { get; set; }

        public new SMTC.DateTimeAxis NativeControl => (SMTC.DateTimeAxis)((BindableObject)this).NativeControl;

        protected override SMTC.DateTimeAxis CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(AutoScrollingDeltaType):
                    if (!Equals(AutoScrollingDeltaType, value))
                    {
                        AutoScrollingDeltaType = (SMTC.DateTimeIntervalType?)value;
                        NativeControl.AutoScrollingDeltaType = AutoScrollingDeltaType ?? (SMTC.DateTimeIntervalType)SMTC.DateTimeAxis.AutoScrollingDeltaTypeProperty.DefaultValue;
                    }
                    break;
                case nameof(Interval):
                    if (!Equals(Interval, value))
                    {
                        Interval = (double?)value;
                        NativeControl.Interval = Interval ?? (double)SMTC.DateTimeAxis.IntervalProperty.DefaultValue;
                    }
                    break;
                case nameof(IntervalType):
                    if (!Equals(IntervalType, value))
                    {
                        IntervalType = (SMTC.DateTimeIntervalType?)value;
                        NativeControl.IntervalType = IntervalType ?? (SMTC.DateTimeIntervalType)SMTC.DateTimeAxis.IntervalTypeProperty.DefaultValue;
                    }
                    break;
                case nameof(Maximum):
                    if (!Equals(Maximum, value))
                    {
                        Maximum = (Nullable<DateTime>)value;
                        NativeControl.Maximum = Maximum;
                    }
                    break;
                case nameof(Minimum):
                    if (!Equals(Minimum, value))
                    {
                        Minimum = (Nullable<DateTime>)value;
                        NativeControl.Minimum = Minimum;
                    }
                    break;
                case nameof(RangePadding):
                    if (!Equals(RangePadding, value))
                    {
                        RangePadding = (SMTC.DateTimeRangePadding?)value;
                        NativeControl.RangePadding = RangePadding ?? (SMTC.DateTimeRangePadding)SMTC.DateTimeAxis.RangePaddingProperty.DefaultValue;
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
            RenderTreeBuilderHelper.AddListContentProperty<SMTC.DateTimeAxis, SMTC.DateTimePlotBand>(builder, sequence++, PlotBands, x => x.PlotBands);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
