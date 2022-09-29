using AlohaKit.Controls;
using BlazorBindings.Maui.ComponentGenerator;
using XCalendar.Maui.Views;

// AlohaKit
[assembly: GenerateComponent(typeof(Rating))]
[assembly: GenerateComponent(typeof(BusyIndicator))]
[assembly: GenerateComponent(typeof(Avatar))]
[assembly: GenerateComponent(typeof(AlohaKit.Controls.Button))]
[assembly: GenerateComponent(typeof(AlohaKit.Controls.CheckBox))]
[assembly: GenerateComponent(typeof(HorizontalProgressBar))]
[assembly: GenerateComponent(typeof(VerticalProgressBar))]
[assembly: GenerateComponent(typeof(NumericUpDown))]
[assembly: GenerateComponent(typeof(ProgressRadial))]
[assembly: GenerateComponent(typeof(ToggleSwitch),
    Exclude = new[] { nameof(ToggleSwitch.Toggled) },
    PropertyChangedEvents = new[] { nameof(ToggleSwitch.IsOn) })]
[assembly: GenerateComponent(typeof(AlohaKit.Controls.Slider))]
[assembly: GenerateComponent(typeof(PulseIcon))]

// CommunityToolkit
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Views.AvatarView))]
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Views.DrawingView))]

// XCalendar
[assembly: GenerateComponent(typeof(CalendarView),
    GenericProperties = new[] {
        $"{nameof(CalendarView.DayNameTemplate)}:XCalendar.Core.Interfaces.ICalendarDay",
        $"{nameof(CalendarView.DayTemplate)}:XCalendar.Core.Interfaces.ICalendarDay",
    })]