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
    /// A <see cref="T:Microsoft.Maui.Controls.ShellItem" /> that has a collection of <see cref="T:Microsoft.Maui.Controls.Tab" /> objects.
    /// </summary>
    public partial class FlyoutItem : ShellItem
    {
        static FlyoutItem()
        {
            RegisterAdditionalHandlers();
        }

        public new MC.FlyoutItem NativeControl => (MC.FlyoutItem)((BindableObject)this).NativeControl;

        protected override MC.FlyoutItem CreateNativeElement() => new();


        static partial void RegisterAdditionalHandlers();
    }
}
