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
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using SMTC = Syncfusion.Maui.Toolkit.Charts;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.Charts
{
    /// <summary>
    /// It is a base class for the <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartAxisLabelStyle" />, <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartAxisTitle" />, and <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartDataLabelStyle" /> classes.
    /// </summary>
    public partial class ChartLabelStyle : BlazorBindings.Maui.Elements.Element
    {
        static ChartLabelStyle()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a value to customize the appearance of the label's background.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Controls.Brush" /> values.
        /// </value>
        [Parameter] public Color BackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the rounded corners for labels.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.CornerRadius" /> values and the default value is 0.
        /// </value>
        [Parameter] public CornerRadius? CornerRadius { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates the font attributes of the label.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Controls.FontAttributes" /> values.
        /// </value>
        [Parameter] public MC.FontAttributes? FontAttributes { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates the font family of the label.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:System.String" /> values.
        /// </value>
        [Parameter] public string FontFamily { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates the label's size.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:System.Double" /> values.
        /// </value>
        [Parameter] public double? FontSize { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the label's format.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:System.String" /> values.
        /// </value>
        [Parameter] public string LabelFormat { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates the margin of the label.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Thickness" /> values and the default value is 3.5.
        /// </value>
        [Parameter] public Thickness? Margin { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the outer stroke appearance of the label.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Controls.Brush" /> values.
        /// </value>
        [Parameter] public Color StrokeColor { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates the stroke thickness of the label.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:System.Double" /> values and the default value is 0.
        /// </value>
        [Parameter] public double? StrokeWidth { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the appearance of the label's text color.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Graphics.Color" /> values.
        /// </value>
        [Parameter] public Color TextColor { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the appearance of the label's background.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Controls.Brush" /> values.
        /// </value>
        [Parameter] public RenderFragment Background { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the outer stroke appearance of the label.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Controls.Brush" /> values.
        /// </value>
        [Parameter] public RenderFragment Stroke { get; set; }

        public new SMTC.ChartLabelStyle NativeControl => (SMTC.ChartLabelStyle)((BindableObject)this).NativeControl;

        protected override SMTC.ChartLabelStyle CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(BackgroundColor):
                    if (!Equals(BackgroundColor, value))
                    {
                        BackgroundColor = (Color)value;
                        NativeControl.Background = BackgroundColor;
                    }
                    break;
                case nameof(CornerRadius):
                    if (!Equals(CornerRadius, value))
                    {
                        CornerRadius = (CornerRadius?)value;
                        NativeControl.CornerRadius = CornerRadius ?? (CornerRadius)SMTC.ChartLabelStyle.CornerRadiusProperty.DefaultValue;
                    }
                    break;
                case nameof(FontAttributes):
                    if (!Equals(FontAttributes, value))
                    {
                        FontAttributes = (MC.FontAttributes?)value;
                        NativeControl.FontAttributes = FontAttributes ?? (MC.FontAttributes)SMTC.ChartLabelStyle.FontAttributesProperty.DefaultValue;
                    }
                    break;
                case nameof(FontFamily):
                    if (!Equals(FontFamily, value))
                    {
                        FontFamily = (string)value;
                        NativeControl.FontFamily = FontFamily;
                    }
                    break;
                case nameof(FontSize):
                    if (!Equals(FontSize, value))
                    {
                        FontSize = (double?)value;
                        NativeControl.FontSize = FontSize ?? (double)SMTC.ChartLabelStyle.FontSizeProperty.DefaultValue;
                    }
                    break;
                case nameof(LabelFormat):
                    if (!Equals(LabelFormat, value))
                    {
                        LabelFormat = (string)value;
                        NativeControl.LabelFormat = LabelFormat;
                    }
                    break;
                case nameof(Margin):
                    if (!Equals(Margin, value))
                    {
                        Margin = (Thickness?)value;
                        NativeControl.Margin = Margin ?? (Thickness)SMTC.ChartLabelStyle.MarginProperty.DefaultValue;
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
                        NativeControl.StrokeWidth = StrokeWidth ?? (double)SMTC.ChartLabelStyle.StrokeWidthProperty.DefaultValue;
                    }
                    break;
                case nameof(TextColor):
                    if (!Equals(TextColor, value))
                    {
                        TextColor = (Color)value;
                        NativeControl.TextColor = TextColor;
                    }
                    break;
                case nameof(Background):
                    Background = (RenderFragment)value;
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
            RenderTreeBuilderHelper.AddContentProperty<SMTC.ChartLabelStyle>(builder, sequence++, Background, (x, value) => x.Background = (MC.Brush)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.ChartLabelStyle>(builder, sequence++, Stroke, (x, value) => x.Stroke = (MC.Brush)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
