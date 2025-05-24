// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.ComponentGenerator;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlazorBindings.UnitTests")]

[assembly: GenerateComponentsFromAssembly(typeof(Element),
    Exclude = [
        typeof(Application),
        typeof(AbsoluteLayout),
        typeof(ContentPresenter),
        typeof(Microsoft.Maui.Controls.Compatibility.RelativeLayout),
        typeof(ImageSource),
        typeof(FileImageSource),
        typeof(UriImageSource),
        typeof(StreamImageSource),
        typeof(FontImageSource),
        typeof(FormattedString),
        typeof(NavigationPage),
        typeof(AppLinkEntry)
    ])]

[assembly: GenerateComponent(typeof(Behavior))]
[assembly: GenerateComponent(typeof(CarouselView), GenericProperties = [nameof(CarouselView.CurrentItem)])]
[assembly: GenerateComponent(typeof(DatePicker),
    Exclude = [nameof(DatePicker.Date), nameof(DatePicker.DateSelected), nameof(DatePicker.MaximumDate), nameof(DatePicker.MinimumDate)])]
[assembly: GenerateComponent(typeof(Element),
    Exclude = [ nameof(Element.ChildAdded), nameof(Element.ChildRemoved),
        nameof(Element.DescendantAdded), nameof(Element.DescendantRemoved),
        nameof(Element.Parent), nameof(Element.ParentChanging), nameof(Element.ParentChanged),
        nameof(Element.Handler), nameof(Element.HandlerChanged), nameof(Element.HandlerChanging) ])]
[assembly: GenerateComponent(typeof(Entry), IsGeneric = true)]
[assembly: GenerateComponent(typeof(FlyoutPage), Exclude = [nameof(FlyoutPage.Detail)])]
[assembly: GenerateComponent(typeof(Frame))]
[assembly: GenerateComponent(typeof(Grid), Exclude = [nameof(Grid.ColumnDefinitions), nameof(Grid.RowDefinitions)])]
[assembly: GenerateComponent(typeof(GroupableItemsView),
    Exclude = [nameof(GroupableItemsView.GroupFooterTemplate), nameof(GroupableItemsView.GroupHeaderTemplate), nameof(GroupableItemsView.IsGrouped)])]
[assembly: GenerateComponent(typeof(IndicatorView),
    PropertyChangedEvents = [nameof(IndicatorView.Position)],
    Exclude = [nameof(IndicatorView.ItemsSource), nameof(IndicatorView.IndicatorLayout)])]
[assembly: GenerateComponent(typeof(InputView), Exclude = [nameof(InputView.Text), nameof(InputView.TextChanged)])]
[assembly: GenerateComponent(typeof(ItemsView),
    GenericProperties = [nameof(ItemsView.ItemsSource), nameof(ItemsView.ItemTemplate)],
    ContentProperties = [nameof(ItemsView.EmptyView)],
    Exclude = [nameof(ItemsView.EmptyViewTemplate), nameof(ItemsView.ItemsSource)])]
[assembly: GenerateComponent(typeof(ListView),
    Exclude = [nameof(ListView.ItemTemplate), nameof(ListView.Header), nameof(ListView.Footer)],
    GenericProperties = [
        nameof(ListView.ItemsSource),
        $"{nameof(ListView.GroupHeaderTemplate)}:System.Object",
        nameof(ListView.GroupDisplayBinding),
        nameof(ListView.SelectedItem),
        nameof(ListView.GroupShortNameBinding)],
    Aliases = [
        $"{nameof(ListView.HeaderTemplate)}:Header",
        $"{nameof(ListView.FooterTemplate)}:Footer" ])]
[assembly: GenerateComponent(typeof(Picker),
    GenericProperties = [nameof(Picker.ItemsSource), nameof(Picker.SelectedItem), nameof(Picker.ItemDisplayBinding)],
    PropertyChangedEvents = [nameof(Picker.SelectedItem)])]
[assembly: GenerateComponent(typeof(RadioButton),
    Exclude = [nameof(RadioButton.Value), nameof(RadioButton.Content)])]
[assembly: GenerateComponent(typeof(RefreshView),
    Exclude = [nameof(RefreshView.Refreshing)],
    PropertyChangedEvents = [nameof(RefreshView.IsRefreshing)])]
[assembly: GenerateComponent(typeof(ReorderableItemsView),
    Exclude = [nameof(ReorderableItemsView.CanMixGroups)])]
[assembly: GenerateComponent(typeof(SearchHandler),
    GenericProperties = [nameof(SearchHandler.ItemsSource), nameof(SearchHandler.SelectedItem), nameof(SearchHandler.ItemTemplate)],
    PropertyChangedEvents = [nameof(SearchHandler.Query), nameof(SearchHandler.SelectedItem)])]
[assembly: GenerateComponent(typeof(SelectableItemsView),
    GenericProperties = [nameof(SelectableItemsView.SelectedItem)],
    PropertyChangedEvents = [nameof(SelectableItemsView.SelectedItem), nameof(SelectableItemsView.SelectedItems)])]
[assembly: GenerateComponent(typeof(Shadow),
    Exclude = [nameof(Shadow.Brush)])]
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
    Exclude = [nameof(Shell.Items), nameof(Shell.FlyoutHeader), nameof(Shell.FlyoutFooter), nameof(Shell.FlyoutContent)])]
[assembly: GenerateComponent(typeof(ShellContent), Exclude = [nameof(ShellContent.ContentTemplate), nameof(ShellContent.Content)])]
[assembly: GenerateComponent(typeof(ShellItem), Exclude = [nameof(ShellItem.Items)])]
[assembly: GenerateComponent(typeof(ShellSection), Exclude = [nameof(ShellSection.Items)])]
[assembly: GenerateComponent(typeof(StructuredItemsView),
    ContentProperties = [nameof(StructuredItemsView.Header), nameof(StructuredItemsView.Footer)],
    Exclude = [nameof(StructuredItemsView.HeaderTemplate), nameof(StructuredItemsView.FooterTemplate)])]
[assembly: GenerateComponent(typeof(SwipeView))]
[assembly: GenerateComponent(typeof(SwipeItems), Exclude = [nameof(SwipeItems.CollectionChanged)])]
[assembly: GenerateComponent(typeof(TabbedPage), Exclude = [nameof(TabbedPage.ItemsSource), nameof(TabbedPage.ItemTemplate)])]
[assembly: GenerateComponent(typeof(TableView), Exclude = [nameof(TableView.Root)])]
[assembly: GenerateComponent(typeof(TableRoot))]
[assembly: GenerateComponent(typeof(TableSection))]
[assembly: GenerateComponent(typeof(TableSectionBase))]
[assembly: GenerateComponent(typeof(TemplatedPage), Exclude = [nameof(TemplatedPage.ControlTemplate)])]
[assembly: GenerateComponent(typeof(TimePicker), Exclude = [nameof(TimePicker.Time), nameof(TimePicker.TimeSelected)])]
[assembly: GenerateComponent(typeof(VisualElement),
    Exclude = [nameof(VisualElement.BackgroundColor), nameof(VisualElement.Triggers)])]
[assembly: GenerateComponent(typeof(WebView), NonContentProperties = [nameof(WebView.Source)])]
[assembly: GenerateComponent(typeof(KeyboardAccelerator))]

[assembly: GenerateComponent(typeof(EntryCell),
    PropertyChangedEvents = [nameof(EntryCell.Text)])]

// Compatibility
[assembly: GenerateComponent(typeof(Microsoft.Maui.Controls.Compatibility.Layout))]