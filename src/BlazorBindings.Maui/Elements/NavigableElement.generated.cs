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
    /// Represents an <see cref="T:Microsoft.Maui.Controls.Element" /> with base functionality for <see cref="T:Microsoft.Maui.Controls.Page" /> navigation. Does not necessarily render on screen.
    /// </summary>
    public abstract partial class NavigableElement : StyleableElement
    {
        static NavigableElement()
        {
            RegisterAdditionalHandlers();
        }

        public new MC.NavigableElement NativeControl => (MC.NavigableElement)((BindableObject)this).NativeControl;



        static partial void RegisterAdditionalHandlers();
    }
}
