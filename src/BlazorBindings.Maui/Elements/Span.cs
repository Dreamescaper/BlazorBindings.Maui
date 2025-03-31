// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Maui.Elements;

public partial class Span : GestureElement, IHandleChildContentText
{

    private readonly TextSpanContainer _textSpanContainer = new(trimWhitespace: false);

    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(ChildContent))
        {
            ChildContent = (RenderFragment)value;
            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }

    protected override RenderFragment GetChildContent() => ChildContent;

    void IHandleChildContentText.HandleText(int index, string text)
    {
        NativeControl.Text = _textSpanContainer.GetUpdatedText(index, text);
    }
}
