// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Controls;
using System;
using System.Linq;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements
{
    public abstract partial class View : VisualElement
    {
        [Parameter] public EventCallback OnTap { get; set; }
        [Parameter] public EventCallback OnDoubleTap { get; set; }
        [Parameter] public EventCallback<SwipedEventArgs> OnSwipe { get; set; }
        [Parameter] public EventCallback<PinchGestureUpdatedEventArgs> OnPinchUpdate { get; set; }
        [Parameter] public EventCallback<PanUpdatedEventArgs> OnPanUpdate { get; set; }

        protected override bool HandleAdditionalParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(OnTap):
                    if (!Equals(value, OnTap))
                    {
                        HandleTap();
                        OnTap = (EventCallback)value;
                    }
                    return true;

                case nameof(OnDoubleTap):
                    if (!Equals(value, OnDoubleTap))
                    {
                        HandleDoubleTap();
                        OnDoubleTap = (EventCallback)value;
                    }
                    return true;

                case nameof(OnSwipe):
                    if (!Equals(value, OnSwipe))
                    {
                        HandleSwipe();
                        OnSwipe = (EventCallback<SwipedEventArgs>)value;
                    }
                    return true;

                case nameof(OnPinchUpdate):
                    if (!Equals(value, OnPinchUpdate))
                    {
                        HandlePinch();
                        OnPinchUpdate = (EventCallback<PinchGestureUpdatedEventArgs>)value;
                    }
                    return true;

                case nameof(OnPanUpdate):
                    if (!Equals(value, OnPanUpdate))
                    {
                        HandlePan();
                        OnPanUpdate = (EventCallback<PanUpdatedEventArgs>)value;
                    }
                    return true;

                default:
                    return base.HandleAdditionalParameter(name, value);

            }
        }

        private void HandleTap()
        {
            void GestureRecognizerTapped(object sender, EventArgs e) => InvokeEventCallback(OnTap);

            var tapGestureRecognizer = NativeControl.GestureRecognizers
                .OfType<MC.TapGestureRecognizer>()
                .FirstOrDefault(gr => gr.NumberOfTapsRequired == 1);

            if (tapGestureRecognizer is null)
            {
                tapGestureRecognizer = new MC.TapGestureRecognizer();
                NativeControl.GestureRecognizers.Add(tapGestureRecognizer);
            }

            tapGestureRecognizer.Tapped -= GestureRecognizerTapped;
            tapGestureRecognizer.Tapped += GestureRecognizerTapped;
        }

        private void HandleDoubleTap()
        {
            void GestureRecognizerTapped(object sender, EventArgs e) => InvokeEventCallback(OnDoubleTap);

            var tapGestureRecognizer = NativeControl.GestureRecognizers
                .OfType<MC.TapGestureRecognizer>()
                .FirstOrDefault(gr => gr.NumberOfTapsRequired == 2);

            if (tapGestureRecognizer is null)
            {
                tapGestureRecognizer = new MC.TapGestureRecognizer { NumberOfTapsRequired = 2 };
                NativeControl.GestureRecognizers.Add(tapGestureRecognizer);
            }

            tapGestureRecognizer.Tapped -= GestureRecognizerTapped;
            tapGestureRecognizer.Tapped += GestureRecognizerTapped;
        }

        private void HandleSwipe()
        {
            void GestureRecognizerSwiped(object sender, SwipedEventArgs e) => InvokeEventCallback(OnSwipe, e);

            var swipeGestureRecognizer = NativeControl.GestureRecognizers
                .OfType<MC.SwipeGestureRecognizer>()
                .FirstOrDefault();

            if (swipeGestureRecognizer is null)
            {
                swipeGestureRecognizer = new MC.SwipeGestureRecognizer();
                NativeControl.GestureRecognizers.Add(swipeGestureRecognizer);
            }

            swipeGestureRecognizer.Swiped -= GestureRecognizerSwiped;
            swipeGestureRecognizer.Swiped += GestureRecognizerSwiped;
        }

        private void HandlePinch()
        {
            void GestureRecognizerPinchUpdated(object sender, PinchGestureUpdatedEventArgs e) => InvokeEventCallback(OnPinchUpdate, e);

            var pinchGestureRecognizer = NativeControl.GestureRecognizers
                .OfType<MC.PinchGestureRecognizer>()
                .FirstOrDefault();

            if (pinchGestureRecognizer is null)
            {
                pinchGestureRecognizer = new MC.PinchGestureRecognizer();
                NativeControl.GestureRecognizers.Add(pinchGestureRecognizer);
            }

            pinchGestureRecognizer.PinchUpdated -= GestureRecognizerPinchUpdated;
            pinchGestureRecognizer.PinchUpdated += GestureRecognizerPinchUpdated;
        }

        private void HandlePan()
        {
            void GestureRecognizerPanUpdated(object sender, PanUpdatedEventArgs e) => InvokeEventCallback(OnPanUpdate, e);

            var pinchGestureRecognizer = NativeControl.GestureRecognizers
                .OfType<MC.PanGestureRecognizer>()
                .FirstOrDefault();

            if (pinchGestureRecognizer is null)
            {
                pinchGestureRecognizer = new MC.PanGestureRecognizer();
                NativeControl.GestureRecognizers.Add(pinchGestureRecognizer);
            }

            pinchGestureRecognizer.PanUpdated -= GestureRecognizerPanUpdated;
            pinchGestureRecognizer.PanUpdated += GestureRecognizerPanUpdated;
        }
    }
}
