using BlazorBindings.Maui;
using BlazorBindings.Maui.Elements;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorBindings.UnitTests.Components;

public class EagerTemplateOwner : MC.BindableObject
{
    private MC.DataTemplate _template;

    public MC.DataTemplate Template
    {
        get => _template;
        set
        {
            _template = value;
            CreatedContent = value?.CreateContent();
        }
    }

    public object CreatedContent { get; private set; }
}

public class EagerSyncTemplateControl : BlazorBindings.Maui.Elements.BindableObject
{
    [Parameter] public RenderFragment Template { get; set; }

    public new EagerTemplateOwner NativeControl =>
        (EagerTemplateOwner)((BlazorBindings.Maui.Elements.BindableObject)this).NativeControl;

    protected override MC.BindableObject CreateNativeElement() => new EagerTemplateOwner();

    protected override void HandleParameter(string name, object value)
    {
        if (name == nameof(Template))
        {
            Template = CastParameter<RenderFragment>(value, name);
            return;
        }

        base.HandleParameter(name, value);
    }

    protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
    {
        base.RenderAdditionalElementContent(builder, ref sequence);
        RenderTreeBuilderHelper.AddSyncDataTemplateProperty<EagerTemplateOwner>(
            builder,
            sequence++,
            Template,
            (x, template) => x.Template = template);
    }
}
