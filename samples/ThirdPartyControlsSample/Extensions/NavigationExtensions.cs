using BlazorBindings.Maui;
using CommunityToolkit.Maui.Views;

namespace ThirdPartyControlsSample.Extensions
{
    public static class NavigationExtensions
    {
        public static async Task<object> ShowPopupAsync<T>(this Navigation navigationService)
        {
#pragma warning disable CA2252 // This API requires opting into preview features
            var popup = await navigationService.BuildElement<Popup>(typeof(T), null);
#pragma warning restore CA2252 // This API requires opting into preview features
            return await Application.Current.MainPage.ShowPopupAsync(popup);
        }
    }
}
