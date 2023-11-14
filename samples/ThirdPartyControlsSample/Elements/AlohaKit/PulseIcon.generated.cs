// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using AC = AlohaKit.Controls;
using BlazorBindings.Core;
using BlazorBindings.Maui.Elements;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Maui.Graphics;
using System;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.AlohaKit
{
    public partial class PulseIcon : BlazorBindings.Maui.Elements.GraphicsView
    {
        static PulseIcon()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public bool? IsPulsing { get; set; }
        [Parameter] public Color PulseColor { get; set; }
        [Parameter] public AC.PulseIconDrawable PulseIconDrawable { get; set; }
        [Parameter] public string Source { get; set; }
        [Parameter] public EventCallback OnClick { get; set; }

        public new AC.PulseIcon NativeControl => (AC.PulseIcon)((BindableObject)this).NativeControl;

        protected override AC.PulseIcon CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(IsPulsing):
                    if (!Equals(IsPulsing, value))
                    {
                        IsPulsing = (bool?)value;
                        NativeControl.IsPulsing = IsPulsing ?? (bool)AC.PulseIcon.IsPulsingProperty.DefaultValue;
                    }
                    break;
                case nameof(PulseColor):
                    if (!Equals(PulseColor, value))
                    {
                        PulseColor = (Color)value;
                        NativeControl.PulseColor = PulseColor;
                    }
                    break;
                case nameof(PulseIconDrawable):
                    if (!Equals(PulseIconDrawable, value))
                    {
                        PulseIconDrawable = (AC.PulseIconDrawable)value;
                        NativeControl.PulseIconDrawable = PulseIconDrawable;
                    }
                    break;
                case nameof(Source):
                    if (!Equals(Source, value))
                    {
                        Source = (string)value;
                        NativeControl.Source = Source;
                    }
                    break;
                case nameof(Background):
                    Background = (RenderFragment)value;
                    break;
                case nameof(OnClick):
                    if (!Equals(OnClick, value))
                    {
                        void NativeControlClicked(object sender, EventArgs e) => InvokeEventCallback(OnClick);

                        OnClick = (EventCallback)value;
                        NativeControl.Clicked -= NativeControlClicked;
                        NativeControl.Clicked += NativeControlClicked;
                    }
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<AC.PulseIcon>(builder, sequence++, Background, (x, value) => x.Background = (MC.Brush)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
