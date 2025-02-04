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
    /// Enables the selection of individual or multiple data points in a series.
    /// </summary>
    public partial class DataPointSelectionBehavior : ChartSelectionBehavior
    {
        static DataPointSelectionBehavior()
        {
            RegisterAdditionalHandlers();
        }

        public new SMTC.DataPointSelectionBehavior NativeControl => (SMTC.DataPointSelectionBehavior)((BindableObject)this).NativeControl;

        protected override SMTC.DataPointSelectionBehavior CreateNativeElement() => new();


        static partial void RegisterAdditionalHandlers();
    }
}
