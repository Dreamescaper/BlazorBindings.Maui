using BlazorBindings.Maui;
using StylingSample.Resources;

namespace StylingSample;

public class App : BlazorBindingsApplication<AppShell>
{
    public App(IServiceProvider services) : base(services)
    {
        Resources.Add(AppStyles.Default);
    }
}
