// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components.RenderTree;
using System.Diagnostics;

namespace BlazorBindings.Core;

/// <summary>
/// Represents a "shadow" item that Blazor uses to map changes into the live native UI tree.
/// </summary>
[DebuggerDisplay("{DebugName}")]
internal sealed class NativeComponentAdapter : IDisposable
{
    public NativeComponentAdapter(NativeComponentRenderer renderer, NativeComponentAdapter closestPhysicalParent, IElementHandler knownTargetElement = null)
    {
        Renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        _closestPhysicalParent = closestPhysicalParent;
        _targetElement = knownTargetElement;
    }


    /// <summary>
    /// Used for debugging purposes.
    /// </summary>
    public string Name { get; internal set; }

    private string Text
    {
        get
        {
            try
            {
                return (_targetElement?.TargetElement as dynamic)?.Text;
            }
            catch
            {
                return null;
            }
        }
    }

    private string DebugName => $"[\"{Text}\" {Name}";

    public int DeepLevel { get; init; }

    public NativeComponentAdapter Parent { get; private set; }
    public List<NativeComponentAdapter> Children { get; } = new();

    private readonly NativeComponentAdapter _closestPhysicalParent;
    private IElementHandler _targetElement;

    private NativeComponentAdapter PhysicalTarget => _targetElement != null ? this : _closestPhysicalParent;

    public NativeComponentRenderer Renderer { get; }

    private List<PendingEdit> _pendingEdits;

    internal void ApplyEdits(
        int componentId,
        ArrayBuilderSegment<RenderTreeEdit> edits,
        RenderBatch batch,
        HashSet<NativeComponentAdapter> adaptersWithPendingEdits)
    {
        var referenceFrames = batch.ReferenceFrames.Array;

        foreach (var edit in edits)
        {
            switch (edit.Type)
            {
                case RenderTreeEditType.PrependFrame:
                    ApplyPrependFrame(batch, componentId, edit.SiblingIndex, referenceFrames, edit.ReferenceFrameIndex, adaptersWithPendingEdits);
                    break;
                case RenderTreeEditType.RemoveFrame:
                    ApplyRemoveFrame(edit.SiblingIndex, adaptersWithPendingEdits);
                    break;
                case RenderTreeEditType.UpdateText:
                    {
                        var frame = referenceFrames[edit.ReferenceFrameIndex];
                        if (_targetElement is IHandleChildContentText handleChildContentText)
                        {
                            handleChildContentText.HandleText(edit.SiblingIndex, frame.TextContent);
                        }
                        else if (!string.IsNullOrWhiteSpace(frame.TextContent))
                        {
                            throw new Exception("Cannot set text content on child that doesn't handle inner text content.");
                        }
                        break;
                    }
                case RenderTreeEditType.StepIn:
                case RenderTreeEditType.StepOut:
                    {
                        // TODO: Need to implement this. For now it seems safe to ignore.
                        break;
                    }
                case RenderTreeEditType.UpdateMarkup:
                    {
                        var frame = referenceFrames[edit.ReferenceFrameIndex];
                        if (!string.IsNullOrWhiteSpace(frame.MarkupContent))
                            throw new NotImplementedException($"Not supported edit type: {edit.Type}");

                        break;
                    }
                default:
                    throw new NotImplementedException($"Not supported edit type: {edit.Type}");
            }
        }
    }

    // a) We want to add child element from the deepest element to the top one, so that elements are added to parents with all the required changes.
    // b) If elements are replaced, we want to have a single edit instead of two separate ones (remove+add) - it's more efficient, and 
    // the only way in some cases (when elements don't support empty content).
    // Therefore we store all add/remove actions, and apply them (rearranged) after other edits.
    public void ApplyPendingEdits()
    {
        if (_pendingEdits == null)
            return;

        for (var i = 0; i < _pendingEdits.Count; i++)
        {
            var edit = _pendingEdits[i];
            var nextEdit = _pendingEdits.ElementAtOrDefault(i + 1);

            // If we have two consequent edits (Add -> Remove or Remove -> Add) for the same index,
            // and non of them are INonPhysicalChild elements,
            // we try to replace them instead of adding and removing separately.
            if (nextEdit.Index == edit.Index
                && edit is { Type: EditType.Remove, Element._targetElement: not INonPhysicalChild }
                && nextEdit is { Type: EditType.Add, Element._targetElement: not INonPhysicalChild })
            {
                Renderer.ElementManager.ReplaceChildElement(_targetElement, edit.Element._targetElement, nextEdit.Element._targetElement, edit.Index);
                i++;
            }
            else if (edit.Type == EditType.Remove)
            {
                Renderer.ElementManager.RemoveChildElement(_targetElement, edit.Element._targetElement, edit.Index);
            }
            else if (edit.Type == EditType.Add)
            {
                Renderer.ElementManager.AddChildElement(_targetElement, edit.Element._targetElement, edit.Index);
            }
        }

        _pendingEdits.Clear();
    }

    private void AddPendingRemoval(NativeComponentAdapter childToRemove, int index, HashSet<NativeComponentAdapter> adaptersWithPendingEdits)
    {
        var targetEdits = PhysicalTarget._pendingEdits ??= new();
        adaptersWithPendingEdits.Add(PhysicalTarget);

        if (targetEdits.Count == 0)
        {
            targetEdits.Add(new(EditType.Remove, index, childToRemove));
            return;
        }

        // If elements are added and removed, we want to put removal closer before the corresponding addition,
        // to allow replacing instead.
        // But because the order of operations changes, we need to adjust indexes.
        int i;
        for (i = targetEdits.Count; i > 0; i--)
        {
            var previousEdit = targetEdits[i - 1];

            if (previousEdit.Type == EditType.Remove)
                break;

            if (previousEdit.Index < index - 1)
                break;

            // Generally we try to put Remove edit before an Add edit.
            // But if there's already a Remove edit before that Add edit, with a matching index, 
            // we don't need to put another Remove there.
            if (i >= 2
                && previousEdit.Type == EditType.Add
                && targetEdits[i - 2] is { Type: EditType.Remove } previousRemoval
                && previousRemoval.Index == previousEdit.Index)
            {
                break;
            }

            if (previousEdit.Index <= index)
                index--;

            if (previousEdit.Index > index)
                targetEdits[i - 1] = previousEdit with { Index = previousEdit.Index - 1 };
        }

        targetEdits.Insert(i, new(EditType.Remove, index, childToRemove));
    }

    private void AddPendingAddition(NativeComponentAdapter childToAdd, int index, HashSet<NativeComponentAdapter> adaptersWithPendingEdits)
    {
        var targetEdits = PhysicalTarget._pendingEdits ??= new();
        targetEdits.Add(new(EditType.Add, index, childToAdd));
        adaptersWithPendingEdits.Add(PhysicalTarget);
    }

    private void ApplyRemoveFrame(int siblingIndex, HashSet<NativeComponentAdapter> adaptersWithPendingEdits)
    {
        var childToRemove = Children[siblingIndex];
        RemoveChildElementAndDescendants(childToRemove, adaptersWithPendingEdits);
        Children.RemoveAt(siblingIndex);
    }

    private void RemoveChildElementAndDescendants(NativeComponentAdapter childToRemove, HashSet<NativeComponentAdapter> adaptersWithPendingEdits)
    {
        if (childToRemove?._targetElement != null)
        {
            // This adapter represents a physical element, so by removing it, we implicitly
            // remove all descendants.
            var index = PhysicalTarget.GetChildPhysicalIndex(childToRemove);
            PhysicalTarget.AddPendingRemoval(childToRemove, index, adaptersWithPendingEdits);

            if (PhysicalTarget._targetElement is INonPhysicalChild { ShouldAddChildrenToParent: true })
            {
                // Since element was added to parent previously, we have to remove it from there.
                PhysicalTarget.Parent.RemoveChildElementAndDescendants(childToRemove, adaptersWithPendingEdits);
            }
        }
        else if (childToRemove != null)
        {
            // This adapter is just a container for other adapters
            for (int i = 0; i < childToRemove.Children.Count; i++)
                childToRemove.ApplyRemoveFrame(i, adaptersWithPendingEdits);
        }
    }

    private int ApplyPrependFrame(
        RenderBatch batch,
        int componentId,
        int siblingIndex,
        RenderTreeFrame[] frames,
        int frameIndex,
        HashSet<NativeComponentAdapter> adaptersWithPendingEdits)
    {
        ref var frame = ref frames[frameIndex];
        switch (frame.FrameType)
        {
            case RenderTreeFrameType.Component:
                {
                    var childAdapter = AddChildAdapter(siblingIndex, frame);

                    if (childAdapter._targetElement is not null)
                        AddElementAsChildElement(childAdapter, adaptersWithPendingEdits);

                    return 1;
                }
            case RenderTreeFrameType.Region:
                {
                    return InsertFrameRange(batch, componentId, siblingIndex, frames, frameIndex + 1, frameIndex + frame.RegionSubtreeLength, adaptersWithPendingEdits);
                }
            case RenderTreeFrameType.Markup:
                {
                    if (!string.IsNullOrWhiteSpace(frame.MarkupContent))
                    {
                        throw new NotImplementedException($"Not supported frame type: {frame.FrameType}");
                    }
                    // We don't need any adapter for Markup frames, but we care about frame position, therefore we simply insert null here.
                    Children.Insert(siblingIndex, null);
                    return 1;
                }
            case RenderTreeFrameType.Text:
                {
                    if (_targetElement is IHandleChildContentText handleChildContentText)
                    {
                        handleChildContentText.HandleText(siblingIndex, frame.TextContent);
                    }
                    else if (!string.IsNullOrWhiteSpace(frame.TextContent))
                    {
                        var typeName = _targetElement?.TargetElement?.GetType()?.Name;
                        throw new NotImplementedException($"Element {typeName} does not support text content: " + frame.MarkupContent);
                    }
                    // We don't need any adapter for Text frames, but we care about frame position, therefore we simply insert null here.
                    Children.Insert(siblingIndex, null);
                    return 1;
                }
            default:
                throw new NotImplementedException($"Not supported frame type: {frame.FrameType}");
        }
    }

    /// <summary>
    /// Add element as a child element for closest physical parent.
    /// </summary>
    private void AddElementAsChildElement(NativeComponentAdapter childAdapter, HashSet<NativeComponentAdapter> adaptersWithPendingEdits)
    {
        if (childAdapter is null)
            return;

        var elementIndex = PhysicalTarget.GetChildPhysicalIndex(childAdapter);

        // For most elements we should add element as child after all properties to have them fully initialized before rendering.
        // However, INonPhysicalChild elements are not real elements, but apply to parent instead, therefore should be added as child before any properties are set.
        if (childAdapter._targetElement is INonPhysicalChild)
        {
            Renderer.ElementManager.AddChildElement(PhysicalTarget._targetElement, childAdapter._targetElement, elementIndex);
        }
        else
        {
            AddPendingAddition(childAdapter, elementIndex, adaptersWithPendingEdits);
        }

        if (PhysicalTarget._targetElement is INonPhysicalChild { ShouldAddChildrenToParent: true })
        {
            PhysicalTarget.Parent.AddElementAsChildElement(childAdapter, adaptersWithPendingEdits);
        }
    }

    /// <summary>
    /// Finds the sibling index to insert this adapter's element into.
    /// <code>
    /// * Adapter0
    /// * Adapter1
    /// * Adapter2
    /// * Adapter3 (native)
    ///     * Adapter3.0 (searchOrder=2)
    ///         * Adapter3.0.0 (searchOrder=3)
    ///         * Adapter3.0.1 (native)  (searchOrder=4) &lt;-- This is the nearest earlier sibling that has a physical element)
    ///         * Adapter3.0.2
    ///     * Adapter3.1 (searchOrder=1)
    ///         * Adapter3.1.0 (searchOrder=0)
    ///         * Adapter3.1.1 (native) &lt;-- Current adapter
    ///         * Adapter3.1.2
    ///     * Adapter3.2
    /// * Adapter4
    /// </code>
    /// </summary>
    private int GetChildPhysicalIndex(NativeComponentAdapter childAdapter)
    {
        var index = 0;
        return FindChildPhysicalIndexRecursive(this, childAdapter, ref index) ? index : -1;

        static bool FindChildPhysicalIndexRecursive(NativeComponentAdapter parent, NativeComponentAdapter targetChild, ref int index)
        {
            foreach (var child in parent.Children)
            {
                if (child is null)
                    continue;

                if (child == targetChild)
                    return true;

                if (child._targetElement != null && child._targetElement is not INonPhysicalChild)
                {
                    index++;
                }

                if (child._targetElement == null || child._targetElement is INonPhysicalChild { ShouldAddChildrenToParent: true })
                {
                    if (FindChildPhysicalIndexRecursive(child, targetChild, ref index))
                        return true;
                }
            }

            return false;
        }
    }

    private int InsertFrameRange(
        RenderBatch batch,
        int componentId,
        int childIndex,
        RenderTreeFrame[] frames,
        int startIndex,
        int endIndexExcl,
        HashSet<NativeComponentAdapter> adaptersWithPendingEdits)
    {
        var origChildIndex = childIndex;
        for (var frameIndex = startIndex; frameIndex < endIndexExcl; frameIndex++)
        {
            ref var frame = ref batch.ReferenceFrames.Array[frameIndex];
            var numChildrenInserted = ApplyPrependFrame(batch, componentId, childIndex, frames, frameIndex, adaptersWithPendingEdits);
            childIndex += numChildrenInserted;

            // Skip over any descendants, since they are already dealt with recursively
            frameIndex += CountDescendantFrames(frame);
        }

        return (childIndex - origChildIndex); // Total number of children inserted     
    }

    private static int CountDescendantFrames(RenderTreeFrame frame)
    {
        return frame.FrameType switch
        {
            // The following frame types have a subtree length. Other frames may use that memory slot
            // to mean something else, so we must not read it. We should consider having nominal subtypes
            // of RenderTreeFramePointer that prevent access to non-applicable fields.
            RenderTreeFrameType.Component => frame.ComponentSubtreeLength - 1,
            RenderTreeFrameType.Element => frame.ElementSubtreeLength - 1,
            RenderTreeFrameType.Region => frame.RegionSubtreeLength - 1,
            _ => 0,
        };
        ;
    }

    private NativeComponentAdapter AddChildAdapter(int siblingIndex, RenderTreeFrame frame)
    {
        var name = frame.FrameType is RenderTreeFrameType.Component
            ? $"For: '{frame.Component.GetType().FullName}'"
            : $"{frame.FrameType}, sib#={siblingIndex}";

        var childAdapter = new NativeComponentAdapter(Renderer, PhysicalTarget)
        {
            Parent = this,
            Name = name,
            DeepLevel = DeepLevel + 1
        };

        if (frame.FrameType is RenderTreeFrameType.Component)
        {
            Renderer.RegisterComponentAdapter(childAdapter, frame.ComponentId);

            if (frame.Component is IElementHandler targetHandler)
            {
                childAdapter._targetElement = targetHandler;
            }
        }

        Children.Insert(siblingIndex, childAdapter);

        return childAdapter;
    }

    public void Dispose()
    {
        if (_targetElement is IDisposable disposableTargetElement)
        {
            disposableTargetElement.Dispose();
        }
    }

    record struct PendingEdit(EditType Type, int Index, NativeComponentAdapter Element);
    enum EditType { Add, Remove }
}
