# Mobile Guidelines

## Platform

- This project uses .NET MAUI with BlazorBindings.Maui and Razor syntax, not XAML.
- Do not add web-specific concepts such as `@page` or URL navigation.
- Avoid unnecessary StateHasChanged calls.

## Razor And Component Conventions

- Use fully qualified enum values such as `FontAttributes="FontAttributes.Bold"`, `LayoutOptions.Center`, and `Aspect.AspectFill`.
- Use constructor syntax for non-uniform Thickness values, for example `new Thickness(0, 0, 0, 10)`.
- Use `Colors.Name` for named colors, e.g. `Colors.Red`, `Colors.Transparent`.
- Do not use XAML-style named content properties like `<Control.Property>`. Use Razor child content fragments such as `<Header>`, `<Footer>`, and explicit `<ChildContent>` when needed.
- Define shadows as child `<Shadow>` elements, e.g. `<Shadow Brush="Colors.Black" Opacity="0.3" />`.
- For Border rounding, prefer `CornerRadius` over an inline `IShape` fragment unless you need a non-rectangular clip shape (e.g. a star or custom path).
- Use `Color` for solid `Brush` values and `StrokeColor` for solid `Stroke` values, e.g. `Color="Colors.Blue"`, `StrokeColor="Colors.Gray"`.
- Prefix C# expressions passed to string properties with `@`, e.g. `Text="@myVar"`.
- Use INavigation from BlazorBindings.Maui for navigation. Inject it with `[Inject] INavigation Navigation { get; set; }`. For push navigation, pass a Razor component fragment: `await Navigation.PushAsync(@<SecondPage SomeParameter="value" />)`.
- Do not use obsolete controls such as Frame, ListView, TableView, or StackLayout.
- Setting HorizontalOptions on HorizontalStackLayout or VerticalOptions on VerticalStackLayout has no effect; use Grid for those layouts.
- Do not add using directives that are already provided by _Imports.razor.
- DatePicker uses DateOnly? and TimePicker uses TimeOnly?.
- Do not bind `RefreshView.IsRefreshing` only to reset it after refresh. BlazorBindings.Maui sets `IsRefreshing` to `false` after the `OnRefreshing` callback completes. Keep a separate refreshing flag only when the UI itself needs it, such as passing loading state to a child component.
- It is acceptable to bind MAUI collection controls, such as `CollectionView.ItemsSource`, to a mutable `List<T>` and append items in place. BlazorBindings.Maui emits change notifications for non-observable collections, so do not require `ObservableCollection<T>` solely for UI refresh behavior.

## Blazor Bindings Generation

Third-party MAUI controls are wrapped for Blazor use via the `BlazorBindings.Maui.ComponentGenerator` tool. Generated files live under `Elements/` and are suffixed `.generated.cs`. Do not edit them by hand.

Generation is configured via `[assembly: GenerateComponent(...)]` attributes in `Properties/Elements.cs`. Re-run the tool after changing that file or updating a wrapped package:

```sh
dotnet tool install --global BlazorBindings.Maui.ComponentGenerator   # first time only
dotnet generate-maui-blazor-components
```
