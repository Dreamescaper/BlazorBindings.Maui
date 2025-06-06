// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using BlazorBindings.Maui.Elements;
using MC = Microsoft.Maui.Controls;
using MCS = Microsoft.Maui.Controls.Shapes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Shapes
{
    public partial class Path : Shape
    {
        static Path()
        {
            RegisterAdditionalHandlers();
        }

        /// <remarks>
        /// Accepts single Geometry element.
        /// </remarks>
        [Parameter] public RenderFragment Data { get; set; }
        /// <remarks>
        /// Accepts single Transform element.
        /// </remarks>
        [Parameter] public RenderFragment RenderTransform { get; set; }

        public new MCS.Path NativeControl => (MCS.Path)((BindableObject)this).NativeControl;

        protected override MCS.Path CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Data):
                    Data = (RenderFragment)value;
                    break;
                case nameof(RenderTransform):
                    RenderTransform = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<MCS.Path>(builder, sequence++, Data, (x, value) => x.Data = (MCS.Geometry)value);
            RenderTreeBuilderHelper.AddContentProperty<MCS.Path>(builder, sequence++, RenderTransform, (x, value) => x.RenderTransform = (MCS.Transform)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
