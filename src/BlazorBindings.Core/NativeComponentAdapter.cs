// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Diagnostics;

namespace BlazorBindings.Core;

/// <summary>
/// Represents a "shadow" item that Blazor uses to map changes into the live native UI tree.
/// </summary>
[DebuggerDisplay("{DebugName}")]
internal sealed class NativeComponentAdapter : IDisposable
{
    private static volatile int DebugInstanceCounter;

    public NativeComponentAdapter(NativeComponentRenderer renderer, NativeComponentAdapter closestPhysicalParent, IElementHandler knownTargetElement = null)
    {
        Renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        _closestPhysicalParent = closestPhysicalParent;
        _targetElement = knownTargetElement;

        // Assign unique counter value. This *should* all be done on one thread, but just in case, make it thread-safe.
        _debugInstanceCounterValue = Interlocked.Increment(ref DebugInstanceCounter);
    }

    private readonly int _debugInstanceCounterValue;

    private string DebugName => $"[#{_debugInstanceCounterValue}] {Name}";

    public NativeComponentAdapter Parent { get; private set; }
    public List<NativeComponentAdapter> Children { get; } = new();

    private readonly NativeComponentAdapter _closestPhysicalParent;
    private IElementHandler _targetElement;
    private IComponent _targetComponent;

    private NativeComponentAdapter PhysicalTarget => _targetElement != null ? this : _closestPhysicalParent;

    public NativeComponentRenderer Renderer { get; }

    /// <summary>
    /// Used for debugging purposes.
    /// </summary>
    public string Name { get; internal set; }

    public override string ToString()
    {
        return $"{nameof(NativeComponentAdapter)}: Name={Name ?? "<?>"}, Target={_targetElement?.GetType().Name ?? "<None>"}, #Children={Children.Count}";
    }

    internal void ApplyEdits(int componentId, ArrayBuilderSegment<RenderTreeEdit> edits, ArrayRange<RenderTreeFrame> referenceFrames, RenderBatch batch, HashSet<int> processedComponentIds)
    {
        Renderer.Dispatcher.AssertAccess();

        foreach (var edit in edits)
        {
            switch (edit.Type)
            {
                case RenderTreeEditType.PrependFrame:
                    ApplyPrependFrame(batch, componentId, edit.SiblingIndex, referenceFrames.Array, edit.ReferenceFrameIndex, processedComponentIds);
                    break;
                case RenderTreeEditType.RemoveFrame:
                    ApplyRemoveFrame(edit.SiblingIndex);
                    break;
                case RenderTreeEditType.UpdateText:
                    {
                        var frame = batch.ReferenceFrames.Array[edit.ReferenceFrameIndex];
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
                    {
                        // TODO: Need to implement this. For now it seems safe to ignore.
                        break;
                    }
                case RenderTreeEditType.StepOut:
                    {
                        // TODO: Need to implement this. For now it seems safe to ignore.
                        break;
                    }
                case RenderTreeEditType.UpdateMarkup:
                    {
                        var frame = batch.ReferenceFrames.Array[edit.ReferenceFrameIndex];
                        if (!string.IsNullOrWhiteSpace(frame.MarkupContent))
                            throw new NotImplementedException($"Not supported edit type: {edit.Type}");

                        break;
                    }
                default:
                    throw new NotImplementedException($"Not supported edit type: {edit.Type}");
            }
        }
    }

    private void ApplyRemoveFrame(int siblingIndex)
    {
        var childToRemove = Children[siblingIndex];
        RemoveChildElementAndDescendants(childToRemove);
        Children.RemoveAt(siblingIndex);
    }

    private void RemoveChildElementAndDescendants(NativeComponentAdapter childToRemove)
    {
        if (childToRemove._targetElement != null)
        {
            // This adapter represents a physical element, so by removing it, we implicitly
            // remove all descendants.
            var index = PhysicalTarget.GetChildPhysicalIndex(childToRemove);
            Renderer.ElementManager.RemoveChildElement(PhysicalTarget._targetElement, childToRemove._targetElement, index);
        }
        else
        {
            // This adapter is just a container for other adapters
            foreach (var child in childToRemove.Children)
            {
                childToRemove.RemoveChildElementAndDescendants(child);
            }
        }
    }

    private int ApplyPrependFrame(RenderBatch batch, int componentId, int siblingIndex, RenderTreeFrame[] frames, int frameIndex, HashSet<int> processedComponentIds)
    {
        ref var frame = ref frames[frameIndex];
        switch (frame.FrameType)
        {
            case RenderTreeFrameType.Component:
                {
                    var childAdapter = AddChildAdapter(siblingIndex, frame);

                    // For most elements we should add element as child after all properties to have them fully initialized before rendering.
                    // However, INonPhysicalChild elements are not real elements, but apply to parent instead, therefore should be added as child before any properties are set.
                    if (childAdapter._targetElement is INonPhysicalChild)
                    {
                        AddElementAsChildElement(childAdapter);
                    }

                    // Apply edits for child component recursively.
                    // That is done to fully initialize elements before adding to the UI tree.
                    processedComponentIds.Add(frame.ComponentId);

                    for (var i = 0; i < batch.UpdatedComponents.Count; i++)
                    {
                        var componentEdits = batch.UpdatedComponents.Array[i];
                        if (componentEdits.ComponentId == frame.ComponentId && componentEdits.Edits.Count > 0)
                        {
                            childAdapter.ApplyEdits(frame.ComponentId, componentEdits.Edits, batch.ReferenceFrames, batch, processedComponentIds);
                        }
                    }

                    if (childAdapter._targetElement is not INonPhysicalChild and not null)
                    {
                        AddElementAsChildElement(childAdapter);
                    }

                    return 1;
                }
            case RenderTreeFrameType.Region:
                {
                    return InsertFrameRange(batch, componentId, siblingIndex, frames, frameIndex + 1, frameIndex + frame.RegionSubtreeLength, processedComponentIds);
                }
            case RenderTreeFrameType.Markup:
                {
                    if (!string.IsNullOrWhiteSpace(frame.MarkupContent))
                    {
                        throw new NotImplementedException($"Not supported frame type: {frame.FrameType}");
                    }
                    AddChildAdapter(siblingIndex, frame);
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
                    AddChildAdapter(siblingIndex, frame);
                    return 1;
                }
            default:
                throw new NotImplementedException($"Not supported frame type: {frame.FrameType}");
        }
    }

    /// <summary>
    /// Add element as a child element for closest physical parent.
    /// </summary>
    private void AddElementAsChildElement(NativeComponentAdapter childAdapter)
    {
        var elementIndex = PhysicalTarget.GetChildPhysicalIndex(childAdapter);
        Renderer.ElementManager.AddChildElement(PhysicalTarget._targetElement, childAdapter._targetElement, elementIndex);
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
                if (child == targetChild)
                    return true;

                if (child._targetElement != null)
                {
                    if (child._targetElement is not INonPhysicalChild)
                        index++;
                }
                else
                {
                    if (FindChildPhysicalIndexRecursive(child, targetChild, ref index))
                        return true;
                }
            }

            return false;
        }
    }

    private int InsertFrameRange(RenderBatch batch, int componentId, int childIndex, RenderTreeFrame[] frames, int startIndex, int endIndexExcl, HashSet<int> processedComponentIds)
    {
        var origChildIndex = childIndex;
        for (var frameIndex = startIndex; frameIndex < endIndexExcl; frameIndex++)
        {
            ref var frame = ref batch.ReferenceFrames.Array[frameIndex];
            var numChildrenInserted = ApplyPrependFrame(batch, componentId, childIndex, frames, frameIndex, processedComponentIds);
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
            Name = name
        };

        if (frame.FrameType is RenderTreeFrameType.Component)
        {
            childAdapter._targetComponent = frame.Component;
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
}
