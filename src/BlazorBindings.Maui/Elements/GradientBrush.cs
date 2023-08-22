// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Diagnostics;
using MC = Microsoft.Maui.Controls;


namespace BlazorBindings.Maui.Elements;

public abstract partial class GradientBrush : Brush, IContainerElementHandler
{
#pragma warning disable CA1721 // Property names should not match get methods
    [Parameter] public RenderFragment ChildContent { get; set; }
#pragma warning restore CA1721 // Property names should not match get methods

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
        if (child is not MC.GradientStop gradientStopChild)
        {
            throw new ArgumentException($"GradientBrush support GradientStop child elements only, but {child?.GetType()} found instead.", nameof(child));
        }

        if (physicalSiblingIndex <= NativeControl.GradientStops.Count)
        {
            NativeControl.GradientStops.Insert(physicalSiblingIndex, gradientStopChild);
        }
        else
        {
            Debug.WriteLine($"WARNING: {nameof(IContainerElementHandler.AddChild)} called with {nameof(physicalSiblingIndex)}={physicalSiblingIndex}, but GradientBrushControl.GradientStops.Count={NativeControl.GradientStops}");
            NativeControl.GradientStops.Add(gradientStopChild);
        }
    }

    int IContainerElementHandler.GetChildIndex(object child)
    {
        if (child is not MC.GradientStop gradientStopChild)
        {
            throw new ArgumentException($"GradientBrush support GradientStop child elements only, but {child?.GetType()} found instead.", nameof(child));
        }

        return NativeControl.GradientStops.IndexOf(gradientStopChild);
    }

    void IContainerElementHandler.RemoveChild(object child)
    {
        if (child is not MC.GradientStop gradientStopChild)
        {
            throw new ArgumentException($"GradientBrush support GradientStop child elements only, but {child?.GetType()} found instead.", nameof(child));
        }

        NativeControl.GradientStops.Remove(gradientStopChild);
    }
}
