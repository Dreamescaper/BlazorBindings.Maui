// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Maui;

internal class MauiBlazorBindingsElementManager : ElementManager
{
    public override void AddChildElement(
        IElementHandler parentHandler,
        IElementHandler childHandler,
        int physicalSiblingIndex)
    {
        if (childHandler is INonPhysicalChild nonPhysicalChild)
        {
            // If the child is a non-child container then we shouldn't try to add it to a parent.
            // This is used in cases such as ModalContainer, which exists for the purposes of Blazor
            // markup and is not represented in the Xamarin.Forms control hierarchy.

            nonPhysicalChild.SetParent(parentHandler.TargetElement);
            return;
        }

        if (parentHandler is not IContainerElementHandler parent)
        {
            throw new NotSupportedException($"Handler of type '{parentHandler.GetType().FullName}' representing element type " +
                $"'{parentHandler.TargetElement?.GetType().FullName ?? "<null>"}' doesn't support adding a child " +
                $"(child type is '{childHandler.TargetElement?.GetType().FullName}').");
        }

        parent.AddChild(childHandler.TargetElement, physicalSiblingIndex);
    }

    public override void RemoveChildElement(IElementHandler parentHandler, IElementHandler childHandler, int physicalSiblingIndex)
    {
        if (childHandler is INonPhysicalChild nonPhysicalChild)
        {
            nonPhysicalChild.RemoveFromParent(parentHandler.TargetElement);
        }
        else if (parentHandler is IContainerElementHandler parent)
        {
            parent.RemoveChild(childHandler.TargetElement, physicalSiblingIndex);
        }
        else
        {
            throw new NotSupportedException($"Handler of type '{parentHandler.GetType().FullName}' representing element type " +
                $"'{parentHandler.TargetElement?.GetType().FullName ?? "<null>"}' doesn't support removing a child " +
                $"(child type is '{childHandler.TargetElement?.GetType().FullName}').");
        }
    }
}
