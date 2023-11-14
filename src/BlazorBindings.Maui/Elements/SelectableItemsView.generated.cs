// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    public partial class SelectableItemsView<T> : StructuredItemsView<T>
    {
        static SelectableItemsView()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public T SelectedItem { get; set; }
        [Parameter] public IList<object> SelectedItems { get; set; }
        [Parameter] public MC.SelectionMode? SelectionMode { get; set; }
        [Parameter] public EventCallback<T> SelectedItemChanged { get; set; }
        [Parameter] public EventCallback<IList<object>> SelectedItemsChanged { get; set; }
        [Parameter] public EventCallback<MC.SelectionChangedEventArgs> OnSelectionChanged { get; set; }

        public new MC.SelectableItemsView NativeControl => (MC.SelectableItemsView)((BindableObject)this).NativeControl;

        protected override MC.SelectableItemsView CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(SelectedItem):
                    if (!Equals(SelectedItem, value))
                    {
                        SelectedItem = (T)value;
                        NativeControl.SelectedItem = SelectedItem;
                    }
                    break;
                case nameof(SelectedItems):
                    if (!Equals(SelectedItems, value))
                    {
                        SelectedItems = (IList<object>)value;
                        NativeControl.SelectedItems = SelectedItems;
                    }
                    break;
                case nameof(SelectionMode):
                    if (!Equals(SelectionMode, value))
                    {
                        SelectionMode = (MC.SelectionMode?)value;
                        NativeControl.SelectionMode = SelectionMode ?? (MC.SelectionMode)MC.SelectableItemsView.SelectionModeProperty.DefaultValue;
                    }
                    break;
                case nameof(SelectedItemChanged):
                    if (!Equals(SelectedItemChanged, value))
                    {
                        void NativeControlPropertyChanged(object sender, PropertyChangedEventArgs e)
                        {
                            if (e.PropertyName == nameof(NativeControl.SelectedItem))
                            {
                                var value = NativeControl.SelectedItem is T item ? item : default(T);
                                SelectedItem = value;
                                InvokeEventCallback(SelectedItemChanged, value);
                            }
                        }

                        SelectedItemChanged = (EventCallback<T>)value;
                        NativeControl.PropertyChanged -= NativeControlPropertyChanged;
                        NativeControl.PropertyChanged += NativeControlPropertyChanged;
                    }
                    break;
                case nameof(SelectedItemsChanged):
                    if (!Equals(SelectedItemsChanged, value))
                    {
                        void NativeControlPropertyChanged(object sender, PropertyChangedEventArgs e)
                        {
                            if (e.PropertyName == nameof(NativeControl.SelectedItems))
                            {
                                var value = NativeControl.SelectedItems;
                                SelectedItems = value;
                                InvokeEventCallback(SelectedItemsChanged, value);
                            }
                        }

                        SelectedItemsChanged = (EventCallback<IList<object>>)value;
                        NativeControl.PropertyChanged -= NativeControlPropertyChanged;
                        NativeControl.PropertyChanged += NativeControlPropertyChanged;
                    }
                    break;
                case nameof(OnSelectionChanged):
                    if (!Equals(OnSelectionChanged, value))
                    {
                        void NativeControlSelectionChanged(object sender, MC.SelectionChangedEventArgs e) => InvokeEventCallback(OnSelectionChanged, e);

                        OnSelectionChanged = (EventCallback<MC.SelectionChangedEventArgs>)value;
                        NativeControl.SelectionChanged -= NativeControlSelectionChanged;
                        NativeControl.SelectionChanged += NativeControlSelectionChanged;
                    }
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        static partial void RegisterAdditionalHandlers();
    }
}
