using BlazorBindings.Maui;
using CommunityToolkit.Maui.Views;
using The49.Maui.BottomSheet;
using INavigation = BlazorBindings.Maui.INavigation;

namespace ThirdPartyControlsSample.Extensions;

public static class NavigationExtensions
{
    public static async Task<object> ShowCommunityToolkitPopupAsync<T>(this INavigation navigation)
    {
        var popup = await ((Navigation)navigation).BuildElement<CommunityToolkit.Maui.Views.Popup>(typeof(T), null);
        return await Application.Current.MainPage.ShowPopupAsync(popup);
    }

    public static async Task<object> ShowMDPopupAsync<T>(this INavigation navigation)
    {
        var popup = await ((Navigation)navigation).BuildElement<Material.Components.Maui.Popup>(typeof(T), null);
        return await popup.ShowAtAsync(Application.Current.MainPage);
    }

    public static async Task ShowBottomSheet<T>(this INavigation navigation)
    {
        var bottomSheet = await ((Navigation)navigation).BuildElement<BottomSheet>(typeof(T), null);
        await bottomSheet.ShowAsync();
    }
}