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
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.AlohaKit
{
    public partial class Avatar : BlazorBindings.Maui.Elements.GraphicsView
    {
        static Avatar()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public AC.AvatarSize? AvatarSize { get; set; }
        [Parameter] public Color FillColor { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public AC.AvatarDrawable PersonaDrawable { get; set; }
        [Parameter] public Color TextColor { get; set; }
        [Parameter] public RenderFragment Fill { get; set; }

        public new AC.Avatar NativeControl => (AC.Avatar)((BindableObject)this).NativeControl;

        protected override AC.Avatar CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(AvatarSize):
                    if (!Equals(AvatarSize, value))
                    {
                        AvatarSize = (AC.AvatarSize?)value;
                        NativeControl.AvatarSize = AvatarSize ?? (AC.AvatarSize)AC.Avatar.AvatarSizeProperty.DefaultValue;
                    }
                    break;
                case nameof(FillColor):
                    if (!Equals(FillColor, value))
                    {
                        FillColor = (Color)value;
                        NativeControl.Fill = FillColor;
                    }
                    break;
                case nameof(Name):
                    if (!Equals(Name, value))
                    {
                        Name = (string)value;
                        NativeControl.Name = Name;
                    }
                    break;
                case nameof(PersonaDrawable):
                    if (!Equals(PersonaDrawable, value))
                    {
                        PersonaDrawable = (AC.AvatarDrawable)value;
                        NativeControl.PersonaDrawable = PersonaDrawable;
                    }
                    break;
                case nameof(TextColor):
                    if (!Equals(TextColor, value))
                    {
                        TextColor = (Color)value;
                        NativeControl.TextColor = TextColor;
                    }
                    break;
                case nameof(Background):
                    Background = (RenderFragment)value;
                    break;
                case nameof(Fill):
                    Fill = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<AC.Avatar>(builder, sequence++, Background, (x, value) => x.Background = (MC.Brush)value);
            RenderTreeBuilderHelper.AddContentProperty<AC.Avatar>(builder, sequence++, Fill, (x, value) => x.Fill = (MC.Brush)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
