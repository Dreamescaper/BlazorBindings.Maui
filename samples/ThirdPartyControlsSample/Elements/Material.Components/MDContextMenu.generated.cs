// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using BlazorBindings.Maui.Elements;
using MC = Microsoft.Maui.Controls;
using MCM = Material.Components.Maui;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Material.Components
{
    public partial class MDContextMenu : BlazorBindings.Maui.Elements.ContentView
    {
        static MDContextMenu()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public Color BackgroundColour { get; set; }
        [Parameter] public Color RippleColor { get; set; }
        [Parameter] public int? VisibleItemCount { get; set; }
        /// <remarks>
        /// Accepts one or more MenuItem elements.
        /// </remarks>
        [Parameter] public new RenderFragment ChildContent { get; set; }

        public new MCM.ContextMenu NativeControl => (MCM.ContextMenu)((BindableObject)this).NativeControl;

        protected override MCM.ContextMenu CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(BackgroundColour):
                    if (!Equals(BackgroundColour, value))
                    {
                        BackgroundColour = (Color)value;
                        NativeControl.BackgroundColour = BackgroundColour;
                    }
                    break;
                case nameof(RippleColor):
                    if (!Equals(RippleColor, value))
                    {
                        RippleColor = (Color)value;
                        NativeControl.RippleColor = RippleColor;
                    }
                    break;
                case nameof(VisibleItemCount):
                    if (!Equals(VisibleItemCount, value))
                    {
                        VisibleItemCount = (int?)value;
                        NativeControl.VisibleItemCount = VisibleItemCount ?? (int)MCM.ContextMenu.VisibleItemCountProperty.DefaultValue;
                    }
                    break;
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
            RenderTreeBuilderHelper.AddListContentProperty<MCM.ContextMenu, MCM.MenuItem>(builder, sequence++, ChildContent, x => x.Items);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
