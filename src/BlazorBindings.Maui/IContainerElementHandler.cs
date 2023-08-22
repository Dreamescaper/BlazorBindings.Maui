// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Maui;

public interface IContainerElementHandler : IElementHandler
{
    void AddChild(object child, int physicalSiblingIndex);
    void RemoveChild(object child);
    int GetChildIndex(object child);
}
