// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.UnitTests.Components;

namespace BlazorBindings.UnitTests.Navigation;

public class UriNavigationTests
{
    private readonly Maui.Navigation _navigationService;
    private readonly MC.INavigation _mauiNavigation;

    public UriNavigationTests()
    {
        var shell = new MC.Shell { Items = { new MC.ContentPage { Title = "Root" } } };
        var sp = TestServiceProvider.Create();
        MC.Application.Current = new TestApplication(sp) { MainPage = shell };
        _navigationService = sp.GetRequiredService<Maui.Navigation>();
        _mauiNavigation = shell.Navigation;
    }

    [TestCase("/test/path/TestTitle123/subpath")]
    [TestCase("/test/path/TestTitle123")]
    public async Task NavigateToPageWithUrlParameters(string uri)
    {
        await _navigationService.NavigateToAsync(uri);

        var mauiPage = _mauiNavigation.NavigationStack.Last();
        Assert.That(mauiPage.Title, Is.EqualTo("TestTitle123"));
        PageWithUrl.ValidateContent(mauiPage);
    }

    [Test]
    public async Task NavigateToPageWithIntParameter()
    {
        await _navigationService.NavigateToAsync("/test/int-route/42/subpath");

        var mauiPage = _mauiNavigation.NavigationStack.Last();
        PageWithUrl.ValidateContent(mauiPage, i: 42);
    }

    [TestCase("/test/nullable-long-route/1234", 1234L)]
    [TestCase("/test/nullable-long-route/", null)]
    public async Task NavigateToPageWithNullableLongParameter(string uri, long? expectedValue)
    {
        await _navigationService.NavigateToAsync(uri);

        var mauiPage = _mauiNavigation.NavigationStack.Last();
        PageWithUrl.ValidateContent(mauiPage, l: expectedValue);
    }

    [Test]
    public async Task NavigateToPageWithDateTimeParameterWithoutRouteConstraint()
    {
        await _navigationService.NavigateToAsync("/test/datetime/03-29-2023/without-constraint");

        var mauiPage = _mauiNavigation.NavigationStack.Last();
        PageWithUrl.ValidateContent(mauiPage, dt: new DateTime(2023, 03, 29));
    }

    [Test]
    public async Task NavigateToPageWithAdditionalParameter()
    {
        var lines = new[] { "Hello there!", "General Kenobi!" };
        await _navigationService.NavigateToAsync("/test/path/TestTitle123", new()
        {
            ["AdditionalText"] = lines
        });

        var mauiPage = _mauiNavigation.NavigationStack.Last();
        PageWithUrl.ValidateContent(mauiPage, additionalLines: lines);
    }

    [Test]
    public void ShouldFailIfRouteConstraintDoesNotMatch()
    {
        Assert.That(() => _navigationService.NavigateToAsync("/test/int-route/not-an-int/subpath"),
            Throws.InvalidOperationException.With.Message.Contains("not registered"));
    }

    [Test]
    public void ShouldFailIfRouteNotFound()
    {
        Assert.That(() => _navigationService.NavigateToAsync("/non/existing/route"),
            Throws.InvalidOperationException.With.Message.Contains("not registered"));
    }

    [Test]
    public async Task ComponentShouldBeDisposedOnPopAsync()
    {
        await _navigationService.NavigateToAsync($"/test/path/DisposeTest");
        var mauiPage = _mauiNavigation.NavigationStack.Last();
        var component = (PageWithUrl)mauiPage.GetValue(TestProperties.ComponentProperty);

        var isDisposed = false;
        component.OnDispose += () => isDisposed = true;

        await _mauiNavigation.PopAsync();

        Assert.That(isDisposed);
    }

    [Test]
    public async Task NavigatedComponentShouldBeAbleToReplacePage()
    {
        await _navigationService.NavigateToAsync("/switchable-pages");
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
    public async Task PushPageWithRootWrapper()
    {
        _navigationService.SetWrapperComponentType(typeof(WrapperWithCascadingValue));

        await _navigationService.NavigateToAsync("/page-with-cascading-param");
        var navigatedPage = _mauiNavigation.NavigationStack.Last();

        PageContentWithCascadingParameter.ValidateContent(navigatedPage, WrapperWithCascadingValue.Value);
    }
}
