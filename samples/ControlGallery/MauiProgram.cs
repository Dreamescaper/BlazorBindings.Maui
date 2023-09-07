// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui;
using UraniumUI;

namespace ControlGallery;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiBlazorBindings()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFontAwesomeIconFonts();
            });

        builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);

        return builder.Build();
    }
}

public class App : BlazorBindingsApplication<AppShell>
{
    public App(IServiceProvider services) : base(services) { }

    public override Type WrapperComponentType => typeof(Root);
}