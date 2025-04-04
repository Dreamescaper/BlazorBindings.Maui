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
using Microsoft.Maui.Graphics;
using SMTT = Syncfusion.Maui.Toolkit.TabView;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.TabView
{
    /// <summary>
    /// Represents a class which defines the visual and interactive behavior of individual tab item inside a <see cref="T:Syncfusion.Maui.Toolkit.TabView.SfTabView" /> control.
    /// </summary>
    public partial class SfTabItem : BlazorBindings.Maui.Elements.ContentView
    {
        static SfTabItem()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the value that defines the font attributes of the tab header.
        /// </summary>
        /// <value>
        /// One of the <see cref="T:Microsoft.Maui.Controls.FontAttributes" /> enumeration that specifies the font attributes of the tab item's header text. The default value is is <see cref="F:Microsoft.Maui.Controls.FontAttributes.None" />.
        /// </value>
        [Parameter] public MC.FontAttributes? FontAttributes { get; set; }
        /// <summary>
        /// Gets or sets a value that determines whether the font of the control should scale automatically according to the operating system settings.
        /// </summary>
        /// <value>
        /// A boolean value indicating whether the font should scale automatically. The default value is false.
        /// </value>
        [Parameter] public bool? FontAutoScalingEnabled { get; set; }
        /// <summary>
        /// Gets or sets the value that defines the font family of the header.
        /// </summary>
        /// <value>
        /// Specifies the font family of the tab item's header text. The default value is null.
        /// </value>
        [Parameter] public string FontFamily { get; set; }
        /// <summary>
        /// Gets or sets the font size of the tab header.
        /// </summary>
        /// <value>
        /// Specifies the font size of the tab item's header text. The default value is 14d.
        /// </value>
        [Parameter] public double? FontSize { get; set; }
        /// <summary>
        /// Gets or sets the text for the tab header.
        /// </summary>
        /// <value>
        /// Specifies the header text of the tab item. The default value is an empty string.
        /// </value>
        [Parameter] public string Header { get; set; }
        /// <summary>
        /// Gets or sets the position of the image relative to the text in a tab item.
        /// </summary>
        /// <value>
        /// One of the <see cref="T:Syncfusion.Maui.Toolkit.TabView.TabImagePosition" /> enumeration that specifies the image position relative to the text. The default mode is <see cref="F:Syncfusion.Maui.Toolkit.TabView.TabImagePosition.Top" />.
        /// </value>
        [Parameter] public SMTT.TabImagePosition? ImagePosition { get; set; }
        /// <summary>
        /// Gets or sets a value that can be used to customize the image size in tab header.
        /// </summary>
        [Parameter] public double? ImageSize { get; set; }
        /// <summary>
        /// Gets or sets the image source for the tab header.
        /// </summary>
        /// <value>
        /// Specifies the image of the tab item. The default value is null.
        /// </value>
        [Parameter] public MC.ImageSource ImageSource { get; set; }
        /// <summary>
        /// Gets or sets the spacing between the header text and image.
        /// </summary>
        /// <value>
        /// Specifies the spacing between the header and image. The default spacing value is 10d.
        /// </value>
        [Parameter] public double? ImageTextSpacing { get; set; }
        /// <summary>
        /// Gets or sets the text color of the tab header.
        /// </summary>
        /// <value>
        /// Specifies the color of the tab item's header text. The default value is black.
        /// </value>
        [Parameter] public Color TextColor { get; set; }

        public new SMTT.SfTabItem NativeControl => (SMTT.SfTabItem)((BindableObject)this).NativeControl;

        protected override SMTT.SfTabItem CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(FontAttributes):
                    if (!Equals(FontAttributes, value))
                    {
                        FontAttributes = (MC.FontAttributes?)value;
                        NativeControl.FontAttributes = FontAttributes ?? (MC.FontAttributes)SMTT.SfTabItem.FontAttributesProperty.DefaultValue;
                    }
                    break;
                case nameof(FontAutoScalingEnabled):
                    if (!Equals(FontAutoScalingEnabled, value))
                    {
                        FontAutoScalingEnabled = (bool?)value;
                        NativeControl.FontAutoScalingEnabled = FontAutoScalingEnabled ?? (bool)SMTT.SfTabItem.FontAutoScalingEnabledProperty.DefaultValue;
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
                        NativeControl.FontSize = FontSize ?? (double)SMTT.SfTabItem.FontSizeProperty.DefaultValue;
                    }
                    break;
                case nameof(Header):
                    if (!Equals(Header, value))
                    {
                        Header = (string)value;
                        NativeControl.Header = Header;
                    }
                    break;
                case nameof(ImagePosition):
                    if (!Equals(ImagePosition, value))
                    {
                        ImagePosition = (SMTT.TabImagePosition?)value;
                        NativeControl.ImagePosition = ImagePosition ?? (SMTT.TabImagePosition)SMTT.SfTabItem.ImagePositionProperty.DefaultValue;
                    }
                    break;
                case nameof(ImageSize):
                    if (!Equals(ImageSize, value))
                    {
                        ImageSize = (double?)value;
                        NativeControl.ImageSize = ImageSize ?? (double)SMTT.SfTabItem.ImageSizeProperty.DefaultValue;
                    }
                    break;
                case nameof(ImageSource):
                    if (!Equals(ImageSource, value))
                    {
                        ImageSource = (MC.ImageSource)value;
                        NativeControl.ImageSource = ImageSource;
                    }
                    break;
                case nameof(ImageTextSpacing):
                    if (!Equals(ImageTextSpacing, value))
                    {
                        ImageTextSpacing = (double?)value;
                        NativeControl.ImageTextSpacing = ImageTextSpacing ?? (double)SMTT.SfTabItem.ImageTextSpacingProperty.DefaultValue;
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
