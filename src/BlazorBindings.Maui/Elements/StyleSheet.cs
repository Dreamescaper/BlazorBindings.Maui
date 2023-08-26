// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.Extensions;
using System.IO;
using System.Reflection;
using MC = Microsoft.Maui.Controls;
using MCS = Microsoft.Maui.Controls.StyleSheets;

namespace BlazorBindings.Maui.Elements;

public class StyleSheet : NativeControlComponentBase, IElementHandler, INonPhysicalChild
{
    private MC.VisualElement _parentVisualElement;

    [Parameter] public Assembly Assembly { get; set; }
    [Parameter] public string Resource { get; set; }
    [Parameter] public string Text { get; set; }

    // TODO: Consider adding properties for using the full set of StyleSheet factories:
    // - OBSOLETE: public static StyleSheet FromAssemblyResource(Assembly assembly, string resourceId, IXmlLineInfo lineInfo = null);
    // - public static StyleSheet FromResource(string resourcePath, Assembly assembly, IXmlLineInfo lineInfo = null);
    // - public static StyleSheet FromString(string stylesheet);
    // - public static StyleSheet FromReader(TextReader reader);

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        UpdateParentStyleSheetIfPossible();
    }

    private void UpdateParentStyleSheetIfPossible()
    {
        if (_parentVisualElement != null)
        {
            // TODO: Add logic to ensure same resource isn't added multiple times
            if (Resource != null)
            {
                if (Assembly == null)
                {
                    throw new InvalidOperationException($"Specifying a '{nameof(Resource)}' property value '{Resource}' requires also specifying the '{nameof(Assembly)}' property to indicate the assembly containing the resource.");
                }
                var styleSheet = MCS.StyleSheet.FromResource(resourcePath: Resource, assembly: Assembly);
                _parentVisualElement.Resources.Add(styleSheet);
            }
            if (Text != null)
            {
                using var reader = new StringReader(Text);
                var styleSheet = MCS.StyleSheet.FromReader(reader);
                _parentVisualElement.Resources.Add(styleSheet);
            }
        }
    }

    void INonPhysicalChild.SetParent(object parentElement)
    {
        _parentVisualElement = parentElement.Cast<MC.VisualElement>();
        UpdateParentStyleSheetIfPossible();
    }

    void INonPhysicalChild.RemoveFromParent(object parentElement)
    {
        throw new InvalidOperationException("Removing StyleSheet element is not supported.");
    }

    object IElementHandler.TargetElement => null;
}
