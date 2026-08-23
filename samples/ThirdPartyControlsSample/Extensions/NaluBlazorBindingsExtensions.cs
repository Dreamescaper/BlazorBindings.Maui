using BlazorBindings.Maui;
using Nalu;

namespace ThirdPartyControlsSample;

public static class NaluBlazorBindingsExtensions
{
    public static MauiAppBuilder UseNaluBlazorBindings(this MauiAppBuilder builder)
    {
#pragma warning disable MBB001
        AttachedPropertyRegistry.RegisterAttachedPropertyHandler(
            "Scaffold.TransitionName",
            static (element, value) => Scaffold.SetTransitionName(element, (string)value));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler(
            "Scaffold.TabBarVisibility",
            static (element, value) => Scaffold.SetTabBarVisibility(element, (ScaffoldTabBarVisibility)value));
#pragma warning restore MBB001

        return builder;
    }
}
