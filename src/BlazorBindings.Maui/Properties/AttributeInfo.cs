// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.ComponentGenerator;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlazorBindings.UnitTests")]

[assembly: GenerateComponent(typeof(ActivityIndicator))]
[assembly: GenerateComponent(typeof(BaseMenuItem))]
[assembly: GenerateComponent(typeof(BaseShellItem))]
[assembly: GenerateComponent(typeof(Border))]
[assembly: GenerateComponent(typeof(BoxView))]
[assembly: GenerateComponent(typeof(Brush))]
[assembly: GenerateComponent(typeof(Button))]
[assembly: GenerateComponent(typeof(CarouselView),
    GenericProperties = new[] { nameof(CarouselView.CurrentItem) })]
[assembly: GenerateComponent(typeof(CheckBox))]
[assembly: GenerateComponent(typeof(CollectionView))]
[assembly: GenerateComponent(typeof(ContentPage))]
[assembly: GenerateComponent(typeof(ContentView))]
[assembly: GenerateComponent(typeof(DatePicker),
    Exclude = new[] { nameof(DatePicker.Date), nameof(DatePicker.DateSelected), nameof(DatePicker.MaximumDate), nameof(DatePicker.MinimumDate) })]
[assembly: GenerateComponent(typeof(Editor))]
[assembly: GenerateComponent(typeof(Entry))]
[assembly: GenerateComponent(typeof(FlexLayout))]
[assembly: GenerateComponent(typeof(FlyoutItem))]
[assembly: GenerateComponent(typeof(FlyoutPage), Aliases = new[] { "Detail:Detail" })]
[assembly: GenerateComponent(typeof(Frame))]
[assembly: GenerateComponent(typeof(GestureElement))]
[assembly: GenerateComponent(typeof(GradientBrush))]
[assembly: GenerateComponent(typeof(GradientStop))]
[assembly: GenerateComponent(typeof(GraphicsView))]
[assembly: GenerateComponent(typeof(Grid))]
[assembly: GenerateComponent(typeof(GroupableItemsView),
    Exclude = new[] { nameof(GroupableItemsView.GroupFooterTemplate), nameof(GroupableItemsView.GroupHeaderTemplate), nameof(GroupableItemsView.IsGrouped) })]
[assembly: GenerateComponent(typeof(HorizontalStackLayout))]
[assembly: GenerateComponent(typeof(Image))]
[assembly: GenerateComponent(typeof(ImageButton))]
[assembly: GenerateComponent(typeof(InputView))]
[assembly: GenerateComponent(typeof(ItemsView),
    GenericProperties = new[] { nameof(ItemsView.ItemsSource), nameof(ItemsView.ItemTemplate) },
    ContentProperties = new[] { nameof(ItemsView.EmptyView) },
    Exclude = new[] { nameof(ItemsView.EmptyViewTemplate) })]
[assembly: GenerateComponent(typeof(Label))]
[assembly: GenerateComponent(typeof(Layout))]
[assembly: GenerateComponent(typeof(LinearGradientBrush))]
[assembly: GenerateComponent(typeof(MenuItem))]
[assembly: GenerateComponent(typeof(NavigableElement))]
[assembly: GenerateComponent(typeof(Page))]
[assembly: GenerateComponent(typeof(Picker),
    GenericProperties = new[] { nameof(Picker.ItemsSource), nameof(Picker.SelectedItem) },
    PropertyChangedEvents = new[] { nameof(Picker.SelectedItem) },
    Exclude = new[] { nameof(Picker.ItemDisplayBinding) })]
[assembly: GenerateComponent(typeof(ProgressBar))]
[assembly: GenerateComponent(typeof(RadialGradientBrush))]
[assembly: GenerateComponent(typeof(RadioButton))]
[assembly: GenerateComponent(typeof(RefreshView),
    Exclude = new[] { nameof(RefreshView.Refreshing) },
    PropertyChangedEvents = new[] { nameof(RefreshView.IsRefreshing) })]
[assembly: GenerateComponent(typeof(ReorderableItemsView), Exclude = new[] { nameof(ReorderableItemsView.CanMixGroups) })]
[assembly: GenerateComponent(typeof(ScrollView))]
[assembly: GenerateComponent(typeof(SelectableItemsView),
    GenericProperties = new[] { nameof(SelectableItemsView.SelectedItem) },
    PropertyChangedEvents = new[] { nameof(SelectableItemsView.SelectedItem), nameof(SelectableItemsView.SelectedItems) })]
[assembly: GenerateComponent(typeof(Shadow), Exclude = new[] { nameof(Shadow.Brush) })]
[assembly: GenerateComponent(typeof(Shell),
    GenericProperties = new[] {
        $"{nameof(Shell.ItemTemplate)}:Microsoft.Maui.Controls.BaseShellItem",
        $"{nameof(Shell.MenuItemTemplate)}:Microsoft.Maui.Controls.BaseShellItem"
    },
    Aliases = new[] {
        $"{nameof(Shell.FlyoutHeaderTemplate)}:FlyoutHeader",
        $"{nameof(Shell.FlyoutFooterTemplate)}:FlyoutFooter",
        $"{nameof(Shell.FlyoutContentTemplate)}:FlyoutContent",
    })]
[assembly: GenerateComponent(typeof(ShellContent),
    Exclude = new[] { nameof(ShellContent.ContentTemplate) })]
[assembly: GenerateComponent(typeof(ShellGroupItem))]
[assembly: GenerateComponent(typeof(ShellItem))]
[assembly: GenerateComponent(typeof(ShellSection))]
[assembly: GenerateComponent(typeof(Slider))]
[assembly: GenerateComponent(typeof(SolidColorBrush))]
[assembly: GenerateComponent(typeof(Span))]
[assembly: GenerateComponent(typeof(StackBase))]
[assembly: GenerateComponent(typeof(StackLayout))]
[assembly: GenerateComponent(typeof(StructuredItemsView),
    ContentProperties = new[] { nameof(StructuredItemsView.Header), nameof(StructuredItemsView.Footer) },
    Exclude = new[] { nameof(StructuredItemsView.HeaderTemplate), nameof(StructuredItemsView.FooterTemplate) })]
[assembly: GenerateComponent(typeof(Stepper))]
[assembly: GenerateComponent(typeof(Switch))]
[assembly: GenerateComponent(typeof(Tab))]
[assembly: GenerateComponent(typeof(TabBar))]
[assembly: GenerateComponent(typeof(TabbedPage))]
[assembly: GenerateComponent(typeof(TemplatedPage), Exclude = new[] { nameof(TemplatedPage.ControlTemplate) })]
[assembly: GenerateComponent(typeof(TemplatedView))]
[assembly: GenerateComponent(typeof(TimePicker), Exclude = new[] { nameof(TimePicker.Time) })]
[assembly: GenerateComponent(typeof(ToolbarItem))]
[assembly: GenerateComponent(typeof(VerticalStackLayout))]
[assembly: GenerateComponent(typeof(View))]
[assembly: GenerateComponent(typeof(VisualElement))]
[assembly: GenerateComponent(typeof(WebView))]

// Compatibility
[assembly: GenerateComponent(typeof(Microsoft.Maui.Controls.Compatibility.Layout))]

// Shapes
[assembly: GenerateComponent(typeof(Ellipse))]
[assembly: GenerateComponent(typeof(Line))]
[assembly: GenerateComponent(typeof(Polygon))]
[assembly: GenerateComponent(typeof(Polyline))]
[assembly: GenerateComponent(typeof(Rectangle))]
[assembly: GenerateComponent(typeof(Shape))]