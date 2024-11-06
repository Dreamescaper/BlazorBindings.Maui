using BlazorBindings.Maui;
using Material.Components.Maui.Styles;

namespace ThirdPartyControlsSample;

public class App(IServiceProvider services) : BlazorBindingsApplication<AppShell>(services)
{
    protected override void Configure()
    {
        Resources.Add(new MaterialStyles());
    }
}
