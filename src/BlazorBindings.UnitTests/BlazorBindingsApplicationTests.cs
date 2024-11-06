using BlazorBindings.Maui;
using BlazorBindings.UnitTests.Components;

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

    private static BlazorBindingsApplication<T> CreateApplication<T>() where T : IComponent
    {
        return new BlazorBindingsApplication<T>(TestServiceProvider.Create());
    }

    private static BlazorBindingsApplication<TMain> CreateApplicationWithWrapper<TMain, TWrapper>()
        where TMain : IComponent
        where TWrapper : IComponent
    {
        return new BlazorBindingsApplicationWithWrapper<TMain, TWrapper>(TestServiceProvider.Create());
    }

    class BlazorBindingsApplicationWithWrapper<TMain, TWrapper>(IServiceProvider services) : BlazorBindingsApplication<TMain>(services)
        where TMain : IComponent
        where TWrapper : IComponent
    {
        public override Type WrapperComponentType => typeof(TWrapper);
    }
}
