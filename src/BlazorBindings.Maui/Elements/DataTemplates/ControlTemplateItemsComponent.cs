﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.DataTemplates
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Class is used as generic parameter.
    internal class ControlTemplateItemsComponent : ComponentBase
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
                    Template.Invoke(builder);
                }));

                builder.CloseComponent();
            }

            builder.CloseElement();
        }

        // ElementName is parametrized so that component would be handled by appropriate handler.
        [Parameter] public string ElementName { get; set; }
        [Parameter] public RenderFragment Template { get; set; }

        private readonly List<MC.VerticalStackLayout> _itemRoots = new();

        public MC.View AddTemplateRoot()
        {
            var templateRoot = new MC.VerticalStackLayout();
            _itemRoots.Add(templateRoot);
            StateHasChanged();

            return templateRoot;
        }
    }
}
