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
    /// The <see cref="T:Syncfusion.Maui.Toolkit.Charts.StackingColumn100Series" /> is a chart type that shows a set of vertical bars stacked on top of each other to represent a percentage of the total for each category.
    /// </summary>
    public partial class StackingColumn100Series : StackingColumnSeries
    {
        static StackingColumn100Series()
        {
            RegisterAdditionalHandlers();
        }

        public new SMTC.StackingColumn100Series NativeControl => (SMTC.StackingColumn100Series)((BindableObject)this).NativeControl;

        protected override SMTC.StackingColumn100Series CreateNativeElement() => new();


        static partial void RegisterAdditionalHandlers();
    }
}
