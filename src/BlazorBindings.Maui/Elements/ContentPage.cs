using BlazorBindings.Maui.Elements.Input;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Maui.Graphics;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class ContentPage : TemplatedPage
{
    /// <summary>
    /// Sets a value that indicates whether or not this Page element has a navigation bar.
    /// </summary>
    [Parameter] public bool? NavBarIsVisible { get; set; }

    /// <summary>
    /// Sets a value that indicates whether or not this Page element navigation bar has a shadow.
    /// This property is ignored if the application does not use Shell.
    /// </summary>
    [Parameter] public bool? NavBarHasShadow { get; set; }

    /// <summary>
    /// Sets a value that indicates whether or not this Page element has tab bar visible.
    /// This property is ignored if the application does not use Shell.
    /// </summary>
    [Parameter] public bool? TabBarIsVisible { get; set; }

    /// <summary>
    /// Change Shell navigation behavior.
    /// This property is ignored if the application does not use Shell.
    /// </summary>
    [Parameter] public MC.PresentationMode? ShellPresentationMode { get; set; }

    /// <summary>
    /// Defines the color used for the title of the page.
    /// This property is ignored if the application does not use Shell.
    /// </summary>
    [Parameter] public Color TitleColor { get; set; }

    /// <summary>
    /// Defines the view that can be displayed in the navigation bar.
    /// </summary>
    [Parameter] public RenderFragment TitleView { get; set; }

    /// <summary>
    /// Configures Shell integrated search capabilities. It accepts <see cref="Elements.SearchHandler{T}"/> child only.
    /// </summary>
    [Parameter] public RenderFragment ShellSearchHandler { get; set; }

    /// <summary>
    /// Sets the title that appears on the back button for page.
    /// </summary>
    [Parameter] public string BackButtonText { get; set; }

    /// <summary>
    /// Adds or removes a back button to page.
    /// </summary>
    [Parameter] public bool? BackButtonVisible { get; set; }

    /// <summary>
    /// Overrides default back button action.
    /// This property is ignored if the application does not use Shell.
    /// </summary>
    [Parameter] public EventCallback OnBackButtonPressed { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        switch (name)
        {
            case nameof(NavBarIsVisible):
                if (!Equals(NavBarIsVisible, value))
                {
                    NavBarIsVisible = CastParameter<bool?>(value, name);
                    MC.Shell.SetNavBarIsVisible(NativeControl, NavBarIsVisible ?? true);
                    MC.NavigationPage.SetHasNavigationBar(NativeControl, NavBarIsVisible ?? true);
                }
                return true;
            case nameof(NavBarHasShadow):
                if (!Equals(NavBarHasShadow, value))
                {
                    NavBarHasShadow = CastParameter<bool?>(value, name);
                    MC.Shell.SetNavBarHasShadow(NativeControl, NavBarHasShadow ?? true);
                }
                return true;
            case nameof(TabBarIsVisible):
                if (!Equals(TabBarIsVisible, value))
                {
                    TabBarIsVisible = CastParameter<bool?>(value, name);
                    MC.Shell.SetTabBarIsVisible(NativeControl, TabBarIsVisible ?? true);
                }
                return true;
            case nameof(ShellPresentationMode):
                if (!Equals(ShellPresentationMode, value))
                {
                    ShellPresentationMode = CastParameter<MC.PresentationMode?>(value, name);
                    MC.Shell.SetPresentationMode(NativeControl, ShellPresentationMode ?? (MC.PresentationMode)MC.Shell.PresentationModeProperty.DefaultValue);
                }
                return true;
            case nameof(TitleColor):
                if (!Equals(TitleColor, value))
                {
                    TitleColor = CastParameter<Color>(value, name);
                    MC.Shell.SetTitleColor(NativeControl, TitleColor);
                }
                return true;
            case nameof(BackButtonText):
                if (!Equals(BackButtonText, value))
                {
                    BackButtonText = CastParameter<string>(value, name);
                    MC.NavigationPage.SetBackButtonTitle(NativeControl, BackButtonText);
                    GetBackButtonBehavior().TextOverride = BackButtonText;
                }
                return true;
            case nameof(BackButtonVisible):
                if (!Equals(BackButtonVisible, value))
                {
                    BackButtonVisible = CastParameter<bool?>(value, name);
                    MC.NavigationPage.SetHasBackButton(NativeControl, BackButtonVisible ?? true);
                    GetBackButtonBehavior().IsVisible = BackButtonVisible ?? true;
                }
                return true;
            case nameof(OnBackButtonPressed):
                if (!Equals(OnBackButtonPressed, value))
                {
                    OnBackButtonPressed = CastParameter<EventCallback>(value, name);
                    GetBackButtonBehavior().Command = OnBackButtonPressed.HasDelegate
                        ? new EventCallbackCommand(() => InvokeEventCallback(OnBackButtonPressed))
                        : null;
                }
                return true;
            case nameof(TitleView):
                TitleView = CastParameter<RenderFragment>(value, name);
                return true;
            case nameof(ShellSearchHandler):
                ShellSearchHandler = CastParameter<RenderFragment>(value, name);
                return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }

    private MC.BackButtonBehavior GetBackButtonBehavior()
    {
        var backBehavior = MC.Shell.GetBackButtonBehavior(NativeControl);
        if (backBehavior is null)
        {
            backBehavior = new MC.BackButtonBehavior();
            MC.Shell.SetBackButtonBehavior(NativeControl, backBehavior);
        }
        return backBehavior;
    }

    protected override void RenderAdditionalPartialElementContent(RenderTreeBuilder builder, ref int sequence)
    {
        base.RenderAdditionalPartialElementContent(builder, ref sequence);
        RenderTreeBuilderHelper.AddContentProperty<MC.ContentPage>(builder, sequence++, TitleView, (x, value) =>
        {
            MC.Shell.SetTitleView(x, (MC.View)value);
            MC.NavigationPage.SetTitleView(x, (MC.View)value);
        });
        RenderTreeBuilderHelper.AddContentProperty<MC.ContentView>(builder, sequence++, ShellSearchHandler, (x, value) =>
            MC.Shell.SetSearchHandler(x, (MC.SearchHandler)value));
    }
}
