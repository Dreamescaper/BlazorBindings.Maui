// <auto-generated>
//     This code was generated by a BlazorBindings.Maui component generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui;
using System;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    /// <summary>
    /// Entry is a single line text entry. It is best used for collecting small discrete pieces of information, like usernames and passwords.
    /// </summary>
    public partial class Entry<T> : InputView
    {
        static Entry()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Determines the behavior of the clear text button on this entry.
        /// </summary>
        [Parameter] public ClearButtonVisibility? ClearButtonVisibility { get; set; }
        /// <summary>
        /// Gets or sets the horizontal text alignment.
        /// </summary>
        [Parameter] public TextAlignment? HorizontalTextAlignment { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates if the entry should visually obscure typed text. Value is <see langword="true" /> if the element is a password box; otherwise, <see langword="false" />. Default value is <see langword="false" />.
        /// </summary>
        [Parameter] public bool? IsPassword { get; set; }
        /// <summary>
        /// Determines what the return key on the on-screen keyboard should look like.
        /// </summary>
        [Parameter] public ReturnType? ReturnType { get; set; }
        /// <summary>
        /// Gets or sets the vertical text alignment.
        /// </summary>
        [Parameter] public TextAlignment? VerticalTextAlignment { get; set; }
        [Parameter] public EventCallback OnCompleted { get; set; }

        public new MC.Entry NativeControl => (MC.Entry)((BindableObject)this).NativeControl;

        protected override MC.Entry CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ClearButtonVisibility):
                    if (!Equals(ClearButtonVisibility, value))
                    {
                        ClearButtonVisibility = (ClearButtonVisibility?)value;
                        NativeControl.ClearButtonVisibility = ClearButtonVisibility ?? (ClearButtonVisibility)MC.Entry.ClearButtonVisibilityProperty.DefaultValue;
                    }
                    break;
                case nameof(HorizontalTextAlignment):
                    if (!Equals(HorizontalTextAlignment, value))
                    {
                        HorizontalTextAlignment = (TextAlignment?)value;
                        NativeControl.HorizontalTextAlignment = HorizontalTextAlignment ?? (TextAlignment)MC.Entry.HorizontalTextAlignmentProperty.DefaultValue;
                    }
                    break;
                case nameof(IsPassword):
                    if (!Equals(IsPassword, value))
                    {
                        IsPassword = (bool?)value;
                        NativeControl.IsPassword = IsPassword ?? (bool)MC.Entry.IsPasswordProperty.DefaultValue;
                    }
                    break;
                case nameof(ReturnType):
                    if (!Equals(ReturnType, value))
                    {
                        ReturnType = (ReturnType?)value;
                        NativeControl.ReturnType = ReturnType ?? (ReturnType)MC.Entry.ReturnTypeProperty.DefaultValue;
                    }
                    break;
                case nameof(VerticalTextAlignment):
                    if (!Equals(VerticalTextAlignment, value))
                    {
                        VerticalTextAlignment = (TextAlignment?)value;
                        NativeControl.VerticalTextAlignment = VerticalTextAlignment ?? (TextAlignment)MC.Entry.VerticalTextAlignmentProperty.DefaultValue;
                    }
                    break;
                case nameof(OnCompleted):
                    if (!Equals(OnCompleted, value))
                    {
                        void NativeControlCompleted(object sender, EventArgs e) => InvokeEventCallback(OnCompleted);

                        OnCompleted = (EventCallback)value;
                        NativeControl.Completed -= NativeControlCompleted;
                        NativeControl.Completed += NativeControlCompleted;
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
