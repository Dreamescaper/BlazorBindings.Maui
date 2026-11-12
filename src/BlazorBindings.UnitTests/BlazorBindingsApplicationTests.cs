using BlazorBindings.Maui;
using BlazorBindings.UnitTests.Components;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace BlazorBindings.UnitTests;

public class BlazorBindingsApplicationTests
{
    [Test]
    public void SetsTheMainPage_ContentPage()
    {
        var application = CreateApplication<PageContent>();
        PageContent.ValidateContent(application.MainPage);
    }

    [Test]
    public void SetsTheMainPage_WithRootWrapper()
    {
        var application = CreateApplicationWithWrapper<PageContentWithCascadingParameter, WrapperWithCascadingValue>();
        PageContentWithCascadingParameter.ValidateContent(application.MainPage, WrapperWithCascadingValue.Value);
    }

    [Test]
    public void SetsMainWindow()
    {
        var application = CreateApplication<WindowWithPageContent>();
        WindowWithPageContent.ValidateContent(application.Windows.Single());
    }

    [Test]
    public void ShouldBeAbleToReplaceMainPage()
    {
        var application = CreateApplication<SwitchablePages>();

        Assert.That(application.MainPage.Title, Is.EqualTo("Page1"));

        var switchButton = (MC.Button)((MC.ContentPage)application.MainPage).Content;
        switchButton.SendClicked();

        Assert.That(application.MainPage.Title, Is.EqualTo("Page2"));

        switchButton = (MC.Button)((MC.ContentPage)application.MainPage).Content;
        switchButton.SendClicked();

        Assert.That(application.MainPage.Title, Is.EqualTo("Page1"));
    }

    [Test]
    public void ShouldThrowExceptionIfHappenedDuringSyncRender()
    {
        Assert.That(CreateApplication<ComponentWithException>,
            Throws.InvalidOperationException.With.Message.EqualTo("Should fail here."));
    }

    /// <summary>
    /// The root component rendering successfully is not the same as it producing a Window: one
    /// whose content is only a CascadingValue renders without creating any native element.
    /// CreateWindow must say so rather than hand MAUI a null.
    /// </summary>
    [Test]
    public void ShouldThrowIfRootComponentProducesNoWindow()
    {
        Assert.That(CreateApplication<ComponentWithoutNativeContent>,
            Throws.InvalidOperationException.With.Message.Contains("did not produce a Window or a Page"));
    }

    private class ComponentWithoutNativeContent : ComponentBase
    {
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            builder.OpenComponent<CascadingValue<string>>(0);
            builder.AddAttribute(1, "Value", "nothing native here");
            builder.AddAttribute(2, "ChildContent", (RenderFragment)(_ => { }));
            builder.CloseComponent();
        }
    }

    private static Application CreateApplication<T>() where T : IComponent
    {
        var application = TestApplication.Create<BlazorBindingsApplication<T>>(createWindow: false);
        ((IApplication)application).CreateWindow(new ActivationState(application.Handler.MauiContext));
        return application;
    }

    private static Application CreateApplicationWithWrapper<TMain, TWrapper>()
        where TMain : IComponent
        where TWrapper : IComponent
    {
        var application = TestApplication.Create<BlazorBindingsApplicationWithWrapper<TMain, TWrapper>>(createWindow: false);
        ((IApplication)application).CreateWindow(new ActivationState(application.Handler.MauiContext));
        return application;
    }

    class BlazorBindingsApplicationWithWrapper<TMain, TWrapper> : BlazorBindingsApplication<TMain>
        where TMain : IComponent
        where TWrapper : IComponent
    {
        public override Type WrapperComponentType => typeof(TWrapper);
    }
}
