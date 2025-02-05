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
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using SMTT = Syncfusion.Maui.Toolkit.TextInputLayout;
using System.Threading.Tasks;

#pragma warning disable MBB001

namespace BlazorBindings.Maui.Elements.Syncfusion.Toolkit.TextInputLayout
{
    /// <summary>
    /// Represents the <see cref="T:Syncfusion.Maui.Toolkit.TextInputLayout.SfTextInputLayout" /> control that enhances input views with decorative elements such as floating labels, icons, and assistive labels.
    /// </summary>
    public partial class SfTextInputLayout : BlazorBindings.Maui.Elements.Syncfusion.Toolkit.SfContentView
    {
        static SfTextInputLayout()
        {
            RegisterAdditionalHandlers();
        }

        /// <summary>
        /// Gets or sets the maximum character length for the input. An error color is applied when this limit is exceeded.
        /// </summary>
        [Parameter] public int? CharMaxLength { get; set; }
        /// <summary>
        /// Gets or sets the background of the container.
        /// </summary>
        [Parameter] public Color ContainerBackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets the type of container, which specifies the appearance of the background and its border. The default value is <c>ContainerType.Filled</c>.
        /// </summary>
        [Parameter] public SMTT.ContainerType? ContainerType { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the assistive label should float when the input is focused or unfocused.
        /// </summary>
        /// <value>
        /// <c>true</c> if the label should float; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? EnableFloating { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to enable animation for the hint text when the input view is focused or unfocused.
        /// </summary>
        /// <value>
        /// <c>true</c> if hint animation is enabled; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? EnableHintAnimation { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to show the password visibility toggle.
        /// </summary>
        /// <value>
        /// <c>true</c> if the password visibility toggle is enabled; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? EnablePasswordVisibilityToggle { get; set; }
        /// <summary>
        /// Gets or sets the error text displayed when validation fails.
        /// </summary>
        [Parameter] public string ErrorText { get; set; }
        /// <summary>
        /// Gets or sets the stroke thickness of the bottom line or outline border when control is in a focused state. <remarks>This property is applicable for both filled and outlined container types.</remarks>
        /// </summary>
        /// <value>
        /// The default value is 2.
        /// </value>
        [Parameter] public double? FocusedStrokeThickness { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the input has validation errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if there are validation errors; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? HasError { get; set; }
        /// <summary>
        /// Gets or sets the helper text that provides additional information about the input.
        /// </summary>
        [Parameter] public string HelperText { get; set; }
        /// <summary>
        /// Gets or sets the hint text displayed in the input view.
        /// </summary>
        [Parameter] public string Hint { get; set; }
        /// <summary>
        /// Gets or sets custom padding for the input view, overriding the default padding.
        /// </summary>
        [Parameter] public Thickness? InputViewPadding { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled and can interact with the user. The default value is <c>true</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the control is enabled; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public new bool? IsEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the hint label should always be floated, even when the input text is empty. The default value is <c>false</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the hint label should always be floated; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? IsHintAlwaysFloated { get; set; }
        /// <summary>
        /// Gets or sets the position of the leading view relative to the input layout. The default value is <c>ViewPosition.Inside</c>.
        /// </summary>
        [Parameter] public SMTT.ViewPosition? LeadingViewPosition { get; set; }
        /// <summary>
        /// Gets or sets the corner radius of the outline border.
        /// </summary>
        /// <value>
        /// The default value is 4.
        /// </value>
        [Parameter] public double? OutlineCornerRadius { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether space is reserved for assistive labels such as helper text, error text, and character counters.
        /// </summary>
        /// <value>
        /// <c>true</c> if space should be reserved for assistive labels; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? ReserveSpaceForAssistiveLabels { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the helper text should be displayed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the helper text should be displayed; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? ShowHelperText { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the hint text should be displayed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the hint text should be displayed; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? ShowHint { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the leading view should be displayed. The default value is <c>true</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the leading view should be displayed; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? ShowLeadingView { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the trailing view should be displayed. The default value is <c>true</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the trailing view should be displayed; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool? ShowTrailingView { get; set; }
        /// <summary>
        /// Gets or sets the color of the border or base line, depending on the container type.
        /// </summary>
        [Parameter] public Color Stroke { get; set; }
        /// <summary>
        /// Gets or sets the position of the trailing view relative to the input layout. The default value is <c>ViewPosition.Inside</c>.
        /// </summary>
        [Parameter] public SMTT.ViewPosition? TrailingViewPosition { get; set; }
        /// <summary>
        /// Gets or sets the stroke thickness for the bottom line or outline border when control is in an unfocused state <remarks>This property is applicable for filled and outlined container types.</remarks>
        /// </summary>
        /// <value>
        /// The default value is 1.
        /// </value>
        [Parameter] public double? UnfocusedStrokeThickness { get; set; }
        /// <summary>
        /// Gets or sets the background of the container.
        /// </summary>
        [Parameter] public RenderFragment ContainerBackground { get; set; }
        /// <summary>
        /// Gets or sets the style applied to the error label.
        /// </summary>
        [Parameter] public RenderFragment ErrorLabelStyle { get; set; }
        /// <summary>
        /// Gets or sets the style applied to the helper label.
        /// </summary>
        [Parameter] public RenderFragment HelperLabelStyle { get; set; }
        /// <summary>
        /// Gets or sets the style applied to the hint label.
        /// </summary>
        [Parameter] public RenderFragment HintLabelStyle { get; set; }
        /// <summary>
        /// Gets or sets the view to be displayed before the input view.
        /// </summary>
        [Parameter] public RenderFragment LeadingView { get; set; }
        /// <summary>
        /// Gets or sets the view to be displayed after the input view.
        /// </summary>
        [Parameter] public RenderFragment TrailingView { get; set; }
        [Parameter] public EventCallback<SMTT.PasswordVisibilityToggledEventArgs> OnPasswordVisibilityToggled { get; set; }

        public new SMTT.SfTextInputLayout NativeControl => (SMTT.SfTextInputLayout)((BindableObject)this).NativeControl;

        protected override SMTT.SfTextInputLayout CreateNativeElement() => new();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(CharMaxLength):
                    if (!Equals(CharMaxLength, value))
                    {
                        CharMaxLength = (int?)value;
                        NativeControl.CharMaxLength = CharMaxLength ?? (int)SMTT.SfTextInputLayout.CharMaxLengthProperty.DefaultValue;
                    }
                    break;
                case nameof(ContainerBackgroundColor):
                    if (!Equals(ContainerBackgroundColor, value))
                    {
                        ContainerBackgroundColor = (Color)value;
                        NativeControl.ContainerBackground = ContainerBackgroundColor;
                    }
                    break;
                case nameof(ContainerType):
                    if (!Equals(ContainerType, value))
                    {
                        ContainerType = (SMTT.ContainerType?)value;
                        NativeControl.ContainerType = ContainerType ?? (SMTT.ContainerType)SMTT.SfTextInputLayout.ContainerTypeProperty.DefaultValue;
                    }
                    break;
                case nameof(EnableFloating):
                    if (!Equals(EnableFloating, value))
                    {
                        EnableFloating = (bool?)value;
                        NativeControl.EnableFloating = EnableFloating ?? (bool)SMTT.SfTextInputLayout.EnableFloatingProperty.DefaultValue;
                    }
                    break;
                case nameof(EnableHintAnimation):
                    if (!Equals(EnableHintAnimation, value))
                    {
                        EnableHintAnimation = (bool?)value;
                        NativeControl.EnableHintAnimation = EnableHintAnimation ?? (bool)SMTT.SfTextInputLayout.EnableHintAnimationProperty.DefaultValue;
                    }
                    break;
                case nameof(EnablePasswordVisibilityToggle):
                    if (!Equals(EnablePasswordVisibilityToggle, value))
                    {
                        EnablePasswordVisibilityToggle = (bool?)value;
                        NativeControl.EnablePasswordVisibilityToggle = EnablePasswordVisibilityToggle ?? (bool)SMTT.SfTextInputLayout.EnablePasswordVisibilityToggleProperty.DefaultValue;
                    }
                    break;
                case nameof(ErrorText):
                    if (!Equals(ErrorText, value))
                    {
                        ErrorText = (string)value;
                        NativeControl.ErrorText = ErrorText;
                    }
                    break;
                case nameof(FocusedStrokeThickness):
                    if (!Equals(FocusedStrokeThickness, value))
                    {
                        FocusedStrokeThickness = (double?)value;
                        NativeControl.FocusedStrokeThickness = FocusedStrokeThickness ?? (double)SMTT.SfTextInputLayout.FocusedStrokeThicknessProperty.DefaultValue;
                    }
                    break;
                case nameof(HasError):
                    if (!Equals(HasError, value))
                    {
                        HasError = (bool?)value;
                        NativeControl.HasError = HasError ?? (bool)SMTT.SfTextInputLayout.HasErrorProperty.DefaultValue;
                    }
                    break;
                case nameof(HelperText):
                    if (!Equals(HelperText, value))
                    {
                        HelperText = (string)value;
                        NativeControl.HelperText = HelperText;
                    }
                    break;
                case nameof(Hint):
                    if (!Equals(Hint, value))
                    {
                        Hint = (string)value;
                        NativeControl.Hint = Hint;
                    }
                    break;
                case nameof(InputViewPadding):
                    if (!Equals(InputViewPadding, value))
                    {
                        InputViewPadding = (Thickness?)value;
                        NativeControl.InputViewPadding = InputViewPadding ?? (Thickness)SMTT.SfTextInputLayout.InputViewPaddingProperty.DefaultValue;
                    }
                    break;
                case nameof(IsEnabled):
                    if (!Equals(IsEnabled, value))
                    {
                        IsEnabled = (bool?)value;
                        NativeControl.IsEnabled = IsEnabled ?? (bool)SMTT.SfTextInputLayout.IsEnabledProperty.DefaultValue;
                    }
                    break;
                case nameof(IsHintAlwaysFloated):
                    if (!Equals(IsHintAlwaysFloated, value))
                    {
                        IsHintAlwaysFloated = (bool?)value;
                        NativeControl.IsHintAlwaysFloated = IsHintAlwaysFloated ?? (bool)SMTT.SfTextInputLayout.IsHintAlwaysFloatedProperty.DefaultValue;
                    }
                    break;
                case nameof(LeadingViewPosition):
                    if (!Equals(LeadingViewPosition, value))
                    {
                        LeadingViewPosition = (SMTT.ViewPosition?)value;
                        NativeControl.LeadingViewPosition = LeadingViewPosition ?? (SMTT.ViewPosition)SMTT.SfTextInputLayout.LeadingViewPositionProperty.DefaultValue;
                    }
                    break;
                case nameof(OutlineCornerRadius):
                    if (!Equals(OutlineCornerRadius, value))
                    {
                        OutlineCornerRadius = (double?)value;
                        NativeControl.OutlineCornerRadius = OutlineCornerRadius ?? (double)SMTT.SfTextInputLayout.OutlineCornerRadiusProperty.DefaultValue;
                    }
                    break;
                case nameof(ReserveSpaceForAssistiveLabels):
                    if (!Equals(ReserveSpaceForAssistiveLabels, value))
                    {
                        ReserveSpaceForAssistiveLabels = (bool?)value;
                        NativeControl.ReserveSpaceForAssistiveLabels = ReserveSpaceForAssistiveLabels ?? (bool)SMTT.SfTextInputLayout.ReserveSpaceForAssistiveLabelsProperty.DefaultValue;
                    }
                    break;
                case nameof(ShowHelperText):
                    if (!Equals(ShowHelperText, value))
                    {
                        ShowHelperText = (bool?)value;
                        NativeControl.ShowHelperText = ShowHelperText ?? (bool)SMTT.SfTextInputLayout.ShowHelperTextProperty.DefaultValue;
                    }
                    break;
                case nameof(ShowHint):
                    if (!Equals(ShowHint, value))
                    {
                        ShowHint = (bool?)value;
                        NativeControl.ShowHint = ShowHint ?? (bool)SMTT.SfTextInputLayout.ShowHintProperty.DefaultValue;
                    }
                    break;
                case nameof(ShowLeadingView):
                    if (!Equals(ShowLeadingView, value))
                    {
                        ShowLeadingView = (bool?)value;
                        NativeControl.ShowLeadingView = ShowLeadingView ?? (bool)SMTT.SfTextInputLayout.ShowLeadingViewProperty.DefaultValue;
                    }
                    break;
                case nameof(ShowTrailingView):
                    if (!Equals(ShowTrailingView, value))
                    {
                        ShowTrailingView = (bool?)value;
                        NativeControl.ShowTrailingView = ShowTrailingView ?? (bool)SMTT.SfTextInputLayout.ShowTrailingViewProperty.DefaultValue;
                    }
                    break;
                case nameof(Stroke):
                    if (!Equals(Stroke, value))
                    {
                        Stroke = (Color)value;
                        NativeControl.Stroke = Stroke;
                    }
                    break;
                case nameof(TrailingViewPosition):
                    if (!Equals(TrailingViewPosition, value))
                    {
                        TrailingViewPosition = (SMTT.ViewPosition?)value;
                        NativeControl.TrailingViewPosition = TrailingViewPosition ?? (SMTT.ViewPosition)SMTT.SfTextInputLayout.TrailingViewPositionProperty.DefaultValue;
                    }
                    break;
                case nameof(UnfocusedStrokeThickness):
                    if (!Equals(UnfocusedStrokeThickness, value))
                    {
                        UnfocusedStrokeThickness = (double?)value;
                        NativeControl.UnfocusedStrokeThickness = UnfocusedStrokeThickness ?? (double)SMTT.SfTextInputLayout.UnfocusedStrokeThicknessProperty.DefaultValue;
                    }
                    break;
                case nameof(ContainerBackground):
                    ContainerBackground = (RenderFragment)value;
                    break;
                case nameof(ErrorLabelStyle):
                    ErrorLabelStyle = (RenderFragment)value;
                    break;
                case nameof(HelperLabelStyle):
                    HelperLabelStyle = (RenderFragment)value;
                    break;
                case nameof(HintLabelStyle):
                    HintLabelStyle = (RenderFragment)value;
                    break;
                case nameof(LeadingView):
                    LeadingView = (RenderFragment)value;
                    break;
                case nameof(TrailingView):
                    TrailingView = (RenderFragment)value;
                    break;
                case nameof(OnPasswordVisibilityToggled):
                    if (!Equals(OnPasswordVisibilityToggled, value))
                    {
                        void NativeControlPasswordVisibilityToggled(object sender, SMTT.PasswordVisibilityToggledEventArgs e) => InvokeEventCallback(OnPasswordVisibilityToggled, e);

                        OnPasswordVisibilityToggled = (EventCallback<SMTT.PasswordVisibilityToggledEventArgs>)value;
                        NativeControl.PasswordVisibilityToggled -= NativeControlPasswordVisibilityToggled;
                        NativeControl.PasswordVisibilityToggled += NativeControlPasswordVisibilityToggled;
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
            RenderTreeBuilderHelper.AddContentProperty<SMTT.SfTextInputLayout>(builder, sequence++, ContainerBackground, (x, value) => x.ContainerBackground = (MC.Brush)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTT.SfTextInputLayout>(builder, sequence++, ErrorLabelStyle, (x, value) => x.ErrorLabelStyle = (SMTT.LabelStyle)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTT.SfTextInputLayout>(builder, sequence++, HelperLabelStyle, (x, value) => x.HelperLabelStyle = (SMTT.LabelStyle)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTT.SfTextInputLayout>(builder, sequence++, HintLabelStyle, (x, value) => x.HintLabelStyle = (SMTT.LabelStyle)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTT.SfTextInputLayout>(builder, sequence++, LeadingView, (x, value) => x.LeadingView = (MC.View)value);
            RenderTreeBuilderHelper.AddContentProperty<SMTT.SfTextInputLayout>(builder, sequence++, TrailingView, (x, value) => x.TrailingView = (MC.View)value);
        }

        static partial void RegisterAdditionalHandlers();
    }
}
