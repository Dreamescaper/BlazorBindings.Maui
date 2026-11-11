// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Internal.DataTemplates;

/// <summary>
/// Unlike <see cref="ControlTemplateItemsComponent{T}"/>, this DataTemplate component does not use a wrapping element. 
/// This makes it possible to use when returning a View from template is not an option.
/// It acts as a main-renderer holder while template roots are rendered by child renderers, because Blazor
/// defers renders requested during an active render batch.
/// </summary>
internal class SyncControlTemplateItemsComponent<T> : NativeControlComponentBase, IContainerElementHandler, INonPhysicalChild, IDisposable
    where T : MC.BindableObject
{
    [Parameter] public Action<T, MC.ControlTemplate> SetControlTemplateAction { get; set; }
    [Parameter] public Action<T, MC.DataTemplate> SetDataTemplateAction { get; set; }
    [Parameter] public RenderFragment Template { get; set; }
    [Inject] public ISyncTemplateRendererFactory TemplateRendererFactory { get; set; }

    private readonly List<ISyncTemplateRootHandle<RenderFragment>> _templateRoots = [];
    private bool _disposed;

    public override Task SetParametersAsync(ParameterView parameters)
    {
        var oldTemplate = Template;
        var task = base.SetParametersAsync(parameters);

        if (!ReferenceEquals(oldTemplate, Template))
        {
            foreach (var templateRoot in _templateRoots)
            {
                templateRoot.UpdateTemplate(Template);
            }
        }

        return task;
    }

    private Microsoft.Maui.IView AddTemplateRoot()
    {
        var templateRoot = TemplateRendererFactory.Render(Template);
        _templateRoots.Add(templateRoot);

        return (Microsoft.Maui.IView)templateRoot.RootElement;
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

    void INonPhysicalChild.RemoveFromParent(object parentElement) => Dispose();
    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex) { }
    void IContainerElementHandler.RemoveChild(int physicalSiblingIndex) { }
    object IElementHandler.TargetElement => null;

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        foreach (var templateRoot in _templateRoots)
        {
            templateRoot.Dispose();
        }
        _templateRoots.Clear();
    }
}
