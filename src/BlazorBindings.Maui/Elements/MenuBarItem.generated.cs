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
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    public partial class MenuBarItem : BaseMenuItem
    {
        static MenuBarItem()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public bool? IsEnabled { get; set; }
        [Parameter] public int? Priority { get; set; }
        [Parameter] public string Text { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        public new MC.MenuBarItem NativeControl => (MC.MenuBarItem)((BindableObject)this).NativeControl;

        protected override MC.MenuBarItem CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(IsEnabled):
                    if (!Equals(IsEnabled, value))
                    {
                        IsEnabled = (bool?)value;
                        NativeControl.IsEnabled = IsEnabled ?? (bool)MC.MenuBarItem.IsEnabledProperty.DefaultValue;
                    }
                    break;
                case nameof(Priority):
                    if (!Equals(Priority, value))
                    {
                        Priority = (int?)value;
                        NativeControl.Priority = Priority ?? default;
                    }
                    break;
                case nameof(Text):
                    if (!Equals(Text, value))
                    {
                        Text = (string)value;
                        NativeControl.Text = Text;
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
            RenderTreeBuilderHelper.AddListContentProperty<MC.MenuBarItem, IMenuElement>(builder, sequence++, ChildContent, x => x);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
