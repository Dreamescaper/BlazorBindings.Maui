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
            parent.RemoveChild(physicalSiblingIndex);
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

        GetContainer(parentHandler).ReplaceChild(physicalSiblingIndex, newChild.TargetElement);
    }

    public virtual void AddChildElementRange(
        IElementHandler parentHandler,
        IReadOnlyList<object> children,
        int physicalSiblingIndex)
    {
        if (children.Count == 0)
            return;

        var container = GetContainer(parentHandler);
        if (children.Count == 1)
            container.AddChild(children[0], physicalSiblingIndex);
        else
            container.AddRange(physicalSiblingIndex, children);
    }

    public virtual void RemoveChildElementRange(
        IElementHandler parentHandler,
        int count,
        int physicalSiblingIndex)
    {
        if (count == 0)
            return;

        var container = GetContainer(parentHandler);
        if (count == 1)
            container.RemoveChild(physicalSiblingIndex);
        else
            container.RemoveRange(physicalSiblingIndex, count);
    }

    public virtual void ReplaceChildElementRange(
        IElementHandler parentHandler,
        int removedChildrenCount,
        IReadOnlyList<object> newChildren,
        int physicalSiblingIndex)
    {
        if (removedChildrenCount == 0)
        {
            AddChildElementRange(parentHandler, newChildren, physicalSiblingIndex);
            return;
        }

        if (newChildren.Count == 0)
        {
            RemoveChildElementRange(parentHandler, removedChildrenCount, physicalSiblingIndex);
            return;
        }

        var container = GetContainer(parentHandler);
        if (removedChildrenCount == 1 && newChildren.Count == 1)
            container.ReplaceChild(physicalSiblingIndex, newChildren[0]);
        else
            container.ReplaceRange(physicalSiblingIndex, removedChildrenCount, newChildren);
    }

    private static IContainerElementHandler GetContainer(IElementHandler parentHandler)
        => parentHandler as IContainerElementHandler
            ?? throw new InvalidOperationException($"Handler of type '{parentHandler.GetType().FullName}' does not support adding/removing child elements.");
}
