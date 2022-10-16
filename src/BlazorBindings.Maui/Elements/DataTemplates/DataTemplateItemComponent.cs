﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Threading.Tasks;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.DataTemplates
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Class is used as generic parameter.
    internal class DataTemplateItemComponent<T> : ComponentBase
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
    {
        private object _item;
        private bool _shouldRender = true;

        [Parameter] public RenderFragment<T> Template { get; set; }

        [Parameter] public MC.BindableObject ContentView { get; set; }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            foreach (var parValue in parameters)
            {
                switch (parValue.Name)
                {
                    case nameof(Template):
                        Template = (RenderFragment<T>)parValue.Value;
                        break;

                    case nameof(ContentView):
                        if (ContentView == null)
                        {
                            ContentView = (MC.BindableObject)parValue.Value;
                            OnContentViewSet();
                        }
                        else
                        {
                            if (ContentView != parValue.Value)
                                throw new NotSupportedException("Cannot re-assign ContentView after being originally set.");
                        }
                        break;
                }
            }

            return base.SetParametersAsync(ParameterView.Empty);
        }

        protected override bool ShouldRender()
        {
            // Re-rendering is required only if BindingContext is changed.
            // If this method is not overridden, it re-renders all items in DataTemplateItemsComponent
            // when new item is added there.
            return _shouldRender;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (_item != null)
            {
                builder.AddContent(0, Template.Invoke((T)_item));
                _shouldRender = false;
            }
        }

        private void OnContentViewSet()
        {
            _item = ContentView.BindingContext;

            ContentView.BindingContextChanged += (_, __) =>
            {
                var newItem = ContentView.BindingContext;
                if (newItem != null && newItem != _item)
                {
                    _item = newItem;
                    _shouldRender = true;
                    StateHasChanged();
                }
            };
        }
    }
}
