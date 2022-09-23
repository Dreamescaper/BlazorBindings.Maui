// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Core;
using BlazorBindings.Maui.Elements;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui
{
    public class MauiBlazorBindingsRenderer : NativeComponentRenderer
    {
        public MauiBlazorBindingsRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        public override Dispatcher Dispatcher { get; } = new MauiDeviceDispatcher();

        public Task<TComponent> AddComponent<TComponent>(MC.Element parent, Dictionary<string, object> parameters = null) where TComponent : IComponent
        {
            if (parent is MC.Application app)
            {
                app.MainPage ??= new MC.ContentPage();
            }

            var handler = CreateHandler(parent, this);
            return AddComponent<TComponent>(handler, parameters);
        }

        protected override ElementManager CreateNativeControlManager()
        {
            return new MauiBlazorBindingsElementManager();
        }

        private static IElementHandler CreateHandler(MC.Element parent, MauiBlazorBindingsRenderer renderer)
        {
            return parent switch
            {
                MC.ContentPage => CreateHandler<ContentPage>(),
                MC.ContentView => CreateHandler<ContentView>(),
                MC.FlyoutPage => CreateHandler<FlyoutPage>(),
                MC.ScrollView => CreateHandler<ScrollView>(),
                MC.ShellContent => CreateHandler<ShellContent>(),
                MC.Shell => CreateHandler<Shell>(),
                MC.ShellItem => CreateHandler<ShellItem>(),
                MC.ShellSection => CreateHandler<ShellSection>(),
                MC.TabbedPage => CreateHandler<TabbedPage>(),
                _ => new InitializedElement(parent)
            };

            IElementHandler CreateHandler<T>() where T : Element, new() => new T { NativeControl = parent };
        }

        private class InitializedElement : Element
        {
            private readonly MC.Element _element;
            public InitializedElement(MC.Element element) => _element = element;
            protected override MC.Element CreateNativeElement() => _element;
        }
    }
}
