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
    /// A <see cref="T:Microsoft.Maui.Controls.SelectableItemsView" /> that presents a collection of items.
    /// </summary>
    public partial class CollectionView<T> : ReorderableItemsView<T>
    {
        static CollectionView()
        {
            RegisterAdditionalHandlers();
        }

        public new MC.CollectionView NativeControl => (MC.CollectionView)((BindableObject)this).NativeControl;

        protected override MC.CollectionView CreateNativeElement() => new();


        static partial void RegisterAdditionalHandlers();
    }
}
