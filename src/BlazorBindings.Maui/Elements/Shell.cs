// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Maui.Graphics;
using System.Diagnostics;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class Shell : Page, IContainerElementHandler
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
            (element, value) => MC.Shell.SetBackgroundColor(element, AttributeHelper.GetColor(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.DisabledColor",
            (element, value) => MC.Shell.SetDisabledColor(element, AttributeHelper.GetColor(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.ForegroundColor",
            (element, value) => MC.Shell.SetForegroundColor(element, AttributeHelper.GetColor(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarBackgroundColor",
            (element, value) => MC.Shell.SetTabBarBackgroundColor(element, AttributeHelper.GetColor(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarDisabledColor",
            (element, value) => MC.Shell.SetTabBarDisabledColor(element, AttributeHelper.GetColor(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarForegroundColor",
            (element, value) => MC.Shell.SetTabBarForegroundColor(element, AttributeHelper.GetColor(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarTitleColor",
            (element, value) => MC.Shell.SetTabBarTitleColor(element, AttributeHelper.GetColor(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TabBarUnselectedColor",
            (element, value) => MC.Shell.SetTabBarUnselectedColor(element, AttributeHelper.GetColor(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.TitleColor",
            (element, value) => MC.Shell.SetTitleColor(element, AttributeHelper.GetColor(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Shell.UnselectedColor",
            (element, value) => MC.Shell.SetUnselectedColor(element, AttributeHelper.GetColor(value)));
    }

    [Parameter] public RenderFragment ChildContent { get; set; }

    /// <summary>
    /// Defines the background color in the Shell chrome. The color will not fill in behind the Shell content.
    /// </summary>
    [Parameter] public new Color BackgroundColor { get; set; }

    /// <summary>
    /// Defines the color to shade text and icons that are disabled.
    /// </summary>
    [Parameter] public Color DisabledColor { get; set; }

    /// <summary>
    /// That defines the color to shade text and icons.
    /// </summary>
    [Parameter] public Color ForegroundColor { get; set; }

    /// <summary>
    /// Defines the color used for the title of the page.
    /// </summary>
    [Parameter] public Color TitleColor { get; set; }

    /// <summary>
    /// Defines the color used for unselected text and icons in the Shell chrome.
    /// </summary>
    [Parameter] public Color UnselectedColor { get; set; }

    /// <summary>
    /// Defines the title color for the tab bar.
    /// </summary>
    [Parameter] public Color TabBarTitleColor { get; set; }

    /// <summary>
    /// Defines the background color for the tab bar.
    /// </summary>
    [Parameter] public Color TabBarBackgroundColor { get; set; }

    /// <summary>
    /// Defines the disabled color for the tab bar.
    /// </summary>
    [Parameter] public Color TabBarDisabledColor { get; set; }

    /// <summary>
    /// Defines the foreground color for the tab bar.
    /// </summary>
    [Parameter] public Color TabBarForegroundColor { get; set; }

    /// <summary>
    /// Defines the unselected color for the tab bar.
    /// </summary>
    [Parameter] public Color TabBarUnselectedColor { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        switch (name)
        {
            case nameof(ChildContent):
                ChildContent = CastParameter<RenderFragment>(value, name);
                return true;

            case nameof(BackgroundColor):
                if (!Equals(BackgroundColor, value))
                {
                    BackgroundColor = CastParameter<Color>(value, name);
                    MC.Shell.SetBackgroundColor(NativeControl, BackgroundColor);
                }
                return true;

            case nameof(DisabledColor):
                if (!Equals(DisabledColor, value))
                {
                    DisabledColor = CastParameter<Color>(value, name);
                    MC.Shell.SetDisabledColor(NativeControl, DisabledColor);
                }
                return true;

            case nameof(ForegroundColor):
                if (!Equals(ForegroundColor, value))
                {
                    ForegroundColor = CastParameter<Color>(value, name);
                    MC.Shell.SetForegroundColor(NativeControl, ForegroundColor);
                }
                return true;

            case nameof(TitleColor):
                if (!Equals(TitleColor, value))
                {
                    TitleColor = CastParameter<Color>(value, name);
                    MC.Shell.SetTitleColor(NativeControl, TitleColor);
                }
                return true;

            case nameof(UnselectedColor):
                if (!Equals(UnselectedColor, value))
                {
                    UnselectedColor = CastParameter<Color>(value, name);
                    MC.Shell.SetUnselectedColor(NativeControl, UnselectedColor);
                }
                return true;

            case nameof(TabBarTitleColor):
                if (!Equals(TabBarTitleColor, value))
                {
                    TabBarTitleColor = CastParameter<Color>(value, name);
                    MC.Shell.SetTabBarTitleColor(NativeControl, TabBarTitleColor);
                }
                return true;

            case nameof(TabBarBackgroundColor):
                if (!Equals(TabBarBackgroundColor, value))
                {
                    TabBarBackgroundColor = CastParameter<Color>(value, name);
                    MC.Shell.SetTabBarBackgroundColor(NativeControl, TabBarBackgroundColor);
                }
                return true;

            case nameof(TabBarDisabledColor):
                if (!Equals(TabBarDisabledColor, value))
                {
                    TabBarDisabledColor = CastParameter<Color>(value, name);
                    MC.Shell.SetTabBarDisabledColor(NativeControl, TabBarDisabledColor);
                }
                return true;

            case nameof(TabBarForegroundColor):
                if (!Equals(TabBarForegroundColor, value))
                {
                    TabBarForegroundColor = CastParameter<Color>(value, name);
                    MC.Shell.SetTabBarForegroundColor(NativeControl, TabBarForegroundColor);
                }
                return true;

            case nameof(TabBarUnselectedColor):
                if (!Equals(TabBarUnselectedColor, value))
                {
                    TabBarUnselectedColor = CastParameter<Color>(value, name);
                    MC.Shell.SetTabBarUnselectedColor(NativeControl, TabBarUnselectedColor);
                }
                return true;

            default:
                return base.HandleAdditionalParameter(name, value);
        }
    }

    protected override RenderFragment GetChildContent() => ChildContent;

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        MC.ShellItem itemToAdd = GetItemToAdd(child);

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

    void IContainerElementHandler.RemoveChild(int physicalSiblingIndex)
    {
        NativeControl.Items.RemoveAt(physicalSiblingIndex);
    }

    void IContainerElementHandler.ReplaceChild(int physicalSiblingIndex, object newChild)
    {
        MC.ShellItem itemToAdd = GetItemToAdd(newChild);
        NativeControl.Items[physicalSiblingIndex] = itemToAdd;
    }

    private MC.ShellItem GetItemToAdd(object child)
    {
        ArgumentNullException.ThrowIfNull(child);

        return child switch
        {
            MC.TemplatedPage childAsTemplatedPage => childAsTemplatedPage, // Implicit conversion
            MC.ShellContent childAsShellContent => childAsShellContent, // Implicit conversion
            MC.ShellSection childAsShellSection => childAsShellSection, // Implicit conversion
            MC.MenuItem childAsMenuItem => childAsMenuItem, // Implicit conversion
            MC.ShellItem childAsShellItem => childAsShellItem,
            _ => throw new NotSupportedException($"Control of type '{GetType().FullName}' doesn't support adding a child (child type is '{child.GetType().FullName}').")
        };
    }
}