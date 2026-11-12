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
/// with an actionable message, rather than in an app at runtime. docs/synchronous-render.md
/// describes how to back the feature out if one of them does change.
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
                "Renderer._isBatchInProgress is gone. See NativeComponentRenderer.BatchInProgressFlag, and "
                + "'How to back it out' in docs/synchronous-render.md.");
            Assert.That(batchInProgress?.FieldType, Is.EqualTo(typeof(bool)),
                "Renderer._isBatchInProgress is no longer a bool; the UnsafeAccessor signature must match it.");

            Assert.That(batchBuilder, Is.Not.Null,
                "Renderer._batchBuilder is gone. See NativeComponentRenderer.SwappedBatchBuilder, and "
                + "'How to back it out' in docs/synchronous-render.md.");
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
