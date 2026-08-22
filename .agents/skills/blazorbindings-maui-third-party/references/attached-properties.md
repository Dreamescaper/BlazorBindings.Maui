# Attached properties

Generated components forward known parameters to native properties. A third-party attached property needs a global handler so an otherwise unknown Razor attribute can call the vendor's native setter.

Register handlers once during app startup, before any affected component renders. Keep registration in a named integration method rather than scattering it through pages:

```csharp
using BlazorBindings.Maui;
using Vendor.Maui.Controls;

namespace MyApp.Integrations;

public static class VendorAttachedProperties
{
    public static MauiAppBuilder UseVendorAttachedProperties(this MauiAppBuilder builder)
    {
#pragma warning disable MBB001
        AttachedPropertyRegistry.RegisterAttachedPropertyHandler(
            "VendorLayout.Span",
            static (element, value) => VendorLayout.SetSpan(element, (int)value));
#pragma warning restore MBB001

        return builder;
    }
}
```

Call it from `CreateMauiApp` before `Build()`:

```csharp
builder
    .UseMauiApp<App>()
    .UseMauiBlazorBindings()
    .UseVendorControls()
    .UseVendorAttachedProperties();
```

Use the exact same key in Razor. A C# expression preserves the value type expected by the handler:

```razor
<VendorCard VendorLayout.Span="@2" />
```

The handler receives a `Microsoft.Maui.Controls.BindableObject` and the raw Razor value. Validate or convert the value and target type when they are not guaranteed. Call the vendor's public attached-property setter rather than manipulating its `BindableProperty` storage directly.

`AttachedPropertyRegistry` is experimental (`MBB001`) and process-global. Suppress the warning only around the registration when the project accepts that risk. Use vendor-qualified keys because registering the same string again replaces the previous handler. Registration provides value forwarding only; it does not generate a typed Razor parameter or vendor startup configuration.
