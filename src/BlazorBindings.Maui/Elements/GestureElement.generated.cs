// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    /// <summary>
    /// An element that can respond to gestures.
    /// </summary>
    public partial class GestureElement : Element
    {
        static GestureElement()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets the list of recognizers that belong to the element.
        /// </summary>
        /// <value>
        /// The list of recognizers that belong to the element.
        /// </value>
        [Parameter] public RenderFragment GestureRecognizers { get; set; }

        public new MC.GestureElement NativeControl => (MC.GestureElement)((BindableObject)this).NativeControl;

        protected override MC.GestureElement CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(GestureRecognizers):
                    GestureRecognizers = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddListContentProperty<MC.GestureElement, MC.IGestureRecognizer>(builder, sequence++, GestureRecognizers, x => x.GestureRecognizers);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
