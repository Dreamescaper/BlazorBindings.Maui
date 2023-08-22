// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Handlers;

/// <remarks>Experimental API, subject to change.</remarks>
[RequiresPreviewFeatures]
public abstract class BaseAttachedPropertiesHandler : IElementHandler, INonPhysicalChild
{
    /// <summary>
    /// The target of the attached property. This will be set to the parent of the attached property container.
    /// </summary>
    protected MC.BindableObject Target { get; private set; }

    public abstract void ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName);
    public abstract void RemoveFromParent();

    void INonPhysicalChild.SetParent(object parentElement)
    {
        Target = (MC.BindableObject)parentElement;
    }

    void INonPhysicalChild.RemoveFromParent(object parentElement)
    {
        RemoveFromParent();
    }

    // Because this is a 'fake' element, all matters related to physical trees
    // should be no-ops.
    public object TargetElement => null;
}
