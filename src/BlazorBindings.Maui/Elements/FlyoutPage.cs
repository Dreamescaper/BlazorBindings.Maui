using BlazorBindings.Maui.Extensions;
using Microsoft.AspNetCore.Components.Rendering;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class FlyoutPage : Page
{
    [Parameter] public RenderFragment Detail { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(Detail))
        {
            Detail = (RenderFragment)value;
            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }

    protected override void RenderAdditionalPartialElementContent(RenderTreeBuilder builder, ref int sequence)
    {
        base.RenderAdditionalPartialElementContent(builder, ref sequence);

        RenderTreeBuilderHelper.AddContentProperty<MC.FlyoutPage>(builder, sequence++, Detail, (page, value) =>
        {
            if (value is not MC.NavigationPage navigationPage)
                navigationPage = new MC.NavigationPage(value.Cast<MC.Page>());

            page.Detail = navigationPage;
        });
    }
}
