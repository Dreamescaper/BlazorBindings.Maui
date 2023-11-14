// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using BlazorBindings.Maui.Elements;
using MC = Microsoft.Maui.Controls;
using MCC = Microsoft.Maui.Controls.Compatibility;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui;
using System;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Compatibility
{
    public abstract partial class Layout : BlazorBindings.Maui.Elements.View
    {
        static Layout()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public bool? CascadeInputTransparent { get; set; }
        [Parameter] public bool? IsClippedToBounds { get; set; }
        [Parameter] public Thickness? Padding { get; set; }
        [Parameter] public EventCallback OnLayoutChanged { get; set; }

        public new MCC.Layout NativeControl => (MCC.Layout)((BindableObject)this).NativeControl;


        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(CascadeInputTransparent):
                    if (!Equals(CascadeInputTransparent, value))
                    {
                        CascadeInputTransparent = (bool?)value;
                        NativeControl.CascadeInputTransparent = CascadeInputTransparent ?? (bool)MCC.Layout.CascadeInputTransparentProperty.DefaultValue;
                    }
                    break;
                case nameof(IsClippedToBounds):
                    if (!Equals(IsClippedToBounds, value))
                    {
                        IsClippedToBounds = (bool?)value;
                        NativeControl.IsClippedToBounds = IsClippedToBounds ?? (bool)MCC.Layout.IsClippedToBoundsProperty.DefaultValue;
                    }
                    break;
                case nameof(Padding):
                    if (!Equals(Padding, value))
                    {
                        Padding = (Thickness?)value;
                        NativeControl.Padding = Padding ?? (Thickness)MCC.Layout.PaddingProperty.DefaultValue;
                    }
                    break;
                case nameof(OnLayoutChanged):
                    if (!Equals(OnLayoutChanged, value))
                    {
                        void NativeControlLayoutChanged(object sender, EventArgs e) => InvokeEventCallback(OnLayoutChanged);

                        OnLayoutChanged = (EventCallback)value;
                        NativeControl.LayoutChanged -= NativeControlLayoutChanged;
                        NativeControl.LayoutChanged += NativeControlLayoutChanged;
                    }
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        static partial void RegisterAdditionalHandlers();
    }
}
