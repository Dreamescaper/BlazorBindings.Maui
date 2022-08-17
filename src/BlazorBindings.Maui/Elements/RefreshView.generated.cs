// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Core;
using MC = Microsoft.Maui.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Graphics;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BlazorBindings.Maui.Elements
{
    public partial class RefreshView : ContentView
    {
        static RefreshView()
        {
            RegisterAdditionalHandlers();
        }

        [Parameter] public bool IsRefreshing { get; set; }
        [Parameter] public Color RefreshColor { get; set; }
        [Parameter] public EventCallback<bool> IsRefreshingChanged { get; set; }

        public new MC.RefreshView NativeControl => (MC.RefreshView)((Element)this).NativeControl;

        protected override MC.Element CreateNativeElement() => new MC.RefreshView();

        protected override void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(IsRefreshing):
                    if (!Equals(IsRefreshing, value))
                    {
                        IsRefreshing = (bool)value;
                        NativeControl.IsRefreshing = IsRefreshing;
                    }
                    break;
                case nameof(RefreshColor):
                    if (!Equals(RefreshColor, value))
                    {
                        RefreshColor = (Color)value;
                        NativeControl.RefreshColor = RefreshColor;
                    }
                    break;
                case nameof(IsRefreshingChanged):
                    if (!Equals(IsRefreshingChanged, value))
                    {
                        void NativeControlPropertyChanged(object sender, PropertyChangedEventArgs e)
                        {
                            if (e.PropertyName == nameof(NativeControl.IsRefreshing))
                            {
                                IsRefreshingChanged.InvokeAsync(NativeControl.IsRefreshing);
                            }
                        }

                        IsRefreshingChanged = (EventCallback<bool>)value;
                        NativeControl.PropertyChanged -= NativeControlPropertyChanged;
                        NativeControl.PropertyChanged += NativeControlPropertyChanged;
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
