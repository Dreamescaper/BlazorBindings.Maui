using Microsoft.AspNetCore.Components.Rendering;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class Element
{
    [Parameter] public RenderFragment ContextFlyout { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(ContextFlyout))
        {
            ContextFlyout = (RenderFragment)value;
            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }

    protected override void RenderAdditionalPartialElementContent(RenderTreeBuilder builder, ref int sequence)
    {
        base.RenderAdditionalPartialElementContent(builder, ref sequence);

        RenderTreeBuilderHelper.AddContentProperty<MC.Element>(builder, sequence++, ContextFlyout, (x, value) =>
            MC.FlyoutBase.SetContextFlyout(x, (MC.FlyoutBase)value));
    }
}
