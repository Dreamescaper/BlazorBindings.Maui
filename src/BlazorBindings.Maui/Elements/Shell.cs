// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements
{
    public partial class Shell : Page, IMauiContainerElementHandler
    {
        static partial void RegisterAdditionalHandlers()
        {
            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.NavBarIsVisible",
                (element, value) => MC.Shell.SetNavBarIsVisible(element, AttributeHelper.GetBool(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.NavBarHasShadow",
                (element, value) => MC.Shell.SetNavBarHasShadow(element, AttributeHelper.GetBool(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarIsVisible",
                (element, value) => MC.Shell.SetTabBarIsVisible(element, AttributeHelper.GetBool(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.BackgroundColor",
                (element, value) => MC.Shell.SetBackgroundColor(element, AttributeHelper.GetString(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.DisabledColor",
                (element, value) => MC.Shell.SetDisabledColor(element, AttributeHelper.GetString(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.ForegroundColor",
                (element, value) => MC.Shell.SetForegroundColor(element, AttributeHelper.GetString(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarBackgroundColor",
                (element, value) => MC.Shell.SetTabBarBackgroundColor(element, AttributeHelper.GetString(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarDisabledColor",
                (element, value) => MC.Shell.SetTabBarDisabledColor(element, AttributeHelper.GetString(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarForegroundColor",
                (element, value) => MC.Shell.SetTabBarForegroundColor(element, AttributeHelper.GetString(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarTitleColor",
                (element, value) => MC.Shell.SetTabBarTitleColor(element, AttributeHelper.GetString(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarUnselectedColor",
                (element, value) => MC.Shell.SetTabBarUnselectedColor(element, AttributeHelper.GetString(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TitleColor",
                (element, value) => MC.Shell.SetTitleColor(element, AttributeHelper.GetString(value)));

            AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.UnselectedColor",
                (element, value) => MC.Shell.SetUnselectedColor(element, AttributeHelper.GetString(value)));
        }

        [Parameter] public RenderFragment ChildContent { get; set; }

        protected override bool HandleAdditionalParameter(string name, object value)
        {
            switch (name)
            {
                case nameof(ChildContent):
                    ChildContent = (RenderFragment)value;
                    return true;

                default:
                    return base.HandleAdditionalParameter(name, value);
            };
        }

        protected override RenderFragment GetChildContent() => ChildContent;

        void IMauiContainerElementHandler.AddChild(MC.Element child, int physicalSiblingIndex)
        {
            ArgumentNullException.ThrowIfNull(child);

            MC.ShellItem itemToAdd = child switch
            {
                MC.TemplatedPage childAsTemplatedPage => childAsTemplatedPage, // Implicit conversion
                MC.ShellContent childAsShellContent => childAsShellContent, // Implicit conversion
                MC.ShellSection childAsShellSection => childAsShellSection, // Implicit conversion
                MC.MenuItem childAsMenuItem => childAsMenuItem, // Implicit conversion
                MC.ShellItem childAsShellItem => childAsShellItem,
                _ => throw new NotSupportedException($"Handler of type '{GetType().FullName}' doesn't support adding a child (child type is '{child.GetType().FullName}').")
            };

            if (NativeControl.Items.Count >= physicalSiblingIndex)
            {
                NativeControl.Items.Insert(physicalSiblingIndex, itemToAdd);
            }
            else
            {
                Debug.WriteLine($"WARNING: AddChild called with {nameof(physicalSiblingIndex)}={physicalSiblingIndex}, but ShellControl.Items.Count={NativeControl.Items.Count}");
                NativeControl.Items.Add(itemToAdd);
            }
        }

        void IMauiContainerElementHandler.RemoveChild(MC.Element child)
        {
            ArgumentNullException.ThrowIfNull(child);

            var itemToRemove = GetItemForElement(child)
                ?? throw new NotSupportedException($"Handler of type '{GetType().FullName}' doesn't support removing a child (child type is '{child.GetType().FullName}').");

            NativeControl.Items.Remove(itemToRemove);
        }

        int IMauiContainerElementHandler.GetChildIndex(MC.Element child)
        {
            var shellItem = GetItemForElement(child);
            return NativeControl.Items.IndexOf(shellItem);
        }


        private MC.ShellItem GetItemForElement(MC.Element child)
        {
            return child switch
            {
                MC.TemplatedPage childAsTemplatedPage => GetItemForTemplatedPage(childAsTemplatedPage),
                MC.ShellContent childAsShellContent => GetItemForContent(childAsShellContent),
                MC.ShellSection childAsShellSection => GetItemForSection(childAsShellSection),
                MC.MenuItem childAsMenuItem => GetItemForMenuItem(childAsMenuItem),
                MC.ShellItem childAsShellItem => childAsShellItem,
                _ => null
            };
        }

        private MC.ShellItem GetItemForTemplatedPage(MC.TemplatedPage childAsTemplatedPage)
        {
            return NativeControl.Items
                .FirstOrDefault(item => item.Items
                    .Any(section => section.Items.Any(content => content.Content == childAsTemplatedPage)));
        }

        private MC.ShellItem GetItemForContent(MC.ShellContent childAsShellContent)
        {
            return NativeControl.Items
                .FirstOrDefault(item => item.Items
                    .Any(section => section.Items.Contains(childAsShellContent)));
        }

        private MC.ShellItem GetItemForSection(MC.ShellSection childAsShellSection)
        {
            return NativeControl.Items.FirstOrDefault(item => item.Items.Contains(childAsShellSection));
        }

        private MC.ShellItem GetItemForMenuItem(MC.MenuItem childAsMenuItem)
        {
            // MenuItem is wrapped in ShellMenuItem, which is internal type.
            // Not sure how to identify this item correctly.
            return NativeControl.Items.FirstOrDefault(item => IsShellItemWithMenuItem(item, childAsMenuItem));
        }

        private static bool IsShellItemWithMenuItem(MC.ShellItem shellItem, MC.MenuItem menuItem)
        {
            // Xamarin.Forms.MenuShellItem is internal so we have to use reflection to check that
            // its MenuItem property is the same as the MenuItem we're looking for.
            if (!MenuShellItemType.IsAssignableFrom(shellItem.GetType()))
            {
                return false;
            }
            var menuItemInMenuShellItem = MenuShellItemMenuItemProperty.GetValue(shellItem);
            return menuItemInMenuShellItem == menuItem;
        }

        private static readonly Type MenuShellItemType = typeof(MC.ShellItem).Assembly.GetType("Microsoft.Maui.Controls.MenuShellItem");
        private static readonly PropertyInfo MenuShellItemMenuItemProperty = MenuShellItemType.GetProperty("MenuItem");
    }
}
