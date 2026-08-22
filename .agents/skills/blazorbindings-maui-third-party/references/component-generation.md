# Component generation

Use this path for ordinary controls, layouts, behaviors, and other MAUI `BindableObject` types that should be rendered from Razor.

## Install and invoke the tool

Install the global tool once:

```sh
dotnet tool install --global BlazorBindings.Maui.ComponentGenerator
```

If it is already installed and needs updating:

```sh
dotnet tool update --global BlazorBindings.Maui.ComponentGenerator
```

Prefer the same package version as `BlazorBindings.Maui` when that version is available. Restore the app first so the generator can load the project and vendor assemblies.

From a directory containing exactly one app project, run:

```sh
dotnet generate-maui-blazor-components
```

In a solution or directory with multiple projects, pass the app project explicitly:

```sh
dotnet generate-maui-blazor-components path/to/App.csproj
```

The default output is `Elements/` beside the project file. `-o|--out-path` changes it. By default, the tool deletes existing `*.generated.cs` files recursively from that output before regenerating; use a dedicated generated directory. `--keep-existing-files` is available only when retaining generated files from another configuration is intentional.

## Declare wrappers

Create or update `Properties/Elements.cs`:

```csharp
using BlazorBindings.Maui.ComponentGenerator;
using Vendor.Maui.Controls;

[assembly: GenerateComponent(typeof(VendorCard))]
[assembly: GenerateComponent(typeof(VendorPicker),
    PropertyChangedEvents = [nameof(VendorPicker.SelectedValue)])]
```

For a deliberately broad wrapper set:

```csharp
[assembly: GenerateComponentsFromAssembly(typeof(VendorCard),
    TypeNamePrefix = "Vendor",
    Exclude = [typeof(UnsupportedControl)])]
```

`GenerateComponentsFromAssembly` includes public, browsable, non-obsolete `Element` descendants by default. Set `IncludeNonElements = true` only when `BindableObject` descendants such as behaviors are also needed. Assembly-wide generation does not include open generic type definitions; declare those individually with `GenerateComponent`.

## Select attribute options

- `Exclude`: omit members that would otherwise be generated but are unsupported, unsafe, or handled another way.
- `Include`: force members the generator does not normally discover, including useful members inherited through a generic base.
- `PropertyChangedEvents`: generate `<Property>Changed` from MAUI `PropertyChanged` when no dedicated event exists, enabling normal two-way binding.
- `GenericProperties`: make named properties and the wrapper generic. Use `"Property:Fully.Qualified.ConstraintType"` for a constraint.
- `ContentProperties` / `NonContentProperties`: override whether a property becomes Razor child content or a plain value parameter.
- `Aliases`: rename a member with `"OldName:NewName"`; using the native type name as `OldName` renames the generated component.
- `IsGeneric`: force a generic wrapper when automatic inference is insufficient.
- `MakeItemsGeneric`: override automatic generic handling for collection-style controls.
- `TypeNamePrefix` on `GenerateComponentsFromAssembly`: prevent collisions across vendor and MAUI component names.

Start with no options and add only those justified by the generated API or a build error. Prefer `nameof(...)` for member names. Use a prefix or type alias when a generated name collides with an existing Razor component.

Example for a constrained template and an open generic popup:

```csharp
[assembly: GenerateComponent(typeof(CalendarView),
    GenericProperties = [
        $"{nameof(CalendarView.DayTemplate)}:Vendor.Calendar.ICalendarDay",
    ])]

[assembly: GenerateComponent(typeof(VendorPopup<>),
    Aliases = ["VendorPopup:VendorPopupWithResult"])]
```

## Consume and verify

Inspect the generated file for its exact namespace and parameter types, then add that namespace to `_Imports.razor` or the specific Razor file. Keep vendor native namespaces out of Razor when they would create ambiguous component names.

Build the relevant target framework after generation. If a needed property is absent, first decide whether `Include`, `PropertyChangedEvents`, a content-property override, or a generic option represents it correctly. Exclude an incompatible member rather than editing generated code. Use a separate partial class only for behavior the attribute model cannot express.
