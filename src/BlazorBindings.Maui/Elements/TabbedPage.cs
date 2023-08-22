// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Diagnostics;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class TabbedPage : Page, IContainerElementHandler
{
    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override RenderFragment GetChildContent() => ChildContent;

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(ChildContent))
        {
            ChildContent = (RenderFragment)value;
            return true;
        }
        else
        {
            return base.HandleAdditionalParameter(name, value);
        }
    }

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        var childAsPage = child as MC.Page;

        if (physicalSiblingIndex <= NativeControl.Children.Count)
        {
            NativeControl.Children.Insert(physicalSiblingIndex, childAsPage);
        }
        else
        {
            Debug.WriteLine($"WARNING: {nameof(NativeControl)} called with {nameof(physicalSiblingIndex)}={physicalSiblingIndex}, but TabbedPageControl.Children.Count={NativeControl.Children.Count}");
            NativeControl.Children.Add(childAsPage);
        }
    }

    int IContainerElementHandler.GetChildIndex(object child)
    {
        return NativeControl.Children.IndexOf(child as MC.Page);
    }

    void IContainerElementHandler.RemoveChild(object child)
    {
        NativeControl.Children.Remove(child as MC.Page);
    }
}
