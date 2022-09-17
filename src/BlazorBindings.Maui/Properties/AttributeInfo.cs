﻿// Copyright (c) Microsoft Corporation.
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
//[assembly: GenerateComponent(typeof(CarouselView))] // Manually written to use custom logic for generics and binding
[assembly: GenerateComponent(typeof(CheckBox))]
//[assembly: GenerateComponent(typeof(CollectionView))] // Manually written to use custom logic for generics and binding
[assembly: GenerateComponent(typeof(ContentPage))]
[assembly: GenerateComponent(typeof(ContentView))]
[assembly: GenerateComponent(typeof(DatePicker),
    Exclude = new[] { nameof(DatePicker.Date), nameof(DatePicker.DateSelected), nameof(DatePicker.MaximumDate), nameof(DatePicker.MinimumDate) })]
[assembly: GenerateComponent(typeof(Editor))]
[assembly: GenerateComponent(typeof(Entry))]
[assembly: GenerateComponent(typeof(FlexLayout))]
[assembly: GenerateComponent(typeof(FlyoutItem))]
[assembly: GenerateComponent(typeof(FlyoutPage))]
[assembly: GenerateComponent(typeof(Frame))]
[assembly: GenerateComponent(typeof(GestureElement))]
[assembly: GenerateComponent(typeof(GradientBrush))]
[assembly: GenerateComponent(typeof(GradientStop))]
[assembly: GenerateComponent(typeof(GraphicsView))]
[assembly: GenerateComponent(typeof(Grid))]
//[assembly: GenerateComponent(typeof(GroupableItemsView))] // Manually written to use custom logic for generics and binding
[assembly: GenerateComponent(typeof(HorizontalStackLayout))]
[assembly: GenerateComponent(typeof(Image))]
[assembly: GenerateComponent(typeof(ImageButton))]
[assembly: GenerateComponent(typeof(InputView))]
//[assembly: GenerateComponent(typeof(ItemsView))] // Manually written to use custom logic for generics and binding
[assembly: GenerateComponent(typeof(Label))]
[assembly: GenerateComponent(typeof(Layout))]
[assembly: GenerateComponent(typeof(LinearGradientBrush))]
[assembly: GenerateComponent(typeof(MenuItem))]
[assembly: GenerateComponent(typeof(NavigableElement))]
[assembly: GenerateComponent(typeof(Page))]
[assembly: GenerateComponent(typeof(ProgressBar))]
[assembly: GenerateComponent(typeof(RadialGradientBrush))]
[assembly: GenerateComponent(typeof(RadioButton))]
[assembly: GenerateComponent(typeof(RefreshView),
    Exclude = new[] { nameof(RefreshView.Refreshing) },
    PropertyChangedEvents = new[] { nameof(RefreshView.IsRefreshing) })]
[assembly: GenerateComponent(typeof(ScrollView))]
[assembly: GenerateComponent(typeof(Shell))]
[assembly: GenerateComponent(typeof(ShellContent))]
[assembly: GenerateComponent(typeof(ShellGroupItem))]
[assembly: GenerateComponent(typeof(ShellItem))]
[assembly: GenerateComponent(typeof(ShellSection))]
[assembly: GenerateComponent(typeof(Slider))]
[assembly: GenerateComponent(typeof(SolidColorBrush))]
[assembly: GenerateComponent(typeof(Span))]
[assembly: GenerateComponent(typeof(StackBase))]
[assembly: GenerateComponent(typeof(StackLayout))]
[assembly: GenerateComponent(typeof(Stepper))]
[assembly: GenerateComponent(typeof(Switch))]
[assembly: GenerateComponent(typeof(Tab))]
[assembly: GenerateComponent(typeof(TabBar))]
[assembly: GenerateComponent(typeof(TabbedPage))]
[assembly: GenerateComponent(typeof(TemplatedPage))]
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