using BlazorBindings.Maui;
using Material.Components.Maui.Styles;

namespace ThirdPartyControlsSample;

public class App : BlazorBindingsApplication<AppShell>
{
    public App(IServiceProvider services) : base(services)
    {
        Resources.Add(new MaterialStyles());
    }
}
