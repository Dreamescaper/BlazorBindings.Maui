// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    public partial class ReorderableItemsView<T> : GroupableItemsView<T>
    {
        static ReorderableItemsView()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public bool? CanReorderItems { get; set; }
        [Parameter] public EventCallback OnReorderCompleted { get; set; }

        public new MC.ReorderableItemsView NativeControl => (MC.ReorderableItemsView)((BindableObject)this).NativeControl;

        protected override MC.ReorderableItemsView CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(CanReorderItems):
                    if (!Equals(CanReorderItems, value))
                    {
                        CanReorderItems = (bool?)value;
                        NativeControl.CanReorderItems = CanReorderItems ?? (bool)MC.ReorderableItemsView.CanReorderItemsProperty.DefaultValue;
                    }
                    break;
                case nameof(OnReorderCompleted):
                    if (!Equals(OnReorderCompleted, value))
                    {
                        void NativeControlReorderCompleted(object sender, EventArgs e) => InvokeEventCallback(OnReorderCompleted);

                        OnReorderCompleted = (EventCallback)value;
                        NativeControl.ReorderCompleted -= NativeControlReorderCompleted;
                        NativeControl.ReorderCompleted += NativeControlReorderCompleted;
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
