using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class DatePicker
{
    [Parameter] public DateOnly? Date { get; set; }
    [Parameter] public EventCallback<DateOnly?> DateChanged { get; set; }
    [Parameter] public DateOnly? MaximumDate { get; set; }
    [Parameter] public DateOnly? MinimumDate { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        switch (name)
        {
            case nameof(Date):
                if (!Equals(Date, value))
                {
                    Date = CastParameter<DateOnly?>(value, name);
                    NativeControl.Date = Date?.ToDateTime(TimeOnly.MinValue);
                }
                return true;
            case nameof(MaximumDate):
                if (!Equals(MaximumDate, value))
                {
                    MaximumDate = CastParameter<DateOnly?>(value, name);
                    NativeControl.MaximumDate = MaximumDate?.ToDateTime(TimeOnly.MinValue);
                }
                return true;
            case nameof(MinimumDate):
                if (!Equals(MinimumDate, value))
                {
                    MinimumDate = CastParameter<DateOnly?>(value, name);
                    NativeControl.MinimumDate = MinimumDate?.ToDateTime(TimeOnly.MinValue);
                }
                return true;
            case nameof(DateChanged):
                if (!Equals(DateChanged, value))
                {
                    void NativeControlDateSelected(object sender, MC.DateChangedEventArgs e)
                    {
                        var value = e.NewDate is null ? (DateOnly?)null : DateOnly.FromDateTime(e.NewDate.Value);
                        Date = value;
                        InvokeEventCallback(DateChanged, value);
                    }

                    DateChanged = CastParameter<EventCallback<DateOnly?>>(value, name);
                    NativeControl.DateSelected -= NativeControlDateSelected;
                    NativeControl.DateSelected += NativeControlDateSelected;
                }
                return true;

            default:
                return base.HandleAdditionalParameter(name, value);
        }
    }
}
