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

namespace BlazorBindings.Maui.Elements
{
    public partial class TapGestureRecognizer : GestureRecognizer
    {
        static TapGestureRecognizer()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public MC.ButtonsMask? Buttons { get; set; }
        /// <summary>
        /// The number of taps required to trigger the callback.
        /// </summary>
        /// <value>
        /// The number of taps to recognize. The default value is 1.
        /// </value>
        [Parameter] public int? NumberOfTapsRequired { get; set; }
        [Parameter] public EventCallback<MC.TappedEventArgs> OnTapped { get; set; }

        public new MC.TapGestureRecognizer NativeControl => (MC.TapGestureRecognizer)((BindableObject)this).NativeControl;

        protected override MC.TapGestureRecognizer CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Buttons):
                    if (!Equals(Buttons, value))
                    {
                        Buttons = (MC.ButtonsMask?)value;
                        NativeControl.Buttons = Buttons ?? (MC.ButtonsMask)MC.TapGestureRecognizer.ButtonsProperty.DefaultValue;
                    }
                    break;
                case nameof(NumberOfTapsRequired):
                    if (!Equals(NumberOfTapsRequired, value))
                    {
                        NumberOfTapsRequired = (int?)value;
                        NativeControl.NumberOfTapsRequired = NumberOfTapsRequired ?? (int)MC.TapGestureRecognizer.NumberOfTapsRequiredProperty.DefaultValue;
                    }
                    break;
                case nameof(OnTapped):
                    if (!Equals(OnTapped, value))
                    {
                        void NativeControlTapped(object sender, MC.TappedEventArgs e) => InvokeEventCallback(OnTapped, e);

                        OnTapped = (EventCallback<MC.TappedEventArgs>)value;
                        NativeControl.Tapped -= NativeControlTapped;
                        NativeControl.Tapped += NativeControlTapped;
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
