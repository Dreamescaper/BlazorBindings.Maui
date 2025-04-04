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
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using SMTC = Syncfusion.Maui.Toolkit.Charts;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.Charts
{
    /// <summary>
    /// The <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartLegendLabelStyle" /> class provides customizable styling options for the labels in a chart's legend, allowing you to define properties such as font size, font family, text color, margin, and font attributes to enhance the visual appearance of the legend labels.
    /// </summary>
    public partial class ChartLegendLabelStyle : BlazorBindings.Maui.Elements.Element
    {
        static ChartLegendLabelStyle()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a value that indicates the font attributes of the label.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:System.String" /> values.
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
        /// Gets or sets a value that indicates the margin of the label.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Microsoft.Maui.Thickness" /> values.
        /// </value>
        [Parameter] public Thickness? Margin { get; set; }
        /// <summary>
        /// Gets or sets a value to customize the appearance of the label's text color.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="T:Microsoft.Maui.Graphics.Color" /> value.
        /// </value>
        [Parameter] public Color TextColor { get; set; }

        public new SMTC.ChartLegendLabelStyle NativeControl => (SMTC.ChartLegendLabelStyle)((BindableObject)this).NativeControl;

        protected override SMTC.ChartLegendLabelStyle CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(FontAttributes):
                    if (!Equals(FontAttributes, value))
                    {
                        FontAttributes = (MC.FontAttributes?)value;
                        NativeControl.FontAttributes = FontAttributes ?? (MC.FontAttributes)SMTC.ChartLegendLabelStyle.FontAttributesProperty.DefaultValue;
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
                        NativeControl.FontSize = FontSize ?? (double)SMTC.ChartLegendLabelStyle.FontSizeProperty.DefaultValue;
                    }
                    break;
                case nameof(Margin):
                    if (!Equals(Margin, value))
                    {
                        Margin = (Thickness?)value;
                        NativeControl.Margin = Margin ?? (Thickness)SMTC.ChartLegendLabelStyle.MarginProperty.DefaultValue;
                    }
                    break;
                case nameof(TextColor):
                    if (!Equals(TextColor, value))
                    {
                        TextColor = (Color)value;
                        NativeControl.TextColor = TextColor;
                    }
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        static partial void RegisterAdditionalHandlers();
    }
}
