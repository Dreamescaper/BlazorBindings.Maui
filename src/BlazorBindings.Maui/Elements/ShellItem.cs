// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Diagnostics;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class ShellItem : ShellGroupItem, IContainerElementHandler
{
    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override RenderFragment GetChildContent() => ChildContent;

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(ChildContent))
        {
            ChildContent = (RenderFragment)value;
            return true;
        }
        else
        {
            return base.HandleAdditionalParameter(name, value);
        }
    }

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        if (child is null)
        {
            throw new ArgumentNullException(nameof(child));
        }

        MC.ShellSection sectionToAdd = child switch
        {
            MC.TemplatedPage childAsTemplatedPage => childAsTemplatedPage,  // Implicit conversion
            MC.ShellContent childAsShellContent => childAsShellContent,  // Implicit conversion
            MC.ShellSection childAsShellSection => childAsShellSection,
            _ => throw new NotSupportedException($"Control of type '{GetType().FullName}' doesn't support adding a child (child type is '{child.GetType().FullName}').")
        };

        if (NativeControl.Items.Count >= physicalSiblingIndex)
        {
            NativeControl.Items.Insert(physicalSiblingIndex, sectionToAdd);
        }
        else
        {
            Debug.WriteLine($"WARNING: {nameof(IContainerElementHandler.AddChild)} called with {nameof(physicalSiblingIndex)}={physicalSiblingIndex}, but NativeControl.Items.Count={NativeControl.Items.Count}");
            NativeControl.Items.Add(sectionToAdd);
        }
    }

    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex)
    {
        if (child is null)
        {
            throw new ArgumentNullException(nameof(child));
        }

        var sectionToRemove = GetSectionForElement(child)
            ?? throw new NotSupportedException($"Control of type '{GetType().FullName}' doesn't support removing a child (child type is '{child.GetType().FullName}').");

        NativeControl.Items.Remove(sectionToRemove);
    }

    private MC.ShellSection GetSectionForElement(object child)
    {
        return child switch
        {
            MC.TemplatedPage childAsTemplatedPage => GetSectionForTemplatedPage(childAsTemplatedPage),
            MC.ShellContent childAsShellContent => GetSectionForContent(childAsShellContent),
            MC.ShellSection childAsShellSection => childAsShellSection,
            _ => null
        };
    }

    private MC.ShellSection GetSectionForContent(MC.ShellContent shellContent)
    {
        return NativeControl.Items.FirstOrDefault(section => section.Items.Contains(shellContent));
    }

    private MC.ShellSection GetSectionForTemplatedPage(MC.TemplatedPage templatedPage)
    {
        return NativeControl.Items
            .FirstOrDefault(section => section.Items.Any(contect => contect.Content == templatedPage));
    }
}
