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
    /// Serves as a base class for all types of annotations such as text, shape, and view. The annotations can be added to the chart.
    /// </summary>
    public abstract partial class ChartAnnotation : BlazorBindings.Maui.Elements.Element
    {
        static ChartAnnotation()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the property that identifies whether the annotation is positioned based on pixel or axis coordinates.
        /// </summary>
        /// <value>
        /// This property takes the <see cref="T:Syncfusion.Maui.Toolkit.Charts.ChartCoordinateUnit" /> as its value. Its default value is <see cref="F:Syncfusion.Maui.Toolkit.Charts.ChartCoordinateUnit.Axis" />.
        /// </value>
        [Parameter] public SMTC.ChartCoordinateUnit? CoordinateUnit { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates whether the annotation is visible or not.
        /// </summary>
        /// <value>
        /// This property takes the <c>bool</c> as its value. Its default value is true.
        /// </value>
        [Parameter] public bool? IsVisible { get; set; }
        /// <summary>
        /// Gets or sets the DateTime or double that represents the x1 position of the chart annotation.
        /// </summary>
        /// <value>
        /// This property takes the <c>object</c> as its value. Its default value is null.
        /// </value>
        [Parameter] public object X1 { get; set; }
        /// <summary>
        /// Get or set the value of a string that represents the name of the x-axis in the annotation.
        /// </summary>
        /// <value>
        /// This property takes the <c>string</c> as its value and its default value is string.Empty.
        /// </value>
        [Parameter] public string XAxisName { get; set; }
        /// <summary>
        /// Gets or sets the double that represents the y1 position of the chart annotation.
        /// </summary>
        /// <value>
        /// This property takes the <c>double</c> as its value and its default value is double.NaN.
        /// </value>
        [Parameter] public double? Y1 { get; set; }
        /// <summary>
        /// Get or set the value of a string that represents the name of the y-axis in the annotation.
        /// </summary>
        /// <value>
        /// This property takes the <c>string</c> as its value and its default value is string.Empty.
        /// </value>
        [Parameter] public string YAxisName { get; set; }

        public new SMTC.ChartAnnotation NativeControl => (SMTC.ChartAnnotation)((BindableObject)this).NativeControl;


        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(CoordinateUnit):
                    if (!Equals(CoordinateUnit, value))
                    {
                        CoordinateUnit = (SMTC.ChartCoordinateUnit?)value;
                        NativeControl.CoordinateUnit = CoordinateUnit ?? (SMTC.ChartCoordinateUnit)SMTC.ChartAnnotation.CoordinateUnitProperty.DefaultValue;
                    }
                    break;
                case nameof(IsVisible):
                    if (!Equals(IsVisible, value))
                    {
                        IsVisible = (bool?)value;
                        NativeControl.IsVisible = IsVisible ?? (bool)SMTC.ChartAnnotation.IsVisibleProperty.DefaultValue;
                    }
                    break;
                case nameof(X1):
                    if (!Equals(X1, value))
                    {
                        X1 = (object)value;
                        NativeControl.X1 = X1;
                    }
                    break;
                case nameof(XAxisName):
                    if (!Equals(XAxisName, value))
                    {
                        XAxisName = (string)value;
                        NativeControl.XAxisName = XAxisName;
                    }
                    break;
                case nameof(Y1):
                    if (!Equals(Y1, value))
                    {
                        Y1 = (double?)value;
                        NativeControl.Y1 = Y1 ?? (double)SMTC.ChartAnnotation.Y1Property.DefaultValue;
                    }
                    break;
                case nameof(YAxisName):
                    if (!Equals(YAxisName, value))
                    {
                        YAxisName = (string)value;
                        NativeControl.YAxisName = YAxisName;
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
