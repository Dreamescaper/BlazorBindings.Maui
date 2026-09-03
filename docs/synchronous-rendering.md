# Synchronous Rendering

`NativeComponentRenderer` (in `BlazorBindings.Core`) is a custom Blazor `Renderer` that projects a
component tree onto native MAUI controls instead of HTML. This document explains the design behind
its **synchronous render** support: the ability to render a component and have its first native
element available immediately, without awaiting anything.

> **Status:** experimental. The public API (`Render`, `NativeRender<T>`, `TryRenderSynchronously`) is
> marked `[Experimental]`/"subject to change" and relies on one non-public Blazor implementation
> detail (see [Risks](#risks-and-fallbacks)).

## The problem

Blazor's `Renderer` is fundamentally asynchronous: `RenderRootComponentAsync` and
`Component.SetParametersAsync` return `Task`s, and `UpdateDisplayAsync` is awaited by the renderer's
internal render queue. That is a fine model for HTML, where nothing needs a DOM node back
synchronously.

MAUI is not that forgiving. Several native APIs are **synchronous factory callbacks** that must
return a fully-formed native object before they return control to MAUI:

- `Application.CreateWindow(...)` must return a `Window` — there is no async overload.
- A `DataTemplate`'s `LoadTemplate` callback must return the realized view synchronously; MAUI calls
  it while it is itself mid-layout, and it caches/measures whatever comes back.

Historically the top-level case was worked around with an ad-hoc blocking helper:
[`ApplicationWindowHandler`](../src/BlazorBindings.Maui/Elements/Handlers/ApplicationWindowHandler.cs)
held a `TaskCompletionSource` that `CreateWindow` blocked on until `AddChild` supplied a `Window`. That
only handled the "top-level, nothing else is rendering" case. `SyncControlTemplateItemsComponent`
(data templates) needed a render to happen **while a render batch was already being processed** — a
case the old approach couldn't handle at all, because Blazor's renderer refuses to start a new batch
while one is in flight.

The goal of this work is a general primitive: *render this component now, synchronously, and give me
back its native element — whether or not the renderer happens to be idle.*

## Design overview

Three pieces work together:

1. **`Render<TComponent>(parent, parameters)`** — the public entry point. Starts a root component
   render and returns as soon as its first render batch has been *applied* (native elements exist),
   without waiting for async lifecycle methods (`OnInitializedAsync`, etc.) to finish.
2. **`NativeRender<TComponent>`** — the struct returned by `Render`. Exposes `Component` (available
   immediately) and `Quiescence` (a `Task`, awaitable via the struct itself, that completes once all
   async lifecycle work across the subtree has finished — the same completion Blazor calls
   "quiescence").
3. **`TryRenderSynchronously(Action requestRender)`** — a lower-level primitive for a render that
   needs to happen **during** an in-progress batch (the data-template case). It runs a nested,
   isolated render batch on top of the one currently being applied, then hands control back.

Both `Render` and `TryRenderSynchronously` bottom out in the same mechanism: forcing a render to be
processed immediately instead of merely being queued.

### Why "just await it" isn't enough

`RenderRootComponentAsync` already renders synchronously *as far as it can* — `SetParametersAsync`
runs to completion before yielding at the first genuine `await`. The problem is guaranteeing that
happens **outside** whatever batch is currently being read/applied. If a batch is already in
progress, a nested render request is only *queued* (`AddToRenderQueue` sees `_isBatchInProgress` and
defers), so `quiescence` returned by `RenderRootComponentAsync` would resolve before the component
had rendered at all. `Render` therefore always ensures the new root component starts outside the
current batch (see `StartRootComponentRender` / `TryRunOutsideCurrentBatch` in
[`NativeComponentRenderer.cs`](../src/BlazorBindings.Core/NativeComponentRenderer.cs)), and throws
`InvalidOperationException` if, after that, the adapter still hasn't received a batch
(`adapter.HasRendered`) — e.g. because the component's `Attach`/`SetParametersAsync` never called
into the render pipeline synchronously.

### Render phases

`UpdateDisplayAsync` — the one method Blazor calls to hand a renderer a batch to apply — is split
into two phases, tracked by a private `RenderPhase` enum (`Idle` / `ReadingRenderTree` /
`ApplyingToNativeTree`):

- **`ReadingRenderTree`**: the loop over `renderBatch.UpdatedComponents`, plus the frame/edit data
  Blazor hands us. These live in buffers the *renderer itself* reuses across batches, so nothing else
  may run here — a nested render would corrupt state the outer batch is still reading.
- **`ApplyingToNativeTree`**: once the edits of interest have been captured
  (`adaptersWithPendingEdits`, a copy of `DisposedComponentIDs`), only `NativeComponentAdapter`
  objects we own are touched. This is the window where a nested render is safe.

`TryRunOutsideCurrentBatch` (the shared helper behind both `Render` and `TryRenderSynchronously`)
uses this phase to decide what to do:

| Renderer state | Action |
|---|---|
| Idle, no batch in progress | Run `action` directly — it will drive its own batch to completion, same as today. |
| `ReadingRenderTree` | Refuse (`return false`) — the caller falls back to its non-synchronous path. |
| `ApplyingToNativeTree` | Run a **nested batch** (below), then resume the outer one. |

### Nested batches

When a render is requested from inside `ApplyingToNativeTree`, `TryRunOutsideCurrentBatch`:

1. Swaps out the renderer's private `_batchBuilder` field for a fresh instance
   (`SwappedBatchBuilder`), so the nested render's frames/edits can't collide with the outer batch's —
   the outer `RenderBatch` still points at the original builder's pooled arrays, and
   `ProcessRenderQueue` reads them again after `UpdateDisplayAsync` returns (for
   `InvokeRenderCompletedCalls`), so they must survive untouched.
2. Clears the private `_isBatchInProgress` flag (via an `UnsafeAccessor`, see below) so Blazor's own
   render pipeline believes it's free to build and apply a whole new batch.
3. Runs the caller's `action` — which requests a render and, since the renderer now looks idle, `push`es
   it through `AddToRenderQueue` → `ProcessPendingRender` → `UpdateDisplayAsync` synchronously, all the
   way to application on the native tree.
4. Restores the original batch builder and re-sets `_isBatchInProgress`, so the *outer* batch's own
   `finally` (back up the call stack, inside Blazor) returns its pooled arrays to `ArrayPool.Shared`
   exactly once, as if the nested render had never happened.

`_batchDepth` (incremented in `ProcessPendingRender` and around `RemoveRootComponent`) tracks nesting
depth independently of the phase enum and backs the public `IsProcessingBatch` — used by call sites
(and in error messages) to tell whether a synchronous render is even possible right now.

### `UnsafeAccessor` for `_isBatchInProgress`

Blazor's `Renderer._isBatchInProgress` is a private field with no public equivalent, and it's the one
piece of state that gates whether `AddToRenderQueue` processes immediately or defers. Rather than
reimplementing the renderer's queueing logic, this design reaches into that field directly via
`[UnsafeAccessor]`:

```csharp
[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_isBatchInProgress")]
private static extern ref bool BatchInProgressFlag(Renderer renderer);
```

This is a deliberate, narrow trapdoor: it avoids duplicating Blazor's batching semantics (which would
drift from the real implementation over time), at the cost of depending on a field that isn't part of
the public contract. `_batchBuilder` is reached the same way, but via reflection
(`FieldInfo.GetValue`/`SetValue` in `SwappedBatchBuilder`) rather than `UnsafeAccessor`, since it also
needs `Activator.CreateInstance` against the field's (non-public) type.

## The two call sites

### `AddComponent` — unchanged behavior, new implementation

`AddComponent` (the pre-existing, `async Task<TComponent>` API) is now implemented in terms of the
same `StartRootComponentRender` helper `Render` uses, but it awaits full quiescence rather than just
the first batch — its contract (wait for everything, including async lifecycle) is unchanged.

### `BlazorBindingsApplication.CreateWindow` — the synchronous top-level render

`CreateWindow` must return a `Window` before it returns, full stop. It now calls
`renderer.Render(componentType, handler, parameters)` and reads `handler.TargetElement` immediately
afterward — `Render` guarantees the element exists (or throws) by the time it returns. The old
`ApplicationWindowHandler.WaitForWindowAsync`/`TaskCompletionSource` plumbing is gone; it's no longer
needed once the renderer itself guarantees synchronous materialization.

### `SyncControlTemplateItemsComponent` — rendering mid-batch

This component realizes MAUI `DataTemplate`s. `AddTemplateRoot()` is called by MAUI *during layout*,
potentially while our own render batch (the one that put the template's owning control on screen) is
still being applied — i.e. from inside `ApplyingToNativeTree`. It wraps its render request
(`_count++; StateHasChanged();`) in `Renderer.TryRenderSynchronously(...)`:

- If the renderer is idle, `TryRenderSynchronously` just runs it directly (same as calling
  `StateHasChanged()` normally) and the existing render queue mechanics apply.
- If a batch is being applied, it runs the nested-batch path from [Nested batches](#nested-batches),
  so `StateHasChanged()`'s queued render is flushed and applied before `AddTemplateRoot` returns the
  new root view to MAUI.
- If `TryRenderSynchronously` returns `false` (i.e. mid-`ReadingRenderTree`, which shouldn't happen in
  practice for this call site but is handled defensively), it falls back to the plain
  `RequestRoot()` call — the old "just queue it" behavior.

## API summary

| Member | Purpose |
|---|---|
| `NativeComponentRenderer.Render<TComponent>(parent, parameters)` | Render a new root component, return once its first batch is applied. Throws `InvalidOperationException` if that isn't possible (e.g. called from inside `ReadingRenderTree`). |
| `NativeComponentRenderer.Render(Type, parent, parameters)` | Non-generic overload of the above. |
| `NativeRender<TComponent>` | `{ Component, Quiescence }` — `Component` is ready immediately; await the struct (or `Quiescence`) for async lifecycle completion. |
| `NativeComponentRenderer.TryRenderSynchronously(Action requestRender)` | Run `requestRender` and flush whatever render(s) it queues before returning, including nested inside an in-progress batch. Returns `false` (having done nothing) when unsafe to do so — caller must fall back. |
| `NativeComponentRenderer.IsProcessingBatch` (protected) | `true` while any batch (outer or nested) is being read or applied — used to give a precise error when a synchronous render is attempted from an unsupported point (e.g. `Dispose`, `OnAfterRender`). |
| `NativeComponentAdapter.HasRendered` (internal) | Set once an adapter's component appears in an applied batch; used to detect a `Render` call that didn't actually materialize synchronously. |

## Risks and fallbacks

- **Depends on Blazor internals.** `_isBatchInProgress` and `_batchBuilder` are private fields of
  `Microsoft.AspNetCore.Components.Renderer`. `[UnsafeAccessor]` binds to them by name/type, so a
  renamed or restructured field in a future `Microsoft.AspNetCore.Components` version fails at JIT
  time (`UnsafeAccessor`) or via a `null` `FieldInfo` (reflection lookup for `_batchBuilder`) rather
  than silently misbehaving — but it does mean an upstream update can require a corresponding change
  here. `src/BlazorBindings.UnitTests/SyncFlushTests.cs` includes a test
  (`NestedRenderWorksAgainstCurrentFrameworkInternals`) specifically to catch this early.
- **Not every call site can render synchronously.** If a `Render` call can't be started outside the
  current batch (i.e. it's requested from inside `ReadingRenderTree`, such as from another
  component's `Dispose` or `OnAfterRender`), it throws rather than silently deferring — callers in
  that position should use `AddComponent` and await it instead. The exception message says which case
  applies (based on `IsProcessingBatch`).
- **First-render exceptions are unwrapped.** If the component's first render throws, `Render` rethrows
  the *actual* exception via `ExceptionDispatchInfo` instead of letting a generic Blazor "unhandled
  exception" surface (`ExceptionDuringFirstRenderIsSurfacedInsteadOfBarrierError`).
- **Ordering/pooling correctness is load-bearing.** The nested-batch swap exists specifically so the
  *outer* batch's pooled arrays are never touched by a nested render and are returned to
  `ArrayPool.Shared` exactly once. `SyncFlushTests` (`OuterBatchArraysAreNotRecycledByThePoolDuringNestedFlush`,
  `BorrowedBatchBuffersAreRestoredAfterTheFlush`, `RepeatedNestedFlushesDuringOneBatch`,
  `FlushNestedInsideAnotherFlushIsHandled`) exercise this directly, along with disposal ordering
  (`OuterBatchRemovalsSurviveNestedFlush`, `FlushDoesNotChangeDisposalOfComponentsRemovedByTheOuterBatch`)
  and `OnAfterRender(Async)` firing correctly around a nested flush.

## Test coverage

- **`SynchronousRenderTests`** — the public `Render`/`NativeRender<T>` surface: element availability
  immediately, quiescence semantics, calling from inside an event callback and from inside a native
  apply phase, the non-generic overload, error paths (`NeverRendersComponent`, `ThrowingComponent`).
- **`SyncTemplateDuringBatchTests`** — `SyncControlTemplateItemsComponent`-style usage: realizing a
  template outside vs. during a batch, repeated realizations, and that the realized template is the
  fully-built subtree.
- **`SyncFlushTests`** — the nested-batch mechanism itself, independent of any particular call site:
  phase gating, pool/array safety, disposal ordering, and `OnAfterRender`/`OnAfterRenderAsync`
  correctness across nested flushes.
