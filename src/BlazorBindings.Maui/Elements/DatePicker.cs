using Microsoft.AspNetCore.Components;
using System;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements
{
    public partial class DatePicker
    {
        [Parameter] public DateOnly? Date { get; set; }
        [Parameter] public EventCallback<DateOnly> DateChanged { get; set; }
        [Parameter] public DateOnly? MaximumDate { get; set; }
        [Parameter] public DateOnly? MinimumDate { get; set; }

        protected override bool HandleAdditionalParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Date):
                    if (!Equals(Date, value))
                    {
                        Date = (DateOnly?)value;
                        NativeControl.Date = Date?.ToDateTime(TimeOnly.MinValue) ?? (DateTime)MC.DatePicker.DateProperty.DefaultValue;
                    }
                    return true;
                case nameof(MaximumDate):
                    if (!Equals(MaximumDate, value))
                    {
                        MaximumDate = (DateOnly?)value;
                        NativeControl.MaximumDate = MaximumDate?.ToDateTime(TimeOnly.MinValue) ?? (DateTime)MC.DatePicker.MaximumDateProperty.DefaultValue;
                    }
                    return true;
                case nameof(MinimumDate):
                    if (!Equals(MinimumDate, value))
                    {
                        MinimumDate = (DateOnly?)value;
                        NativeControl.MinimumDate = MinimumDate?.ToDateTime(TimeOnly.MinValue) ?? (DateTime)MC.DatePicker.MinimumDateProperty.DefaultValue;
                    }
                    return true;
                case nameof(DateChanged):
                    if (!Equals(DateChanged, value))
                    {
                        void NativeControlDateSelected(object sender, MC.DateChangedEventArgs e)
                        {
                            var value = DateOnly.FromDateTime(NativeControl.Date);
                            Date = value;
                            DateChanged.InvokeAsync(value);
                        }

                        DateChanged = (EventCallback<DateOnly>)value;
                        NativeControl.DateSelected -= NativeControlDateSelected;
                        NativeControl.DateSelected += NativeControlDateSelected;
                    }
                    return true;

                default:
                    return base.HandleAdditionalParameter(name, value);
            }
        }
    }
}
