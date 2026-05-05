using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Immutable;
using System.Collections.Specialized;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public abstract partial class ItemsView<T>
{
    [Parameter] public IEnumerable<T> ItemsSource { get; set; }
    [Parameter] public Func<T, object> ItemKeySelector { get; set; }
    [Parameter] public RenderFragment<T> ItemTemplateSelector { get; set; }

    // Whether we should attempt to create ObservableCollection on our own (via diffing), or assign it directly.
    private bool AssignItemsSourceDirectly => ItemsSource is INotifyCollectionChanged || ItemsSource is IImmutableList<T>;

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        switch (name)
        {
            case nameof(ItemsSource):
                if (!Equals(ItemsSource, value))
                {
                    ItemsSource = CastParameter<IEnumerable<T>>(value, name);

                    if (AssignItemsSourceDirectly)
                        NativeControl.ItemsSource = ItemsSource;
                }
                return true;

            case nameof(ItemKeySelector):
                ItemKeySelector = CastParameter<Func<T, object>>(value, name);
                return true;

            case nameof(ItemTemplateSelector):
                ItemTemplateSelector = CastParameter<RenderFragment<T>>(value, name);
                return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }

    protected override void RenderAdditionalPartialElementContent(RenderTreeBuilder builder, ref int sequence)
    {
        base.RenderAdditionalPartialElementContent(builder, ref sequence);

        RenderTreeBuilderHelper.AddDataTemplateSelectorProperty<MC.ItemsView, T>(builder, sequence++, ItemTemplateSelector, (x, template) => x.ItemTemplate = template);

        sequence++;
        if (!AssignItemsSourceDirectly)
            RenderTreeBuilderHelper.AddItemsSourceProperty<MC.ItemsView, T>(builder, sequence, ItemsSource, ItemKeySelector, (x, items) => x.ItemsSource = items);
    }
}