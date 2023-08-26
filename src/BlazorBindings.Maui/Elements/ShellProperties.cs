// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.Extensions;
using Microsoft.Maui.Graphics;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public class ShellProperties : NativeControlComponentBase, IElementHandler, INonPhysicalChild
{
    private MC.BindableObject _target;

    [Parameter] public bool? NavBarIsVisible { get; set; }
    [Parameter] public bool? NavBarHasShadow { get; set; }
    [Parameter] public bool? TabBarIsVisible { get; set; }
    [Parameter] public Color BackgroundColor { get; set; }
    [Parameter] public Color DisabledColor { get; set; }
    [Parameter] public Color ForegroundColor { get; set; }
    [Parameter] public Color TabBarBackgroundColor { get; set; }
    [Parameter] public Color TabBarDisabledColor { get; set; }
    [Parameter] public Color TabBarForegroundColor { get; set; }
    [Parameter] public Color TabBarTitleColor { get; set; }
    [Parameter] public Color TabBarUnselectedColor { get; set; }
    [Parameter] public Color TitleColor { get; set; }
    [Parameter] public Color UnselectedColor { get; set; }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        foreach (var parameterValue in parameters)
        {
            switch (parameterValue.Name)
            {
                case nameof(NavBarIsVisible):
                    var navBarVisible = (bool?)parameterValue.Value;
                    if (navBarVisible != NavBarIsVisible)
                    {
                        NavBarIsVisible = navBarVisible;
                        MC.Shell.SetNavBarIsVisible(_target, navBarVisible ?? (bool)MC.Shell.NavBarIsVisibleProperty.DefaultValue);
                    }
                    break;
                case nameof(NavBarHasShadow):
                    var navBarHasShadow = (bool?)parameterValue.Value;
                    if (navBarHasShadow != NavBarHasShadow)
                    {
                        NavBarHasShadow = navBarHasShadow;
                        MC.Shell.SetNavBarHasShadow(_target, navBarHasShadow ?? (bool)MC.Shell.NavBarHasShadowProperty.DefaultValue);
                    }
                    break;
                case nameof(TabBarIsVisible):
                    var tabBarIsVisible = (bool?)parameterValue.Value;
                    if (tabBarIsVisible != TabBarIsVisible)
                    {
                        TabBarIsVisible = tabBarIsVisible;
                        MC.Shell.SetTabBarIsVisible(_target, tabBarIsVisible ?? (bool)MC.Shell.TabBarIsVisibleProperty.DefaultValue);
                    }
                    break;
                case nameof(BackgroundColor):
                    var backgroundColor = (Color)parameterValue.Value;
                    if (backgroundColor != BackgroundColor)
                    {
                        BackgroundColor = backgroundColor;
                        MC.Shell.SetBackgroundColor(_target, backgroundColor);
                    }
                    break;
                case nameof(DisabledColor):
                    var disabledColor = (Color)parameterValue.Value;
                    if (disabledColor != DisabledColor)
                    {
                        DisabledColor = disabledColor;
                        MC.Shell.SetDisabledColor(_target, disabledColor);
                    }
                    break;
                case nameof(ForegroundColor):
                    var foregroundColor = (Color)parameterValue.Value;
                    if (foregroundColor != ForegroundColor)
                    {
                        ForegroundColor = foregroundColor;
                        MC.Shell.SetForegroundColor(_target, foregroundColor);
                    }
                    break;
                case nameof(TabBarBackgroundColor):
                    var tabBarBackgroundColor = (Color)parameterValue.Value;
                    if (tabBarBackgroundColor != TabBarBackgroundColor)
                    {
                        TabBarBackgroundColor = tabBarBackgroundColor;
                        MC.Shell.SetTabBarBackgroundColor(_target, tabBarBackgroundColor);
                    }
                    break;
                case nameof(TabBarDisabledColor):
                    var tabBarDisabledColor = (Color)parameterValue.Value;
                    if (tabBarDisabledColor != TabBarDisabledColor)
                    {
                        TabBarDisabledColor = tabBarDisabledColor;
                        MC.Shell.SetTabBarDisabledColor(_target, tabBarDisabledColor);
                    }
                    break;
                case nameof(TabBarForegroundColor):
                    var tabBarForegroundColor = (Color)parameterValue.Value;
                    if (tabBarForegroundColor != TabBarForegroundColor)
                    {
                        TabBarForegroundColor = tabBarForegroundColor;
                        MC.Shell.SetTabBarForegroundColor(_target, tabBarForegroundColor);
                    }
                    break;
                case nameof(TabBarTitleColor):
                    var tabBarTitleColor = (Color)parameterValue.Value;
                    if (tabBarTitleColor != TabBarTitleColor)
                    {
                        TabBarTitleColor = tabBarTitleColor;
                        MC.Shell.SetTabBarTitleColor(_target, tabBarTitleColor);
                    }
                    break;
                case nameof(TabBarUnselectedColor):
                    var tabBarUnselectedColor = (Color)parameterValue.Value;
                    if (tabBarUnselectedColor != TabBarUnselectedColor)
                    {
                        TabBarUnselectedColor = tabBarUnselectedColor;
                        MC.Shell.SetTabBarUnselectedColor(_target, tabBarUnselectedColor);
                    }
                    break;
                case nameof(TitleColor):
                    var titleColor = (Color)parameterValue.Value;
                    if (titleColor != TitleColor)
                    {
                        TitleColor = titleColor;
                        MC.Shell.SetTitleColor(_target, titleColor);
                    }
                    break;
                case nameof(UnselectedColor):
                    var unselectedColor = (Color)parameterValue.Value;
                    if (unselectedColor != UnselectedColor)
                    {
                        UnselectedColor = unselectedColor;
                        MC.Shell.SetUnselectedColor(_target, unselectedColor);
                    }
                    break;
            }
        }
        return base.SetParametersAsync(ParameterView.Empty);
    }

    void INonPhysicalChild.SetParent(object parentElement)
    {
        _target = parentElement.Cast<MC.BindableObject>();
    }

    void INonPhysicalChild.RemoveFromParent(object parentElement)
    {
        MC.Shell.SetNavBarIsVisible(_target, (bool)MC.Shell.NavBarIsVisibleProperty.DefaultValue);
        MC.Shell.SetNavBarHasShadow(_target, (bool)MC.Shell.NavBarHasShadowProperty.DefaultValue);
        MC.Shell.SetTabBarIsVisible(_target, (bool)MC.Shell.NavBarHasShadowProperty.DefaultValue);
        MC.Shell.SetBackgroundColor(_target, (Color)MC.Shell.BackgroundColorProperty.DefaultValue);
        MC.Shell.SetDisabledColor(_target, (Color)MC.Shell.DisabledColorProperty.DefaultValue);
        MC.Shell.SetForegroundColor(_target, (Color)MC.Shell.ForegroundColorProperty.DefaultValue);
        MC.Shell.SetTabBarBackgroundColor(_target, (Color)MC.Shell.TabBarBackgroundColorProperty.DefaultValue);
        MC.Shell.SetTabBarTitleColor(_target, (Color)MC.Shell.TabBarTitleColorProperty.DefaultValue);
        MC.Shell.SetTabBarUnselectedColor(_target, (Color)MC.Shell.TabBarUnselectedColorProperty.DefaultValue);
        MC.Shell.SetTitleColor(_target, (Color)MC.Shell.TitleColorProperty.DefaultValue);
        MC.Shell.SetUnselectedColor(_target, (Color)MC.Shell.UnselectedColorProperty.DefaultValue);
    }

    object IElementHandler.TargetElement => null;
}
