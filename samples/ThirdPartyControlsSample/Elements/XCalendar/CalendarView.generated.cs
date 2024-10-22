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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XMV = XCalendar.Maui.Views;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.XCalendar
{
    public partial class CalendarView : BlazorBindings.Maui.Elements.ContentView
    {
        static CalendarView()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public double? DayNameHorizontalSpacing { get; set; }
        /// <summary>
        /// The height of the view showing the days of the week.
        /// </summary>
        [Parameter] public double? DayNamesHeightRequest { get; set; }
        [Parameter] public double? DayNameVerticalSpacing { get; set; }
        [Parameter] public IEnumerable<object> Days { get; set; }
        [Parameter] public IList<DayOfWeek> DaysOfWeek { get; set; }
        /// <summary>
        /// The height of the view used to display the <see cref="P:XCalendar.Maui.Views.CalendarView.Days" />
        /// </summary>
        [Parameter] public double? DaysViewHeightRequest { get; set; }
        [Parameter] public DateTime? NavigatedDate { get; set; }
        [Parameter] public RenderFragment DayNamesTemplate { get; set; }
        /// <summary>
        /// The template used to display the days of the week.
        /// </summary>
        [Parameter] public RenderFragment<global::XCalendar.Core.Interfaces.ICalendarDay> DayNameTemplate { get; set; }
        /// <summary>
        /// The template used to display the <see cref="P:XCalendar.Maui.Views.CalendarView.Days" />.
        /// </summary>
        [Parameter] public RenderFragment DaysViewTemplate { get; set; }
        /// <summary>
        /// The template used to display a <see cref="T:XCalendar.Core.Interfaces.ICalendarDay" />
        /// </summary>
        [Parameter] public RenderFragment<global::XCalendar.Core.Interfaces.ICalendarDay> DayTemplate { get; set; }
        /// <summary>
        /// The template used to display the view for navigating the calendar.
        /// </summary>
        [Parameter] public RenderFragment NavigationViewTemplate { get; set; }

        public new XMV.CalendarView NativeControl => (XMV.CalendarView)((BindableObject)this).NativeControl;

        protected override XMV.CalendarView CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(DayNameHorizontalSpacing):
                    if (!Equals(DayNameHorizontalSpacing, value))
                    {
                        DayNameHorizontalSpacing = (double?)value;
                        NativeControl.DayNameHorizontalSpacing = DayNameHorizontalSpacing ?? (double)XMV.CalendarView.DayNameHorizontalSpacingProperty.DefaultValue;
                    }
                    break;
                case nameof(DayNamesHeightRequest):
                    if (!Equals(DayNamesHeightRequest, value))
                    {
                        DayNamesHeightRequest = (double?)value;
                        NativeControl.DayNamesHeightRequest = DayNamesHeightRequest ?? (double)XMV.CalendarView.DayNamesHeightRequestProperty.DefaultValue;
                    }
                    break;
                case nameof(DayNameVerticalSpacing):
                    if (!Equals(DayNameVerticalSpacing, value))
                    {
                        DayNameVerticalSpacing = (double?)value;
                        NativeControl.DayNameVerticalSpacing = DayNameVerticalSpacing ?? (double)XMV.CalendarView.DayNameVerticalSpacingProperty.DefaultValue;
                    }
                    break;
                case nameof(Days):
                    if (!Equals(Days, value))
                    {
                        Days = (IEnumerable<object>)value;
                        NativeControl.Days = Days;
                    }
                    break;
                case nameof(DaysOfWeek):
                    if (!Equals(DaysOfWeek, value))
                    {
                        DaysOfWeek = (IList<DayOfWeek>)value;
                        NativeControl.DaysOfWeek = DaysOfWeek;
                    }
                    break;
                case nameof(DaysViewHeightRequest):
                    if (!Equals(DaysViewHeightRequest, value))
                    {
                        DaysViewHeightRequest = (double?)value;
                        NativeControl.DaysViewHeightRequest = DaysViewHeightRequest ?? (double)XMV.CalendarView.DaysViewHeightRequestProperty.DefaultValue;
                    }
                    break;
                case nameof(NavigatedDate):
                    if (!Equals(NavigatedDate, value))
                    {
                        NavigatedDate = (DateTime?)value;
                        NativeControl.NavigatedDate = NavigatedDate ?? (DateTime)XMV.CalendarView.NavigatedDateProperty.DefaultValue;
                    }
                    break;
                case nameof(DayNamesTemplate):
                    DayNamesTemplate = (RenderFragment)value;
                    break;
                case nameof(DayNameTemplate):
                    DayNameTemplate = (RenderFragment<global::XCalendar.Core.Interfaces.ICalendarDay>)value;
                    break;
                case nameof(DaysViewTemplate):
                    DaysViewTemplate = (RenderFragment)value;
                    break;
                case nameof(DayTemplate):
                    DayTemplate = (RenderFragment<global::XCalendar.Core.Interfaces.ICalendarDay>)value;
                    break;
                case nameof(NavigationViewTemplate):
                    NavigationViewTemplate = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddControlTemplateProperty<XMV.CalendarView>(builder, sequence++, DayNamesTemplate, (x, template) => x.DayNamesTemplate = template);
            RenderTreeBuilderHelper.AddDataTemplateProperty<XMV.CalendarView, global::XCalendar.Core.Interfaces.ICalendarDay>(builder, sequence++, DayNameTemplate, (x, template) => x.DayNameTemplate = template);
            RenderTreeBuilderHelper.AddControlTemplateProperty<XMV.CalendarView>(builder, sequence++, DaysViewTemplate, (x, template) => x.DaysViewTemplate = template);
            RenderTreeBuilderHelper.AddDataTemplateProperty<XMV.CalendarView, global::XCalendar.Core.Interfaces.ICalendarDay>(builder, sequence++, DayTemplate, (x, template) => x.DayTemplate = template);
            RenderTreeBuilderHelper.AddControlTemplateProperty<XMV.CalendarView>(builder, sequence++, NavigationViewTemplate, (x, template) => x.NavigationViewTemplate = template);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
