// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class GestureRecognizer : INonPhysicalChild
{
    // This INonPhysicalChild staff was done to allow GestureElements to be the direct child of the element.
    // However, not all elements support that (if they have no ChildContent property). Therefore. GestureElements property is added.
    // This thing remains in order to preserve backward compatibility.
    // Probably should be removed at some point.

    void INonPhysicalChild.RemoveFromParent(object parentElement)
    {
        switch (parentElement)
        {
            case MC.View view:
                view.GestureRecognizers.Remove(NativeControl);
                break;
            case MC.GestureElement gestureElement:
                gestureElement.GestureRecognizers.Remove(NativeControl);
                break;
            default:
                throw new InvalidOperationException($"Gesture of type {NativeControl.GetType().Name} can't be removed from parent of type {parentElement.GetType().FullName}.");
        }
    }

    void INonPhysicalChild.SetParent(object parentElement)
    {
        switch (parentElement)
        {
            case MC.View view:
                view.GestureRecognizers.Add(NativeControl);
                break;
            case MC.GestureElement gestureElement:
                gestureElement.GestureRecognizers.Add(NativeControl);
                break;
            default:
                throw new InvalidOperationException($"Gesture of type {NativeControl.GetType().Name} can't be added to parent of type {parentElement.GetType().FullName}.");
        }
    }
}
