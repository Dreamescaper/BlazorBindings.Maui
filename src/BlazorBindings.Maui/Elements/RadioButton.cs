// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Maui.Elements;

public partial class RadioButton : TemplatedView
{
    [Parameter] public string Text { get; set; }
    [Parameter] public object Value { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        switch (name)
        {
            case nameof(Text):
                Text = CastParameter<string>(value, name);
                NativeControl.Content = Text;
                return true;
            case nameof(Value):
                Value = value;
                NativeControl.Value = Value;

                if (Text is null && Value is not null)
                {
                    NativeControl.Content = Value.ToString();
                }

                return true;
            default:
                return base.HandleAdditionalParameter(name, value);
        }
    }
}
