// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    /// <summary>
    /// A layout that arranges views in rows and columns.
    /// </summary>
    public partial class Grid : Layout
    {
        static Grid()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the amount of space between columns in the Grid.
        /// </summary>
        /// <value>
        /// The space between columns in this <see cref="T:Microsoft.Maui.Controls.Grid" /> layout. The default is 0.
        /// </value>
        [Parameter] public double? ColumnSpacing { get; set; }
        /// <summary>
        /// Gets or sets the amount of space between rows in the Grid.
        /// </summary>
        /// <value>
        /// The space between rows in this <see cref="T:Microsoft.Maui.Controls.Grid" /> layout. The default is 0.
        /// </value>
        [Parameter] public double? RowSpacing { get; set; }

        public new MC.Grid NativeControl => (MC.Grid)((BindableObject)this).NativeControl;

        protected override MC.Grid CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ColumnSpacing):
                    if (!Equals(ColumnSpacing, value))
                    {
                        ColumnSpacing = (double?)value;
                        NativeControl.ColumnSpacing = ColumnSpacing ?? (double)MC.Grid.ColumnSpacingProperty.DefaultValue;
                    }
                    break;
                case nameof(RowSpacing):
                    if (!Equals(RowSpacing, value))
                    {
                        RowSpacing = (double?)value;
                        NativeControl.RowSpacing = RowSpacing ?? (double)MC.Grid.RowSpacingProperty.DefaultValue;
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
