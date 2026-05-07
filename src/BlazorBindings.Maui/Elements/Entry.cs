using System.Globalization;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class Entry<T>
{
    [Parameter] public new T Text { get; set; }
    [Parameter] public new EventCallback<T> TextChanged { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        switch (name)
        {
            case nameof(Text):
                if (!Equals(Text, value))
                {
                    Text = CastParameter<T>(value, name);
                    UpdateText(Text);
                }
                return true;

            case nameof(TextChanged):
                if (!Equals(TextChanged, value))
                {
                    TextChanged = CastParameter<EventCallback<T>>(value, name);
                    NativeControl.TextChanged -= NativeControlTextChanged;
                    NativeControl.TextChanged += NativeControlTextChanged;

                    if (typeof(T) != typeof(string))
                    {
                        NativeControl.Unfocused -= NativeControlUnfocused;
                        NativeControl.Unfocused += NativeControlUnfocused;
                    }
                }
                return true;

            default:
                return base.HandleAdditionalParameter(name, value);
        }
    }

    private void UpdateText(T newValue)
    {
        NativeControl.Text = newValue?.ToString();
    }

    private void NativeControlTextChanged(object sender, MC.TextChangedEventArgs e)
    {
        var text = NativeControl.Text;

        _ = TryParse(text, out var value);

        if (!Equals(Text, value))
        {
            Text = value;
            InvokeAsync(() => TextChanged.InvokeAsync(value));
        }
    }

    void NativeControlUnfocused(object sender, MC.FocusEventArgs e)
    {
        if (!TryParse(NativeControl.Text, out _))
        {
            var value = default(T);
            NativeControl.Text = value?.ToString();
        }
    }

    static bool TryParse(string text, out T result)
    {
        if (typeof(T) == typeof(string))
        {
            result = (T)(object)text;
            return true;
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            result = default;
            return true;
        }

        if (StringConverter.TryParse(typeof(T), text, CultureInfo.CurrentCulture, out var resultObj)
            || StringConverter.TryParse(typeof(T), text, CultureInfo.InvariantCulture, out resultObj))
        {
            result = (T)resultObj;
            return true;
        }

        result = default;
        return false;
    }
}
