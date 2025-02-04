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
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Maui.Graphics;
using SMTE = Syncfusion.Maui.Toolkit.EffectsView;
using System;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.EffectsView
{
    /// <summary>
    /// <see cref="T:Syncfusion.Maui.Toolkit.EffectsView.SfEffectsView" /> is a container control that provides the out-of-the-box effects such as highlight, ripple, selection, scaling, and rotation.
    /// </summary>
    public partial class SfEffectsView : BlazorBindings.Maui.Elements.Syncfusion.Toolkit.SfContentView
    {
        static SfEffectsView()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the rotation angle.
        /// </summary>
        /// <value>
        /// Specifies the rotation angle of the effects view. The default value is 0.
        /// </value>
        [Parameter] public int? Angle { get; set; }
        /// <summary>
        /// Gets or sets the effect that was start rendering on touch down and start removing on touch up in Android and UWP platforms.
        /// </summary>
        /// <value>
        /// One of the <see cref="T:Syncfusion.Maui.Toolkit.EffectsView.AutoResetEffects" /> enumeration that specifies the auto reset effect of the effects view. The default value is <see cref="F:Syncfusion.Maui.Toolkit.EffectsView.AutoResetEffects.None" />.
        /// </value>
        [Parameter] public SMTE.AutoResetEffects? AutoResetEffects { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether or not the ripple color should fade out as it grows.
        /// </summary>
        /// <value>
        /// Specifies the value whether or not the ripple color should fade out as it grows. The default value is false.
        /// </value>
        [Parameter] public bool? FadeOutRipple { get; set; }
        /// <summary>
        /// Gets or sets the color to highlight the effects view.
        /// </summary>
        /// <value>
        /// Specifies the highlight color of the effects view. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        [Parameter] public Color HighlightBackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets the initial radius factor of ripple effect.
        /// </summary>
        /// <value>
        /// Specifies the initial radius factor of ripple effect. The default value is 0.25d.
        /// </value>
        [Parameter] public double? InitialRippleFactor { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to set the view state as selected.
        /// </summary>
        /// <value>
        /// Specifies the value that indicates whether the view state should be set to selected or not. The default value is false.
        /// </value>
        [Parameter] public bool? IsSelected { get; set; }
        /// <summary>
        /// Gets or sets the long-press effect.
        /// </summary>
        /// <value>
        /// One of the <see cref="T:Syncfusion.Maui.Toolkit.EffectsView.SfEffects" /> enumeration that specifies the long press effect of the effects view. The default value is <see cref="F:Syncfusion.Maui.Toolkit.EffectsView.SfEffects.None" />.
        /// </value>
        [Parameter] public SMTE.SfEffects? LongPressEffects { get; set; }
        /// <summary>
        /// Gets or sets the duration of the ripple animation in milliseconds.
        /// </summary>
        /// <value>
        /// Specifies the duration of the ripple animation. The default value is 180d.
        /// </value>
        [Parameter] public double? RippleAnimationDuration { get; set; }
        /// <summary>
        /// Gets or sets the color of the ripple.
        /// </summary>
        /// <value>
        /// Specifies the color of the ripple effect. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        [Parameter] public Color RippleBackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets the duration of the rotation animation in milliseconds.
        /// </summary>
        /// <value>
        /// Specifies the duration of the rotation animation. The default value is 200d.
        /// </value>
        [Parameter] public double? RotationAnimationDuration { get; set; }
        /// <summary>
        /// Gets or sets the duration of the scale animation in milliseconds.
        /// </summary>
        /// <value>
        /// Specifies the duration of the scale animation. The default value is 150d.
        /// </value>
        [Parameter] public double? ScaleAnimationDuration { get; set; }
        /// <summary>
        /// Gets or sets the scale factor used for scale effect.
        /// </summary>
        /// <value>
        /// Specifies the scale factor of the scale effect. The default value is 1d.
        /// </value>
        [Parameter] public double? ScaleFactor { get; set; }
        /// <summary>
        /// Gets or sets the color applied when the view is on selected state.
        /// </summary>
        /// <value>
        /// Specifies the selection color of the effects view. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        [Parameter] public Color SelectionBackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to ignore the touches in EffectsView.
        /// </summary>
        /// <value>
        /// Specifies the value which indicates whether to ignore the touches in EffectsView. The default value is false.
        /// </value>
        [Parameter] public bool? ShouldIgnoreTouches { get; set; }
        /// <summary>
        /// Gets or sets the effects for touch down.
        /// </summary>
        /// <value>
        /// One of the <see cref="T:Syncfusion.Maui.Toolkit.EffectsView.SfEffects" /> enumeration that specifies the touch down effect of the effects view. The default value is <see cref="F:Syncfusion.Maui.Toolkit.EffectsView.SfEffects.Ripple" />.
        /// </value>
        [Parameter] public SMTE.SfEffects? TouchDownEffects { get; set; }
        /// <summary>
        /// Gets or sets the effects for touch up.
        /// </summary>
        /// <value>
        /// One of the <see cref="T:Syncfusion.Maui.Toolkit.EffectsView.SfEffects" /> enumeration that specifies the touch up effect of the effects view. The default value is <see cref="F:Syncfusion.Maui.Toolkit.EffectsView.SfEffects.None" />.
        /// </value>
        [Parameter] public SMTE.SfEffects? TouchUpEffects { get; set; }
        /// <summary>
        /// Gets or sets the color to highlight the effects view.
        /// </summary>
        /// <value>
        /// Specifies the highlight color of the effects view. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        [Parameter] public RenderFragment HighlightBackground { get; set; }
        /// <summary>
        /// Gets or sets the color of the ripple.
        /// </summary>
        /// <value>
        /// Specifies the color of the ripple effect. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        [Parameter] public RenderFragment RippleBackground { get; set; }
        /// <summary>
        /// Gets or sets the color applied when the view is on selected state.
        /// </summary>
        /// <value>
        /// Specifies the selection color of the effects view. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        [Parameter] public RenderFragment SelectionBackground { get; set; }
        [Parameter] public EventCallback OnAnimationCompleted { get; set; }
        [Parameter] public EventCallback OnSelectionChanged { get; set; }
        [Parameter] public EventCallback OnTouchDown { get; set; }
        [Parameter] public EventCallback OnTouchUp { get; set; }
        [Parameter] public EventCallback OnLongPressed { get; set; }

        public new SMTE.SfEffectsView NativeControl => (SMTE.SfEffectsView)((BindableObject)this).NativeControl;

        protected override SMTE.SfEffectsView CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Angle):
                    if (!Equals(Angle, value))
                    {
                        Angle = (int?)value;
                        NativeControl.Angle = Angle ?? (int)SMTE.SfEffectsView.AngleProperty.DefaultValue;
                    }
                    break;
                case nameof(AutoResetEffects):
                    if (!Equals(AutoResetEffects, value))
                    {
                        AutoResetEffects = (SMTE.AutoResetEffects?)value;
                        NativeControl.AutoResetEffects = AutoResetEffects ?? (SMTE.AutoResetEffects)SMTE.SfEffectsView.AutoResetEffectsProperty.DefaultValue;
                    }
                    break;
                case nameof(FadeOutRipple):
                    if (!Equals(FadeOutRipple, value))
                    {
                        FadeOutRipple = (bool?)value;
                        NativeControl.FadeOutRipple = FadeOutRipple ?? (bool)SMTE.SfEffectsView.FadeOutRippleProperty.DefaultValue;
                    }
                    break;
                case nameof(HighlightBackgroundColor):
                    if (!Equals(HighlightBackgroundColor, value))
                    {
                        HighlightBackgroundColor = (Color)value;
                        NativeControl.HighlightBackground = HighlightBackgroundColor;
                    }
                    break;
                case nameof(InitialRippleFactor):
                    if (!Equals(InitialRippleFactor, value))
                    {
                        InitialRippleFactor = (double?)value;
                        NativeControl.InitialRippleFactor = InitialRippleFactor ?? (double)SMTE.SfEffectsView.InitialRippleFactorProperty.DefaultValue;
                    }
                    break;
                case nameof(IsSelected):
                    if (!Equals(IsSelected, value))
                    {
                        IsSelected = (bool?)value;
                        NativeControl.IsSelected = IsSelected ?? (bool)SMTE.SfEffectsView.IsSelectedProperty.DefaultValue;
                    }
                    break;
                case nameof(LongPressEffects):
                    if (!Equals(LongPressEffects, value))
                    {
                        LongPressEffects = (SMTE.SfEffects?)value;
                        NativeControl.LongPressEffects = LongPressEffects ?? (SMTE.SfEffects)SMTE.SfEffectsView.LongPressEffectsProperty.DefaultValue;
                    }
                    break;
                case nameof(RippleAnimationDuration):
                    if (!Equals(RippleAnimationDuration, value))
                    {
                        RippleAnimationDuration = (double?)value;
                        NativeControl.RippleAnimationDuration = RippleAnimationDuration ?? (double)SMTE.SfEffectsView.RippleAnimationDurationProperty.DefaultValue;
                    }
                    break;
                case nameof(RippleBackgroundColor):
                    if (!Equals(RippleBackgroundColor, value))
                    {
                        RippleBackgroundColor = (Color)value;
                        NativeControl.RippleBackground = RippleBackgroundColor;
                    }
                    break;
                case nameof(RotationAnimationDuration):
                    if (!Equals(RotationAnimationDuration, value))
                    {
                        RotationAnimationDuration = (double?)value;
                        NativeControl.RotationAnimationDuration = RotationAnimationDuration ?? (double)SMTE.SfEffectsView.RotationAnimationDurationProperty.DefaultValue;
                    }
                    break;
                case nameof(ScaleAnimationDuration):
                    if (!Equals(ScaleAnimationDuration, value))
                    {
                        ScaleAnimationDuration = (double?)value;
                        NativeControl.ScaleAnimationDuration = ScaleAnimationDuration ?? (double)SMTE.SfEffectsView.ScaleAnimationDurationProperty.DefaultValue;
                    }
                    break;
                case nameof(ScaleFactor):
                    if (!Equals(ScaleFactor, value))
                    {
                        ScaleFactor = (double?)value;
                        NativeControl.ScaleFactor = ScaleFactor ?? (double)SMTE.SfEffectsView.ScaleFactorProperty.DefaultValue;
                    }
                    break;
                case nameof(SelectionBackgroundColor):
                    if (!Equals(SelectionBackgroundColor, value))
                    {
                        SelectionBackgroundColor = (Color)value;
                        NativeControl.SelectionBackground = SelectionBackgroundColor;
                    }
                    break;
                case nameof(ShouldIgnoreTouches):
                    if (!Equals(ShouldIgnoreTouches, value))
                    {
                        ShouldIgnoreTouches = (bool?)value;
                        NativeControl.ShouldIgnoreTouches = ShouldIgnoreTouches ?? (bool)SMTE.SfEffectsView.ShouldIgnoreTouchesProperty.DefaultValue;
                    }
                    break;
                case nameof(TouchDownEffects):
                    if (!Equals(TouchDownEffects, value))
                    {
                        TouchDownEffects = (SMTE.SfEffects?)value;
                        NativeControl.TouchDownEffects = TouchDownEffects ?? (SMTE.SfEffects)SMTE.SfEffectsView.TouchDownEffectsProperty.DefaultValue;
                    }
                    break;
                case nameof(TouchUpEffects):
                    if (!Equals(TouchUpEffects, value))
                    {
                        TouchUpEffects = (SMTE.SfEffects?)value;
                        NativeControl.TouchUpEffects = TouchUpEffects ?? (SMTE.SfEffects)SMTE.SfEffectsView.TouchUpEffectsProperty.DefaultValue;
                    }
                    break;
                case nameof(HighlightBackground):
                    HighlightBackground = (RenderFragment)value;
                    break;
                case nameof(RippleBackground):
                    RippleBackground = (RenderFragment)value;
                    break;
                case nameof(SelectionBackground):
                    SelectionBackground = (RenderFragment)value;
                    break;
                case nameof(OnAnimationCompleted):
                    if (!Equals(OnAnimationCompleted, value))
                    {
                        void NativeControlAnimationCompleted(object sender, EventArgs e) => InvokeEventCallback(OnAnimationCompleted);

                        OnAnimationCompleted = (EventCallback)value;
                        NativeControl.AnimationCompleted -= NativeControlAnimationCompleted;
                        NativeControl.AnimationCompleted += NativeControlAnimationCompleted;
                    }
                    break;
                case nameof(OnSelectionChanged):
                    if (!Equals(OnSelectionChanged, value))
                    {
                        void NativeControlSelectionChanged(object sender, EventArgs e) => InvokeEventCallback(OnSelectionChanged);

                        OnSelectionChanged = (EventCallback)value;
                        NativeControl.SelectionChanged -= NativeControlSelectionChanged;
                        NativeControl.SelectionChanged += NativeControlSelectionChanged;
                    }
                    break;
                case nameof(OnTouchDown):
                    if (!Equals(OnTouchDown, value))
                    {
                        void NativeControlTouchDown(object sender, EventArgs e) => InvokeEventCallback(OnTouchDown);

                        OnTouchDown = (EventCallback)value;
                        NativeControl.TouchDown -= NativeControlTouchDown;
                        NativeControl.TouchDown += NativeControlTouchDown;
                    }
                    break;
                case nameof(OnTouchUp):
                    if (!Equals(OnTouchUp, value))
                    {
                        void NativeControlTouchUp(object sender, EventArgs e) => InvokeEventCallback(OnTouchUp);

                        OnTouchUp = (EventCallback)value;
                        NativeControl.TouchUp -= NativeControlTouchUp;
                        NativeControl.TouchUp += NativeControlTouchUp;
                    }
                    break;
                case nameof(OnLongPressed):
                    if (!Equals(OnLongPressed, value))
                    {
                        void NativeControlLongPressed(object sender, EventArgs e) => InvokeEventCallback(OnLongPressed);

                        OnLongPressed = (EventCallback)value;
                        NativeControl.LongPressed -= NativeControlLongPressed;
                        NativeControl.LongPressed += NativeControlLongPressed;
                    }
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        protected override void RenderAdditionalElementContent(RenderTreeBuilder builder, ref int sequence)
        {
            base.RenderAdditionalElementContent(builder, ref sequence);
            RenderTreeBuilderHelper.AddContentProperty<SMTE.SfEffectsView>(builder, sequence++, HighlightBackground, (x, value) => x.HighlightBackground = (MC.Brush)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTE.SfEffectsView>(builder, sequence++, RippleBackground, (x, value) => x.RippleBackground = (MC.Brush)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTE.SfEffectsView>(builder, sequence++, SelectionBackground, (x, value) => x.SelectionBackground = (MC.Brush)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
