// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using MC = Microsoft.Maui.Controls;
using WVM = Microsoft.AspNetCore.Components.WebView.Maui;

namespace BlazorBindings.Maui.Elements;

public class BlazorWebView : View
{
    [Parameter] public string HostPage { get; set; }
    [Parameter] public RenderFragment RootComponents { get; set; }

    public new WVM.BlazorWebView NativeControl => (WVM.BlazorWebView)((Element)this).NativeControl;

    protected override MC.BindableObject CreateNativeElement() => new WVM.BlazorWebView();

    protected override RenderFragment GetChildContent() => RootComponents;

    protected override void HandleParameter(string name, object value)
    {
        switch (name)
        {
            case nameof(HostPage):
                if (!Equals(HostPage, value))
                {
                    HostPage = (string)value;
                    NativeControl.HostPage = HostPage;
                }
                break;

            case nameof(RootComponents):
                RootComponents = (RenderFragment)value;
                break;

            default:
                base.HandleParameter(name, value);
                break;
        }
    }
}