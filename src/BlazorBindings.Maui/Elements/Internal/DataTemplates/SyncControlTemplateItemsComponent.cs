// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Internal.DataTemplates;

/// <summary>
/// Unlike <see cref="ControlTemplateItemsComponent{T}"/>, this DataTemplate component does not use a wrapping element. 
/// This makes it possible to use when returning a View from template is not an option.
/// However, it requires a DataTemplate to render synchronously, which does not always work with Blazor.
/// </summary>
internal class SyncControlTemplateItemsComponent<T> : NativeControlComponentBase, IContainerElementHandler, INonPhysicalChild
    where T : MC.BindableObject
{
    protected override RenderFragment GetChildContent()
    {
        return builder =>
        {
            for (var i = 0; i < _count; i++)
            {
                builder.OpenComponent<RootContainerComponent>(1);
                builder.AddAttribute(2, nameof(RootContainerComponent.ChildContent), Template);
                builder.AddComponentReferenceCapture(3, c => _lastRootContainer = (RootContainerComponent)c);

                builder.CloseComponent();
            }
        };
    }

    [Parameter] public Action<T, MC.ControlTemplate> SetControlTemplateAction { get; set; }
    [Parameter] public Action<T, MC.DataTemplate> SetDataTemplateAction { get; set; }
    [Parameter] public RenderFragment Template { get; set; }

    private RootContainerComponent _lastRootContainer;
    private int _count;

    [Inject] private MauiBlazorBindingsRenderer Renderer { get; set; }

    private Microsoft.Maui.IView AddTemplateRoot()
    {
        void RequestRoot()
        {
            _count++;
            StateHasChanged();
        }

        // StateHasChanged only queues the render when a batch is already in progress, which is the
        // case when MAUI realizes the template while we are attaching it. Requesting the render from
        // inside TryRenderSynchronously gets it applied before we need the element.
        if (!Renderer.TryRenderSynchronously(RequestRoot))
            RequestRoot();

        var rootElement = _lastRootContainer?.Elements?.FirstOrDefault()
            ?? throw new InvalidOperationException("Template root control is supposed to be rendered at this point.");
        _lastRootContainer = null;

        return (Microsoft.Maui.IView)rootElement;
    }

    void INonPhysicalChild.SetParent(object parentElement)
    {
        var parent = (T)parentElement;

        if (SetControlTemplateAction != null)
        {
            var controlTemplate = new MC.ControlTemplate(AddTemplateRoot);
            SetControlTemplateAction(parent, controlTemplate);
        }

        if (SetDataTemplateAction != null)
        {
            var dataTemplate = new MC.DataTemplate(AddTemplateRoot);
            SetDataTemplateAction(parent, dataTemplate);
        }
    }

    void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex) { }
    void IContainerElementHandler.RemoveChild(int physicalSiblingIndex) { }
    object IElementHandler.TargetElement => null;
}
