using AlohaKit.Controls;
using BlazorBindings.Maui.ComponentGenerator;
using SkiaSharp.Views.Maui.Controls;
using XCalendar.Maui.Views;

// AlohaKit.Controls
[assembly: GenerateComponent(typeof(Rating))]
[assembly: GenerateComponent(typeof(BusyIndicator))]
[assembly: GenerateComponent(typeof(Avatar))]
[assembly: GenerateComponent(typeof(AlohaKit.Controls.Button), Aliases = new[] { "Button:AlhButton" })]
[assembly: GenerateComponent(typeof(AlohaKit.Controls.CheckBox), Aliases = new[] { "CheckBox:AlhCheckBox" })]
[assembly: GenerateComponent(typeof(AlohaKit.Controls.ProgressBar), Aliases = new[] { "ProgressBar:AlhProgressBar" })]
[assembly: GenerateComponent(typeof(NumericUpDown))]
[assembly: GenerateComponent(typeof(ProgressRadial))]
[assembly: GenerateComponent(typeof(ToggleSwitch),
    Exclude = new[] { nameof(ToggleSwitch.Toggled) },
    PropertyChangedEvents = new[] { nameof(ToggleSwitch.IsOn) })]
[assembly: GenerateComponent(typeof(AlohaKit.Controls.Slider), Aliases = new[] { "Slider:AlhSlider" })]
[assembly: GenerateComponent(typeof(PulseIcon))]

// CommunityToolkit
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Views.AvatarView), Exclude = new[] { nameof(CommunityToolkit.Maui.Views.AvatarView.CornerRadius) })]
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Views.DrawingView))]
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Views.Popup), Exclude = new[] { nameof(CommunityToolkit.Maui.Views.Popup.Anchor) })]

[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Behaviors.MaskedBehavior))]
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Behaviors.UserStoppedTypingBehavior))]
[assembly: GenerateComponent(typeof(CommunityToolkit.Maui.Behaviors.StatusBarBehavior))]

// XCalendar
[assembly: GenerateComponent(typeof(CalendarView),
    GenericProperties = new[] {
        $"{nameof(CalendarView.DayNameTemplate)}:XCalendar.Core.Interfaces.ICalendarDay",
        $"{nameof(CalendarView.DayTemplate)}:XCalendar.Core.Interfaces.ICalendarDay",
    })]

// Material.Components.Maui
[assembly: GenerateComponent(typeof(Material.Components.Maui.ProgressIndicator), Aliases = new[] { "ProgressIndicator:MDProgressIndicator" })]
[assembly: GenerateComponent(typeof(Material.Components.Maui.Button), Aliases = new[] { "Button:MDButton" })]
[assembly: GenerateComponent(typeof(Material.Components.Maui.NavigationBar), Aliases = new[] { "NavigationBar:MDNavigationBar" })]
[assembly: GenerateComponent(typeof(Material.Components.Maui.ComboBox), Aliases = new[] { "ComboBox:MDComboBox" })]
[assembly: GenerateComponent(typeof(Material.Components.Maui.ComboBoxItem), Aliases = new[] { "ComboBoxItem:MDComboBoxItem" })]
[assembly: GenerateComponent(typeof(Material.Components.Maui.MenuItem), Aliases = new[] { "MenuItem:MDMenuItem" })]
[assembly: GenerateComponent(typeof(Material.Components.Maui.Card), Aliases = new[] { "Card:MDCard" })]
[assembly: GenerateComponent(typeof(Material.Components.Maui.WrapLayout), Aliases = new[] { "WrapLayout:MDWrapLayout" })]

[assembly: GenerateComponent(typeof(Material.Components.Maui.SKTouchCanvasView))]
[assembly: GenerateComponent(typeof(SKCanvasView))]
