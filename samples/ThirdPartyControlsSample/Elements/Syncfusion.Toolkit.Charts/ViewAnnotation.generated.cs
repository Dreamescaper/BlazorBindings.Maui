// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using BlazorBindings.Maui.Elements;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using SMTC = Syncfusion.Maui.Toolkit.Charts;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.Charts
{
    /// <summary>
    /// This class is used to add an view annotation in <see cref="T:Syncfusion.Maui.Toolkit.Charts.SfCartesianChart" />. An instance of this class need to be added to <see cref="P:Syncfusion.Maui.Toolkit.Charts.SfCartesianChart.Annotations" /> collection.
    /// </summary>
    public partial class ViewAnnotation : ChartAnnotation
    {
        static ViewAnnotation()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the view.
        /// </summary>
        /// <value>
        /// This property takes the <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartAlignment" /> as its value and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.ChartAlignment.Center" />.
        /// </value>
        [Parameter] public SMTC.ChartAlignment? HorizontalAlignment { get; set; }
        /// <summary>
        /// Gets or sets the vertical alignment of the view.
        /// </summary>
        /// <value>
        /// This property takes the <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartAlignment" /> as its value and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.ChartAlignment.Center" />.
        /// </value>
        [Parameter] public SMTC.ChartAlignment? VerticalAlignment { get; set; }
        /// <summary>
        /// Gets or sets the View that represents custom view for annotation
        /// </summary>
        /// <value>
        /// This property takes the <see cref="T:Microsoft.Maui.Controls.View" /> as its value and its default value is null
        /// </value>
        [Parameter] public RenderFragment View { get; set; }

        public new SMTC.ViewAnnotation NativeControl => (SMTC.ViewAnnotation)((BindableObject)this).NativeControl;

        protected override SMTC.ViewAnnotation CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(HorizontalAlignment):
                    if (!Equals(HorizontalAlignment, value))
                    {
                        HorizontalAlignment = (SMTC.ChartAlignment?)value;
                        NativeControl.HorizontalAlignment = HorizontalAlignment ?? (SMTC.ChartAlignment)SMTC.ViewAnnotation.HorizontalAlignmentProperty.DefaultValue;
                    }
                    break;
                case nameof(VerticalAlignment):
                    if (!Equals(VerticalAlignment, value))
                    {
                        VerticalAlignment = (SMTC.ChartAlignment?)value;
                        NativeControl.VerticalAlignment = VerticalAlignment ?? (SMTC.ChartAlignment)SMTC.ViewAnnotation.VerticalAlignmentProperty.DefaultValue;
                    }
                    break;
                case nameof(View):
                    View = (RenderFragment)value;
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<SMTC.ViewAnnotation>(builder, sequence++, View, (x, value) => x.View = (MC.View)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
