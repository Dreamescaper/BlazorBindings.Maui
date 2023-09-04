// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class ShellContent : BaseShellItem, IContainerElementHandler
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

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        NativeControl.Content = child;
    }

    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex)
    {
        Debug.Assert(NativeControl.Content == child);
        NativeControl.Content = null;
    }

    void IContainerElementHandler.ReplaceChild(int physicalSiblingIndex, object oldChild, object newChild)
    {
        Debug.Assert(NativeControl.Content == oldChild);
        NativeControl.Content = newChild;
    }

    protected override void RenderAdditionalPartialElementContent(RenderTreeBuilder builder, ref int sequence)
    {
        base.RenderAdditionalPartialElementContent(builder, ref sequence);

        RenderTreeBuilderHelper.AddSyncDataTemplateProperty<MC.ShellContent>(builder, sequence++, ContentTemplate, (x, template) => x.ContentTemplate = template);
    }
}
