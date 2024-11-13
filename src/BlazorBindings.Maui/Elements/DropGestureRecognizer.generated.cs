// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    public partial class DropGestureRecognizer : GestureRecognizer
    {
        static DropGestureRecognizer()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public bool? AllowDrop { get; set; }
        [Parameter] public EventCallback<MC.DragEventArgs> OnDragLeave { get; set; }
        [Parameter] public EventCallback<MC.DragEventArgs> OnDragOver { get; set; }
        [Parameter] public EventCallback<MC.DropEventArgs> OnDrop { get; set; }

        public new MC.DropGestureRecognizer NativeControl => (MC.DropGestureRecognizer)((BindableObject)this).NativeControl;

        protected override MC.DropGestureRecognizer CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(AllowDrop):
                    if (!Equals(AllowDrop, value))
                    {
                        AllowDrop = (bool?)value;
                        NativeControl.AllowDrop = AllowDrop ?? (bool)MC.DropGestureRecognizer.AllowDropProperty.DefaultValue;
                    }
                    break;
                case nameof(OnDragLeave):
                    if (!Equals(OnDragLeave, value))
                    {
                        void NativeControlDragLeave(object sender, MC.DragEventArgs e) => InvokeEventCallback(OnDragLeave, e);

                        OnDragLeave = (EventCallback<MC.DragEventArgs>)value;
                        NativeControl.DragLeave -= NativeControlDragLeave;
                        NativeControl.DragLeave += NativeControlDragLeave;
                    }
                    break;
                case nameof(OnDragOver):
                    if (!Equals(OnDragOver, value))
                    {
                        void NativeControlDragOver(object sender, MC.DragEventArgs e) => InvokeEventCallback(OnDragOver, e);

                        OnDragOver = (EventCallback<MC.DragEventArgs>)value;
                        NativeControl.DragOver -= NativeControlDragOver;
                        NativeControl.DragOver += NativeControlDragOver;
                    }
                    break;
                case nameof(OnDrop):
                    if (!Equals(OnDrop, value))
                    {
                        void NativeControlDrop(object sender, MC.DropEventArgs e) => InvokeEventCallback(OnDrop, e);

                        OnDrop = (EventCallback<MC.DropEventArgs>)value;
                        NativeControl.Drop -= NativeControlDrop;
                        NativeControl.Drop += NativeControlDrop;
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
