using BlazorBindings.Maui;
using CommunityToolkit.Maui;
using Material.Components.Maui.Extensions;
using Nalu;
using Syncfusion.Maui.Toolkit.Hosting;
using The49.Maui.BottomSheet;
using ThirdPartyControlsSample.NaluIntegration;
using ThirdPartyControlsSample.Pages;

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
            .UseNaluNavigation<App>(navigation => navigation
                .AddPage<RazorComponentPage<NaluSharedElementsView>>()
                .AddPage<RazorComponentPage<NaluSharedElementDetailsView>>()
                .AddPage<RazorComponentPage<CommunityToolkitPage>>()
                .AddPage<RazorComponentPage<CommunityToolkitBehaviors>>()
                .AddPage<RazorComponentPage<XCalendarPage>>()
                .AddPage<RazorComponentPage<SkiaCanvasPage>>()
                .AddPage<RazorComponentPage<SyncfusionToolkitPage>>()
                .AddPage<RazorComponentPage<MaterialComponentsPage>>()
                .AddPage<RazorComponentPage<BottomSheetPage>>())
            .UseNaluScaffold()
            .UseNaluBlazorBindings()
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
