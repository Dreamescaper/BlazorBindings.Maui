// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Core;

/// <summary>
/// Utilities needed by the system to manage native controls. Implementations
/// of native rendering systems have their own quirks in terms of dealing with
/// parent/child relationships, so each must implement this given the constraints
/// and requirements of their systems.
/// </summary>
public class ElementManager
{
    public virtual void AddChildElement(
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

    public virtual void RemoveChildElement(IElementHandler parentHandler, IElementHandler childHandler, int physicalSiblingIndex)
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

    public virtual void ReplaceChildElement(IElementHandler parentHandler, IElementHandler oldChild, IElementHandler newChild, int physicalSiblingIndex)
    {
        if (oldChild is INonPhysicalChild || newChild is INonPhysicalChild)
            throw new NotSupportedException("NonPhysicalChild elements cannot be replaced.");

        if (parentHandler is not IContainerElementHandler container)
            throw new InvalidOperationException($"Handler of type '{parentHandler.GetType().FullName}' does not support replacing child elements.");

        container.ReplaceChild(physicalSiblingIndex, oldChild.TargetElement, newChild.TargetElement);
    }
}
