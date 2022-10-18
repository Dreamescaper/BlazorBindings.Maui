﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.DataTemplates
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Class is used as generic parameter.
    internal class DataTemplateItemsComponent<T> : ComponentBase
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, ElementName);

            foreach (var itemRoot in _itemRoots)
            {
                builder.OpenComponent<InitializedVerticalStackLayout>(1);

                builder.AddAttribute(2, nameof(InitializedVerticalStackLayout.NativeControl), itemRoot);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(builder =>
                {
                    builder.OpenComponent<DataTemplateItemComponent<T>>(4);
                    builder.AddAttribute(5, nameof(DataTemplateItemComponent<T>.ContentView), itemRoot);
                    builder.AddAttribute(6, nameof(DataTemplateItemComponent<T>.Template), Template);
                    builder.CloseComponent();
                }));

                builder.CloseComponent();
            }

            builder.CloseElement();
        }

        // ElementName is parametrized so that component would be handled by appropriate handler.
        [Parameter] public string ElementName { get; set; }
        [Parameter] public RenderFragment<T> Template { get; set; }

        private readonly List<MC.VerticalStackLayout> _itemRoots = new();
        private bool _shouldRender;

        public MC.View AddTemplateRoot()
        {
            var templateRoot = new MC.VerticalStackLayout();
            _itemRoots.Add(templateRoot);

            _shouldRender = true;
            StateHasChanged();

            return templateRoot;
        }

        protected override bool ShouldRender()
        {
            // It makes sense to re-render this component only if new template root was added, makes no sense otherwise.
            if (_shouldRender)
            {
                _shouldRender = false;
                return true;
            }

            return false;
        }
    }
}
