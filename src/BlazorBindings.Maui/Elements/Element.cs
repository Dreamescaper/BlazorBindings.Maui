﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements
{
    public abstract class Element : NativeControlComponentBase, IMauiElementHandler
    {
        private MC.Element _nativeControl;

        [Parameter] public string AutomationId { get; set; }
        [Parameter] public string ClassId { get; set; }
        [Parameter] public string StyleId { get; set; }

        public MC.Element NativeControl
        {
            get => _nativeControl ??= CreateNativeElement();
            internal set => _nativeControl = value;
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            foreach (var parameterValue in parameters)
            {
                HandleParameter(parameterValue.Name, parameterValue.Value);
            }

            return base.SetParametersAsync(ParameterView.Empty);
        }

        protected sealed override void RenderAttributes(AttributesBuilder builder)
        {
            // Since we set attributes directly in SetParametersAsync, this method is empty and sealed.
        }

        protected virtual void HandleParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(AutomationId):
                    if (!Equals(AutomationId, value))
                    {
                        AutomationId = (string)value;
                        NativeControl.AutomationId = AutomationId;
                    }
                    break;
                case nameof(ClassId):
                    if (!Equals(ClassId, value))
                    {
                        ClassId = (string)value;
                        NativeControl.ClassId = ClassId;
                    }
                    break;
                case nameof(StyleId):
                    if (!Equals(StyleId, value))
                    {
                        StyleId = (string)value;
                        NativeControl.StyleId = StyleId;
                    }
                    break;

                default:
                    if (HandleAdditionalParameter(name, value))
                        break;

                    if (AttachedPropertyRegistry.AttachedPropertyHandlers.TryGetValue(name, out var handler))
                    {
                        handler(NativeControl, value);
                        break;
                    }

                    throw new NotImplementedException($"{GetType().FullName} doesn't recognize parameter '{name}'");

            }
        }

        protected virtual bool HandleAdditionalParameter(string name, object value) => false;

        protected abstract MC.Element CreateNativeElement();

        void IElementHandler.ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName)
        {
        }

        MC.Element IMauiElementHandler.ElementControl => NativeControl;

        object IElementHandler.TargetElement => NativeControl;


        bool IMauiElementHandler.IsParented()
        {
            return NativeControl.Parent != null;
        }

        void IMauiElementHandler.SetParent(MC.Element parent)
        {
            NativeControl.Parent = parent;
        }
    }
}
