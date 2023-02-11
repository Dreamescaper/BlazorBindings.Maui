// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui;
using System.Threading.Tasks;

namespace BlazorBindings.Maui.Elements
{
    /// <summary>
    /// <see cref="T:Microsoft.Maui.Controls.View" /> that holds an image.
    /// </summary>
    public partial class Image : View
    {
        static Image()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the scaling mode for the image.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.Maui.Aspect" /> representing the scaling mode of the image. Default is <see cref="F:Microsoft.Maui.Aspect.AspectFit" />.
        /// </value>
        [Parameter] public Aspect? Aspect { get; set; }
        [Parameter] public bool? IsAnimationPlaying { get; set; }
        /// <summary>
        /// Gets or sets a Boolean value that, if <see langword="true" /> hints to the rendering engine that it may safely omit drawing visual elements behind the image.
        /// </summary>
        /// <value>
        /// The value of the opacity rendering hint.
        /// </value>
        [Parameter] public bool? IsOpaque { get; set; }
        /// <summary>
        /// Gets or sets the source of the image.
        /// </summary>
        /// <value>
        /// An <see cref="T:Microsoft.Maui.Controls.ImageSource" /> representing the image source. Default is null.
        /// </value>
        [Parameter] public MC.ImageSource Source { get; set; }

        public new MC.Image NativeControl => (MC.Image)((BindableObject)this).NativeControl;

        protected override MC.Image CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Aspect):
                    if (!Equals(Aspect, value))
                    {
                        Aspect = (Aspect?)value;
                        NativeControl.Aspect = Aspect ?? (Aspect)MC.Image.AspectProperty.DefaultValue;
                    }
                    break;
                case nameof(IsAnimationPlaying):
                    if (!Equals(IsAnimationPlaying, value))
                    {
                        IsAnimationPlaying = (bool?)value;
                        NativeControl.IsAnimationPlaying = IsAnimationPlaying ?? (bool)MC.Image.IsAnimationPlayingProperty.DefaultValue;
                    }
                    break;
                case nameof(IsOpaque):
                    if (!Equals(IsOpaque, value))
                    {
                        IsOpaque = (bool?)value;
                        NativeControl.IsOpaque = IsOpaque ?? (bool)MC.Image.IsOpaqueProperty.DefaultValue;
                    }
                    break;
                case nameof(Source):
                    if (!Equals(Source, value))
                    {
                        Source = (MC.ImageSource)value;
                        NativeControl.Source = Source;
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
