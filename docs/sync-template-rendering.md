# Synchronous Template Rendering

Some MAUI APIs require a `DataTemplate` or `ControlTemplate` factory to return the native root immediately. `ShellContent.ContentTemplate` is the most important example: MAUI may call `DataTemplate.CreateContent` while BlazorBindings is still applying the render batch that assigned the template property.

Blazor itself does not guarantee that `StateHasChanged` completes a render synchronously. `RenderHandle.Render` queues work in the ASP.NET Core renderer. If the renderer is not already processing a batch, the queue is drained immediately. If a batch is already in progress, the render is only queued and is processed after the current batch completes.

That distinction matters during `NativeComponentRenderer.UpdateDisplayAsync`. BlazorBindings applies native MAUI edits inside the active Blazor render batch. When a non-physical template component is added, its `SetParent` method can assign a MAUI template property. MAUI may synchronously invoke that template factory as a side effect of the assignment, before Blazor has finished the current batch.

The old sync-template implementation incremented local state, called `StateHasChanged`, then immediately read the root element rendered by that state change. This worked only when no batch was active. During an active batch, the render was deferred, so the root element was not available yet.

## Design

Synchronous templates now use a separate child `MauiBlazorBindingsRenderer` per created template root.

The main renderer still owns the template holder component:

- `SyncControlTemplateItemsComponent<T>` for untyped `DataTemplate` and `ControlTemplate` properties.
- `SyncDataTemplateItemsComponent<TControl, TItem>` for typed item templates.

Those holder components live in the normal main render tree. They set the MAUI template property, track all roots created from that template, update roots when the template fragment changes, and dispose roots when the holder is removed.

Actual template roots are rendered by `SyncTemplateRendererFactory`. The factory creates a child renderer, renders the template into a `RootContainerHandler`, and returns a handle containing the native root element. Since the child renderer is not the main renderer currently applying a batch, it can produce the root synchronously without depending on reentrant main-renderer queue processing.

One child renderer is created per template root. This avoids reintroducing the same reentrancy problem inside a shared template renderer if a sync template itself contains another sync template.

## Behavior Preserved

The template factory still returns the actual template root directly, without an extra wrapper view. This is necessary for MAUI APIs that require a specific root type, such as a `Page` from `ShellContent.ContentTemplate`.

Event callbacks and captured variables continue to work because the render fragment can close over parent state and callbacks. If an event callback targets a component in the main renderer, it queues work on that component's renderer as usual.

Existing template roots are updated when the holder receives a new template fragment. This keeps parent rerenders that replace the fragment connected to already-created roots.

## Limitations

Child-rendered template roots are renderer islands, not true descendants of the main renderer subtree.

Important consequences:

- Cascading values from the main renderer do not automatically flow into child renderers.
- Template content must produce its root before awaiting asynchronous work.
- Disposal is explicit: the holder owns and disposes created template roots.
- Event ordering can involve both the child renderer and main renderer when callbacks cross the renderer boundary.

These limitations are preferable to forcing the main renderer to process a nested render while `_isBatchInProgress` is true. Blazor's renderer deliberately batches and defers nested renders in that state, and bypassing that model would risk invalid render ordering.
