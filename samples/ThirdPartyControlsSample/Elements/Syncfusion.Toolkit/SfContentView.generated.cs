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
using Microsoft.AspNetCore.Components.Rendering;
using SMT = Syncfusion.Maui.Toolkit;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit
{
    /// <summary>
    /// Represents an abstract base class for views that contain a single piece of content.
    /// </summary>
    public abstract partial class SfContentView : SfView
    {
        static SfContentView()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the content of the view.
        /// </summary>
        /// <remarks>
        /// Accepts single View element.
        /// </remarks>
        [Parameter] public new RenderFragment ChildContent { get; set; }

        public new SMT.SfContentView NativeControl => (SMT.SfContentView)((BindableObject)this).NativeControl;


        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ChildContent):
                    ChildContent = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<SMT.SfContentView>(builder, sequence++, ChildContent, (x, value) => x.Content = (MC.View)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
