// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components.Rendering;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public abstract class BindableObject : NativeControlComponentBase, IElementHandler
{
    private MC.BindableObject _nativeControl;

    public MC.BindableObject NativeControl => _nativeControl ??= CreateNativeElement();

    public override Task SetParametersAsync(ParameterView parameters)
    {
        foreach (var parameterValue in parameters)
        {
            HandleParameter(parameterValue.Name, parameterValue.Value);
        }
        StateHasChanged();
        return Task.CompletedTask;
    }

    protected virtual void HandleParameter(string name, object value)
    {
        if (HandleAdditionalParameter(name, value))
            return;

        if (AttachedPropertyRegistry.AttachedPropertyHandlers.TryGetValue(name, out var handler))
        {
            handler(NativeControl, value);
            return;
        }

        throw new NotImplementedException($"{GetType().FullName} doesn't recognize parameter '{name}'");
    }

    protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
    {
        RenderAdditionalPartialElementContent(builder, ref sequence);
    }

    [RequiresPreviewFeatures]
    protected virtual void RenderAdditionalPartialElementContent(RenderTreeBuilder builder, ref int sequence) { }

    protected virtual bool HandleAdditionalParameter(string name, object value) => false;

    protected abstract MC.BindableObject CreateNativeElement();

    object IElementHandler.TargetElement => NativeControl;
}
