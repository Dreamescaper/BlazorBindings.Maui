// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components.RenderTree;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BlazorBindings.Core;

/// <summary>
/// Represents a "shadow" item that Blazor uses to map changes into the live native UI tree.
/// </summary>
[DebuggerDisplay("{GetDebugName}")]
internal sealed class NativeComponentAdapter(
    NativeComponentRenderer renderer,
    NativeComponentAdapter closestPhysicalParent,
    IElementHandler knownTargetElement = null)
    : IDisposable
{
    /// <summary>
    /// Used for debugging purposes.
    /// </summary>
    public string Name { get; internal set; }

    [RequiresUnreferencedCode("This method is used for debug only.")]
    [RequiresDynamicCode("This method is used for debug only.")]
    private string GetDebugName()
    {
        string text = null;
        try
        {
            text = (_targetElement?.TargetElement as dynamic)?.Text;
        }
        catch { }

        return $"[\"{text}\" {Name}";
    }

    public int DeepLevel { get; init; }

    public NativeComponentAdapter Parent { get; private set; }
    public List<NativeComponentAdapter> Children { get; } = [];

    private IElementHandler _targetElement = knownTargetElement;

    private NativeComponentAdapter PhysicalTarget => _targetElement != null ? this : closestPhysicalParent;

    public NativeComponentRenderer Renderer { get; } = renderer ?? throw new ArgumentNullException(nameof(renderer));

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

        var pendingRange = new PendingEditRange();
        var replacementRangeNewChildren = new ValueListBuilder<object>();


        for (var i = 0; i < _pendingEdits.Count; i++)
        {
            var edit = _pendingEdits[i];

            if (TryGetReplacementRange(i, ref replacementRangeNewChildren, out var replacementRangeEndIndex, out var replacementIndex, out var removedChildrenCount))
            {
                ApplyPendingRange(ref pendingRange, Renderer.ElementManager, _targetElement);
                Renderer.ElementManager.ReplaceChildElementRange(_targetElement, removedChildrenCount, replacementRangeNewChildren.AsSpan(), replacementIndex);
                i = replacementRangeEndIndex;
                replacementRangeNewChildren.Length = 0;
            }
            else if (edit.Type == PendingEditType.Remove)
            {
                if (edit.Element._targetElement is INonPhysicalChild)
                {
                    ApplyPendingRange(ref pendingRange, Renderer.ElementManager, _targetElement);
                    Renderer.ElementManager.RemoveChildElement(_targetElement, edit.Element._targetElement, edit.Index);
                }
                else
                {
                    AddToPendingRange(ref pendingRange, PendingEditType.Remove, edit.Index, Renderer.ElementManager, _targetElement);
                }
            }
            else if (edit.Type == PendingEditType.Add)
            {
                if (edit.Element._targetElement is INonPhysicalChild)
                {
                    ApplyPendingRange(ref pendingRange, Renderer.ElementManager, _targetElement);
                    Renderer.ElementManager.AddChildElement(_targetElement, edit.Element._targetElement, edit.Index);
                }
                else
                {
                    AddToPendingRange(ref pendingRange, PendingEditType.Add, edit.Index, Renderer.ElementManager, _targetElement, edit.Element._targetElement.TargetElement);
                }
            }
        }

        ApplyPendingRange(ref pendingRange, Renderer.ElementManager, _targetElement);
        pendingRange.Dispose();
        replacementRangeNewChildren.Dispose();
        _pendingEdits.Clear();

        static void ApplyPendingRange(ref PendingEditRange pendingRange, ElementManager mgr, IElementHandler target)
        {
            if (pendingRange.Type == null)
                return;

            if (pendingRange.Type == PendingEditType.Remove)
                mgr.RemoveChildElementRange(target, pendingRange.Count, pendingRange.Index);
            else if (pendingRange.Type == PendingEditType.Add)
                mgr.AddChildElementRange(target, pendingRange.NewElements, pendingRange.Index);

            pendingRange.Clear();
        }

        static void AddToPendingRange(ref PendingEditRange pendingRange, PendingEditType type, int index, ElementManager mgr, IElementHandler target, object newElement = null)
        {
            if (!pendingRange.CanAppend(type, index))
                ApplyPendingRange(ref pendingRange, mgr, target);

            pendingRange.Append(type, index, newElement);
        }
    }

    private bool TryGetReplacementRange(
        int startIndex,
        ref ValueListBuilder<object> newChildren,
        out int endIndex,
        out int replacementIndex,
        out int removedChildrenCount)
    {
        endIndex = startIndex;
        replacementIndex = -1;
        removedChildrenCount = 0;

        var edit = _pendingEdits[startIndex];
        if (edit is not { Type: PendingEditType.Remove, Element._targetElement: not INonPhysicalChild })
            return false;

        replacementIndex = edit.Index;
        var addStartIndex = startIndex;
        while (addStartIndex < _pendingEdits.Count
            && _pendingEdits[addStartIndex] is { Type: PendingEditType.Remove, Element._targetElement: not INonPhysicalChild } removal
            && removal.Index == replacementIndex)
        {
            removedChildrenCount++;
            addStartIndex++;
        }

        if (addStartIndex == _pendingEdits.Count)
            return false;

        var firstAdd = _pendingEdits[addStartIndex];
        if (firstAdd is not { Type: PendingEditType.Add, Element._targetElement: not INonPhysicalChild } || firstAdd.Index != replacementIndex)
            return false;

        var addEndIndex = addStartIndex;
        var expectedAddIndex = replacementIndex;
        while (addEndIndex < _pendingEdits.Count
            && _pendingEdits[addEndIndex] is { Type: PendingEditType.Add, Element._targetElement: not INonPhysicalChild } addition
            && addition.Index == expectedAddIndex)
        {
            addEndIndex++;
            expectedAddIndex++;
        }

        newChildren.Length = 0;
        for (var i = addStartIndex; i < addEndIndex; i++)
            newChildren.Append(_pendingEdits[i].Element._targetElement.TargetElement);

        endIndex = addEndIndex - 1;
        return true;
    }

    private void AddPendingRemoval(NativeComponentAdapter childToRemove, int index, HashSet<NativeComponentAdapter> adaptersWithPendingEdits)
    {
        var targetEdits = PhysicalTarget._pendingEdits ??= [];
        adaptersWithPendingEdits.Add(PhysicalTarget);

        if (targetEdits.Count == 0)
        {
            targetEdits.Add(new(PendingEditType.Remove, index, childToRemove));
            return;
        }

        // If elements are added and removed, we want to put removal closer before the corresponding addition,
        // to allow replacing instead.
        // But because the order of operations changes, we need to adjust indexes.
        int i;
        for (i = targetEdits.Count; i > 0; i--)
        {
            var previousEdit = targetEdits[i - 1];

            if (previousEdit.Type == PendingEditType.Remove)
                break;

            if (previousEdit.Index < index - 1)
                break;

            // Generally we try to put Remove edit before an Add edit.
            // But if there's already a Remove edit before that Add edit, with a matching index, 
            // we don't need to put another Remove there.
            if (i >= 2
                && previousEdit.Type == PendingEditType.Add
                && targetEdits[i - 2] is { Type: PendingEditType.Remove } previousRemoval
                && previousRemoval.Index == previousEdit.Index)
            {
                break;
            }

            if (previousEdit.Index <= index)
                index--;

            if (previousEdit.Index > index)
                targetEdits[i - 1] = previousEdit with { Index = previousEdit.Index - 1 };
        }

        targetEdits.Insert(i, new(PendingEditType.Remove, index, childToRemove));
    }

    private void AddPendingAddition(NativeComponentAdapter childToAdd, int index, HashSet<NativeComponentAdapter> adaptersWithPendingEdits)
    {
        /* In cases when there are non-elements involved, the order of add operations could be wrong. E.g. 
        AppShell.razor
        <Shell>
            <UserPageComponent Title="Page1" />
            <ContentPage Title="Page2" />
        </Shell>
        
        UserPageComponent.razor
        <ContentPage Title="Page1" />

        In this case, Page1 is added to Shell first (with index 0), and then Page2 is added (again, with index 0).
        So the final order is correct - Page1, Page2.
        But because Page1 was added first, Shell would set it as a current page.

        To avoid such behavior, we attempt to re-order Add operations by index - to add Page1 with index 0, then Page2 with index1.
        */

        var targetEdits = PhysicalTarget._pendingEdits ??= [];


        int i;
        for (i = targetEdits.Count; i > 0; i--)
        {
            var previousEdit = targetEdits[i - 1];

            if (previousEdit.Type != PendingEditType.Add)
                break;

            if (previousEdit.Index < index)
                break;

            // If previous addition has greater index - we switch them places, and increment previous index.
            targetEdits[i - 1] = previousEdit with { Index = previousEdit.Index + 1 };
        }

        targetEdits.Insert(i, new(PendingEditType.Add, index, childToAdd));
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
                        if (_targetElement is IHandleChildContentText handleChildContentText)
                        {
                            handleChildContentText.HandleText(siblingIndex, frame.MarkupContent);
                        }
                        else
                        {
                            throw new NotImplementedException($"Element {GetDebugTypeName()} does not support text content: " + frame.MarkupContent);
                        }
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
                        throw new NotImplementedException($"Element {GetDebugTypeName()} does not support text content: " + frame.MarkupContent);
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

    private string GetDebugTypeName()
    {
        var obj = _targetElement?.TargetElement
            ?? _targetElement
            ?? closestPhysicalParent?._targetElement?.TargetElement
            ?? closestPhysicalParent?._targetElement;

        return obj?.GetType().Name;
    }

    record struct PendingEdit(PendingEditType Type, int Index, NativeComponentAdapter Element);

    private ref struct PendingEditRange
    {
        public PendingEditType? Type { get; private set; }
        public int Index { get; private set; }
        public int Count { get; private set; }
        private ValueListBuilder<object> _newElements;

        public ReadOnlySpan<object> NewElements => _newElements.AsSpan();

        public bool CanAppend(PendingEditType type, int index)
        {
            if (Type == null)
                return true;

            if (Type != type)
                return false;

            return type switch
            {
                PendingEditType.Add => index == Index + Count,
                PendingEditType.Remove => index == Index,
                _ => false
            };
        }

        public void Append(PendingEditType type, int index, object newElement)
        {
            Type ??= type;
            Index = Count > 0 ? Index : index;
            Count++;

            if (type == PendingEditType.Add)
                _newElements.Append(newElement);
        }

        public void Clear()
        {
            Type = null;
            Index = 0;
            Count = 0;
            _newElements.Length = 0;
        }

        public void Dispose() => _newElements.Dispose();
    }

    enum PendingEditType { Add, Remove }
}
