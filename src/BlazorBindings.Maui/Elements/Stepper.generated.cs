// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorBindings.Maui.Elements
{
    public partial class Stepper : View
    {
        static Stepper()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public double Increment { get; set; }
        [Parameter] public double Maximum { get; set; }
        [Parameter] public double Minimum { get; set; }
        [Parameter] public double Value { get; set; }
        [Parameter] public EventCallback<double> ValueChanged { get; set; }

        public new MC.Stepper NativeControl => (MC.Stepper)((Element)this).NativeControl;

        protected override MC.Element CreateNativeElement() => new MC.Stepper();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(Increment):
                    if (!Equals(Increment, value))
                    {
                        Increment = (double)value;
                        NativeControl.Increment = Increment;
                    }
                    break;
                case nameof(Maximum):
                    if (!Equals(Maximum, value))
                    {
                        Maximum = (double)value;
                        NativeControl.Maximum = Maximum;
                    }
                    break;
                case nameof(Minimum):
                    if (!Equals(Minimum, value))
                    {
                        Minimum = (double)value;
                        NativeControl.Minimum = Minimum;
                    }
                    break;
                case nameof(Value):
                    if (!Equals(Value, value))
                    {
                        Value = (double)value;
                        NativeControl.Value = Value;
                    }
                    break;
                case nameof(ValueChanged):
                    if (!Equals(ValueChanged, value))
                    {
                        void NativeControlValueChanged(object sender, MC.ValueChangedEventArgs e) => ValueChanged.InvokeAsync(NativeControl.Value);

                        ValueChanged = (EventCallback<double>)value;
                        NativeControl.ValueChanged -= NativeControlValueChanged;
                        NativeControl.ValueChanged += NativeControlValueChanged;
                    }
                    break;

                default:
                    base.HandleParameter(name, value);
                    break;
            }
        }

        static partial void RegisterAdditionalHandlers();
    }
}
