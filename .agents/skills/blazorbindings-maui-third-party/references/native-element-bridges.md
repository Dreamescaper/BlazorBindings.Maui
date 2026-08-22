# Native element bridges

Use this path only when a vendor's imperative API requires the native MAUI instance—for example, a popup or bottom sheet that must be passed to `ShowAsync`, attached to a native host page, or queried for a result.

For ordinary pages and modal pages, stay on the public `BlazorBindings.Maui.INavigation` surface:

```csharp
await Navigation.PushAsync(@<DetailsPage Item="@item" />);
await Navigation.PushModalAsync(@<EditorPage Model="@model" />);
```

For a vendor overlay, hide the concrete and experimental bridge in an extension method so Razor components continue to depend on `INavigation`. This example follows the Community Toolkit pattern:

```csharp
using BlazorBindings.Maui;
using CommunityToolkit.Maui.Extensions;
using IComponent = Microsoft.AspNetCore.Components.IComponent;
using INavigation = BlazorBindings.Maui.INavigation;

namespace MyApp.Integrations;

public static class NavigationExtensions
{
    public static async Task<TResult> ShowToolkitPopupAsync<TPopup, TResult>(
        this INavigation navigation,
        Dictionary<string, object>? arguments = null)
        where TPopup : IComponent
    {
#pragma warning disable MBB001
        var popup = await ((Navigation)navigation)
            .BuildElement<CommunityToolkit.Maui.Views.Popup<TResult>>(
                typeof(TPopup), arguments);
#pragma warning restore MBB001

        var result = await Application.Current!.MainPage!
            .ShowPopupAsync<TResult>(popup);
        return result.Result;
    }
}
```

The Razor component supplied as `TPopup` must render a root native element assignable to the `BuildElement<TNative>` type. Pass component parameters through the argument dictionary using parameter names as keys. The vendor still needs its normal builder registration and the native popup type still needs a generated wrapper.

The concrete cast to `BlazorBindings.Maui.Navigation` and `BuildElement<TNative>` use the experimental `MBB001` extensibility API. Keep that dependency localized and suppress the diagnostic narrowly. Do not add `BuildElement` calls directly to page components.

`BuildElement` keeps the rendered component alive until the resulting native element's parent becomes `null`, then removes its renderer root. Confirm that the vendor attaches the element while presented and detaches it when dismissed. If the requested native type does not match the Razor component's root, the bridge throws; fix the root/type contract rather than weakening the cast.

For vendors with a different result or presentation shape, adapt only the final native call. For example, build a vendor `BottomSheet`, call its `ShowAsync`, and return no result; or build a vendor popup and return the value from its `ShowAtAsync` method.
