using BlazorBindings.Core;
using BlazorBindings.Maui;
using BlazorBindings.Maui.Elements.Handlers;
using BlazorBindings.UnitTests.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Reflection;

namespace BlazorBindings.UnitTests;

/// <summary>
/// The nested synchronous render reaches into two <c>Renderer</c> internals, neither of which
/// carries a compatibility guarantee. These tests exist so that a framework change shows up here,
/// with an actionable message, rather than in an app at runtime - and so that the documented way to
/// turn the whole thing off keeps working.
/// </summary>
public class SyncRenderFrameworkInternalsTests
{
    private const BindingFlags Instance = BindingFlags.Instance | BindingFlags.NonPublic;

    private MC.Application _application;
    private TestBlazorBindingsRenderer _renderer;

    [SetUp]
    public void SetUp()
    {
        _application = TestApplication.Create();
        _renderer = (TestBlazorBindingsRenderer)_application.Handler.MauiContext.Services
            .GetRequiredService<MauiBlazorBindingsRenderer>();
        MC.Application.Current = _application;
    }

    [TearDown]
    public void TearDown()
        => AppContext.SetSwitch(NativeComponentRenderer.DisableSynchronousNestedRenderSwitch, false);

    // ------------------------------------------------------------------ the internals themselves

    /// <summary>
    /// Names each dependency separately, so a rename points straight at what moved instead of
    /// surfacing as a NullReferenceException or MissingFieldException from inside a render.
    /// </summary>
    [Test]
    public void RendererStillExposesTheInternalsWeDependOn()
    {
        var rendererType = typeof(Microsoft.AspNetCore.Components.RenderTree.Renderer);

        var batchInProgress = rendererType.GetField("_isBatchInProgress", Instance);
        var batchBuilder = rendererType.GetField("_batchBuilder", Instance);

        Assert.Multiple(() =>
        {
            Assert.That(batchInProgress, Is.Not.Null,
                "Renderer._isBatchInProgress is gone. See NativeComponentRenderer.BatchInProgressFlag; "
                + "either adapt it or default the DisableSynchronousNestedRender switch to true.");
            Assert.That(batchInProgress?.FieldType, Is.EqualTo(typeof(bool)),
                "Renderer._isBatchInProgress is no longer a bool; the UnsafeAccessor signature must match it.");

            Assert.That(batchBuilder, Is.Not.Null,
                "Renderer._batchBuilder is gone. See NativeComponentRenderer.SwappedBatchBuilder; "
                + "either adapt it or default the DisableSynchronousNestedRender switch to true.");
            Assert.That(batchBuilder?.FieldType.IsValueType, Is.False,
                "Renderer._batchBuilder is no longer a reference type; it can no longer be swapped this way.");
            Assert.That(batchBuilder?.FieldType.GetConstructor(Type.EmptyTypes), Is.Not.Null,
                "The batch builder no longer has a parameterless constructor, so a fresh one cannot be "
                + "created for the nested batch. See NativeComponentRenderer.CreateBatchBuilder.");
        });
    }

    /// <summary>
    /// The behavioural failure mode that would not throw: if the framework stopped letting a nested
    /// batch run on a swapped builder, this is what would start going wrong silently.
    /// </summary>
    [Test]
    public void NestedRenderStillMaterializesAndLeavesTheOuterBatchIntact()
    {
        var nested = new RootContainerHandler();
        Exception caught = null;
        var attempted = false;

        var container = new ProbingContainerHandler(_ =>
        {
            if (attempted)
                return;
            attempted = true;

            try
            {
                _renderer.Render<WrapperComponent>(nested);
            }
            catch (Exception ex)
            {
                caught = ex;
            }
        });

        _renderer.Render<WrapperComponent>(container);

        Assert.Multiple(() =>
        {
            Assert.That(attempted, Is.True, "the nested render never ran, so this proves nothing");
            Assert.That(caught, Is.Null,
                $"Nested render stopped working - the framework's batching may have changed: {caught}");
            Assert.That(nested.Elements, Has.Count.EqualTo(1), "nested element was not materialized");
            Assert.That(container.Elements, Has.Count.EqualTo(1), "outer element was lost");
        });
    }

    // ------------------------------------------------------------------ the documented opt-out

    [Test]
    public void DisablingTheSwitchRefusesNestedRenderInsteadOfTouchingInternals()
    {
        AppContext.SetSwitch(NativeComponentRenderer.DisableSynchronousNestedRenderSwitch, true);

        bool? result = null;

        var container = new ProbingContainerHandler(_ =>
            result ??= _renderer.TryRenderSynchronously(() => { }));

        _renderer.Render<WrapperComponent>(container);

        Assert.That(result, Is.False,
            "With the switch set, a render requested during a batch must be refused.");
    }

    [Test]
    public void DisablingTheSwitchRestoresTheDeterministicErrorDuringABatch()
    {
        AppContext.SetSwitch(NativeComponentRenderer.DisableSynchronousNestedRenderSwitch, true);

        Exception caught = null;
        var nested = new RootContainerHandler();

        var container = new ProbingContainerHandler(_ =>
        {
            if (caught is not null)
                return;

            try
            {
                _renderer.Render<WrapperComponent>(nested);
            }
            catch (Exception ex)
            {
                caught = ex;
            }
        });

        _renderer.Render<WrapperComponent>(container);

        Assert.Multiple(() =>
        {
            Assert.That(caught, Is.InstanceOf<InvalidOperationException>());
            Assert.That(caught?.Message, Does.Contain("did not render synchronously"));

            // Still must not leave a root behind that renders later.
            Assert.That(nested.Elements, Is.Empty, "a refused render leaked its element");
        });
    }

    /// <summary>
    /// The opt-out must only give up rendering *during* a batch - ordinary rendering, which is the
    /// overwhelmingly common case, has to keep working with no internals involved at all.
    /// </summary>
    [Test]
    public void DisablingTheSwitchKeepsOrdinaryRenderingWorking()
    {
        AppContext.SetSwitch(NativeComponentRenderer.DisableSynchronousNestedRenderSwitch, true);

        var container = new RootContainerHandler();

        var render = _renderer.Render<WrapperComponent>(container);

        Assert.Multiple(() =>
        {
            Assert.That(container.Elements, Has.Count.EqualTo(1));
            Assert.That(render.Quiescence.IsCompleted, Is.True);
        });
    }

    [Test]
    public async Task DisablingTheSwitchKeepsAddComponentWorking()
    {
        AppContext.SetSwitch(NativeComponentRenderer.DisableSynchronousNestedRenderSwitch, true);

        var container = new RootContainerHandler();

        await _renderer.AddComponent<WrapperComponent>(container);

        Assert.That(container.Elements, Has.Count.EqualTo(1));
    }

    private class ProbingContainerHandler(Action<object> onAddChild) : IContainerElementHandler, INonPhysicalChild
    {
        public List<object> Elements { get; } = [];

        void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
        {
            Elements.Add(child);
            onAddChild(child);
        }

        void IContainerElementHandler.RemoveChild(int physicalSiblingIndex) => Elements.RemoveAt(physicalSiblingIndex);

        object IElementHandler.TargetElement => null;
        void INonPhysicalChild.SetParent(object parentElement) { }
        void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    }

    private class WrapperComponent : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<TestContainerComponent>(0);
            builder.CloseComponent();
        }
    }
}
