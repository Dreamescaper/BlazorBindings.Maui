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
using SMTC = Syncfusion.Maui.Toolkit.Calendar;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of footer view of the SfCalendar.
    /// </summary>
    public partial class CalendarFooterView : BlazorBindings.Maui.Elements.Element
    {
        static CalendarFooterView()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the background of the footer view in SfCalendar.
        /// </summary>
        /// <value>
        /// The Default value of <see cref="P:Syncfusion.Maui.Toolkit.Calendar.CalendarFooterView.Background" /> is Transparent.
        /// </value>
        [Parameter] public Color BackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets the background of the footer separator line background in SfCalendar.
        /// </summary>
        /// <value>
        /// The Default value of <see cref="P:Syncfusion.Maui.Toolkit.Calendar.CalendarFooterView.DividerColor" /> is Transparent.
        /// </value>
        [Parameter] public Color DividerColor { get; set; }
        /// <summary>
        /// Gets or sets the value to specify the height of footer view on SfCalendar.
        /// </summary>
        /// <value>
        /// The default value of <see cref="P:Syncfusion.Maui.Toolkit.Calendar.CalendarFooterView.Height" /> is 50.
        /// </value>
        [Parameter] public double? Height { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to show the cancel button in the footer view of SfCalendar.
        /// </summary>
        /// <value>
        /// The Default value of <see cref="P:Syncfusion.Maui.Toolkit.Calendar.CalendarFooterView.ShowActionButtons" /> is false.
        /// </value>
        [Parameter] public bool? ShowActionButtons { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to show the today button in the footer view of SfCalendar.
        /// </summary>
        /// <value>
        /// The Default value of <see cref="P:Syncfusion.Maui.Toolkit.Calendar.CalendarFooterView.ShowTodayButton" /> is false.
        /// </value>
        [Parameter] public bool? ShowTodayButton { get; set; }
        /// <summary>
        /// Gets or sets the background of the footer view in SfCalendar.
        /// </summary>
        /// <value>
        /// The Default value of <see cref="P:Syncfusion.Maui.Toolkit.Calendar.CalendarFooterView.Background" /> is Transparent.
        /// </value>
        [Parameter] public RenderFragment Background { get; set; }
        /// <summary>
        /// Gets or sets the ok and cancel button text style in the footer view of SfCalendar.
        /// </summary>
        [Parameter] public RenderFragment TextStyle { get; set; }

        public new SMTC.CalendarFooterView NativeControl => (SMTC.CalendarFooterView)((BindableObject)this).NativeControl;

        protected override SMTC.CalendarFooterView CreateNativeElement() => new();

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
                case nameof(DividerColor):
                    if (!Equals(DividerColor, value))
                    {
                        DividerColor = (Color)value;
                        NativeControl.DividerColor = DividerColor;
                    }
                    break;
                case nameof(Height):
                    if (!Equals(Height, value))
                    {
                        Height = (double?)value;
                        NativeControl.Height = Height ?? (double)SMTC.CalendarFooterView.HeightProperty.DefaultValue;
                    }
                    break;
                case nameof(ShowActionButtons):
                    if (!Equals(ShowActionButtons, value))
                    {
                        ShowActionButtons = (bool?)value;
                        NativeControl.ShowActionButtons = ShowActionButtons ?? (bool)SMTC.CalendarFooterView.ShowActionButtonsProperty.DefaultValue;
                    }
                    break;
                case nameof(ShowTodayButton):
                    if (!Equals(ShowTodayButton, value))
                    {
                        ShowTodayButton = (bool?)value;
                        NativeControl.ShowTodayButton = ShowTodayButton ?? (bool)SMTC.CalendarFooterView.ShowTodayButtonProperty.DefaultValue;
                    }
                    break;
                case nameof(Background):
                    Background = (RenderFragment)value;
                    break;
                case nameof(TextStyle):
                    TextStyle = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.CalendarFooterView>(builder, sequence++, Background, (x, value) => x.Background = (MC.Brush)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.CalendarFooterView>(builder, sequence++, TextStyle, (x, value) => x.TextStyle = (SMTC.CalendarTextStyle)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
