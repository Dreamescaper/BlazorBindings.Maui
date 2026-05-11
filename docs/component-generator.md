# BlazorBindings.Maui Component Generator

The Component Generator is a .NET global tool (`dotnet-generate-maui-blazor-components`) that automatically generates Blazor wrapper components for .NET MAUI types. It is used to produce the `*.generated.cs` files that make up the `BlazorBindings.Maui` element library, and can also be used in any third-party project that wants to expose custom MAUI controls as Blazor components.

## How It Works

### High-Level Flow

1. **Locate the target project** – The tool opens a `.csproj` file using Roslyn's `MSBuildWorkspace`, producing a full `Compilation` object that includes all referenced assemblies.
2. **Discover types to generate** – Assembly-level attributes (`[GenerateComponent]` and `[GenerateComponentsFromAssembly]`) are read from the compiled project. Each attribute describes one or more MAUI types that should receive a generated Blazor wrapper.
3. **Build a generation model** – For each requested type, a `GeneratedTypeInfo` record is constructed. It resolves the base component class, determines which MAUI properties map to Blazor `[Parameter]`s, and classifies each property as a value, `EventCallback`, or `RenderFragment`.
4. **Emit C# source** – `ComponentWrapperGenerator` turns each `GeneratedTypeInfo` into a complete C# source string. Files are written to the output directory as `<TypeName>.generated.cs` (optionally inside a sub-folder named after the `ComponentGroup`).
5. **Clean up stale files** – Unless `--keep-existing-files` is passed, all previously generated `*.generated.cs` files in the output directory are deleted before writing new ones.

### Property Classification

For each public, non-obsolete, non-excluded property on the MAUI type the generator decides how to expose it:

| MAUI property type | Generated Blazor parameter |
|---|---|
| Primitive / value type | `[Parameter] public T Prop { get; set; }` |
| MAUI `Element` or list of elements | `[Parameter] public RenderFragment Prop { get; set; }` (child content) |
| MAUI event | `[Parameter] public EventCallback Handler { get; set; }` |
| Filtered `PropertyChanged` event | `[Parameter] public EventCallback<T> PropChanged { get; set; }` |

The classification can be overridden per-property using attributes (see [Attribute Reference](#attribute-reference) below).

### Generic Components

When a MAUI type exposes an `ItemsSource` property (and has no `[ContentProperty]` attribute), the generator automatically makes the component generic (`MyComponent<T>`), so `ItemsSource` becomes `IList<T>` and `ItemTemplate` becomes `RenderFragment<T>`. This heuristic can be overridden with `MakeItemsGeneric = false`.

Individual properties can also be promoted to a generic type parameter using `GenericProperties`, which supports optional constraints (`"PropertyName:Fully.Qualified.ConstraintType"`).

---

## Installation

The tool is distributed as a NuGet package. Install it globally or locally:

```bash
# Global installation
dotnet tool install -g BlazorBindings.Maui.ComponentGenerator

# Local installation (requires a tool manifest)
dotnet tool install BlazorBindings.Maui.ComponentGenerator
```

---

## Usage

### Command-Line

```
dotnet-generate-maui-blazor-components [project-path] [options]
```

| Argument / Option | Description |
|---|---|
| `project-path` (positional) | Path to the `.csproj` file to process. Defaults to the first `.csproj` found in the current directory. |
| `-o`, `--out-path` | Directory where generated files are written. Defaults to `../Elements` relative to the project file. |
| `--keep-existing-files` | Do not delete `*.generated.cs` files from a previous run before writing new ones. Defaults to `false`. |

**Example**

```bash
dotnet-generate-maui-blazor-components src/MyControls/MyControls.csproj -o src/MyControls.Blazor/Elements
```

### Declaring Types to Generate

Attributes are placed at the **assembly level** of the project being compiled — conventionally in a file called `Properties/Elements.cs`:

```csharp
// Properties/Elements.cs

// Generate a wrapper for a single MAUI type
[assembly: GenerateComponent(typeof(MyCustomView))]

// Generate wrappers for every public MAUI Element in an assembly
[assembly: GenerateComponentsFromAssembly(typeof(SomeThirdPartyControl))]
```

> **Note:** `GenerateComponentAttribute` is decorated with `[Conditional("CODE_ANALYSIS")]`, so its constructor argument is never evaluated at run-time; there is no overhead in production builds.

---

## Attribute Reference

### `[GenerateComponent(Type typeToGenerate)]`

Generates a single Blazor wrapper component for the specified MAUI type.

| Property | Type | Description |
|---|---|---|
| `Exclude` | `string[]` | Members (properties / events) to omit from the generated component. |
| `Include` | `string[]` | Members to force-include even when the generator would skip them (e.g. properties on a generic base type). |
| `ContentProperties` | `string[]` | Properties to treat as `RenderFragment` child content even if not auto-detected as such. |
| `NonContentProperties` | `string[]` | Properties to treat as plain value parameters even if they would be detected as child content. |
| `PropertyChangedEvents` | `string[]` | Properties for which an `EventCallback<T> PropChanged` should be synthesised by subscribing to `INotifyPropertyChanged.PropertyChanged`. |
| `GenericProperties` | `string[]` | Properties to expose as the generic type parameter `T`. Format: `"PropName"` or `"PropName:Constraint.Type"`. |
| `Aliases` | `string[]` | Rename generated parameters or the component itself. Format: `"MauiName:GeneratedName"`. Also accepts the type's own name as key to rename the component class. |
| `IsGeneric` | `bool` | Force the component to be generic (`MyComponent<T>`) even with no `GenericProperties`. |
| `MakeItemsGeneric` | `bool` | Override the automatic heuristic that makes collection-oriented components generic. |

**Examples**

```csharp
// Exclude a property
[assembly: GenerateComponent(typeof(AvatarView),
	Exclude = [nameof(AvatarView.CornerRadius)])]

// Force-include a property from a generic base class
[assembly: GenerateComponent(typeof(SelectableItemsView),
	Include = [nameof(SelectableItemsView.SelectedItem)])]

// Synthesise a two-way binding helper for a property that lacks a dedicated Changed event
[assembly: GenerateComponent(typeof(MySlider),
	PropertyChangedEvents = [nameof(MySlider.Value)])]

// Constrained generic component
[assembly: GenerateComponent(typeof(CalendarView),
	GenericProperties = [$"{nameof(CalendarView.DayTemplate)}:XCalendar.Core.Interfaces.ICalendarDay"])]

// Rename the generated component class and a property
[assembly: GenerateComponent(typeof(Popup<>),
	Aliases = ["Popup:PopupWithResult", "Content:Body"])]

// Opt out of automatic generic items
[assembly: GenerateComponent(typeof(SfTabView), MakeItemsGeneric = false)]
```

---

### `[GenerateComponentsFromAssembly(Type containingType)]`

Generates wrappers for **all** qualifying public types in the assembly that contains `containingType`.

By default only types derived from `Microsoft.Maui.Controls.Element` are included.

| Property | Type | Description |
|---|---|---|
| `TypeNamePrefix` | `string` | Prefix prepended to every generated class name (e.g. `"EX"` → `EXButton`). |
| `Exclude` | `Type[]` | Specific types to skip. |
| `IncludeNonElements` | `bool` | When `true`, also includes types derived from `BindableObject` that are not `Element`s. |

**Example**

```csharp
[assembly: GenerateComponentsFromAssembly(
	typeof(Syncfusion.Maui.Core.SfView),
	TypeNamePrefix = "Sf",
	Exclude = [typeof(SfInternalHelper)])]
```

---

## Generated File Structure

Each type produces one `<TypeName>.generated.cs` file. If a `ComponentGroup` is resolved for the type the file is placed in a sub-folder:

```
<OutPath>/
  Button.generated.cs
  Label.generated.cs
  ListView/
	ListView.generated.cs
	Cell.generated.cs
```

Generated files begin with a standard header warning that they are auto-generated and should not be edited manually:

```csharp
// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
```

---

## Extending Generated Components

Because every generated class is declared `partial`, you can add extra logic in a hand-written companion file alongside the generated one:

```csharp
// Button.cs  (hand-written)
namespace BlazorBindings.Maui.Elements;

public partial class Button
{
	// Add extra helpers, override HandleParameter, etc.
	partial void RegisterAdditionalHandlers()
	{
		// custom event wiring
	}
}
```

The generated static constructor always calls `RegisterAdditionalHandlers()`, which is defined as a `partial void` in the generated file and therefore a no-op unless you provide an implementation.

---

## Re-generating After MAUI Updates

When the underlying MAUI library is updated, re-run the tool against the same project to refresh all generated files:

```bash
dotnet-generate-maui-blazor-components src/BlazorBindings.Maui/BlazorBindings.Maui.csproj
```

Then review the diff to catch any renamed or removed properties before committing.
