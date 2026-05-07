using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class InputView
{
    /// <summary>
    /// Gets or sets the text of the input view.
    /// </summary>
    /// <value>
    /// A string containing the text of the input view. The default value is null.
    /// </value>
    [Parameter] public string Text { get; set; }

    [Parameter] public EventCallback<string> TextChanged { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        // Adding those parameters manually to be able to override behavior in Entry.

        switch (name)
        {
            case nameof(Text):
                if (!Equals(Text, value))
                {
                    Text = CastParameter<string>(value, name);
                    NativeControl.Text = Text;
                }
                return true;

            case nameof(TextChanged):
                if (!Equals(TextChanged, value))
                {
                    void NativeControlTextChanged(object sender, MC.TextChangedEventArgs e)
                    {
                        var value = NativeControl.Text;
                        Text = value;
                        InvokeEventCallback(TextChanged, value);
                    }

                    TextChanged = CastParameter<EventCallback<string>>(value, name);
                    NativeControl.TextChanged -= NativeControlTextChanged;
                    NativeControl.TextChanged += NativeControlTextChanged;
                }
                return true;

        }

        return base.HandleAdditionalParameter(name, value);
    }
}
