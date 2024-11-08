// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.UnitTests.Components;

namespace BlazorBindings.UnitTests.Navigation;

[TestFixture(nameof(MC.Shell))]
[TestFixture(nameof(MC.NavigationPage))]
public class NonUriNavigationTests
{
    private readonly Maui.Navigation _navigationService;
    private readonly MC.INavigation _mauiNavigation;
    private readonly MC.Page _rootPage;

    public NonUriNavigationTests(string root)
    {
        var mainPage = root == nameof(MC.Shell)
            ? (MC.Page)new MC.Shell { Items = { new MC.ContentPage { Title = "Root" } } }
            : new MC.NavigationPage(new MC.ContentPage { Title = "Root" });

        var application = TestApplication.Create();
        application.MainPage = mainPage;
        MC.Application.Current = application;

        _navigationService = application.Handler.MauiContext.Services.GetRequiredService<Maui.Navigation>();
        _mauiNavigation = mainPage.Navigation;
        _rootPage = _mauiNavigation.NavigationStack[0];
    }

    [Test]
    public async Task PushAsyncWithParameters()
    {
        var par1 = "Test6543";
        var par2 = 3242342;

        await _navigationService.PushAsync<PageContentWithParameters>(new()
        {
            [nameof(PageContentWithParameters.Par1)] = par1,
            [nameof(PageContentWithParameters.Par2)] = par2
        });

        var navigatedPage = _mauiNavigation.NavigationStack.Last();
        PageContentWithParameters.ValidateContent(navigatedPage, par1, par2);
    }

    [Test]
    public async Task PushAsyncWithParameters_RenderFragment()
    {
        var par1 = "Test6543";
        var par2 = 3242342;
        var renderFragment = NavigationRenderFragments.PageWithParameters(par1, par2);

        await _navigationService.PushAsync(renderFragment);

        var navigatedPage = _mauiNavigation.NavigationStack.Last();
        PageContentWithParameters.ValidateContent(navigatedPage, par1, par2);
    }

    [Test]
    public async Task PushModalAsyncWithParameters()
    {
        var par1 = "Test6543";
        var par2 = 3242342;

        await _navigationService.PushModalAsync<PageContentWithParameters>(new()
        {
            [nameof(PageContentWithParameters.Par1)] = par1,
            [nameof(PageContentWithParameters.Par2)] = par2
        });

        var navigatedPage = _mauiNavigation.ModalStack.Last();
        PageContentWithParameters.ValidateContent(navigatedPage, par1, par2);
    }

    [Test]
    public async Task PushModalAsyncWithParameters_RenderFragment()
    {
        var par1 = "Test6543";
        var par2 = 3242342;
        var renderFragment = NavigationRenderFragments.PageWithParameters(par1, par2);

        await _navigationService.PushModalAsync(renderFragment);

        var navigatedPage = _mauiNavigation.ModalStack.Last();
        PageContentWithParameters.ValidateContent(navigatedPage, par1, par2);
    }

    [Test]
    public async Task PopAsync()
    {
        await _navigationService.PushAsync<PageContent>();

        Assert.That(_mauiNavigation.NavigationStack, Has.Count.EqualTo(2));

        await _navigationService.PopAsync();

        Assert.That(_mauiNavigation.NavigationStack, Has.Count.EqualTo(1));
        Assert.That(_mauiNavigation.NavigationStack[0], Is.EqualTo(_rootPage));
    }

    [Test]
    public async Task PopToRootAsync()
    {
        await _navigationService.PushAsync<PageContent>();
        await _navigationService.PushAsync<PageContentWithParameters>();

        Assert.That(_mauiNavigation.NavigationStack, Has.Count.EqualTo(3));

        await _navigationService.PopToRootAsync();

        Assert.That(_mauiNavigation.NavigationStack, Has.Count.EqualTo(1));
        Assert.That(_mauiNavigation.NavigationStack[0], Is.EqualTo(_rootPage));
    }

    [Test]
    public async Task PopModalAsync()
    {
        await _navigationService.PushModalAsync<PageContent>();

        Assert.That(_mauiNavigation.ModalStack, Has.Count.EqualTo(1));

        await _navigationService.PopModalAsync();

        Assert.That(_mauiNavigation.ModalStack, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task ComponentShouldBeDisposedOnPopAsync()
    {
        var isDisposed = false;
        await _navigationService.PushAsync<PageContentWithDispose>();
        var mauiPage = _mauiNavigation.NavigationStack.Last();
        var component = (PageContentWithDispose)mauiPage.GetValue(TestProperties.ComponentProperty);
        component.OnDispose += () => isDisposed = true;

        await _navigationService.PopAsync();

        Assert.That(isDisposed);
    }

    [Test]
    public async Task NavigatedComponentShouldBeAbleToReplacePage()
    {
        await _navigationService.PushAsync<SwitchablePages>();
        var navigatedPage = _mauiNavigation.NavigationStack.Last();

        Assert.That(_mauiNavigation.NavigationStack, Has.Count.EqualTo(2));
        Assert.That(navigatedPage.Title, Is.EqualTo("Page1"));

        var switchButton = (MC.Button)((MC.ContentPage)navigatedPage).Content;
        switchButton.SendClicked();
        navigatedPage = _mauiNavigation.NavigationStack.Last();

        Assert.That(_mauiNavigation.NavigationStack, Has.Count.EqualTo(2));
        Assert.That(navigatedPage.Title, Is.EqualTo("Page2"));

        switchButton = (MC.Button)((MC.ContentPage)navigatedPage).Content;
        switchButton.SendClicked();
        navigatedPage = _mauiNavigation.NavigationStack.Last();

        Assert.That(_mauiNavigation.NavigationStack, Has.Count.EqualTo(2));
        Assert.That(navigatedPage.Title, Is.EqualTo("Page1"));
    }

    [Test]
    public async Task NavigatedModalShouldBeAbleToReplacePage()
    {
        await _navigationService.PushModalAsync<SwitchablePages>();
        var navigatedPage = _mauiNavigation.ModalStack.Last();

        Assert.That(_mauiNavigation.ModalStack, Has.Count.EqualTo(1));
        Assert.That(navigatedPage.Title, Is.EqualTo("Page1"));

        var switchButton = (MC.Button)((MC.ContentPage)navigatedPage).Content;
        switchButton.SendClicked();
        navigatedPage = _mauiNavigation.ModalStack.Last();

        Assert.That(_mauiNavigation.ModalStack, Has.Count.EqualTo(1));
        Assert.That(navigatedPage.Title, Is.EqualTo("Page2"));

        switchButton = (MC.Button)((MC.ContentPage)navigatedPage).Content;
        switchButton.SendClicked();
        navigatedPage = _mauiNavigation.ModalStack.Last();

        Assert.That(_mauiNavigation.ModalStack, Has.Count.EqualTo(1));
        Assert.That(navigatedPage.Title, Is.EqualTo("Page1"));
    }

    [Test]
    public async Task PushPageWithRootWrapper()
    {
        _navigationService.SetWrapperComponentType(typeof(WrapperWithCascadingValue));

        await _navigationService.PushAsync<PageContentWithCascadingParameter>();
        var navigatedPage = _mauiNavigation.NavigationStack.Last();

        PageContentWithCascadingParameter.ValidateContent(navigatedPage, WrapperWithCascadingValue.Value);
    }

    [Test]
    public async Task PushModalWithRootWrapper()
    {
        _navigationService.SetWrapperComponentType(typeof(WrapperWithCascadingValue));

        await _navigationService.PushModalAsync<PageContentWithCascadingParameter>();
        var navigatedPage = _mauiNavigation.ModalStack.Last();

        PageContentWithCascadingParameter.ValidateContent(navigatedPage, WrapperWithCascadingValue.Value);
    }
}
