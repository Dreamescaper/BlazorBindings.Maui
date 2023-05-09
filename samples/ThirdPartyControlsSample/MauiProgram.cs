using BlazorBindings.Maui;
using CommunityToolkit.Maui;
using Material.Components.Maui.Extensions;

namespace ThirdPartyControlsSample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiBlazorBindings()
            .UseMauiCommunityToolkit()
            .UseMaterialComponents(new() { "OpenSans-Regular.ttf" })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }
}