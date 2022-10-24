﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MC = Microsoft.Maui.Controls;
using WVM = Microsoft.AspNetCore.Components.WebView.Maui;

namespace BlazorBindings.Maui.Elements
{
    public class RootComponent : NativeControlComponentBase, IMauiElementHandler, INonPhysicalChild
    {
        [Parameter] public string Selector { get; set; }
        [Parameter] public Type ComponentType { get; set; }
        [Parameter] public IDictionary<string, object> Parameters { get; set; }

        public WVM.RootComponent NativeControl { get; } = new WVM.RootComponent();

        void INonPhysicalChild.SetParent(object parentElement)
        {
            var parentWebView = parentElement as WVM.BlazorWebView
                ?? throw new InvalidOperationException($"RootComponent can't be added to parent of type {parentElement.GetType().FullName}."); ;
            parentWebView.RootComponents.Add(NativeControl);
        }

        void INonPhysicalChild.RemoveFromParent(object parentElement)
        {
            (parentElement as WVM.BlazorWebView)?.RootComponents.Remove(NativeControl);
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            foreach (var parameterValue in parameters)
            {
                switch (parameterValue.Name)
                {
                    case nameof(Selector):
                        if (!Equals(Selector, parameterValue.Value))
                        {
                            Selector = (string)parameterValue.Value;
                            NativeControl.Selector = Selector;
                        }
                        break;
                    case nameof(ComponentType):
                        if (!Equals(ComponentType, parameterValue.Value))
                        {
                            ComponentType = (Type)parameterValue.Value;
                            NativeControl.ComponentType = ComponentType;
                        }
                        break;
                    case nameof(Parameters):
                        if (!Equals(Parameters, parameterValue.Value))
                        {
                            Parameters = (IDictionary<string, object>)parameterValue.Value;
                            NativeControl.Parameters = Parameters;
                        }
                        break;
                }
            }

            return base.SetParametersAsync(ParameterView.Empty);
        }


        protected override void RenderAttributes(AttributesBuilder builder)
        {
        }

        void IElementHandler.ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName)
        {
        }
        MC.Element IMauiElementHandler.ElementControl => null;
        object IElementHandler.TargetElement => NativeControl;
    }
}