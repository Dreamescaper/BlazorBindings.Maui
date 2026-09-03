using BlazorBindings.Core;
using BlazorBindings.Maui;
using BlazorBindings.Maui.Elements.Handlers;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorBindings.UnitTests;

/// <summary>
/// Covers the behaviour of the Sync*TemplateItemsComponent types, whose AddTemplateRoot has to
/// return a native element synchronously - including while a render batch is being applied, when
/// StateHasChanged can only queue a render rather than flush one.
/// </summary>
public class SyncTemplateDuringBatchTests
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
        ShellWithTemplateComponent.TemplateRenderCount = 0;
        TemplateBodyComponent.InitCount = 0;
    }

    [Test]
    public async Task TemplateRealizedOutsideBatchSucceeds()
    {
        var container = new RootContainerHandler();
        await _renderer.AddComponent<ShellWithTemplateComponent>(container);

        var shell = (MC.Shell)container.Elements[0];
        var shellContent = shell.CurrentItem.Items[0].Items[0];

        var page = ((MC.IShellContentController)shellContent).GetOrCreateContent();

        Assert.That(page, Is.Not.Null);
    }

    [Test]
    public async Task TemplateRealizedDuringBatchSucceeds()
    {
        Exception caught = null;
        MC.Page page = null;

        // This handler realizes the template from AddChild - i.e. while the batch is still being
        // applied - which is what MAUI itself can do when it materializes a template in response
        // to a property we set during the batch.
        var container = new ProbingContainerHandler(child =>
        {
            try
            {
                var shell = (MC.Shell)child;
                var shellContent = shell.CurrentItem.Items[0].Items[0];
                page = ((MC.IShellContentController)shellContent).GetOrCreateContent();
            }
            catch (Exception ex)
            {
                caught = ex;
            }
        });

        await _renderer.AddComponent<ShellWithTemplateComponent>(container);

        Assert.That(caught, Is.Null, $"Realizing the template during the batch failed: {caught?.Message}");
        Assert.That(page, Is.Not.Null);
    }

    [Test]
    public async Task TemplateIsNotRenderedUntilItIsRequested()
    {
        // ContentTemplate is meant to stay lazy until the ShellContent becomes active. The flush
        // buys batch-safety without spending that laziness - nothing is rendered ahead of demand.
        var container = new RootContainerHandler();
        await _renderer.AddComponent<ShellWithTemplateComponent>(container);

        Assert.That(ShellWithTemplateComponent.TemplateRenderCount, Is.Zero,
            "Nothing should be rendered before MAUI asks for the template.");
    }

    [Test]
    public async Task RepeatedRealizationsEachProduceADistinctRoot()
    {
        var container = new RootContainerHandler();
        await _renderer.AddComponent<ShellWithTemplateComponent>(container);

        var shell = (MC.Shell)container.Elements[0];
        var shellContent = shell.CurrentItem.Items[0].Items[0];
        var template = shellContent.ContentTemplate;

        // Realize several times; each hand-out should be followed by a refill.
        var pages = new List<object>();
        for (var i = 0; i < 3; i++)
            pages.Add(template.CreateContent());

        Assert.That(pages, Has.Count.EqualTo(3));
        Assert.That(pages, Is.Unique);
        Assert.That(pages, Has.None.Null);
    }

    [Test]
    public async Task RealizedTemplateIsTheFullyBuiltPage()
    {
        var container = new RootContainerHandler();
        await _renderer.AddComponent<ShellWithTemplateComponent>(container);

        // Nobody has asked MAUI for the content yet.
        var builtBeforeRealization = TemplateBodyComponent.InitCount;

        var shell = (MC.Shell)container.Elements[0];
        var shellContent = shell.CurrentItem.Items[0].Items[0];
        var page = (MC.ContentPage)((MC.IShellContentController)shellContent).GetOrCreateContent();

        Assert.Multiple(() =>
        {
            Assert.That(builtBeforeRealization, Is.Zero, "template must not be built before it is asked for");

            // What MAUI receives is the complete page, not a placeholder to be filled in later.
            Assert.That(page.Title, Is.EqualTo("Templated"));
            Assert.That(page.Content, Is.InstanceOf<MC.Label>());
            Assert.That(((MC.Label)page.Content).Text, Is.EqualTo("Hello from the template"));
            Assert.That(TemplateBodyComponent.InitCount, Is.EqualTo(1), "exactly one page instance built");
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

        void IContainerElementHandler.RemoveChild(int physicalSiblingIndex) { }

        object IElementHandler.TargetElement => null;
        void INonPhysicalChild.SetParent(object parentElement) { }
        void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    }

    /// <summary>The body of the ContentTemplate - i.e. what a real app would put on the page.</summary>
    private class TemplateBodyComponent : ComponentBase
    {
        public static int InitCount;

        protected override void OnInitialized() => InitCount++;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<Maui.Elements.ContentPage>(0);
            builder.AddAttribute(1, "Title", "Templated");
            builder.AddAttribute(2, "ChildContent", (RenderFragment)(pageBuilder =>
            {
                pageBuilder.OpenComponent<Maui.Elements.Label>(0);
                pageBuilder.AddAttribute(1, "Text", "Hello from the template");
                pageBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        }
    }

    private class ShellWithTemplateComponent : ComponentBase
    {
        public static int TemplateRenderCount;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<Maui.Elements.Shell>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(shellBuilder =>
            {
                shellBuilder.OpenComponent<Maui.Elements.ShellContent>(0);
                shellBuilder.AddAttribute(1, "Title", "Some Page");
                shellBuilder.AddAttribute(2, "ContentTemplate", (RenderFragment)(templateBuilder =>
                {
                    TemplateRenderCount++;
                    templateBuilder.OpenComponent<TemplateBodyComponent>(0);
                    templateBuilder.CloseComponent();
                }));
                shellBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        }
    }
}
