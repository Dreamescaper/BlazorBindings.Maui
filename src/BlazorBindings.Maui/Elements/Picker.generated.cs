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
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements
{
    /// <summary>
    /// A <see cref="T:Microsoft.Maui.Controls.View" /> control for picking an element in a list.
    /// </summary>
    public partial class Picker<T> : View
    {
        static Picker()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public double? CharacterSpacing { get; set; }
        /// <summary>
        /// Gets a value that indicates whether the font for the searchbar text is bold, italic, or neither.
        /// </summary>
        [Parameter] public MC.FontAttributes? FontAttributes { get; set; }
        [Parameter] public bool? FontAutoScalingEnabled { get; set; }
        /// <summary>
        /// Gets or sets the font family for the picker text.
        /// </summary>
        [Parameter] public string FontFamily { get; set; }
        /// <summary>
        /// Gets or sets the size of the font for the text in the picker.
        /// </summary>
        /// <value>
        /// A <see langword="double" /> that indicates the size of the font.
        /// </value>
        [Parameter] public double? FontSize { get; set; }
        [Parameter] public TextAlignment? HorizontalTextAlignment { get; set; }
        /// <summary>
        /// Gets or sets a binding that selects the property that will be displayed for each object in the list of items.
        /// </summary>
        [Parameter] public Func<T, string> ItemDisplayBinding { get; set; }
        /// <summary>
        /// Gets or sets the source list of items to template and display.
        /// </summary>
        [Parameter] public IList<T> ItemsSource { get; set; }
        /// <summary>
        /// Gets or sets the index of the selected item of the picker.
        /// </summary>
        /// <value>
        /// An 0-based index representing the selected item in the list. Default is -1.
        /// </value>
        [Parameter] public int? SelectedIndex { get; set; }
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        [Parameter] public T SelectedItem { get; set; }
        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        [Parameter] public Color TextColor { get; set; }
        /// <summary>
        /// Gets or sets the title for the Picker.
        /// </summary>
        /// <value>
        /// A string.
        /// </value>
        [Parameter] public string Title { get; set; }
        [Parameter] public Color TitleColor { get; set; }
        [Parameter] public TextAlignment? VerticalTextAlignment { get; set; }
        [Parameter] public EventCallback<T> SelectedItemChanged { get; set; }
        [Parameter] public EventCallback<int> SelectedIndexChanged { get; set; }

        public new MC.Picker NativeControl => (MC.Picker)((BindableObject)this).NativeControl;

        protected override MC.Picker CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(CharacterSpacing):
                    if (!Equals(CharacterSpacing, value))
                    {
                        CharacterSpacing = (double?)value;
                        NativeControl.CharacterSpacing = CharacterSpacing ?? (double)MC.Picker.CharacterSpacingProperty.DefaultValue;
                    }
                    break;
                case nameof(FontAttributes):
                    if (!Equals(FontAttributes, value))
                    {
                        FontAttributes = (MC.FontAttributes?)value;
                        NativeControl.FontAttributes = FontAttributes ?? (MC.FontAttributes)MC.Picker.FontAttributesProperty.DefaultValue;
                    }
                    break;
                case nameof(FontAutoScalingEnabled):
                    if (!Equals(FontAutoScalingEnabled, value))
                    {
                        FontAutoScalingEnabled = (bool?)value;
                        NativeControl.FontAutoScalingEnabled = FontAutoScalingEnabled ?? (bool)MC.Picker.FontAutoScalingEnabledProperty.DefaultValue;
                    }
                    break;
                case nameof(FontFamily):
                    if (!Equals(FontFamily, value))
                    {
                        FontFamily = (string)value;
                        NativeControl.FontFamily = FontFamily;
                    }
                    break;
                case nameof(FontSize):
                    if (!Equals(FontSize, value))
                    {
                        FontSize = (double?)value;
                        NativeControl.FontSize = FontSize ?? (double)MC.Picker.FontSizeProperty.DefaultValue;
                    }
                    break;
                case nameof(HorizontalTextAlignment):
                    if (!Equals(HorizontalTextAlignment, value))
                    {
                        HorizontalTextAlignment = (TextAlignment?)value;
                        NativeControl.HorizontalTextAlignment = HorizontalTextAlignment ?? (TextAlignment)MC.Picker.HorizontalTextAlignmentProperty.DefaultValue;
                    }
                    break;
                case nameof(ItemDisplayBinding):
                    if (!Equals(ItemDisplayBinding, value))
                    {
                        ItemDisplayBinding = (Func<T, string>)value;
                        NativeControl.ItemDisplayBinding = AttributeHelper.GetBinding(ItemDisplayBinding);
                    }
                    break;
                case nameof(ItemsSource):
                    if (!Equals(ItemsSource, value))
                    {
                        ItemsSource = (IList<T>)value;
                        NativeControl.ItemsSource = AttributeHelper.GetIList(ItemsSource);
                    }
                    break;
                case nameof(SelectedIndex):
                    if (!Equals(SelectedIndex, value))
                    {
                        SelectedIndex = (int?)value;
                        NativeControl.SelectedIndex = SelectedIndex ?? (int)MC.Picker.SelectedIndexProperty.DefaultValue;
                    }
                    break;
                case nameof(SelectedItem):
                    if (!Equals(SelectedItem, value))
                    {
                        SelectedItem = (T)value;
                        NativeControl.SelectedItem = SelectedItem;
                    }
                    break;
                case nameof(TextColor):
                    if (!Equals(TextColor, value))
                    {
                        TextColor = (Color)value;
                        NativeControl.TextColor = TextColor;
                    }
                    break;
                case nameof(Title):
                    if (!Equals(Title, value))
                    {
                        Title = (string)value;
                        NativeControl.Title = Title;
                    }
                    break;
                case nameof(TitleColor):
                    if (!Equals(TitleColor, value))
                    {
                        TitleColor = (Color)value;
                        NativeControl.TitleColor = TitleColor;
                    }
                    break;
                case nameof(VerticalTextAlignment):
                    if (!Equals(VerticalTextAlignment, value))
                    {
                        VerticalTextAlignment = (TextAlignment?)value;
                        NativeControl.VerticalTextAlignment = VerticalTextAlignment ?? (TextAlignment)MC.Picker.VerticalTextAlignmentProperty.DefaultValue;
                    }
                    break;
                case nameof(SelectedItemChanged):
                    if (!Equals(SelectedItemChanged, value))
                    {
                        void NativeControlPropertyChanged(object sender, PropertyChangedEventArgs e)
                        {
                            if (e.PropertyName == nameof(NativeControl.SelectedItem))
                            {
                                var value = NativeControl.SelectedItem is T item ? item : default(T);
                                SelectedItem = value;
                                InvokeEventCallback(SelectedItemChanged, value);
                            }
                        }

                        SelectedItemChanged = (EventCallback<T>)value;
                        NativeControl.PropertyChanged -= NativeControlPropertyChanged;
                        NativeControl.PropertyChanged += NativeControlPropertyChanged;
                    }
                    break;
                case nameof(SelectedIndexChanged):
                    if (!Equals(SelectedIndexChanged, value))
                    {
                        void NativeControlSelectedIndexChanged(object sender, EventArgs e)
                        {
                            var value = NativeControl.SelectedIndex;
                            SelectedIndex = value;
                            InvokeEventCallback(SelectedIndexChanged, value);
                        }

                        SelectedIndexChanged = (EventCallback<int>)value;
                        NativeControl.SelectedIndexChanged -= NativeControlSelectedIndexChanged;
                        NativeControl.SelectedIndexChanged += NativeControlSelectedIndexChanged;
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
