using BlazorBindings.Maui;
using CommunityToolkit.Maui;
using Material.Components.Maui.Extensions;
using Syncfusion.Maui.Toolkit.Hosting;
using The49.Maui.BottomSheet;

namespace ThirdPartyControlsSample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .ConfigureSyncfusionToolkit()
            .UseMauiApp<App>()
            .UseMauiBlazorBindings()
            .UseMauiCommunityToolkit()
            .UseBottomSheet()
            .UseMaterialComponents(["OpenSans-Regular.ttf"])
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }
}