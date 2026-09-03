using BlazorBindings.Core;
using BlazorBindings.Maui;
using BlazorBindings.Maui.Elements.Handlers;
using BlazorBindings.UnitTests.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorBindings.UnitTests;

/// <summary>
/// Tests for the synchronous first-materialization guarantee of
/// <see cref="NativeComponentRenderer.Render{TComponent}(IElementHandler, Dictionary{string, object})"/>.
/// </summary>
public class SynchronousRenderTests
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
    }

    private NativeRender<RenderFragmentComponent> Render(IElementHandler container, RenderFragment fragment)
        => _renderer.Render<RenderFragmentComponent>(container, new() { ["RenderFragment"] = fragment });

    private static RenderFragment Fragment<T>() where T : IComponent
        => builder =>
        {
            builder.OpenComponent<T>(0);
            builder.CloseComponent();
        };

    [Test]
    public void ElementIsAvailableSynchronously()
    {
        var container = new RootContainerHandler();

        Render(container, Fragment<TestContainerComponent>());

        Assert.That(container.Elements, Has.Count.EqualTo(1));
    }

    [Test]
    public void ElementIsAvailableSynchronouslyWhenLifecycleIsAsync()
    {
        var container = new RootContainerHandler();

        var render = Render(container, Fragment<AsyncInitComponent>());

        // First render must have materialized even though OnInitializedAsync is still pending.
        Assert.That(container.Elements, Has.Count.EqualTo(1));
        Assert.That(render.Quiescence.IsCompleted, Is.False);
    }

    [Test]
    public async Task AwaitingTheResultWaitsForQuiescence()
    {
        var container = new RootContainerHandler();

        var render = Render(container, Fragment<AsyncInitComponent>());
        var component = AsyncInitComponent.Last;

        Assert.That(component.Loaded, Is.False, "Async lifecycle should not have completed yet.");

        component.Complete();
        await render;

        Assert.That(component.Loaded, Is.True);
        Assert.That(container.Elements, Has.Count.EqualTo(1));
    }

    [Test]
    public void ElementIsAvailableSynchronouslyInsideEventCallback()
    {
        var outerContainer = new RootContainerHandler();
        var outer = Render(outerContainer, Fragment<TestContainerComponent>());

        var nestedContainer = new RootContainerHandler();
        var nestedElementCount = -1;

        var callback = EventCallback.Factory.Create(outer.Component, () =>
        {
            Render(nestedContainer, Fragment<TestContainerComponent>());
            nestedElementCount = nestedContainer.Elements.Count;
        });

        callback.InvokeAsync().GetAwaiter().GetResult();

        Assert.That(nestedElementCount, Is.EqualTo(1));
    }

    [Test]
    public void RenderDuringNativeApplyPhaseSucceeds()
    {
        // AddChild runs in the native-apply phase, where a nested flush is safe.
        var nestedContainer = new RootContainerHandler();
        Exception caught = null;

        var container = new ProbingContainerHandler(() =>
        {
            try
            {
                Render(nestedContainer, Fragment<TestContainerComponent>());
            }
            catch (Exception ex)
            {
                caught = ex;
            }
        });

        Render(container, Fragment<TestContainerComponent>());

        Assert.That(caught, Is.Null, $"Nested render failed: {caught?.Message}");
        Assert.That(nestedContainer.Elements, Has.Count.EqualTo(1));
    }


    [Test]
    public void NonGenericRenderOverloadMaterializesSynchronously()
    {
        var container = new RootContainerHandler();

        var render = _renderer.Render(typeof(SimpleWrapperComponent), container);

        Assert.Multiple(() =>
        {
            Assert.That(container.Elements, Has.Count.EqualTo(1));
            Assert.That(render.Component, Is.InstanceOf<SimpleWrapperComponent>());
            Assert.That(render.Quiescence.IsCompleted, Is.True);
        });
    }

    [Test]
    public void DefaultNativeRenderHasCompletedQuiescence()
    {
        var render = default(NativeRender<TestContainerComponent>);

        Assert.Multiple(() =>
        {
            Assert.That(render.Quiescence, Is.Not.Null);
            Assert.That(render.Quiescence.IsCompleted, Is.True);
            Assert.That(render.Component, Is.Null);
        });
    }

    /// <summary>
    /// A component that accepts parameters but never renders produces no element. Without a check
    /// that the render actually happened, callers would get an element-less result - CreateWindow
    /// would hand MAUI a null window.
    /// </summary>
    [Test]
    public void ComponentThatNeverRendersThrowsInsteadOfReturningNothing()
    {
        Assert.That(() => _renderer.Render<NeverRendersComponent>(new RootContainerHandler()),
            Throws.InvalidOperationException.With.Message.Contains("did not render synchronously"));
    }

    [Test]
    public void ExceptionDuringFirstRenderIsSurfacedInsteadOfBarrierError()
    {
        Assert.That(() => _renderer.Render<ThrowingComponent>(new RootContainerHandler()),
            Throws.InvalidOperationException.With.Message.EqualTo("Boom."));
    }

    private class ProbingContainerHandler(Action onAddChild) : IContainerElementHandler, INonPhysicalChild
    {
        public List<object> Elements { get; } = [];

        void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
        {
            Elements.Add(child);
            onAddChild();
        }

        void IContainerElementHandler.RemoveChild(int physicalSiblingIndex) { }

        object IElementHandler.TargetElement => null;
        void INonPhysicalChild.SetParent(object parentElement) { }
        void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    }

    public class RenderFragmentComponent : ComponentBase
    {
        [Parameter] public RenderFragment RenderFragment { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder) => RenderFragment(builder);
    }

    private class SimpleWrapperComponent : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<TestContainerComponent>(0);
            builder.CloseComponent();
        }
    }

    public class NeverRendersComponent : IComponent
    {
        public void Attach(RenderHandle renderHandle) { }
        public Task SetParametersAsync(ParameterView parameters) => Task.CompletedTask;
    }

    public class ThrowingComponent : ComponentBase
    {
        protected override void OnInitialized() => throw new InvalidOperationException("Boom.");
    }

    /// <summary>
    /// A page-like component that renders a native control, but whose OnInitializedAsync does not
    /// complete until <see cref="Complete"/> is called.
    /// </summary>
    public class AsyncInitComponent : ComponentBase
    {
        private readonly TaskCompletionSource _source = new();

        public static AsyncInitComponent Last { get; private set; }

        public bool Loaded { get; private set; }

        public AsyncInitComponent() => Last = this;

        public void Complete() => _source.TrySetResult();

        protected override async Task OnInitializedAsync()
        {
            await _source.Task;
            Loaded = true;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<TestContainerComponent>(0);
            builder.CloseComponent();
        }
    }
}
