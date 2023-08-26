// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Maui.Elements.Internal;

internal class RootContainerComponent : NativeControlComponentBase, IContainerElementHandler, INonPhysicalChild
{
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public EventCallback<object> OnElementAdded { get; set; }

    protected override RenderFragment GetChildContent() => ChildContent;

    public List<object> Elements { get; } = new();

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        var index = Math.Min(physicalSiblingIndex, Elements.Count);
        Elements.Insert(index, child);
        OnElementAdded.InvokeAsync(child);
    }

    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex)
    {
        Elements.Remove(child);
    }

    // Because this is a 'fake' container element, all matters related to physical trees
    // should be no-ops.
    object IElementHandler.TargetElement => null;
    void INonPhysicalChild.SetParent(object parentElement) { }
    void INonPhysicalChild.RemoveFromParent(object parentElement) { }
}
