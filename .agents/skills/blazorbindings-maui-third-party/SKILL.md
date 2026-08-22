---
name: blazorbindings-maui-third-party
description: Integrate third-party .NET MAUI control libraries into BlazorBindings.Maui apps by generating Razor wrappers, registering attached properties, and bridging to native elements for imperative APIs such as popups or bottom sheets. Use when adding, updating, or troubleshooting a MAUI control package in a BlazorBindings.Maui project; do not use for XAML or Blazor Hybrid HTML components.
---

# BlazorBindings.Maui Third-Party Controls

Make the smallest integration that preserves the library's native MAUI behavior while exposing a Razor-friendly surface.

## Choose the integration path

1. Generate a wrapper for a control, behavior, or other `BindableObject` that participates in Razor rendering. Read [component generation](references/component-generation.md).
2. Additionally register an attached-property handler when Razor must pass a vendor attached property to a generated element. Read [attached properties](references/attached-properties.md) only for this case.
3. Use the native-element bridge when the vendor API must receive a constructed native element and invoke an imperative presentation API, as with popups or bottom sheets. Read [native element bridges](references/native-element-bridges.md) only for this case.

Prefer normal generated components and `BlazorBindings.Maui.INavigation` APIs. Do not obtain a native element merely to set properties that the generated wrapper can expose.

## Workflow

- Inspect the app's target frameworks, package versions, `_Imports.razor`, `MauiProgram`, and existing `Properties/Elements.cs` and `Elements/` output before editing.
- Add the vendor NuGet package and perform its documented MAUI startup registration in `MauiProgram`. Keep vendor licensing or platform setup explicit; wrapper generation does not replace it.
- Install or update `BlazorBindings.Maui.ComponentGenerator`, preferably at a version compatible with the app's `BlazorBindings.Maui` package.
- Declare generation attributes in `Properties/Elements.cs`. Generate individual types by default; use assembly-wide generation only when the requested breadth and naming-collision risk are understood.
- Run generation against the intended app project, inspect the generated namespaces and parameters, then import only the namespaces the Razor files need.
- Keep all `Elements/**/*.generated.cs` files generated and never hand-edit them. Put custom code in separate non-generated files.
- Build at least one relevant target framework. Treat generator warnings, duplicate component names, invalid Razor parameters, and native-type cast failures as integration defects rather than suppressing them broadly.
- Re-run generation whenever the vendor package, generation attributes, or BlazorBindings.Maui version changes.

## Project conventions

- This is native .NET MAUI UI expressed with Razor, not XAML and not web navigation. Do not add `@page` or URL routes for native screens.
- Use Razor child-content fragments such as `<Header>` or `<ChildContent>` for generated content properties; do not emit XAML-style `<Control.Property>` elements.
- Use fully qualified enum values and MAUI values such as `Colors.Red` in Razor.
- Preserve the vendor's normal `MauiAppBuilder` initialization order and platform-specific setup.

## Completion checks

Confirm that the generated wrapper derives from the expected BlazorBindings.Maui base, creates the intended native type, exposes the required properties/events/content, and compiles in actual Razor usage. For attached properties, confirm the registered key exactly matches the Razor attribute. For native bridges, confirm the Razor component's root native element is assignable to the requested native type and is detached when dismissed.
