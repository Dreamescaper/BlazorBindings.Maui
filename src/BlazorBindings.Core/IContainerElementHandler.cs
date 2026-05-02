// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Core;

public interface IContainerElementHandler : IElementHandler
{
    void AddChild(object child, int physicalSiblingIndex);
    void RemoveChild(int physicalSiblingIndex);

    void RemoveRange(int index, int count)
    {
        for (var i = 0; i < count; i++)
            RemoveChild(index);
    }

    void AddRange(int index, IReadOnlyList<object> children)
    {
        for (var i = 0; i < children.Count; i++)
            AddChild(children[i], index + i);
    }

    void ReplaceRange(int index, int count, IReadOnlyList<object> newChildren)
    {
        var replacementsCount = Math.Min(count, newChildren.Count);

        for (var i = 0; i < replacementsCount; i++)
        {
            ReplaceChild(index + i, newChildren[i]);
        }

        if (count > replacementsCount)
            RemoveRange(index + replacementsCount, count - replacementsCount);
        else if (newChildren.Count > replacementsCount)
            AddRange(index + replacementsCount, newChildren.Skip(replacementsCount).ToArray());
    }

    void ReplaceChild(int physicalSiblingIndex, object newChild)
    {
        RemoveChild(physicalSiblingIndex);
        AddChild(newChild, physicalSiblingIndex);
    }
}
