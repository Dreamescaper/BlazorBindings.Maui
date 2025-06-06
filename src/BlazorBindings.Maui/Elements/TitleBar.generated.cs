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
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    /// <summary>
    /// A <see cref="T:Microsoft.Maui.Controls.View" /> control that provides title bar functionality for a window. <br /><br /> The standard title bar height is 32px, but can be set to a larger value. <br /><br /> The title bar can also be hidden by setting the <see cref="P:Microsoft.Maui.Controls.VisualElement.IsVisible" /> property, which will cause the window content to be displayed in the title bar region.
    /// </summary>
    public partial class TitleBar : TemplatedView
    {
        static TitleBar()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the foreground color of the title bar. This color is used for the title and subtitle text.
        /// </summary>
        [Parameter] public Color ForegroundColor { get; set; }
        /// <summary>
        /// Gets or sets an optional icon image of the title bar. Icon images should be 16x16 pixels in size. <br /><br /> Setting this property to an empty value will hide the icon.
        /// </summary>
        [Parameter] public MC.ImageSource Icon { get; set; }
        /// <summary>
        /// Gets or sets the subtitle text of the title bar. The subtitle usually specifies the secondary information about the application or window
        /// </summary>
        [Parameter] public string Subtitle { get; set; }
        /// <summary>
        /// Gets or sets the title text of the title bar. The title usually specifies the name of the application or indicates the purpose of the window
        /// </summary>
        [Parameter] public string Title { get; set; }
        /// <summary>
        /// Gets or sets a <see cref="T:Microsoft.Maui.Controls.View" /> control that represents the content.<br /><br /> This content is centered in the title bar, and is allocated the remaining space between the leading and trailing content.<br /><br /><br /> Views set here will block all input to the title bar region and handle input directly.
        /// </summary>
        /// <remarks>
        /// Accepts single IView element.
        /// </remarks>
        [Parameter] public RenderFragment Content { get; set; }
        /// <summary>
        /// Gets or sets a <see cref="T:Microsoft.Maui.Controls.View" /> control that represents the leading content.<br /><br /> The leading content follows the optional <see cref="P:Microsoft.Maui.Controls.TitleBar.Icon" /> and is aligned to the left or right of the title bar, depending on the <see cref="T:Microsoft.Maui.FlowDirection" />. Views set here will be allocated as much space as they require. <br /><br /> Views set here will block all input to the title bar region and handle input directly.
        /// </summary>
        /// <remarks>
        /// Accepts single IView element.
        /// </remarks>
        [Parameter] public RenderFragment LeadingContent { get; set; }
        /// <remarks>
        /// Accepts one or more IView elements.
        /// </remarks>
        [Parameter] public RenderFragment PassthroughElements { get; set; }
        /// <summary>
        /// Gets or sets a <see cref="T:Microsoft.Maui.Controls.View" /> control that represents the trailing content.<br /><br /> The trailing content is aligned to the right or left of the title bar, depending on the <see cref="T:Microsoft.Maui.FlowDirection" />. Views set here will be allocated as much space as they require. <br /><br /> Views set here will block all input to the title bar region and handle input directly.
        /// </summary>
        /// <remarks>
        /// Accepts single IView element.
        /// </remarks>
        [Parameter] public RenderFragment TrailingContent { get; set; }

        public new MC.TitleBar NativeControl => (MC.TitleBar)((BindableObject)this).NativeControl;

        protected override MC.TitleBar CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ForegroundColor):
                    if (!Equals(ForegroundColor, value))
                    {
                        ForegroundColor = (Color)value;
                        NativeControl.ForegroundColor = ForegroundColor;
                    }
                    break;
                case nameof(Icon):
                    if (!Equals(Icon, value))
                    {
                        Icon = (MC.ImageSource)value;
                        NativeControl.Icon = Icon;
                    }
                    break;
                case nameof(Subtitle):
                    if (!Equals(Subtitle, value))
                    {
                        Subtitle = (string)value;
                        NativeControl.Subtitle = Subtitle;
                    }
                    break;
                case nameof(Title):
                    if (!Equals(Title, value))
                    {
                        Title = (string)value;
                        NativeControl.Title = Title;
                    }
                    break;
                case nameof(Content):
                    Content = (RenderFragment)value;
                    break;
                case nameof(LeadingContent):
                    LeadingContent = (RenderFragment)value;
                    break;
                case nameof(PassthroughElements):
                    PassthroughElements = (RenderFragment)value;
                    break;
                case nameof(TrailingContent):
                    TrailingContent = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<MC.TitleBar>(builder, sequence++, Content, (x, value) => x.Content = (IView)value);
            RenderTreeBuilderHelper.AddContentProperty<MC.TitleBar>(builder, sequence++, LeadingContent, (x, value) => x.LeadingContent = (IView)value);
            RenderTreeBuilderHelper.AddListContentProperty<MC.TitleBar, IView>(builder, sequence++, PassthroughElements, x => x.PassthroughElements);
            RenderTreeBuilderHelper.AddContentProperty<MC.TitleBar>(builder, sequence++, TrailingContent, (x, value) => x.TrailingContent = (IView)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
