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
    /// Represents the base class for all chart series types, including <see cref="T:Syncfusion.Maui.Toolkit.Charts.SfCartesianChart" />, <see cref="T:Syncfusion.Maui.Toolkit.Charts.SfCircularChart" />, and <see cref="T:Syncfusion.Maui.Toolkit.Charts.SfPolarChart" />.
    /// </summary>
    public abstract partial class ChartSeries : BlazorBindings.Maui.Elements.Element
    {
        static ChartSeries()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a value indicating whether to animate the chart series on loading.
        /// </summary>
        /// <value>
        /// It accepts <c>bool</c> values and its default value is <c>False</c>.
        /// </value>
        [Parameter] public bool? EnableAnimation { get; set; }
        /// <summary>
        /// Gets or sets a boolean value indicating whether the tooltip for series should be shown or hidden.
        /// </summary>
        /// <value>
        /// It accepts <c>bool</c> values and its default value is <c>False</c>.
        /// </value>
        [Parameter] public bool? EnableTooltip { get; set; }
        /// <summary>
        /// Gets or sets a brush value to customize the series appearance.
        /// </summary>
        /// <value>
        /// It accepts a <see cref="T:Microsoft.Maui.Controls.Brush" /> value and its default value is null.
        /// </value>
        [Parameter] public Color FillColor { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates whether the series is visible or not.
        /// </summary>
        /// <value>
        /// It accepts <c>bool</c> values and its default value is <c>True</c>.
        /// </value>
        [Parameter] public bool? IsVisible { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates whether to show a legend item for this series.
        /// </summary>
        /// <value>
        /// It accepts <c>bool</c> values and its default value is <c>True</c>.
        /// </value>
        [Parameter] public bool? IsVisibleOnLegend { get; set; }
        /// <summary>
        /// Gets or sets a data points collection that will be used to plot a chart.
        /// </summary>
        [Parameter] public object ItemsSource { get; set; }
        /// <summary>
        /// Gets or sets an option that determines the content to be displayed in the data labels. It is recommended to use PieSeries, DoughnutSeries, and BarSeries with LabelContext set to Percentage.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Syncfusion.Maui.Toolkit.Charts.LabelContext" /> values and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.LabelContext.YValue" />.
        /// </value>
        [Parameter] public SMTC.LabelContext? LabelContext { get; set; }
        /// <summary>
        /// Gets or sets a legend icon that will be displayed in the associated legend item.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartLegendIconType" /> values and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.ChartLegendIconType.Circle" />.
        /// </value>
        [Parameter] public SMTC.ChartLegendIconType? LegendIcon { get; set; }
        /// <summary>
        /// Gets or sets opacity of the chart series.
        /// </summary>
        /// <value>
        /// Accepts <c>double</c> values ranging from 0 to 1, where 0 represents fully transparent, 1 represents fully opaque, and intermediate values provide partial transparency. The default value is 1.
        /// </value>
        [Parameter] public double? Opacity { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates to enable the data labels for the series..
        /// </summary>
        /// <value>
        /// It accepts <c>bool</c> values and the default value is <c>False</c>.
        /// </value>
        [Parameter] public bool? ShowDataLabels { get; set; }
        /// <summary>
        /// Gets or sets a path value on the source object to serve a x value to the series.
        /// </summary>
        /// <value>
        /// The <c>string</c> that represents the property name for the x plotting data, and its default value is null.
        /// </value>
        [Parameter] public string XBindingPath { get; set; }
        /// <summary>
        /// Gets or sets a brush value to customize the series appearance.
        /// </summary>
        /// <value>
        /// It accepts a <see cref="T:Microsoft.Maui.Controls.Brush" /> value and its default value is null.
        /// </value>
        [Parameter] public RenderFragment Fill { get; set; }
        /// <summary>
        /// Gets or sets the <b> DataTemplate </b> that can be used to customize the appearance of the data label.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Microsoft.Maui.Controls.DataTemplate" /> values.
        /// </value>
        [Parameter] public RenderFragment LabelTemplate { get; set; }
        /// <summary>
        /// Gets or sets the list of brushes that can be used to customize the appearance of the series.
        /// </summary>
        /// <value>
        /// This property accepts a list of brushes as input and comes with a set of predefined brushes by default.
        /// </value>
        [Parameter] public RenderFragment PaletteBrushes { get; set; }
        /// <summary>
        /// Gets or sets a value for initiating selection or highlighting of a single or multiple data points in the series.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Syncfusion.Maui.Toolkit.Charts.DataPointSelectionBehavior" /> values and its default value is null
        /// </value>
        [Parameter] public RenderFragment SelectionBehavior { get; set; }
        /// <summary>
        /// Gets or sets the DataTemplate that can be used to customize the appearance of the tooltip.
        /// </summary>
        /// <value>
        /// It accepts a <see cref="T:Microsoft.Maui.Controls.DataTemplate" /> value.
        /// </value>
        [Parameter] public RenderFragment TooltipTemplate { get; set; }

        public new SMTC.ChartSeries NativeControl => (SMTC.ChartSeries)((BindableObject)this).NativeControl;


        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(EnableAnimation):
                    if (!Equals(EnableAnimation, value))
                    {
                        EnableAnimation = (bool?)value;
                        NativeControl.EnableAnimation = EnableAnimation ?? (bool)SMTC.ChartSeries.EnableAnimationProperty.DefaultValue;
                    }
                    break;
                case nameof(EnableTooltip):
                    if (!Equals(EnableTooltip, value))
                    {
                        EnableTooltip = (bool?)value;
                        NativeControl.EnableTooltip = EnableTooltip ?? (bool)SMTC.ChartSeries.EnableTooltipProperty.DefaultValue;
                    }
                    break;
                case nameof(FillColor):
                    if (!Equals(FillColor, value))
                    {
                        FillColor = (Color)value;
                        NativeControl.Fill = FillColor;
                    }
                    break;
                case nameof(IsVisible):
                    if (!Equals(IsVisible, value))
                    {
                        IsVisible = (bool?)value;
                        NativeControl.IsVisible = IsVisible ?? (bool)SMTC.ChartSeries.IsVisibleProperty.DefaultValue;
                    }
                    break;
                case nameof(IsVisibleOnLegend):
                    if (!Equals(IsVisibleOnLegend, value))
                    {
                        IsVisibleOnLegend = (bool?)value;
                        NativeControl.IsVisibleOnLegend = IsVisibleOnLegend ?? (bool)SMTC.ChartSeries.IsVisibleOnLegendProperty.DefaultValue;
                    }
                    break;
                case nameof(ItemsSource):
                    if (!Equals(ItemsSource, value))
                    {
                        ItemsSource = (object)value;
                        NativeControl.ItemsSource = ItemsSource;
                    }
                    break;
                case nameof(LabelContext):
                    if (!Equals(LabelContext, value))
                    {
                        LabelContext = (SMTC.LabelContext?)value;
                        NativeControl.LabelContext = LabelContext ?? (SMTC.LabelContext)SMTC.ChartSeries.LabelContextProperty.DefaultValue;
                    }
                    break;
                case nameof(LegendIcon):
                    if (!Equals(LegendIcon, value))
                    {
                        LegendIcon = (SMTC.ChartLegendIconType?)value;
                        NativeControl.LegendIcon = LegendIcon ?? (SMTC.ChartLegendIconType)SMTC.ChartSeries.LegendIconProperty.DefaultValue;
                    }
                    break;
                case nameof(Opacity):
                    if (!Equals(Opacity, value))
                    {
                        Opacity = (double?)value;
                        NativeControl.Opacity = Opacity ?? (double)SMTC.ChartSeries.OpacityProperty.DefaultValue;
                    }
                    break;
                case nameof(ShowDataLabels):
                    if (!Equals(ShowDataLabels, value))
                    {
                        ShowDataLabels = (bool?)value;
                        NativeControl.ShowDataLabels = ShowDataLabels ?? (bool)SMTC.ChartSeries.ShowDataLabelsProperty.DefaultValue;
                    }
                    break;
                case nameof(XBindingPath):
                    if (!Equals(XBindingPath, value))
                    {
                        XBindingPath = (string)value;
                        NativeControl.XBindingPath = XBindingPath;
                    }
                    break;
                case nameof(Fill):
                    Fill = (RenderFragment)value;
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
            RenderTreeBuilderHelper.AddContentProperty<SMTC.ChartSeries>(builder, sequence++, Fill, (x, value) => x.Fill = (MC.Brush)value);
            RenderTreeBuilderHelper.AddDataTemplateProperty<SMTC.ChartSeries>(builder, sequence++, LabelTemplate, (x, template) => x.LabelTemplate = template);
            RenderTreeBuilderHelper.AddListContentProperty<SMTC.ChartSeries, MC.Brush>(builder, sequence++, PaletteBrushes, x => x.PaletteBrushes);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.ChartSeries>(builder, sequence++, SelectionBehavior, (x, value) => x.SelectionBehavior = (SMTC.DataPointSelectionBehavior)value);
            RenderTreeBuilderHelper.AddDataTemplateProperty<SMTC.ChartSeries>(builder, sequence++, TooltipTemplate, (x, template) => x.TooltipTemplate = template);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
