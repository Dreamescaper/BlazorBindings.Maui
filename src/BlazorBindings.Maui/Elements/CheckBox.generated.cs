// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    public partial class CheckBox : View
    {
        static CheckBox()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public Color Color { get; set; }
        [Parameter] public bool? IsChecked { get; set; }
        [Parameter] public EventCallback<bool> IsCheckedChanged { get; set; }

        public new MC.CheckBox NativeControl => (MC.CheckBox)((BindableObject)this).NativeControl;

        protected override MC.CheckBox CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Color):
                    if (!Equals(Color, value))
                    {
                        Color = (Color)value;
                        NativeControl.Color = Color;
                    }
                    break;
                case nameof(IsChecked):
                    if (!Equals(IsChecked, value))
                    {
                        IsChecked = (bool?)value;
                        NativeControl.IsChecked = IsChecked ?? (bool)MC.CheckBox.IsCheckedProperty.DefaultValue;
                    }
                    break;
                case nameof(IsCheckedChanged):
                    if (!Equals(IsCheckedChanged, value))
                    {
                        void NativeControlCheckedChanged(object sender, MC.CheckedChangedEventArgs e)
                        {
                            var value = NativeControl.IsChecked;
                            IsChecked = value;
                            InvokeEventCallback(IsCheckedChanged, value);
                        }

                        IsCheckedChanged = (EventCallback<bool>)value;
                        NativeControl.CheckedChanged -= NativeControlCheckedChanged;
                        NativeControl.CheckedChanged += NativeControlCheckedChanged;
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
