// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Diagnostics;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class ShellSection : ShellGroupItem, IContainerElementHandler
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
        var contentToAdd = GetContentToAdd(child);

        if (NativeControl.Items.Count >= physicalSiblingIndex)
        {
            NativeControl.Items.Insert(physicalSiblingIndex, contentToAdd);
        }
        else
        {
            Debug.WriteLine($"WARNING: {nameof(IContainerElementHandler.AddChild)} called with {nameof(physicalSiblingIndex)}={physicalSiblingIndex}, but NativeControl.Items.Count={NativeControl.Items.Count}");
            NativeControl.Items.Add(contentToAdd);
        }
    }

    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex)
    {
        ArgumentNullException.ThrowIfNull(child);

        MC.ShellContent contentToRemove = GetContentForChild(child)
            ?? throw new NotSupportedException($"Control of type '{GetType().FullName}' doesn't support removing a child (child type is '{child.GetType().FullName}').");

        NativeControl.Items.Remove(contentToRemove);
    }

    void IContainerElementHandler.ReplaceChild(int physicalSiblingIndex, object oldChild, object newChild)
    {
        var contentToAdd = GetContentToAdd(newChild);
        NativeControl.Items[physicalSiblingIndex] = contentToAdd;
    }

    private MC.ShellContent GetContentToAdd(object child)
    {
        ArgumentNullException.ThrowIfNull(child);

        MC.ShellContent contentToAdd = child switch
        {
            MC.TemplatedPage childAsTemplatedPage => childAsTemplatedPage,  // Implicit conversion
            MC.ShellContent childAsShellContent => childAsShellContent,
            _ => throw new NotSupportedException($"Control of type '{GetType().FullName}' doesn't support adding a child (child type is '{child.GetType().FullName}').")
        };
        return contentToAdd;
    }

    private MC.ShellContent GetContentForChild(object child)
    {
        return child switch
        {
            MC.TemplatedPage childAsTemplatedPage => GetContentForTemplatePage(childAsTemplatedPage),
            MC.ShellContent childAsShellContent => childAsShellContent,
            _ => null
        };
    }

    private MC.ShellContent GetContentForTemplatePage(MC.TemplatedPage childAsTemplatedPage)
    {
        return NativeControl.Items.FirstOrDefault(content => content.Content == childAsTemplatedPage);
    }
}
