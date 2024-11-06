// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Internal.DataTemplates;

internal class InitializedContentView : ContentView
{
    [Parameter] public new MC.ContentView NativeControl { get; set; }

    protected override MC.ContentView CreateNativeElement() => NativeControl;

    protected override void HandleParameter(string name, object value)
    {
        if (name == nameof(NativeControl))
        {
            NativeControl = (MC.ContentView)value;
        }
        else
        {
            base.HandleParameter(name, value);
        }
    }
}
