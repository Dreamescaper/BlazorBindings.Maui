using System.ComponentModel;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class TimePicker
{
    [Parameter] public TimeOnly? Time { get; set; }
    [Parameter] public EventCallback<TimeOnly> TimeChanged { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        switch (name)
        {
            case nameof(Time):
                if (!Equals(Time, value))
                {
                    Time = (TimeOnly?)value;
                    NativeControl.Time = Time?.ToTimeSpan() ?? (TimeSpan)MC.TimePicker.TimeProperty.DefaultValue;
                }
                return true;
            case nameof(TimeChanged):
                if (!Equals(TimeChanged, value))
                {
                    void NativeControlPropertyChanged(object sender, PropertyChangedEventArgs e)
                    {
                        if (e.PropertyName == nameof(NativeControl.Time))
                        {
                            var value = TimeOnly.FromTimeSpan(NativeControl.Time);
                            Time = value;
                            InvokeEventCallback(TimeChanged, value);
                        }
                    }

                    TimeChanged = (EventCallback<TimeOnly>)value;
                    NativeControl.PropertyChanged -= NativeControlPropertyChanged;
                    NativeControl.PropertyChanged += NativeControlPropertyChanged;
                }
                return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }
}
