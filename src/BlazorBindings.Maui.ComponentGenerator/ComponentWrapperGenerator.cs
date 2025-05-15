// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.ComponentGenerator.Extensions;
using BlazorBindings.Maui.ComponentGenerator.Model;
using BlazorBindings.Maui.ComponentGenerator2.Extensions;
using Microsoft.CodeAnalysis;

namespace BlazorBindings.Maui.ComponentGenerator;

public partial class ComponentWrapperGenerator
{
    public string GenerateComponentFileSource(GeneratedTypeInfo generatedType)
    {
        //if (!System.Diagnostics.Debugger.IsAttached)
        //{
        //    System.Diagnostics.Debugger.Launch();
        //}

        var headerText = generatedType.Settings.FileHeader;
        var usings = generatedType.Usings;
        var mauiTypeName = generatedType.GetTypeNameAndAddNamespace(generatedType.MauiType);
        var componentName = generatedType.TypeName;
        var componentNamespace = generatedType.Namespace;
        var componentBaseName = generatedType.BaseTypeName;

        // props
        var allProperties = generatedType.Properties;

        var propertyDeclarations = string.Concat(allProperties.Select(p => p.GetPropertyDeclaration()));
        if (propertyDeclarations.Length > 0)
            propertyDeclarations = Environment.NewLine + propertyDeclarations;

        var handleProperties = string.Concat(allProperties.Select(p => p.GetHandlePropertyCase()));

        var classModifiers = string.Empty;
        if (generatedType.IsAbstract)
        {
            classModifiers += "abstract ";
        }

        var staticConstructorBody = "\r\n            RegisterAdditionalHandlers();";

        var createNativeElement = generatedType.IsAbstract ? "" : $@"
        protected override {mauiTypeName} CreateNativeElement() => new();";

        var handleParameter = !allProperties.Any() ? "" : $$"""

                    protected override void HandleParameter(string name, object value)
                    {
                        switch (name)
                        {
                            {{handleProperties.Trim()}}

                            default:
                                base.HandleParameter(name, value);
                                break;
                        }
                    }
            """;

        var additionalContents = string.Concat(allProperties.Select(prop => prop.AdditionalContentForProperty()));
        var renderAdditionalElementContent = additionalContents.Length == 0 ? "" : $$"""


                    protected override void RenderAdditionalElementContent({{generatedType.GetTypeNameAndAddNamespace("Microsoft.AspNetCore.Components.Rendering", "RenderTreeBuilder")}} builder, ref int sequence)
                    {
                        base.RenderAdditionalElementContent(builder, ref sequence);{{additionalContents}}
                    }
            """;


        var usingsText = string.Join(
            Environment.NewLine,
            usings
                .Distinct()
                .Where(u => u.Namespace != componentNamespace)
                .Where(u => u.IsUsed)
                .OrderBy(u => u.ComparableString)
                .Select(u => u.UsingText));

        var genericModifier = generatedType.IsGeneric ? "<T>" : "";
        var baseGenericModifier = generatedType.IsBaseTypeGeneric ? "<T>" : "";

        var xmlDoc = generatedType.MauiType.GetXmlDocContents()?.Indent("    ");

        var content = $$"""
            {{headerText}}
            {{usingsText}}

            #pragma warning disable MBB001

            namespace {{componentNamespace}}
            {
            {{xmlDoc}}    public {{classModifiers}}partial class {{componentName}}{{genericModifier}} : {{componentBaseName}}{{baseGenericModifier}}
                {
                    static {{componentName}}()
                    {
                        {{staticConstructorBody.Trim()}}
                    }
            {{propertyDeclarations}}
                    public new {{mauiTypeName}} NativeControl => ({{mauiTypeName}})((BindableObject)this).NativeControl;
            {{createNativeElement}}
            {{handleParameter}}{{renderAdditionalElementContent}}

                    static partial void RegisterAdditionalHandlers();
                }
            }

            """;

        return content;
    }
}
