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
    public partial class GestureRecognizer : Element
    {
        static GestureRecognizer()
        {
            RegisterAdditionalHandlers();
        }

        public new MC.GestureRecognizer NativeControl => (MC.GestureRecognizer)((Element)this).NativeControl;

        protected override MC.Element CreateNativeElement() => new MC.GestureRecognizer();


        static partial void RegisterAdditionalHandlers();
    }
}
