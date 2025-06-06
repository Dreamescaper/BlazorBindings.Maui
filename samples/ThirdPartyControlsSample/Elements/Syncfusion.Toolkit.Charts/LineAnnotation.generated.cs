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
using SMTC = Syncfusion.Maui.Toolkit.Charts;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.Charts
{
    /// <summary>
    /// This class is used to add a line annotation to the <see cref="T:Syncfusion.Maui.Toolkit.Charts.SfCartesianChart" />. An instance of this class needs to be added to the <see cref="P:Syncfusion.Maui.Toolkit.Charts.SfCartesianChart.Annotations" /> collection.
    /// </summary>
    public partial class LineAnnotation : ShapeAnnotation
    {
        static LineAnnotation()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Represents the type of cap for line annotation. Gets or sets the line cap value for the line annotation.
        /// </summary>
        /// <value>
        /// This property takes the <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartLineCap" /> as its value and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.ChartLineCap.None" />.
        /// </value>
        [Parameter] public SMTC.ChartLineCap? LineCap { get; set; }

        public new SMTC.LineAnnotation NativeControl => (SMTC.LineAnnotation)((BindableObject)this).NativeControl;

        protected override SMTC.LineAnnotation CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(LineCap):
                    if (!Equals(LineCap, value))
                    {
                        LineCap = (SMTC.ChartLineCap?)value;
                        NativeControl.LineCap = LineCap ?? (SMTC.ChartLineCap)SMTC.LineAnnotation.LineCapProperty.DefaultValue;
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
