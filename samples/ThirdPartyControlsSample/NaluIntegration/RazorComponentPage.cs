using BlazorBindings.Core;
using BlazorBindings.Maui;
using BlazorBindings.Maui.Elements.Handlers;
using Microsoft.AspNetCore.Components;
using Nalu;
using MC = Microsoft.Maui.Controls;

namespace ThirdPartyControlsSample.NaluIntegration;

public sealed class RazorComponentPage<TComponent> : MC.ContentPage, IEnteringAware, IDisposable
    where TComponent : IComponent
{
    private readonly MauiBlazorBindingsRenderer _renderer;
    private readonly Task _initializationTask;
    private IComponent _component;
    private bool _disposed;

    public RazorComponentPage(MauiBlazorBindingsRenderer renderer)
    {
        _renderer = renderer;
        _initializationTask = InitializeAsync();
    }

    public async ValueTask OnEnteringAsync() => await _initializationTask;

    private async Task InitializeAsync()
    {
#pragma warning disable MBB001
        var container = new RootContainerHandler();
#pragma warning restore MBB001
        var component = await _renderer.AddComponent<TComponent>(container);

        if (_disposed)
        {
            _renderer.RemoveRootComponent(component);
            return;
        }

        switch (container.Elements.SingleOrDefault())
        {
            case MC.ContentPage renderedPage:
                Title = renderedPage.Title;

                if (renderedPage.IsSet(Scaffold.TabBarVisibilityProperty))
                {
                    Scaffold.SetTabBarVisibility(this, Scaffold.GetTabBarVisibility(renderedPage));
                }

                var content = renderedPage.Content;
                renderedPage.Content = null;
                Content = content;

                foreach (var behavior in renderedPage.Behaviors.ToArray())
                {
                    renderedPage.Behaviors.Remove(behavior);
                    Behaviors.Add(behavior);
                }
                break;
            case MC.View view:
                Content = view;
                break;
            case { } root:
                throw new InvalidOperationException(
                    $"The root element of {typeof(TComponent).Name} must be a ContentPage or View, but was {root.GetType().Name}.");
            default:
                throw new InvalidOperationException($"{typeof(TComponent).Name} did not render a root element.");
        }

        _component = component;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        Content = null;

        if (_component is not null)
        {
            _renderer.RemoveRootComponent(_component);
            _component = null;
        }
    }
}
