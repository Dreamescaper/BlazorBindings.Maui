using BlazorBindings.Core;
using BlazorBindings.Maui;
using BlazorBindings.Maui.Elements.Handlers;
using BlazorBindings.UnitTests.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Buffers;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlazorBindings.UnitTests;

/// <summary>
/// Tests for the nested synchronous render path - rendering while a batch is already being applied.
/// <para>
/// The renderer's batch buffers are shared and pool-backed, so a nested batch has to be handed a
/// clean builder and the outer batch's arrays given back afterwards. These tests cover the refusal
/// paths, that the outer batch survives intact, and that component lifecycle still runs correctly.
/// </para>
/// </summary>
public class SyncFlushTests
{
    private MC.Application _application;
    private TestBlazorBindingsRenderer _renderer;

    [SetUp]
    public void SetUp()
    {
        _application = TestApplication.Create();
        _renderer = (TestBlazorBindingsRenderer)_application.Handler.MauiContext.Services
            .GetRequiredService<MauiBlazorBindingsRenderer>();
        MC.Application.Current = _application;
        Probe.Reset();
    }

    private NativeRender<RenderFragmentComponent> Render(IElementHandler container, RenderFragment fragment)
        => _renderer.Render<RenderFragmentComponent>(container, new() { ["RenderFragment"] = fragment });

    private static RenderFragment Single<T>() where T : IComponent => builder =>
    {
        builder.OpenComponent<T>(0);
        builder.CloseComponent();
    };

    // ------------------------------------------------------------------ refusal paths

    /// <summary>
    /// Canary for the framework internals the nested render depends on. If a future framework
    /// version moves or reshapes them, this is the test that fails.
    /// </summary>
    [Test]
    public void NestedRenderWorksAgainstCurrentFrameworkInternals()
    {
        bool? result = null;

        var container = new ProbingContainerHandler(_ => result ??= _renderer.TryRenderSynchronously(() => { }));
        Render(container, Single<TestContainerComponent>());

        Assert.That(result, Is.True,
            "Nested render was refused during the native-apply phase - the framework internals it relies on may have moved.");
    }

    [Test]
    public void FlushOutsideAnyBatchReportsSuccess()
    {
        Assert.That(_renderer.TryRenderSynchronously(() => { }), Is.True);
    }

    [Test]
    public void FlushIsRefusedWhileReadingRenderTree()
    {
        bool? flushResult = null;

        RenderWithNonPhysicalProbe(() => flushResult = _renderer.TryRenderSynchronously(() => { }));

        Assert.That(flushResult, Is.False,
            "Flushing while the shared batch buffers are still being read must be refused.");
    }

    [Test]
    public void RenderDuringReadPhaseThrowsDeterministically()
    {
        Exception caught = null;

        RenderWithNonPhysicalProbe(() =>
        {
            try
            {
                Render(new RootContainerHandler(), Single<TestContainerComponent>());
            }
            catch (Exception ex)
            {
                caught = ex;
            }
        });

        Assert.Multiple(() =>
        {
            Assert.That(caught, Is.InstanceOf<InvalidOperationException>());
            Assert.That(caught?.Message, Does.Contain("did not render synchronously"));
            Assert.That(caught?.Message, Does.Contain("render batch was being processed"));
        });
    }

    // ------------------------------------------------------------------ the outer batch survives

    [Test]
    public void OuterBatchStaysIntactAfterNestedFlush()
    {
        RecordingContainerComponent.RecordingContainerTarget nested = null;
        Exception caught = null;

        var outerContainer = new ProbingContainerHandler(Once(_ =>
        {
            try
            {
                var nestedContainer = new RootContainerHandler();
                Render(nestedContainer, RecordingFragment(10, 11, 12));
                nested = (RecordingContainerComponent.RecordingContainerTarget)nestedContainer.Elements[0];
            }
            catch (Exception ex)
            {
                caught = ex;
            }
        }));

        Render(outerContainer, RecordingFragment(1, 2, 3));

        var outer = (RecordingContainerComponent.RecordingContainerTarget)outerContainer.Elements[0];

        Assert.Multiple(() =>
        {
            Assert.That(caught, Is.Null, $"Nested render failed: {caught}");
            Assert.That(outer.Children.Select(c => c.Id), Is.EqualTo(new[] { 1, 2, 3 }), "outer tree corrupted");
            Assert.That(nested?.Children.Select(c => c.Id), Is.EqualTo(new[] { 10, 11, 12 }), "nested tree corrupted");
        });
    }

    [Test]
    public void RepeatedNestedFlushesDuringOneBatch()
    {
        var nestedTargets = new List<RecordingContainerComponent.RecordingContainerTarget>();
        Exception caught = null;

        var outerContainer = new ProbingContainerHandler(Once(_ =>
        {
            try
            {
                for (var i = 0; i < 10; i++)
                {
                    var nestedContainer = new RootContainerHandler();
                    Render(nestedContainer, RecordingFragment(100 + i));
                    nestedTargets.Add((RecordingContainerComponent.RecordingContainerTarget)nestedContainer.Elements[0]);
                }
            }
            catch (Exception ex)
            {
                caught = ex;
            }
        }));

        Render(outerContainer, RecordingFragment(1, 2, 3));

        var outer = (RecordingContainerComponent.RecordingContainerTarget)outerContainer.Elements[0];

        Assert.Multiple(() =>
        {
            Assert.That(caught, Is.Null, $"Nested render failed: {caught}");
            Assert.That(outer.Children.Select(c => c.Id), Is.EqualTo(new[] { 1, 2, 3 }), "outer tree corrupted");
            Assert.That(nestedTargets, Has.Count.EqualTo(10));
            for (var i = 0; i < nestedTargets.Count; i++)
                Assert.That(nestedTargets[i].Children.Select(c => c.Id), Is.EqualTo(new[] { 100 + i }));
        });
    }

    [Test]
    public void OuterBatchRemovalsSurviveNestedFlush()
    {
        var showMiddle = true;
        var flushed = false;
        Exception caught = null;

        var container = new ProbingContainerHandler(_ => { })
        {
            OnRemoveChild = () =>
            {
                if (flushed)
                    return;
                flushed = true;

                try
                {
                    Render(new RootContainerHandler(), RecordingFragment(99));
                }
                catch (Exception ex)
                {
                    caught = ex;
                }
            }
        };

        var render = Render(container, builder =>
        {
            builder.OpenComponent<RecordingChildComponent>(0);
            builder.AddAttribute(1, "Id", 1);
            builder.CloseComponent();

            if (showMiddle)
            {
                builder.OpenComponent<RecordingChildComponent>(2);
                builder.AddAttribute(3, "Id", 2);
                builder.CloseComponent();
            }

            builder.OpenComponent<RecordingChildComponent>(4);
            builder.AddAttribute(5, "Id", 3);
            builder.CloseComponent();
        });

        Assert.That(container.Elements.Cast<RecordingChildTarget>().Select(c => c.Id), Is.EqualTo(new[] { 1, 2, 3 }));

        showMiddle = false;
        render.Component.StateHasChanged();

        Assert.Multiple(() =>
        {
            Assert.That(caught, Is.Null, $"Nested render failed: {caught}");
            Assert.That(flushed, Is.True, "the flush did not happen, so this proves nothing");
            Assert.That(container.Elements.Cast<RecordingChildTarget>().Select(c => c.Id),
                Is.EqualTo(new[] { 1, 3 }), "Outer removal was lost or misapplied across the nested flush.");
        });
    }

    [Test]
    public void FlushNestedInsideAnotherFlushIsHandled()
    {
        Exception caught = null;
        var innerContainer = new RootContainerHandler();

        // outer batch -> flush -> middle batch -> flush -> inner batch
        var middleContainer = new ProbingContainerHandler(Once(_ =>
        {
            try
            {
                Render(innerContainer, Single<TestContainerComponent>());
            }
            catch (Exception ex)
            {
                caught = ex;
            }
        }));

        var outerContainer = new ProbingContainerHandler(Once(_ =>
        {
            try
            {
                Render(middleContainer, Single<TestContainerComponent>());
            }
            catch (Exception ex)
            {
                caught = ex;
            }
        }));

        Render(outerContainer, Single<TestContainerComponent>());

        Assert.Multiple(() =>
        {
            Assert.That(caught, Is.Null, $"Recursive flush failed: {caught}");
            Assert.That(outerContainer.Elements, Has.Count.EqualTo(1), "outer tree");
            Assert.That(middleContainer.Elements, Has.Count.EqualTo(1), "middle tree");
            Assert.That(innerContainer.Elements, Has.Count.EqualTo(1), "inner tree");
        });
    }

    // ------------------------------------------------------------------ borrowed buffers

    /// <summary>
    /// The flush lends the shared builder a clean set of arrays and must give the outer batch's
    /// own arrays back, with their counts, once the nested batch is done.
    /// </summary>
    [Test]
    public void BorrowedBatchBuffersAreRestoredAfterTheFlush()
    {
        (int ArrayId, int Count) before = default;
        (int ArrayId, int Count) after = default;

        var container = new ProbingContainerHandler(Once(_ =>
        {
            before = DiffBufferState();
            Render(new RootContainerHandler(), Single<TestContainerComponent>());
            after = DiffBufferState();
        }));

        Render(container, Single<TestContainerComponent>());

        Assert.Multiple(() =>
        {
            Assert.That(after.ArrayId, Is.EqualTo(before.ArrayId), "The outer batch's diff array was not restored.");
            Assert.That(after.Count, Is.EqualTo(before.Count), "The outer batch's diff count was not restored.");
        });
    }

    /// <summary>
    /// ArrayBuilder is pool-backed. If a nested batch were allowed to Clear() the builder while it
    /// still owned the outer batch's arrays, those would go back to ArrayPool.Shared while
    /// ProcessRenderQueue is still going to read them - it calls InvokeRenderCompletedCalls after
    /// UpdateDisplayAsync returns.
    /// </summary>
    [Test]
    public void OuterBatchArraysAreNotRecycledByThePoolDuringNestedFlush()
    {
        var outerArrayId = 0;
        var rentedArrayId = 0;

        var container = new ProbingContainerHandler(Once(_ =>
        {
            outerArrayId = DiffBufferState().ArrayId;

            Render(new RootContainerHandler(), Single<TestContainerComponent>());

            // Anything in the process may now rent that memory. Simulate that, and scribble on it.
            var rented = ArrayPool<RenderTreeDiff>.Shared.Rent(32);
            rentedArrayId = RuntimeHelpers.GetHashCode(rented);
            Array.Clear(rented);
        }));

        Render(container, TrackedTree());

        Assert.Multiple(() =>
        {
            Assert.That(rentedArrayId, Is.Not.EqualTo(outerArrayId),
                "The outer batch's live RenderBatch array was returned to the pool and rented out again.");
            Assert.That(Probe.AfterRenderCalls.Order(), Is.EqualTo(new[] { 1, 2, 3 }),
                "Outer batch's OnAfterRender was lost.");
        });
    }

    // ------------------------------------------------------------------ lifecycle around a flush

    [Test]
    public void BaselineOnAfterRenderFiresForEveryComponent()
    {
        Render(new RootContainerHandler(), TrackedTree());

        Assert.That(Probe.AfterRenderCalls.Order(), Is.EqualTo(new[] { 1, 2, 3 }));
    }

    [Test]
    public void OnAfterRenderStillFiresWhenNestedFlushHappens()
    {
        var container = new ProbingContainerHandler(Once(_ =>
            Render(new RootContainerHandler(), Single<TestContainerComponent>())));

        Render(container, TrackedTree());

        Assert.That(Probe.AfterRenderCalls.Order(), Is.EqualTo(new[] { 1, 2, 3 }),
            "OnAfterRender must still fire exactly once for each outer component.");
    }

    [Test]
    public void OnAfterRenderSurvivesTwoNestedFlushesInOneBatch()
    {
        var flushes = 0;

        var container = new ProbingContainerHandler(Once(_ =>
        {
            // The second flush renders a wide tree so its diffs land on the slots the outer
            // batch's own diffs occupy - which is where any buffer reuse would show up.
            for (var i = 0; i < 2; i++)
            {
                flushes++;
                Render(new RootContainerHandler(), builder =>
                {
                    for (var j = 0; j < 8; j++)
                    {
                        builder.OpenComponent<TestContainerComponent>(j);
                        builder.CloseComponent();
                    }
                });
            }
        }));

        Render(container, TrackedTree());

        Assert.Multiple(() =>
        {
            Assert.That(flushes, Is.EqualTo(2));
            Assert.That(Probe.AfterRenderCalls.Order(), Is.EqualTo(new[] { 1, 2, 3 }));
        });
    }

    [Test]
    public async Task OnAfterRenderAsyncStillRunsWhenAFlushHappens()
    {
        var container = new ProbingContainerHandler(Once(_ =>
            Render(new RootContainerHandler(), Single<TestContainerComponent>())));

        Render(container, Single<AsyncAfterRenderComponent>());

        // OnAfterRenderAsync is not part of quiescence, so wait for its own signal.
        var finished = await Task.WhenAny(AsyncAfterRenderComponent.Finished, Task.Delay(5000));

        Assert.Multiple(() =>
        {
            Assert.That(finished, Is.SameAs(AsyncAfterRenderComponent.Finished), "OnAfterRenderAsync never completed.");
            Assert.That(Probe.AsyncAfterRenders, Is.EqualTo(1));
        });
    }

    [Test]
    public void FlushDoesNotChangeDisposalOfComponentsRemovedByTheOuterBatch()
    {
        var withoutFlush = RunRemoval(flushDuringRemoval: false, out _);
        Probe.Reset();
        var withFlush = RunRemoval(flushDuringRemoval: true, out var flushed);

        Assert.Multiple(() =>
        {
            Assert.That(flushed, Is.True, "the flush did not happen, so this proves nothing");
            Assert.That(withoutFlush, Is.GreaterThan(0), "baseline: removal should dispose the component");
            Assert.That(withFlush, Is.EqualTo(withoutFlush),
                "The nested flush changed how many times a removed component is disposed.");
        });
    }

    /// <summary>
    /// A component rendered during a flush must still get its own async lifecycle. This regressed
    /// once: the render was queued, so RenderRootComponentAsync saw no pending work and reported
    /// quiescence before the component had rendered at all.
    /// </summary>
    [Test]
    public async Task ComponentRenderedDuringAFlushStillReachesQuiescence()
    {
        NativeRender<RenderFragmentComponent> nested = default;
        var nestedContainer = new RootContainerHandler();

        var container = new ProbingContainerHandler(Once(_ =>
            nested = Render(nestedContainer, Single<AsyncInitComponent>())));

        Render(container, Single<TestContainerComponent>());

        Assert.Multiple(() =>
        {
            Assert.That(nestedContainer.Elements, Has.Count.EqualTo(1), "first render must have materialized");
            Assert.That(nested.Quiescence.IsCompleted, Is.False, "async lifecycle should still be pending");
        });

        AsyncInitComponent.Release();
        await nested;

        Assert.That(Probe.AsyncInitsCompleted, Is.EqualTo(1));
    }

    // ------------------------------------------------------------------ helpers

    /// <summary>Wraps a container callback so it only fires on the first child.</summary>
    private static Action<object> Once(Action<object> action)
    {
        var done = false;
        return child =>
        {
            if (done)
                return;
            done = true;
            action(child);
        };
    }

    private void RenderWithNonPhysicalProbe(Action onSetParent)
    {
        var probe = new NonPhysicalProbe(onSetParent);

        // SetParent for a non-physical child runs while we are still reading the batch buffers.
        Render(new RootContainerHandler(), builder =>
        {
            builder.OpenComponent<RecordingContainerComponent>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(inner =>
            {
                inner.OpenComponent<NonPhysicalProbeComponent>(0);
                inner.AddAttribute(1, nameof(NonPhysicalProbeComponent.Probe), probe);
                inner.CloseComponent();
            }));
            builder.CloseComponent();
        });
    }

    private int RunRemoval(bool flushDuringRemoval, out bool flushed)
    {
        var show = true;
        var didFlush = false;

        var container = new ProbingContainerHandler(_ => { })
        {
            OnRemoveChild = () =>
            {
                if (!flushDuringRemoval || didFlush)
                    return;
                didFlush = true;
                Render(new RootContainerHandler(), Single<TestContainerComponent>());
            }
        };

        var render = Render(container, builder =>
        {
            if (show)
            {
                builder.OpenComponent<DisposableProbeComponent>(0);
                builder.CloseComponent();
            }
        });

        Assert.That(Probe.Disposals, Is.Zero, "not disposed yet");

        show = false;
        render.Component.StateHasChanged();

        flushed = didFlush;
        return Probe.Disposals;
    }

    private static RenderFragment RecordingFragment(params int[] ids) => builder =>
    {
        builder.OpenComponent<RecordingContainerComponent>(0);
        builder.AddAttribute(1, "ChildContent", (RenderFragment)(inner =>
        {
            var sequence = 0;
            foreach (var id in ids)
            {
                inner.OpenComponent<RecordingChildComponent>(sequence++);
                inner.AddAttribute(sequence++, "Id", id);
                inner.CloseComponent();
            }
        }));
        builder.CloseComponent();
    };

    /// <summary>A tree of components that record their OnAfterRender.</summary>
    private static RenderFragment TrackedTree() => builder =>
    {
        builder.OpenComponent<RecordingContainerComponent>(0);
        builder.AddAttribute(1, "ChildContent", (RenderFragment)(inner =>
        {
            for (var i = 0; i < 3; i++)
            {
                inner.OpenComponent<AfterRenderProbe>(i * 3);
                inner.AddAttribute(i * 3 + 1, nameof(AfterRenderProbe.Id), i + 1);
                inner.CloseComponent();
            }
        }));
        builder.CloseComponent();
    };

    private (int ArrayId, int Count) DiffBufferState()
    {
        var batchBuilder = typeof(Microsoft.AspNetCore.Components.RenderTree.Renderer)
            .GetField("_batchBuilder", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_renderer);
        var builder = batchBuilder.GetType().GetProperty("UpdatedComponentDiffs").GetValue(batchBuilder);
        var items = (Array)builder.GetType()
            .GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(builder);
        var count = (int)builder.GetType()
            .GetField("_itemsInUse", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(builder);

        return (RuntimeHelpers.GetHashCode(items), count);
    }

    private static class Probe
    {
        public static readonly List<int> AfterRenderCalls = [];
        public static int Disposals;
        public static int AsyncAfterRenders;
        public static int AsyncInitsCompleted;

        public static void Reset()
        {
            AfterRenderCalls.Clear();
            Disposals = 0;
            AsyncAfterRenders = 0;
            AsyncInitsCompleted = 0;
        }
    }

    private class ProbingContainerHandler(Action<object> onAddChild) : IContainerElementHandler, INonPhysicalChild
    {
        public List<object> Elements { get; } = [];
        public Action OnRemoveChild { get; init; }

        void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
        {
            Elements.Insert(Math.Min(physicalSiblingIndex, Elements.Count), child);
            onAddChild(child);
        }

        void IContainerElementHandler.RemoveChild(int physicalSiblingIndex)
        {
            Elements.RemoveAt(physicalSiblingIndex);
            OnRemoveChild?.Invoke();
        }

        object IElementHandler.TargetElement => null;
        void INonPhysicalChild.SetParent(object parentElement) { }
        void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    }

    private class NonPhysicalProbe(Action onSetParent)
    {
        public void SetParent() => onSetParent();
    }

    private class NonPhysicalProbeComponent : NativeControlComponentBase, INonPhysicalChild, IElementHandler
    {
        [Parameter] public NonPhysicalProbe Probe { get; set; }

        public object TargetElement => null;

        void INonPhysicalChild.SetParent(object parentElement) => Probe.SetParent();
        void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    }

    private class DisposableProbeComponent : NativeControlComponentBase, IElementHandler, IDisposable
    {
        public object TargetElement { get; } = new TestContainerComponent.TestTargetElement();

        public void Dispose() => Probe.Disposals++;
    }

    private class AfterRenderProbe : ComponentBase
    {
        [Parameter] public int Id { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                Probe.AfterRenderCalls.Add(Id);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<RecordingChildComponent>(0);
            builder.AddAttribute(1, "Id", Id);
            builder.CloseComponent();
        }
    }

    private class AsyncAfterRenderComponent : ComponentBase
    {
        private static TaskCompletionSource _finished = new();

        public static Task Finished => _finished.Task;

        public AsyncAfterRenderComponent() => _finished = new TaskCompletionSource();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;

            await Task.Yield();
            Probe.AsyncAfterRenders++;
            _finished.TrySetResult();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<TestContainerComponent>(0);
            builder.CloseComponent();
        }
    }

    private class AsyncInitComponent : ComponentBase
    {
        private static TaskCompletionSource _gate = new();

        public static void Release() => _gate.TrySetResult();

        public AsyncInitComponent() => _gate = new TaskCompletionSource();

        protected override async Task OnInitializedAsync()
        {
            await _gate.Task;
            Probe.AsyncInitsCompleted++;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<TestContainerComponent>(0);
            builder.CloseComponent();
        }
    }

    public class RenderFragmentComponent : ComponentBase
    {
        [Parameter] public RenderFragment RenderFragment { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder) => RenderFragment(builder);

        public new void StateHasChanged() => base.StateHasChanged();
    }
}
