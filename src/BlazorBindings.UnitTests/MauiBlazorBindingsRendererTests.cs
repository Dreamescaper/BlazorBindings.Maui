using BlazorBindings.Maui;
using BlazorBindings.UnitTests.Components;

namespace BlazorBindings.UnitTests;

public class MauiBlazorBindingsRendererTests
{
    private readonly TestBlazorBindingsRenderer _renderer;
    private readonly MC.Window _window = new();

    public MauiBlazorBindingsRendererTests()
    {
        var services = TestServiceProvider.CreateMauiAppBuilder().Build().Services;
        _renderer = (TestBlazorBindingsRenderer)services.GetRequiredService<MauiBlazorBindingsRenderer>();
    }

    [Test]
    public void ShouldThrowExceptionIfHappenedDuringSyncRender()
    {
        void action() => _ = _renderer.AddComponent(typeof(ComponentWithException), _window);

        Assert.That(action, Throws.InvalidOperationException.With.Message.EqualTo("Should fail here."));
    }

    [Test]
    public async Task RendererShouldHandleAsyncExceptions()
    {
        _renderer.ThrowExceptions = false;

        await _renderer.AddComponent(typeof(PageWithButtonWithExceptionOnClick), _window);
        var button = (MC.Button)((MC.ContentPage)_window.Page).Content;
        button.SendClicked();

        Assert.That(() => _renderer.Exceptions, Is.Not.Empty.After(1000, 10));
        Assert.That(_renderer.Exceptions[0].Message, Is.EqualTo("HandleExceptionTest"));
    }

    [Test]
    public async Task RenderedComponentShouldBeAbleToReplaceMainPage()
    {
        await _renderer.AddComponent(typeof(SwitchablePages), _window);

        Assert.That(_window.Page.Title, Is.EqualTo("Page1"));

        var switchButton = (MC.Button)((MC.ContentPage)_window.Page).Content;
        switchButton.SendClicked();

        Assert.That(_window.Page.Title, Is.EqualTo("Page2"));

        switchButton = (MC.Button)((MC.ContentPage)_window.Page).Content;
        switchButton.SendClicked();

        Assert.That(_window.Page.Title, Is.EqualTo("Page1"));
    }
}
