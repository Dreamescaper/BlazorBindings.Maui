﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components.Rendering;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class ShellContent : BaseShellItem, IMauiContainerElementHandler
{
    /// <summary>
    /// Gets or sets a data template to create when ShellContent becomes active.
    /// </summary>
    [Parameter] public RenderFragment ContentTemplate { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override RenderFragment GetChildContent() => ChildContent;

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(ChildContent))
        {
            ChildContent = (RenderFragment)value;
            return true;
        }
        if (name == nameof(ContentTemplate))
        {
            ContentTemplate = (RenderFragment)value;
            return true;
        }
        else
        {
            return base.HandleAdditionalParameter(name, value);
        }
    }

    void IMauiContainerElementHandler.AddChild(MC.BindableObject child, int physicalSiblingIndex)
    {
        NativeControl.Content = child;
    }

    int IMauiContainerElementHandler.GetChildIndex(MC.BindableObject child)
    {
        return child == NativeControl.Content ? 0 : -1;
    }

    void IMauiContainerElementHandler.RemoveChild(MC.BindableObject child)
    {
        if (NativeControl.Content == child)
        {
            NativeControl.Content = null;
        }
    }

    protected override void RenderAdditionalPartialElementContent(RenderTreeBuilder builder, ref int sequence)
    {
        base.RenderAdditionalPartialElementContent(builder, ref sequence);

        RenderTreeBuilderHelper.AddSyncDataTemplateProperty<MC.ShellContent>(builder, sequence++, ContentTemplate, (x, template) => x.ContentTemplate = template);
    }
}
