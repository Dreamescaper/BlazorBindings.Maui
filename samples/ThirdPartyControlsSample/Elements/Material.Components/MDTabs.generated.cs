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
    public partial class MDTabs : BlazorBindings.Maui.Elements.TemplatedView
    {
        static MDTabs()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public Color ActiveIndicatorColor { get; set; }
        [Parameter] public MCM.Tokens.Shape? ActiveIndicatorShape { get; set; }
        [Parameter] public bool? HasIcon { get; set; }
        [Parameter] public bool? HasLabel { get; set; }
        [Parameter] public int? SelectedIndex { get; set; }
        /// <remarks>
        /// Accepts one or more TabItem elements.
        /// </remarks>
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public EventCallback<MC.SelectedItemChangedEventArgs> OnSelectedItemChanged { get; set; }

        public new MCM.Tabs NativeControl => (MCM.Tabs)((BindableObject)this).NativeControl;

        protected override MCM.Tabs CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ActiveIndicatorColor):
                    if (!Equals(ActiveIndicatorColor, value))
                    {
                        ActiveIndicatorColor = (Color)value;
                        NativeControl.ActiveIndicatorColor = ActiveIndicatorColor;
                    }
                    break;
                case nameof(ActiveIndicatorShape):
                    if (!Equals(ActiveIndicatorShape, value))
                    {
                        ActiveIndicatorShape = (MCM.Tokens.Shape?)value;
                        NativeControl.ActiveIndicatorShape = ActiveIndicatorShape ?? (MCM.Tokens.Shape)MCM.Tabs.ActiveIndicatorShapeProperty.DefaultValue;
                    }
                    break;
                case nameof(HasIcon):
                    if (!Equals(HasIcon, value))
                    {
                        HasIcon = (bool?)value;
                        NativeControl.HasIcon = HasIcon ?? (bool)MCM.Tabs.HasIconProperty.DefaultValue;
                    }
                    break;
                case nameof(HasLabel):
                    if (!Equals(HasLabel, value))
                    {
                        HasLabel = (bool?)value;
                        NativeControl.HasLabel = HasLabel ?? (bool)MCM.Tabs.HasLabelProperty.DefaultValue;
                    }
                    break;
                case nameof(SelectedIndex):
                    if (!Equals(SelectedIndex, value))
                    {
                        SelectedIndex = (int?)value;
                        NativeControl.SelectedIndex = SelectedIndex ?? (int)MCM.Tabs.SelectedIndexProperty.DefaultValue;
                    }
                    break;
                case nameof(ChildContent):
                    ChildContent = (RenderFragment)value;
                    break;
                case nameof(OnSelectedItemChanged):
                    if (!Equals(OnSelectedItemChanged, value))
                    {
                        void NativeControlSelectedItemChanged(object sender, MC.SelectedItemChangedEventArgs e) => InvokeEventCallback(OnSelectedItemChanged, e);

                        OnSelectedItemChanged = (EventCallback<MC.SelectedItemChangedEventArgs>)value;
                        NativeControl.SelectedItemChanged -= NativeControlSelectedItemChanged;
                        NativeControl.SelectedItemChanged += NativeControlSelectedItemChanged;
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
            RenderTreeBuilderHelper.AddListContentProperty<MCM.Tabs, MCM.TabItem>(builder, sequence++, ChildContent, x => x.Items);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
