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
    /// <summary>
    /// Recognizer for pinch gestures.
    /// </summary>
    public partial class PinchGestureRecognizer : GestureRecognizer
    {
        static PinchGestureRecognizer()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public EventCallback<MC.PinchGestureUpdatedEventArgs> OnPinchUpdated { get; set; }

        public new MC.PinchGestureRecognizer NativeControl => (MC.PinchGestureRecognizer)((BindableObject)this).NativeControl;

        protected override MC.PinchGestureRecognizer CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(OnPinchUpdated):
                    if (!Equals(OnPinchUpdated, value))
                    {
                        void NativeControlPinchUpdated(object sender, MC.PinchGestureUpdatedEventArgs e) => InvokeEventCallback(OnPinchUpdated, e);

                        OnPinchUpdated = (EventCallback<MC.PinchGestureUpdatedEventArgs>)value;
                        NativeControl.PinchUpdated -= NativeControlPinchUpdated;
                        NativeControl.PinchUpdated += NativeControlPinchUpdated;
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
