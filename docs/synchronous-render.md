# Synchronous rendering

Some MAUI APIs demand a native element from a **synchronous** callback, with no opportunity to
await:

- a `DataTemplate` or `ControlTemplate` being realized — for example
  `ShellContent.ContentTemplate`;
- `Application.CreateWindow`, which must return a `Window` there and then.

`NativeComponentRenderer.Render<T>()` serves those callers. It returns only once the component's
first render batch has been applied, so the native elements it creates are already attached:

```csharp
var render = renderer.Render<MyPage>(handler);

var element = handler.TargetElement;  // already present, or Render threw
await render;                         // optional: waits for OnInitializedAsync etc.
```

`TryRenderSynchronously(Action)` is the lower-level form, for re-rendering a component that already
exists. The render must be requested *inside* the callback:

```csharp
if (!renderer.TryRenderSynchronously(RequestRender))
    RequestRender();   // could not be done synchronously; fall back
```

## Why this needs Blazor internals

Blazor's `Renderer` only *queues* a render while a batch is in progress — `AddToRenderQueue` skips
`ProcessPendingRender()` when `_isBatchInProgress` is set. So when MAUI asks for an element in the
middle of the batch that is creating it, `StateHasChanged` cannot satisfy that request, and there is
no public way to ask the renderer to process a nested batch.

To do it anyway, `NativeComponentRenderer` touches two `Renderer` internals:

| Internal | Accessed via | Why |
| --- | --- | --- |
| `_isBatchInProgress` | `UnsafeAccessor` | cleared for the duration of the nested batch, so queued renders are processed instead of deferred |
| `_batchBuilder` | `FieldInfo` | swapped for a fresh builder, so the nested batch cannot clear the outer batch's — those arrays are pool-rented and the outer `RenderBatch` is still read afterwards by `InvokeRenderCompletedCalls` |

`UnsafeAccessor` cannot replace the field swap: its field accessors must return byref, and
`UnsafeAccessorType` is rejected there, so the inaccessible field type cannot be named. That is
deliberate — see [dotnet/runtime#119664][119664], which waits on a usable `TypedReference`
([dotnet/runtime#26186][26186]). Both are milestone *Future*.

A nested render is only attempted once the outer batch has finished reading the render tree. While
it is still reading, the request is refused and the caller gets a deterministic
`InvalidOperationException` rather than an element that does not exist yet.

**Neither internal carries a compatibility guarantee.** This is the risk the feature accepts.

## If a future Blazor version breaks it

Two shapes of breakage are possible:

1. **A rename or reshape.** The nested render fails on first use with a missing field or type. Loud
   and immediate.
2. **A behavioural change** — the names stay but the surrounding logic moves. This is the dangerous
   one, because nothing need throw.

`src/BlazorBindings.UnitTests/SyncRenderFrameworkInternalsTests.cs` is the early-warning system for
both. `RendererStillExposesTheInternalsWeDependOn` names each dependency separately, so a rename
points at exactly what moved; `NestedRenderStillMaterializesAndLeavesTheOuterBatchIntact` covers the
behavioural case. `SyncRenderDuringBatchTests` then checks the observable invariants that must hold
regardless of how the internals are shaped — the outer batch's tree and removals surviving, its
`OnAfterRender`/`OnAfterRenderAsync` still firing, and its pooled arrays not being recycled early.

Note that this repository does not set `TreatWarningsAsErrors`, so a trim/AOT regression would
appear in the CI log without failing the build.

## Turning it off

There is no need to downgrade. Set the documented switch:

```csharp
AppContext.SetSwitch("BlazorBindings.DisableSynchronousNestedRender", true);
```

or, declaratively, in `runtimeconfig.template.json`:

```json
{
  "configProperties": {
    "BlazorBindings.DisableSynchronousNestedRender": true
  }
}
```

The name is also available as
`NativeComponentRenderer.DisableSynchronousNestedRenderSwitch`.

With the switch set, **neither framework internal is touched at all** — the opt-out is checked
before either is read, so it still works when one of them has been renamed away.

### What you give up

Only the ability to render *while a batch is being applied*. Specifically:

- `Render<T>()` and `TryRenderSynchronously` keep working everywhere else, including the
  `CreateWindow` path and inside event callbacks.
- `AddComponent` is unaffected.
- A MAUI template realized mid-batch throws again, as it did before this support existed. For
  `ShellContent.ContentTemplate` that is
  `"Template root control is supposed to be rendered at this point."`

In practice MAUI usually realizes templates outside our batches — on navigation or layout — so most
apps never hit that path. The switch returns you to exactly the earlier behaviour, no worse.

## Known limitation

A synchronous render may not modify the element that is *currently* being updated. The outer
application walks its pending edits by index and clears them at the end, so adding to that list
mid-flight would replay edits already made and discard the rest. That is refused with a clear
error rather than silently corrupting the tree — render into a different parent, or use
`AddComponent` and await it.

[119664]: https://github.com/dotnet/runtime/issues/119664
[26186]: https://github.com/dotnet/runtime/issues/26186
