// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Internal.DataTemplates;

/// <summary>
/// Unlike the regular typed data-template component, this DataTemplate component does not use a wrapping element. 
/// This makes it possible to use when returning a View from template is not an option.
/// It acts as a main-renderer holder while template roots are rendered by child renderers, because Blazor
/// defers renders requested during an active render batch.
/// </summary>
internal class SyncDataTemplateItemsComponent<TControl, TItem> : NativeControlComponentBase, IContainerElementHandler, INonPhysicalChild, IDisposable
{
    [Parameter] public Action<TControl, MC.DataTemplate> SetDataTemplateAction { get; set; }
    [Parameter] public RenderFragment<TItem> Template { get; set; }
    [Inject] public ISyncTemplateRendererFactory TemplateRendererFactory { get; set; }

    private readonly List<ISyncTemplateRootHandle<RenderFragment<TItem>>> _templateRoots = [];
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

    public MC.BindableObject AddTemplateRoot(TItem initialItem)
    {
        var templateRoot = TemplateRendererFactory.Render(Template, initialItem);
        _templateRoots.Add(templateRoot);

        return (MC.BindableObject)templateRoot.RootElement;
    }

    void INonPhysicalChild.SetParent(object parentElement)
    {
        var parent = (TControl)parentElement;
        var dataTemplate = new DataTemplateSelector<TItem>(AddTemplateRoot);
        SetDataTemplateAction(parent, dataTemplate);
    }

    void INonPhysicalChild.RemoveFromParent(object parentElement) => Dispose();
    object IElementHandler.TargetElement => null;
    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex) { }
    void IContainerElementHandler.RemoveChild(int physicalSiblingIndex) { }

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

/// <summary>
/// Captures the selected item before MAUI invokes the shared DataTemplate factory.
/// </summary>
file class DataTemplateSelector<TItem> : MC.DataTemplateSelector
{
    private readonly MC.DataTemplate _dataTemplate;
    private readonly Func<TItem, MC.BindableObject> _loadTemplate;
    private TItem _initialItem;

    public DataTemplateSelector(Func<TItem, MC.BindableObject> loadTemplate)
    {
        _loadTemplate = loadTemplate;
        _dataTemplate = new MC.DataTemplate(() => _loadTemplate(_initialItem));
    }

    protected override MC.DataTemplate OnSelectTemplate(object item, MC.BindableObject container)
    {
        _initialItem = (TItem)item;
        return _dataTemplate;
    }
}
