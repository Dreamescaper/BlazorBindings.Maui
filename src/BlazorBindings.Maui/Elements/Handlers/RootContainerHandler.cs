// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Maui.Elements.Handlers;

/// <summary>
/// Fake element handler, which is used as a root for a renderer to get native Xamarin.Forms elements
/// from a Blazor component.
/// </summary>
/// <remarks>Experimental API, subject to change.</remarks>
[RequiresPreviewFeatures]
public class RootContainerHandler : IContainerElementHandler, INonPhysicalChild
{
    private TaskCompletionSource<object> _taskCompletionSource;

    public List<object> Elements { get; } = new();

    public Task WaitForElementAsync()
    {
        if (Elements.Count > 0)
        {
            return Task.CompletedTask;
        }

        _taskCompletionSource ??= new();
        return _taskCompletionSource.Task;
    }

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        var index = Math.Min(physicalSiblingIndex, Elements.Count);
        Elements.Insert(index, child);
        _taskCompletionSource?.TrySetResult(child);
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
