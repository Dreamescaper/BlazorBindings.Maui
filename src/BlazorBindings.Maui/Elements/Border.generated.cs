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
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    public partial class Border : View
    {
        static Border()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public Thickness? Padding { get; set; }
        [Parameter] public Color StrokeColor { get; set; }
        [Parameter] public double? StrokeDashOffset { get; set; }
        [Parameter] public PenLineCap? StrokeLineCap { get; set; }
        [Parameter] public PenLineJoin? StrokeLineJoin { get; set; }
        [Parameter] public double? StrokeMiterLimit { get; set; }
        [Parameter] public double? StrokeThickness { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public RenderFragment Stroke { get; set; }
        [Parameter] public RenderFragment StrokeShape { get; set; }

        public new MC.Border NativeControl => (MC.Border)((BindableObject)this).NativeControl;

        protected override MC.Border CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Padding):
                    if (!Equals(Padding, value))
                    {
                        Padding = (Thickness?)value;
                        NativeControl.Padding = Padding ?? (Thickness)MC.Border.PaddingProperty.DefaultValue;
                    }
                    break;
                case nameof(StrokeColor):
                    if (!Equals(StrokeColor, value))
                    {
                        StrokeColor = (Color)value;
                        NativeControl.Stroke = StrokeColor;
                    }
                    break;
                case nameof(StrokeDashOffset):
                    if (!Equals(StrokeDashOffset, value))
                    {
                        StrokeDashOffset = (double?)value;
                        NativeControl.StrokeDashOffset = StrokeDashOffset ?? (double)MC.Border.StrokeDashOffsetProperty.DefaultValue;
                    }
                    break;
                case nameof(StrokeLineCap):
                    if (!Equals(StrokeLineCap, value))
                    {
                        StrokeLineCap = (PenLineCap?)value;
                        NativeControl.StrokeLineCap = StrokeLineCap ?? (PenLineCap)MC.Border.StrokeLineCapProperty.DefaultValue;
                    }
                    break;
                case nameof(StrokeLineJoin):
                    if (!Equals(StrokeLineJoin, value))
                    {
                        StrokeLineJoin = (PenLineJoin?)value;
                        NativeControl.StrokeLineJoin = StrokeLineJoin ?? (PenLineJoin)MC.Border.StrokeLineJoinProperty.DefaultValue;
                    }
                    break;
                case nameof(StrokeMiterLimit):
                    if (!Equals(StrokeMiterLimit, value))
                    {
                        StrokeMiterLimit = (double?)value;
                        NativeControl.StrokeMiterLimit = StrokeMiterLimit ?? (double)MC.Border.StrokeMiterLimitProperty.DefaultValue;
                    }
                    break;
                case nameof(StrokeThickness):
                    if (!Equals(StrokeThickness, value))
                    {
                        StrokeThickness = (double?)value;
                        NativeControl.StrokeThickness = StrokeThickness ?? (double)MC.Border.StrokeThicknessProperty.DefaultValue;
                    }
                    break;
                case nameof(ChildContent):
                    ChildContent = (RenderFragment)value;
                    break;
                case nameof(Stroke):
                    Stroke = (RenderFragment)value;
                    break;
                case nameof(StrokeShape):
                    StrokeShape = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<MC.Border>(builder, sequence++, ChildContent, (x, value) => x.Content = (MC.View)value);
            RenderTreeBuilderHelper.AddContentProperty<MC.Border>(builder, sequence++, Stroke, (x, value) => x.Stroke = (MC.Brush)value);
            RenderTreeBuilderHelper.AddContentProperty<MC.Border>(builder, sequence++, StrokeShape, (x, value) => x.StrokeShape = (IShape)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
