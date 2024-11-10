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
using System.Collections.Specialized;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    /// <summary>
    /// A <see cref="T:Microsoft.Maui.Controls.TableSection" /> that contains either a table section or the entire table.
    /// </summary>
    public partial class TableRoot : TableSectionBase
    {
        static TableRoot()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public EventCallback<NotifyCollectionChangedEventArgs> OnCollectionChanged { get; set; }

        public new MC.TableRoot NativeControl => (MC.TableRoot)((BindableObject)this).NativeControl;

        protected override MC.TableRoot CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ChildContent):
                    ChildContent = (RenderFragment)value;
                    break;
                case nameof(OnCollectionChanged):
                    if (!Equals(OnCollectionChanged, value))
                    {
                        void NativeControlCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => InvokeEventCallback(OnCollectionChanged, e);

                        OnCollectionChanged = (EventCallback<NotifyCollectionChangedEventArgs>)value;
                        NativeControl.CollectionChanged -= NativeControlCollectionChanged;
                        NativeControl.CollectionChanged += NativeControlCollectionChanged;
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
            RenderTreeBuilderHelper.AddListContentProperty<MC.TableRoot, MC.TableSection>(builder, sequence++, ChildContent, x => x);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
