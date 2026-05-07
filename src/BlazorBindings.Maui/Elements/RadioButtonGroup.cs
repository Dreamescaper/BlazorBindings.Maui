using System.ComponentModel;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public class RadioButtonGroup<T> : StackLayout
{
    [Parameter] public T SelectedValue { get; set; }
    [Parameter] public EventCallback<T> SelectedValueChanged { get; set; }

    private readonly string _groupId = Guid.NewGuid().ToString();

    protected override MC.StackLayout CreateNativeElement()
    {
        var stackLayout = new MC.StackLayout();
        MC.RadioButtonGroup.SetGroupName(stackLayout, _groupId);
        return stackLayout;
    }

    protected override void HandleParameter(string name, object value)
    {
        switch (name)
        {
            case nameof(SelectedValue):
                if (!Equals(SelectedValue, value))
                {
                    SelectedValue = CastParameter<T>(value, name);
                    MC.RadioButtonGroup.SetSelectedValue(NativeControl, SelectedValue);
                }
                break;

            case nameof(SelectedValueChanged):
                if (!Equals(SelectedValueChanged, value))
                {
                    void NativeControlPropertyChanged(object sender, PropertyChangedEventArgs e)
                    {
                        if (e.PropertyName == "SelectedValue")
                        {
                            var value = (T)MC.RadioButtonGroup.GetSelectedValue(NativeControl);
                            InvokeEventCallback(SelectedValueChanged, value);
                        }
                    }

                    SelectedValueChanged = CastParameter<EventCallback<T>>(value, name);
                    NativeControl.PropertyChanged -= NativeControlPropertyChanged;
                    NativeControl.PropertyChanged += NativeControlPropertyChanged;
                }
                break;

            default:
                base.HandleParameter(name, value);
                break;
        }
    }
}
