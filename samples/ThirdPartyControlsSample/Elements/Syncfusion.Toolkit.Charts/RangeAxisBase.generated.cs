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
    /// The <see cref="T:Syncfusion.Maui.Toolkit.Charts.RangeAxisBase" /> is the base class for all types of range axis.
    /// </summary>
    public abstract partial class RangeAxisBase : ChartAxis
    {
        static RangeAxisBase()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the visibility mode of the edge labels for the axis, allowing options to hide the labels when zooming or to keep them visible.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Syncfusion.Maui.Toolkit.Charts.EdgeLabelsVisibilityMode" /> values and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.EdgeLabelsVisibilityMode.Default" />.
        /// </value>
        [Parameter] public SMTC.EdgeLabelsVisibilityMode? EdgeLabelsVisibilityMode { get; set; }
        /// <summary>
        /// Gets or sets the value that defines the number of minor tick/grid lines to be drawn between the adjacent major tick/grid lines.
        /// </summary>
        /// <value>
        /// It accepts the <c>integer</c> values and its default value is 0.
        /// </value>
        [Parameter] public int? MinorTicksPerInterval { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the axis minor grid lines can be displayed or not.
        /// </summary>
        /// <value>
        /// It accepts the bool value and its default value is <c>True</c>.
        /// </value>
        [Parameter] public bool? ShowMinorGridLines { get; set; }
        /// <summary>
        /// Gets or sets the <b> ChartLineStyle </b> to customize the appearance of the minor grid lines.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartLineStyle" /> value.
        /// </value>
        /// <remarks>
        /// Accepts single ChartLineStyle element.
        /// </remarks>
        [Parameter] public RenderFragment MinorGridLineStyle { get; set; }
        /// <summary>
        /// Gets or sets the <b> ChartAxisTickStyle </b> to customize the appearance of the minor tick lines.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartAxisTickStyle" /> value.
        /// </value>
        /// <remarks>
        /// Accepts single ChartAxisTickStyle element.
        /// </remarks>
        [Parameter] public RenderFragment MinorTickStyle { get; set; }

        public new SMTC.RangeAxisBase NativeControl => (SMTC.RangeAxisBase)((BindableObject)this).NativeControl;


        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(EdgeLabelsVisibilityMode):
                    if (!Equals(EdgeLabelsVisibilityMode, value))
                    {
                        EdgeLabelsVisibilityMode = (SMTC.EdgeLabelsVisibilityMode?)value;
                        NativeControl.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode ?? (SMTC.EdgeLabelsVisibilityMode)SMTC.RangeAxisBase.EdgeLabelsVisibilityModeProperty.DefaultValue;
                    }
                    break;
                case nameof(MinorTicksPerInterval):
                    if (!Equals(MinorTicksPerInterval, value))
                    {
                        MinorTicksPerInterval = (int?)value;
                        NativeControl.MinorTicksPerInterval = MinorTicksPerInterval ?? (int)SMTC.RangeAxisBase.MinorTicksPerIntervalProperty.DefaultValue;
                    }
                    break;
                case nameof(ShowMinorGridLines):
                    if (!Equals(ShowMinorGridLines, value))
                    {
                        ShowMinorGridLines = (bool?)value;
                        NativeControl.ShowMinorGridLines = ShowMinorGridLines ?? (bool)SMTC.RangeAxisBase.ShowMinorGridLinesProperty.DefaultValue;
                    }
                    break;
                case nameof(MinorGridLineStyle):
                    MinorGridLineStyle = (RenderFragment)value;
                    break;
                case nameof(MinorTickStyle):
                    MinorTickStyle = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.RangeAxisBase>(builder, sequence++, MinorGridLineStyle, (x, value) => x.MinorGridLineStyle = (SMTC.ChartLineStyle)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.RangeAxisBase>(builder, sequence++, MinorTickStyle, (x, value) => x.MinorTickStyle = (SMTC.ChartAxisTickStyle)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
