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
    /// Provides the pyramid chart with a unique style of data representation that is more UI-visualising and user-friendly.
    /// </summary>
    public partial class SfPyramidChart : ChartBase
    {
        static SfPyramidChart()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a Boolean value indicating whether the tooltip for chart should be shown or hidden.
        /// </summary>
        /// <value>
        /// This property takes the <c>bool</c> as its values and its default value is <c>False</c>.
        /// </value>
        [Parameter] public bool? EnableTooltip { get; set; }
        /// <summary>
        /// Gets or sets the ratio of the distance between the chart segments.
        /// </summary>
        /// <value>
        /// Its default value is 0. Its value ranges from 0 to 1.
        /// </value>
        [Parameter] public double? GapRatio { get; set; }
        /// <summary>
        /// Gets or sets a data points collection that will be used to plot a chart.
        /// </summary>
        /// <value>
        /// It accepts the data points collections and its default value is null.
        /// </value>
        [Parameter] public object ItemsSource { get; set; }
        /// <summary>
        /// Gets or sets a legend icon that will be displayed in the associated legend item.
        /// </summary>
        /// <value>
        /// This property takes the list of <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartLegendIconType" /> and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.ChartLegendIconType.Circle" />.
        /// </value>
        [Parameter] public SMTC.ChartLegendIconType? LegendIcon { get; set; }
        /// <summary>
        /// Gets or sets mode value which indicates the pyramid rendering.
        /// </summary>
        [Parameter] public SMTC.PyramidMode? Mode { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates to enable the data labels for the chart.
        /// </summary>
        /// <value>
        /// This property takes the <c>bool</c> values and its default value is False.
        /// </value>
        [Parameter] public bool? ShowDataLabels { get; set; }
        /// <summary>
        /// Gets or sets the color used to paint the pyramid segments' outline.
        /// </summary>
        /// <value>
        /// This property takes the <see cref="T:Microsoft.Maui.Controls.Brush" /> values and its default value is <c>Transparent</c>.
        /// </value>
        [Parameter] public Color StrokeColor { get; set; }
        /// <summary>
        /// Gets or sets a value to specify the width of the stroke drawn.
        /// </summary>
        /// <value>
        /// This property takes the <see cref="T:System.Double" /> values and its default value is 2.
        /// </value>
        [Parameter] public double? StrokeWidth { get; set; }
        /// <summary>
        /// Gets or sets a path value on the source object to serve a x value to the chart.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the x plotting data, and its default value is null.
        /// </value>
        [Parameter] public string XBindingPath { get; set; }
        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the chart.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        [Parameter] public string YBindingPath { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the chart.
        /// </summary>
        /// <value>
        /// It takes the <see cref="T:Syncfusion.Maui.Toolkit.Charts.PyramidDataLabelSettings" />.
        /// </value>
        /// <remarks>
        /// Accepts single PyramidDataLabelSettings element.
        /// </remarks>
        [Parameter] public RenderFragment DataLabelSettings { get; set; }
        /// <summary>
        /// Gets or sets the DataTemplate that can be used to customize the appearance of the Data label.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Microsoft.Maui.Controls.DataTemplate" /> value.
        /// </value>
        [Parameter] public RenderFragment LabelTemplate { get; set; }
        /// <summary>
        /// Gets or sets the list of brushes that can be used to customize the appearance of the chart.
        /// </summary>
        /// <value>
        /// This property accepts a list of brushes as input and comes with a set of predefined brushes by default.
        /// </value>
        /// <remarks>
        /// Accepts one or more Brush elements.
        /// </remarks>
        [Parameter] public RenderFragment PaletteBrushes { get; set; }
        /// <summary>
        /// Gets or sets a value for initiating selection or highlighting of a single or multiple data points in the chart.
        /// </summary>
        /// <value>
        /// This property takes a <see cref="T:Syncfusion.Maui.Toolkit.Charts.DataPointSelectionBehavior" /> instance as a value, and its default value is null.
        /// </value>
        /// <remarks>
        /// Accepts single DataPointSelectionBehavior element.
        /// </remarks>
        [Parameter] public RenderFragment SelectionBehavior { get; set; }
        /// <summary>
        /// Gets or sets the color used to paint the pyramid segments' outline.
        /// </summary>
        /// <value>
        /// This property takes the <see cref="T:Microsoft.Maui.Controls.Brush" /> values and its default value is <c>Transparent</c>.
        /// </value>
        /// <remarks>
        /// Accepts single Brush element.
        /// </remarks>
        [Parameter] public RenderFragment Stroke { get; set; }
        /// <summary>
        /// Gets or sets the DataTemplate that can be used to customize the appearance of the tooltip.
        /// </summary>
        /// <value>
        /// It accepts a <see cref="T:Microsoft.Maui.Controls.DataTemplate" /> value. and its default value is null.
        /// </value>
        [Parameter] public RenderFragment TooltipTemplate { get; set; }

        public new SMTC.SfPyramidChart NativeControl => (SMTC.SfPyramidChart)((BindableObject)this).NativeControl;

        protected override SMTC.SfPyramidChart CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(EnableTooltip):
                    if (!Equals(EnableTooltip, value))
                    {
                        EnableTooltip = (bool?)value;
                        NativeControl.EnableTooltip = EnableTooltip ?? (bool)SMTC.SfPyramidChart.EnableTooltipProperty.DefaultValue;
                    }
                    break;
                case nameof(GapRatio):
                    if (!Equals(GapRatio, value))
                    {
                        GapRatio = (double?)value;
                        NativeControl.GapRatio = GapRatio ?? (double)SMTC.SfPyramidChart.GapRatioProperty.DefaultValue;
                    }
                    break;
                case nameof(ItemsSource):
                    if (!Equals(ItemsSource, value))
                    {
                        ItemsSource = (object)value;
                        NativeControl.ItemsSource = ItemsSource;
                    }
                    break;
                case nameof(LegendIcon):
                    if (!Equals(LegendIcon, value))
                    {
                        LegendIcon = (SMTC.ChartLegendIconType?)value;
                        NativeControl.LegendIcon = LegendIcon ?? (SMTC.ChartLegendIconType)SMTC.SfPyramidChart.LegendIconProperty.DefaultValue;
                    }
                    break;
                case nameof(Mode):
                    if (!Equals(Mode, value))
                    {
                        Mode = (SMTC.PyramidMode?)value;
                        NativeControl.Mode = Mode ?? (SMTC.PyramidMode)SMTC.SfPyramidChart.ModeProperty.DefaultValue;
                    }
                    break;
                case nameof(ShowDataLabels):
                    if (!Equals(ShowDataLabels, value))
                    {
                        ShowDataLabels = (bool?)value;
                        NativeControl.ShowDataLabels = ShowDataLabels ?? (bool)SMTC.SfPyramidChart.ShowDataLabelsProperty.DefaultValue;
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
                        NativeControl.StrokeWidth = StrokeWidth ?? (double)SMTC.SfPyramidChart.StrokeWidthProperty.DefaultValue;
                    }
                    break;
                case nameof(XBindingPath):
                    if (!Equals(XBindingPath, value))
                    {
                        XBindingPath = (string)value;
                        NativeControl.XBindingPath = XBindingPath;
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
                case nameof(LabelTemplate):
                    LabelTemplate = (RenderFragment)value;
                    break;
                case nameof(PaletteBrushes):
                    PaletteBrushes = (RenderFragment)value;
                    break;
                case nameof(SelectionBehavior):
                    SelectionBehavior = (RenderFragment)value;
                    break;
                case nameof(Stroke):
                    Stroke = (RenderFragment)value;
                    break;
                case nameof(TooltipTemplate):
                    TooltipTemplate = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.SfPyramidChart>(builder, sequence++, DataLabelSettings, (x, value) => x.DataLabelSettings = (SMTC.PyramidDataLabelSettings)value);
            RenderTreeBuilderHelper.AddDataTemplateProperty<SMTC.SfPyramidChart>(builder, sequence++, LabelTemplate, (x, template) => x.LabelTemplate = template);
            RenderTreeBuilderHelper.AddListContentProperty<SMTC.SfPyramidChart, MC.Brush>(builder, sequence++, PaletteBrushes, x => x.PaletteBrushes);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.SfPyramidChart>(builder, sequence++, SelectionBehavior, (x, value) => x.SelectionBehavior = (SMTC.DataPointSelectionBehavior)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.SfPyramidChart>(builder, sequence++, Stroke, (x, value) => x.Stroke = (MC.Brush)value);
            RenderTreeBuilderHelper.AddDataTemplateProperty<SMTC.SfPyramidChart>(builder, sequence++, TooltipTemplate, (x, template) => x.TooltipTemplate = template);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
