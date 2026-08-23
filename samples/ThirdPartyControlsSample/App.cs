using BlazorBindings.Maui;
using Material.Components.Maui.Styles;

namespace ThirdPartyControlsSample;

public class App : BlazorBindingsApplication<NaluAppScaffold>
{
    public App()
    {
        Resources.Add(new MaterialStyles());
    }
}
