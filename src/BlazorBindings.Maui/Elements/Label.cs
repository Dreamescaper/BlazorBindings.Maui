// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Diagnostics;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class Label : View, IHandleChildContentText, IContainerElementHandler
{
    private TextSpanContainer _textSpanContainer;

#pragma warning disable CA1721 // Property names should not match get methods
    [Parameter] public RenderFragment ChildContent { get; set; }
#pragma warning restore CA1721 // Property names should not match get methods

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(ChildContent))
        {
            ChildContent = (RenderFragment)value;
            return true;
        }
        else
        {
            return base.HandleAdditionalParameter(name, value);
        }
    }

    protected override RenderFragment GetChildContent() => ChildContent;

    void IHandleChildContentText.HandleText(int index, string text)
    {
        if (Text != null && string.IsNullOrWhiteSpace(text))
            return;

        if (NativeControl.FormattedText != null)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException("Cannot use both string content and Spans for Label.");
            }
        }
        else
        {
            _textSpanContainer ??= new();
            NativeControl.Text = _textSpanContainer.GetUpdatedText(index, text);
        }
    }

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        var childAsSpan = child as MC.Span;

        var formattedString = NativeControl.FormattedText ??= new MC.FormattedString();
        if (physicalSiblingIndex <= formattedString.Spans.Count)
        {
            formattedString.Spans.Insert(physicalSiblingIndex, childAsSpan);
        }
        else
        {
            Debug.WriteLine($"WARNING: {nameof(IContainerElementHandler.AddChild)} called with {nameof(physicalSiblingIndex)}={physicalSiblingIndex}, but Label.FormattedText.Spans.Count={NativeControl.FormattedText.Spans.Count}");
            formattedString.Spans.Add(childAsSpan);
        }
    }

    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex)
    {
        var childAsSpan = child as MC.Span;
        NativeControl.FormattedText?.Spans.Remove(childAsSpan);
    }
}
