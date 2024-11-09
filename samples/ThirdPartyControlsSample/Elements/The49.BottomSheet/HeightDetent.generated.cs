// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using BlazorBindings.Maui.Elements;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TMB = The49.Maui.BottomSheet;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.The49.BottomSheet
{
    public partial class HeightDetent : Detent
    {
        static HeightDetent()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public double? Height { get; set; }

        public new TMB.HeightDetent NativeControl => (TMB.HeightDetent)((BindableObject)this).NativeControl;

        protected override TMB.HeightDetent CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Height):
                    if (!Equals(Height, value))
                    {
                        Height = (double?)value;
                        NativeControl.Height = Height ?? (double)TMB.HeightDetent.HeightProperty.DefaultValue;
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
