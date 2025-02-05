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
using SMTB = Syncfusion.Maui.Toolkit.Buttons;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.Buttons
{
    /// <summary>
    /// The <see cref="T:Syncfusion.Maui.Toolkit.Buttons.SfButton" /> class provides a way for users to interact with the application by clicking or tapping. It can display text, an icon, or both, and supports various customization options.
    /// </summary>
    public partial class SfButton : BlazorBindings.Maui.Elements.Syncfusion.Toolkit.ButtonBase
    {
        static SfButton()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the value of the background image aspect. This property can be used to set an aspect for background image of Button.
        /// </summary>
        /// <value>
        /// The default value is Aspect.AspectFill.
        /// </value>
        [Parameter] public Aspect? BackgroundImageAspect { get; set; }
        /// <summary>
        /// Gets or sets the value of corner radius. This property can be used to customize the corners of button control.
        /// </summary>
        /// <value>
        /// The default value is 20.
        /// </value>
        [Parameter] public new CornerRadius? CornerRadius { get; set; }
        /// <summary>
        /// Gets or sets the value of dash array of the border. This property can be used to customize the dash of border.
        /// </summary>
        /// <value>
        /// The default value is float[]{0,0}.
        /// </value>
        [Parameter] public float[] DashArray { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the control is in checkable state or not. This property can be used to change the state of the control.
        /// </summary>
        /// <value>
        /// <c>true</c> if checkable is enabled; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? IsCheckable { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the control is checkable. It is used to check the state of the control.
        /// </summary>
        /// <value>
        /// <c>true</c> if checked is enabled; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? IsChecked { get; set; }
        /// <summary>
        /// Gets or sets the value of line breakmode. This property can be used to customize the line breakmode of the text.
        /// </summary>
        /// <value>
        /// The default value is NoWrap.
        /// </value>
        [Parameter] public LineBreakMode? LineBreakMode { get; set; }
        /// <summary>
        /// Gets or sets the value of stroke thickness. This property can be used to give border thickness to button control.
        /// </summary>
        /// <value>
        /// The default value is 0d.
        /// </value>
        [Parameter] public new double? StrokeThickness { get; set; }
        /// <summary>
        /// Gets or sets the value of text color. This property can be used to give text color to the text in control.
        /// </summary>
        /// <value>
        /// The default value is Colors.White.
        /// </value>
        [Parameter] public new Color TextColor { get; set; }
        /// <summary>
        /// Gets or sets the value of text transform. This property is used to specify the text transformation options for text processing.
        /// </summary>
        /// <value>
        /// The default value is "Default", indicating that default transform is applied.
        /// </value>
        [Parameter] public TextTransform? TextTransform { get; set; }
        /// <summary>
        /// Gets or sets the value of background. This property can be used to give background color to the control.
        /// </summary>
        /// <value>
        /// The default value is SolidColorBrush(Color.FromArgb("#6750A4")) .
        /// </value>
        [Parameter] public new RenderFragment Background { get; set; }
        /// <summary>
        /// Gets or sets the value of the content. This property can be used to set custom view to the button control.
        /// </summary>
        /// <value>
        /// The default value is null.
        /// </value>
        [Parameter] public new RenderFragment ChildContent { get; set; }

        public new SMTB.SfButton NativeControl => (SMTB.SfButton)((BindableObject)this).NativeControl;

        protected override SMTB.SfButton CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(BackgroundImageAspect):
                    if (!Equals(BackgroundImageAspect, value))
                    {
                        BackgroundImageAspect = (Aspect?)value;
                        NativeControl.BackgroundImageAspect = BackgroundImageAspect ?? (Aspect)SMTB.SfButton.BackgroundImageAspectProperty.DefaultValue;
                    }
                    break;
                case nameof(CornerRadius):
                    if (!Equals(CornerRadius, value))
                    {
                        CornerRadius = (CornerRadius?)value;
                        NativeControl.CornerRadius = CornerRadius ?? (CornerRadius)SMTB.SfButton.CornerRadiusProperty.DefaultValue;
                    }
                    break;
                case nameof(DashArray):
                    if (!Equals(DashArray, value))
                    {
                        DashArray = (float[])value;
                        NativeControl.DashArray = DashArray;
                    }
                    break;
                case nameof(IsCheckable):
                    if (!Equals(IsCheckable, value))
                    {
                        IsCheckable = (bool?)value;
                        NativeControl.IsCheckable = IsCheckable ?? (bool)SMTB.SfButton.IsCheckableProperty.DefaultValue;
                    }
                    break;
                case nameof(IsChecked):
                    if (!Equals(IsChecked, value))
                    {
                        IsChecked = (bool?)value;
                        NativeControl.IsChecked = IsChecked ?? (bool)SMTB.SfButton.IsCheckedProperty.DefaultValue;
                    }
                    break;
                case nameof(LineBreakMode):
                    if (!Equals(LineBreakMode, value))
                    {
                        LineBreakMode = (LineBreakMode?)value;
                        NativeControl.LineBreakMode = LineBreakMode ?? (LineBreakMode)SMTB.SfButton.LineBreakModeProperty.DefaultValue;
                    }
                    break;
                case nameof(StrokeThickness):
                    if (!Equals(StrokeThickness, value))
                    {
                        StrokeThickness = (double?)value;
                        NativeControl.StrokeThickness = StrokeThickness ?? (double)SMTB.SfButton.StrokeThicknessProperty.DefaultValue;
                    }
                    break;
                case nameof(TextColor):
                    if (!Equals(TextColor, value))
                    {
                        TextColor = (Color)value;
                        NativeControl.TextColor = TextColor;
                    }
                    break;
                case nameof(TextTransform):
                    if (!Equals(TextTransform, value))
                    {
                        TextTransform = (TextTransform?)value;
                        NativeControl.TextTransform = TextTransform ?? (TextTransform)SMTB.SfButton.TextTransformProperty.DefaultValue;
                    }
                    break;
                case nameof(Background):
                    Background = (RenderFragment)value;
                    break;
                case nameof(ChildContent):
                    ChildContent = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<SMTB.SfButton>(builder, sequence++, Background, (x, value) => x.Background = (MC.Brush)value);
            RenderTreeBuilderHelper.AddDataTemplateProperty<SMTB.SfButton>(builder, sequence++, ChildContent, (x, template) => x.Content = template);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
