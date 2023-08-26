// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Runtime.ExceptionServices;

namespace BlazorBindings.Core;

public abstract class NativeControlComponentBase : IComponent
{
    private readonly RenderFragment _renderFragment;
    private bool _hasPendingQueuedRender;
    private RenderHandle _renderHandle;
    private Exception _eventCallbackException;

    public NativeControlComponentBase()
    {
        _renderFragment = builder =>
        {
            _hasPendingQueuedRender = false;
            BuildRenderTree(builder);
        };
    }

    public virtual Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        StateHasChanged();
        return Task.CompletedTask;
    }

    protected virtual void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (_eventCallbackException != null)
        {
            var oldException = _eventCallbackException;
            _eventCallbackException = null;
            ExceptionDispatchInfo.Throw(oldException);
        }

        var childContent = GetChildContent();
        if (childContent != null)
        {
            builder.AddContent(0, childContent);
        }

        int sequence = 1;
        RenderAdditionalElementContent(builder, ref sequence);
    }

    protected virtual void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
    {
    }

    protected void StateHasChanged()
    {
        if (_hasPendingQueuedRender)
            return;

        try
        {
            _renderHandle.Render(_renderFragment);
        }
        finally
        {
            _hasPendingQueuedRender = false;
        }
    }

    protected Task InvokeAsync(Action workItem) => _renderHandle.Dispatcher.InvokeAsync(workItem);

    protected Task InvokeAsync(Func<Task> workItem) => _renderHandle.Dispatcher.InvokeAsync(workItem);

    protected Task InvokeEventCallbackAsync<T>(EventCallback<T> eventCallback, T value)
    {
        return _renderHandle.Dispatcher.InvokeAsync(() => HandleExceptionAsync(eventCallback.InvokeAsync(value)));
    }

    protected Task InvokeEventCallbackAsync(EventCallback eventCallback)
    {
        return _renderHandle.Dispatcher.InvokeAsync(() => HandleExceptionAsync(eventCallback.InvokeAsync()));
    }

    protected void InvokeEventCallback<T>(EventCallback<T> eventCallback, T value)
    {
        try
        {
            AwaitVoid(InvokeEventCallbackAsync(eventCallback, value));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void InvokeEventCallback(EventCallback eventCallback)
    {
        try
        {
            AwaitVoid(InvokeEventCallbackAsync(eventCallback));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected virtual RenderFragment GetChildContent() => null;

    private async Task HandleExceptionAsync(Task task)
    {
        // Take a look here for the reasoning
        // https://github.com/dotnet/aspnetcore/issues/44920

        if (task.Exception != null)
        {
            // Developer experience for async exceptions is not great in Android. Therefore I try to 
            // throw an exception without awaiting if possible.
            // https://developercommunity.visualstudio.com/t/VS-doesnt-break-properly-on-async-excep/10263624
            HandleException(task.Exception.InnerException);
        }
        else
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    private void HandleException(Exception ex)
    {
        _eventCallbackException = ex;
        StateHasChanged();
    }

    private async void AwaitVoid(Task task)
    {
        // This async void method is needed to handle possible exceptions in the task.
        // EventHandlers are void methods. If we need to use async-await, we have to use async void method.
        // If we simply invoke task without awaiting, the exception will simply be ignored and missed.
        // async void, otoh, raises the exception to async context (which usually makes the process crash).

        // OTOH, exceptions from async void methods are bad during debug
        // https://developercommunity.visualstudio.com/t/VS-doesnt-break-properly-on-async-excep/10263624
        // Therefore we isolate async void here, and try throwing the exceptions without async void method
        // if they happened synchronously.

        try
        {
            await task;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    void IComponent.Attach(RenderHandle renderHandle)
    {
        _renderHandle = renderHandle;
    }
}
