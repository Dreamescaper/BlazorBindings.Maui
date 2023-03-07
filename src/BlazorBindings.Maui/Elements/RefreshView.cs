// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace BlazorBindings.Maui.Elements;

public partial class RefreshView : ContentView
{
    [Parameter] public EventCallback OnRefreshing { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(OnRefreshing))
        {
            if (!Equals(OnRefreshing, value))
            {
                void NativeControlRefreshing(object sender, EventArgs e)
                {
                    InvokeEventCallback(OnRefreshing);
                    NativeControl.IsRefreshing = false;
                }

                OnRefreshing = (EventCallback)value;
                NativeControl.Refreshing -= NativeControlRefreshing;
                NativeControl.Refreshing += NativeControlRefreshing;
            }
            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }
}
