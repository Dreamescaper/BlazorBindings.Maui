using BlazorBindings.Maui.ComponentGenerator;
using SkiaSharp.Views.Maui.Controls;
using XCalendar.Maui.Views;

// CommunityToolkit
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Views.AvatarView), Exclude = [nameof(CommunityToolkit.Maui.Views.AvatarView.CornerRadius)])]
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Views.DrawingView))]
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Views.Popup), Exclude = [nameof(CommunityToolkit.Maui.Views.Popup.Anchor)])]

[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Behaviors.MaskedBehavior))]
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Behaviors.UserStoppedTypingBehavior))]
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Behaviors.StatusBarBehavior))]

// XCalendar
[assembly: GenerateComponent(typeof(CalendarView),
    GenericProperties = [
        $"{nameof(CalendarView.DayNameTemplate)}:XCalendar.Core.Interfaces.ICalendarDay",
        $"{nameof(CalendarView.DayTemplate)}:XCalendar.Core.Interfaces.ICalendarDay",
    ])]

// Material.Components.Maui
[assembly: GenerateComponentsFromAssembly(typeof(Material.Components.Maui.Button),
    TypeNamePrefix = "MD")]

[assembly: GenerateComponent(typeof(Material.Components.Maui.SKTouchCanvasView))]
[assembly: GenerateComponent(typeof(SKCanvasView))]
