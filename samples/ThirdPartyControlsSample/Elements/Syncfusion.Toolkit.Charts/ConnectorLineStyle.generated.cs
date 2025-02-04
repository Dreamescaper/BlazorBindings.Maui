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
    /// The ConnectorLineStyle class is used to customize the appearance of the data label connector line when data labels are placed outside.
    /// </summary>
    public partial class ConnectorLineStyle : ChartLineStyle
    {
        static ConnectorLineStyle()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets a value that specifies the connector type.
        /// </summary>
        /// <value>
        /// It accepts <see cref="T:Syncfusion.Maui.Toolkit.Charts.ConnectorType" /> values and its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.ConnectorType.Line" />.
        /// </value>
        [Parameter] public SMTC.ConnectorType? ConnectorType { get; set; }

        public new SMTC.ConnectorLineStyle NativeControl => (SMTC.ConnectorLineStyle)((BindableObject)this).NativeControl;

        protected override SMTC.ConnectorLineStyle CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ConnectorType):
                    if (!Equals(ConnectorType, value))
                    {
                        ConnectorType = (SMTC.ConnectorType?)value;
                        NativeControl.ConnectorType = ConnectorType ?? (SMTC.ConnectorType)SMTC.ConnectorLineStyle.ConnectorTypeProperty.DefaultValue;
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
