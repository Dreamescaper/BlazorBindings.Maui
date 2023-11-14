// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    /// <summary>
    /// A <see cref="T:Microsoft.Maui.Controls.View" /> control that displays progress.
    /// </summary>
    public partial class ProgressBar : View
    {
        static ProgressBar()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the progress value.
        /// </summary>
        /// <value>
        /// Gets or sets a value that specifies the fraction of the bar that is colored.
        /// </value>
        [Parameter] public double? Progress { get; set; }
        /// <summary>
        /// Get or sets the color of the progress bar.
        /// </summary>
        /// <value>
        /// The color of the progress bar.
        /// </value>
        [Parameter] public Color ProgressColor { get; set; }

        public new MC.ProgressBar NativeControl => (MC.ProgressBar)((BindableObject)this).NativeControl;

        protected override MC.ProgressBar CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Progress):
                    if (!Equals(Progress, value))
                    {
                        Progress = (double?)value;
                        NativeControl.Progress = Progress ?? (double)MC.ProgressBar.ProgressProperty.DefaultValue;
                    }
                    break;
                case nameof(ProgressColor):
                    if (!Equals(ProgressColor, value))
                    {
                        ProgressColor = (Color)value;
                        NativeControl.ProgressColor = ProgressColor;
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
