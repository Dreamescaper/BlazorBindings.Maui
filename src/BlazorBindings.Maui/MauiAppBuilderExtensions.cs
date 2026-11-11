// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using BlazorBindings.Maui.Elements.Internal.DataTemplates;

namespace BlazorBindings.Maui;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseMauiBlazorBindings(this MauiAppBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Use factories for performance.
        builder.Services
            .AddSingleton<Navigation>(svcs => new Navigation(svcs.GetRequiredService<MauiBlazorBindingsServiceProvider>()))
            .AddSingleton<INavigation>(services => services.GetRequiredService<Navigation>())
            .AddSingleton<ISyncTemplateRendererFactory>(svcs => new SyncTemplateRendererFactory(svcs.GetRequiredService<MauiBlazorBindingsServiceProvider>(), svcs.GetRequiredService<ILoggerFactory>()))
            .AddSingleton(svcs => new MauiBlazorBindingsRenderer(svcs.GetRequiredService<MauiBlazorBindingsServiceProvider>(), svcs.GetRequiredService<ILoggerFactory>()))
            .AddSingleton(svcs => new MauiBlazorBindingsServiceProvider(svcs));

        return builder;
    }
}
