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
    /// Represents the chart axis's title class.
    /// </summary>
    public partial class ChartAxisTitle : ChartLabelStyle
    {
        static ChartAxisTitle()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a value that displays the content for the axis title.
        /// </summary>
        /// <value>
        /// It accepts string values.
        /// </value>
        [Parameter] public string Text { get; set; }

        public new SMTC.ChartAxisTitle NativeControl => (SMTC.ChartAxisTitle)((BindableObject)this).NativeControl;

        protected override SMTC.ChartAxisTitle CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Text):
                    if (!Equals(Text, value))
                    {
                        Text = (string)value;
                        NativeControl.Text = Text;
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
