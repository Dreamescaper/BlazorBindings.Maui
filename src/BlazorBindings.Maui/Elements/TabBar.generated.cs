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
    public partial class TabBar : ShellItem
    {
        static TabBar()
        {
            RegisterAdditionalHandlers();
        }

        public new MC.TabBar NativeControl => (MC.TabBar)((BindableObject)this).NativeControl;

        protected override MC.TabBar CreateNativeElement() => new();


        static partial void RegisterAdditionalHandlers();
    }
}
