// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.ComponentGenerator;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlazorBindings.UnitTests")]

//[assembly: GenerateComponentsFromAssembly(typeof(Element),
//    Exclude = [
//        typeof(Application),
//        typeof(AbsoluteLayout),
//        typeof(ContentPresenter),
//        typeof(Microsoft.Maui.Controls.Compatibility.RelativeLayout),
//        typeof(ImageSource),
//        typeof(FileImageSource),
//        typeof(UriImageSource),
//        typeof(StreamImageSource),
//        typeof(FontImageSource),
//        typeof(Path),
//        typeof(FormattedString),
//        typeof(NavigationPage)
//    ])]

[assembly: GenerateComponent(typeof(ActivityIndicator))]
[assembly: GenerateComponent(typeof(BaseMenuItem))]
[assembly: GenerateComponent(typeof(BaseShellItem))]
[assembly: GenerateComponent(typeof(Behavior))]
[assembly: GenerateComponent(typeof(Border))]
[assembly: GenerateComponent(typeof(BoxView))]
[assembly: GenerateComponent(typeof(Brush))]
[assembly: GenerateComponent(typeof(Button))]
[assembly: GenerateComponent(typeof(CarouselView),
    GenericProperties = [nameof(CarouselView.CurrentItem)])]
[assembly: GenerateComponent(typeof(CheckBox))]
[assembly: GenerateComponent(typeof(CollectionView))]
[assembly: GenerateComponent(typeof(ContentPage))]
[assembly: GenerateComponent(typeof(ContentView))]
[assembly: GenerateComponent(typeof(DatePicker),
    Exclude = [nameof(DatePicker.Date), nameof(DatePicker.DateSelected), nameof(DatePicker.MaximumDate), nameof(DatePicker.MinimumDate)])]
[assembly: GenerateComponent(typeof(Editor))]
[assembly: GenerateComponent(typeof(Element),
    Exclude = [ nameof(Element.ChildAdded), nameof(Element.ChildRemoved),
        nameof(Element.DescendantAdded), nameof(Element.DescendantRemoved),
        nameof(Element.Parent), nameof(Element.ParentChanging), nameof(Element.ParentChanged),
        nameof(Element.Handler), nameof(Element.HandlerChanged), nameof(Element.HandlerChanging) ])]
[assembly: GenerateComponent(typeof(Entry), IsGeneric = true)]
[assembly: GenerateComponent(typeof(FlexLayout))]
[assembly: GenerateComponent(typeof(FlyoutItem))]
[assembly: GenerateComponent(typeof(FlyoutPage), Exclude = [nameof(FlyoutPage.Detail)])]
[assembly: GenerateComponent(typeof(Frame))]
[assembly: GenerateComponent(typeof(GestureElement))]
[assembly: GenerateComponent(typeof(GradientBrush))]
[assembly: GenerateComponent(typeof(GradientStop))]
[assembly: GenerateComponent(typeof(GraphicsView))]
[assembly: GenerateComponent(typeof(Grid),
    Exclude = [nameof(Grid.ColumnDefinitions), nameof(Grid.RowDefinitions)])]
[assembly: GenerateComponent(typeof(GroupableItemsView),
    Exclude = [nameof(GroupableItemsView.GroupFooterTemplate), nameof(GroupableItemsView.GroupHeaderTemplate), nameof(GroupableItemsView.IsGrouped)])]
[assembly: GenerateComponent(typeof(HorizontalStackLayout))]
[assembly: GenerateComponent(typeof(Image))]
[assembly: GenerateComponent(typeof(ImageButton))]
[assembly: GenerateComponent(typeof(IndicatorView),
    PropertyChangedEvents = [nameof(IndicatorView.Position)],
    Exclude = [nameof(IndicatorView.ItemsSource), nameof(IndicatorView.IndicatorLayout)])]
[assembly: GenerateComponent(typeof(InputView), Exclude = [nameof(InputView.Text), nameof(InputView.TextChanged)])]
[assembly: GenerateComponent(typeof(ItemsView),
    GenericProperties = [nameof(ItemsView.ItemsSource), nameof(ItemsView.ItemTemplate)],
    ContentProperties = [nameof(ItemsView.EmptyView)],
    Exclude = [nameof(ItemsView.EmptyViewTemplate), nameof(ItemsView.ItemsSource)])]
[assembly: GenerateComponent(typeof(Label))]
[assembly: GenerateComponent(typeof(Layout))]
[assembly: GenerateComponent(typeof(LinearGradientBrush))]
[assembly: GenerateComponent(typeof(ListView),
    Exclude = [nameof(ListView.ItemTemplate)],
    GenericProperties = [
        nameof(ListView.ItemsSource),
        $"{nameof(ListView.GroupHeaderTemplate)}:System.Object",
        nameof(ListView.GroupDisplayBinding),
        nameof(ListView.GroupShortNameBinding)],
    Aliases = [
        $"{nameof(ListView.HeaderTemplate)}:Header",
        $"{nameof(ListView.FooterTemplate)}:Footer" ])]
[assembly: GenerateComponent(typeof(MenuItem))]
[assembly: GenerateComponent(typeof(NavigableElement))]
[assembly: GenerateComponent(typeof(Page))]
[assembly: GenerateComponent(typeof(Picker),
    GenericProperties = [nameof(Picker.ItemsSource), nameof(Picker.SelectedItem), nameof(Picker.ItemDisplayBinding)],
    PropertyChangedEvents = [nameof(Picker.SelectedItem)])]
[assembly: GenerateComponent(typeof(ProgressBar))]
[assembly: GenerateComponent(typeof(RadialGradientBrush))]
[assembly: GenerateComponent(typeof(RadioButton))]
[assembly: GenerateComponent(typeof(RefreshView),
    Exclude = [nameof(RefreshView.Refreshing)],
    PropertyChangedEvents = [nameof(RefreshView.IsRefreshing)])]
[assembly: GenerateComponent(typeof(ReorderableItemsView), Exclude = [nameof(ReorderableItemsView.CanMixGroups)])]
[assembly: GenerateComponent(typeof(ScrollView))]
[assembly: GenerateComponent(typeof(SearchBar))]
[assembly: GenerateComponent(typeof(SearchHandler),
    GenericProperties = [nameof(SearchHandler.ItemsSource), nameof(SearchHandler.SelectedItem), nameof(SearchHandler.ItemTemplate)],
    PropertyChangedEvents = [nameof(SearchHandler.Query), nameof(SearchHandler.SelectedItem)])]
[assembly: GenerateComponent(typeof(SelectableItemsView),
    GenericProperties = [nameof(SelectableItemsView.SelectedItem)],
    PropertyChangedEvents = [nameof(SelectableItemsView.SelectedItem), nameof(SelectableItemsView.SelectedItems)])]
[assembly: GenerateComponent(typeof(Shadow), Exclude = [nameof(Shadow.Brush)])]
[assembly: GenerateComponent(typeof(Shell),
    GenericProperties = [
        $"{nameof(Shell.ItemTemplate)}:Microsoft.Maui.Controls.BaseShellItem",
        $"{nameof(Shell.MenuItemTemplate)}:Microsoft.Maui.Controls.BaseShellItem"
    ],
    Aliases = [
        $"{nameof(Shell.FlyoutHeaderTemplate)}:FlyoutHeader",
        $"{nameof(Shell.FlyoutFooterTemplate)}:FlyoutFooter",
        $"{nameof(Shell.FlyoutContentTemplate)}:FlyoutContent",
    ],
    Exclude = [nameof(Shell.Items)])]
[assembly: GenerateComponent(typeof(ShellContent),
    Exclude = [nameof(ShellContent.ContentTemplate)])]
[assembly: GenerateComponent(typeof(ShellGroupItem))]
[assembly: GenerateComponent(typeof(ShellItem), Exclude = [nameof(ShellItem.Items)])]
[assembly: GenerateComponent(typeof(ShellSection), Exclude = [nameof(ShellSection.Items)])]
[assembly: GenerateComponent(typeof(Slider))]
[assembly: GenerateComponent(typeof(SolidColorBrush))]
[assembly: GenerateComponent(typeof(Span))]
[assembly: GenerateComponent(typeof(StackBase))]
[assembly: GenerateComponent(typeof(StackLayout))]
[assembly: GenerateComponent(typeof(StructuredItemsView),
    ContentProperties = [nameof(StructuredItemsView.Header), nameof(StructuredItemsView.Footer)],
    Exclude = [nameof(StructuredItemsView.HeaderTemplate), nameof(StructuredItemsView.FooterTemplate)])]
[assembly: GenerateComponent(typeof(Stepper))]
[assembly: GenerateComponent(typeof(StyleableElement))]
[assembly: GenerateComponent(typeof(SwipeView),
    ContentProperties = [nameof(SwipeView.LeftItems), nameof(SwipeView.RightItems), nameof(SwipeView.TopItems), nameof(SwipeView.BottomItems)])]
[assembly: GenerateComponent(typeof(SwipeItems),
    Exclude = [nameof(SwipeItems.CollectionChanged)])]
[assembly: GenerateComponent(typeof(SwipeItem))]
[assembly: GenerateComponent(typeof(SwipeItemView))]
[assembly: GenerateComponent(typeof(Switch))]
[assembly: GenerateComponent(typeof(Tab))]
[assembly: GenerateComponent(typeof(TabBar))]
[assembly: GenerateComponent(typeof(TabbedPage),
    Exclude = [nameof(TabbedPage.ItemsSource), nameof(TabbedPage.ItemTemplate)])]
[assembly: GenerateComponent(typeof(TableView),
    Exclude = [nameof(TableView.Root)])]
[assembly: GenerateComponent(typeof(TableRoot))]
[assembly: GenerateComponent(typeof(TableSection))]
[assembly: GenerateComponent(typeof(TableSectionBase))]
[assembly: GenerateComponent(typeof(TemplatedPage), Exclude = [nameof(TemplatedPage.ControlTemplate)])]
[assembly: GenerateComponent(typeof(TemplatedView))]
[assembly: GenerateComponent(typeof(TimePicker), Exclude = [nameof(TimePicker.Time), nameof(TimePicker.TimeSelected)])]
[assembly: GenerateComponent(typeof(ToolbarItem))]
[assembly: GenerateComponent(typeof(VerticalStackLayout))]
[assembly: GenerateComponent(typeof(View))]
[assembly: GenerateComponent(typeof(VisualElement), Exclude = [nameof(VisualElement.BackgroundColor)])]
[assembly: GenerateComponent(typeof(WebView), NonContentProperties = [nameof(WebView.Source)])]

[assembly: GenerateComponent(typeof(FlyoutBase))]
[assembly: GenerateComponent(typeof(KeyboardAccelerator))]
[assembly: GenerateComponent(typeof(MenuBar))]
[assembly: GenerateComponent(typeof(MenuBarItem))]
[assembly: GenerateComponent(typeof(MenuFlyout))]
[assembly: GenerateComponent(typeof(MenuFlyoutItem))]
[assembly: GenerateComponent(typeof(MenuFlyoutSeparator))]
[assembly: GenerateComponent(typeof(MenuFlyoutSubItem))]

[assembly: GenerateComponent(typeof(Window))]

// Cells
[assembly: GenerateComponent(typeof(TextCell))]
[assembly: GenerateComponent(typeof(ImageCell))]
[assembly: GenerateComponent(typeof(SwitchCell))]
[assembly: GenerateComponent(typeof(EntryCell),
    PropertyChangedEvents = [nameof(EntryCell.Text)])]
[assembly: GenerateComponent(typeof(ViewCell))]
[assembly: GenerateComponent(typeof(Cell))]


// GestureRecognizers
[assembly: GenerateComponent(typeof(GestureRecognizer))]
[assembly: GenerateComponent(typeof(PanGestureRecognizer))]
[assembly: GenerateComponent(typeof(PinchGestureRecognizer))]
[assembly: GenerateComponent(typeof(SwipeGestureRecognizer))]
[assembly: GenerateComponent(typeof(TapGestureRecognizer))]
[assembly: GenerateComponent(typeof(PointerGestureRecognizer))]


// Compatibility
[assembly: GenerateComponent(typeof(Microsoft.Maui.Controls.Compatibility.Layout))]

// Shapes
[assembly: GenerateComponent(typeof(Ellipse))]
[assembly: GenerateComponent(typeof(Line))]
[assembly: GenerateComponent(typeof(Polygon))]
[assembly: GenerateComponent(typeof(Polyline))]
[assembly: GenerateComponent(typeof(Rectangle))]
[assembly: GenerateComponent(typeof(Shape))]
[assembly: GenerateComponent(typeof(RoundRectangle))]