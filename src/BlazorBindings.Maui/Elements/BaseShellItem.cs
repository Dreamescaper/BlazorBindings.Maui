// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components.Rendering;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class BaseShellItem : NavigableElement
{
    [Parameter] public RenderFragment<MC.BaseShellItem> ItemTemplate { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(ItemTemplate))
        {
            ItemTemplate = (RenderFragment<MC.BaseShellItem>)value;
            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }

    protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
    {
        base.RenderAdditionalElementContent(builder, ref sequence);

        RenderTreeBuilderHelper.AddDataTemplateProperty<MC.BaseShellItem, MC.BaseShellItem>(builder, sequence++, ItemTemplate,
            (shellItem, dataTemplate) => MC.Shell.SetItemTemplate(shellItem, dataTemplate));
    }
}
