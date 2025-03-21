﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace BlazorBindings.Maui.ComponentGenerator;

public partial class ComponentWrapperGenerator
{
    const string MauiComponentsNamespace = "BlazorBindings.Maui.Elements";

    public (string GroupName, string Name, string Source) GenerateComponentFile(Compilation compilation, GenerateComponentSettings generatedInfo)
    {
        //if (!System.Diagnostics.Debugger.IsAttached)
        //{
        //    System.Diagnostics.Debugger.Launch();
        //}

        var typeToGenerate = generatedInfo.TypeSymbol;
        var componentName = generatedInfo.TypeAlias ?? typeToGenerate.Name;
        var componentNamespace = GetComponentNamespace(typeToGenerate);

        var baseType = GetBaseTypeOfInterest(typeToGenerate);
        var componentBaseName = generatedInfo.BaseTypeInfo?.TypeAlias ?? baseType.Name;

        if (componentNamespace != GetComponentNamespace(baseType))
            componentBaseName = $"{GetComponentNamespace(baseType)}.{componentBaseName}";

        // header
        var headerText = generatedInfo.FileHeader;

        // usings
        var usings = GetDefaultUsings(typeToGenerate, componentNamespace);
        var generatedType = new GeneratedTypeInfo(compilation, generatedInfo, componentName, componentBaseName, typeToGenerate, baseType, usings);

        var mauiTypeName = generatedType.GetTypeNameAndAddNamespace(typeToGenerate);

        // props
        var valueProperties = GeneratedPropertyInfo.GetValueProperties(generatedType);
        var contentProperties = GeneratedPropertyInfo.GetContentProperties(generatedType);
        var eventCallbackProperties = GeneratedPropertyInfo.GetEventCallbackProperties(generatedType);
        var allProperties = valueProperties.Concat(contentProperties).Concat(eventCallbackProperties);
        var propertyDeclarationBuilder = new StringBuilder();
        if (allProperties.Any())
        {
            propertyDeclarationBuilder.AppendLine();
        }
        foreach (var prop in allProperties)
        {
            propertyDeclarationBuilder.Append(prop.GetPropertyDeclaration());
        }
        var propertyDeclarations = propertyDeclarationBuilder.ToString();

        var handlePropertiesBuilder = new StringBuilder();
        foreach (var prop in valueProperties)
        {
            handlePropertiesBuilder.Append(prop.GetHandleValueProperty());
        }
        foreach (var prop in contentProperties)
        {
            handlePropertiesBuilder.Append(prop.GetHandleContentProperty());
        }
        foreach (var prop in eventCallbackProperties)
        {
            handlePropertiesBuilder.Append(prop.GetHandleEventCallbackProperty());
        }
        var handleProperties = handlePropertiesBuilder.ToString();

        var isComponentAbstract = typeToGenerate.IsAbstract || !typeToGenerate.Constructors.Any(c => c.DeclaredAccessibility == Accessibility.Public && c.Parameters.Length == 0);
        var classModifiers = string.Empty;
        if (isComponentAbstract)
        {
            classModifiers += "abstract ";
        }

        var staticConstructorBody = "\r\n            RegisterAdditionalHandlers();";

        var createNativeElement = isComponentAbstract ? "" : $@"
        protected override {generatedType.GetTypeNameAndAddNamespace(typeToGenerate)} CreateNativeElement() => new();";

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

        var renderAdditionalElementContent = contentProperties.Length == 0 ? "" : $$"""


                    protected override void RenderAdditionalElementContent({{generatedType.GetTypeNameAndAddNamespace("Microsoft.AspNetCore.Components.Rendering", "RenderTreeBuilder")}} builder, ref int sequence)
                    {
                        base.RenderAdditionalElementContent(builder, ref sequence);{{string.Concat(contentProperties.Select(prop => prop.RenderContentProperty()))}}
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

        var genericModifier = generatedInfo.IsGeneric ? "<T>" : "";
        var baseGenericModifier = generatedInfo.IsBaseTypeGeneric ? "<T>" : "";

        var xmlDoc = GetXmlDocContents(typeToGenerate, "    ");

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

        return (GetComponentGroup(typeToGenerate), componentName, content);
    }

    private static List<UsingStatement> GetDefaultUsings(INamedTypeSymbol typeToGenerate, string componentNamespace)
    {
        var usings = new List<UsingStatement>
        {
            new() { Namespace = "System" },
            new() { Namespace = "Microsoft.AspNetCore.Components", IsUsed = true, },
            new() { Namespace = "BlazorBindings.Core", IsUsed = true, },
            new() { Namespace = "System.Threading.Tasks", IsUsed = true, },
            new() { Namespace = "Microsoft.Maui.Controls", Alias = "MC", IsUsed = true },
            new() { Namespace = "Microsoft.Maui.Primitives", Alias = "MMP" }
        };

        var existingAliases = usings
            .Where(u => !string.IsNullOrEmpty(u.Alias))
            .Select(u => u.Alias)
            .ToHashSet();

        var typeNamespace = typeToGenerate.ContainingNamespace.GetFullName();
        if (typeNamespace != "Microsoft.Maui.Controls")
        {
            var typeNamespaceAlias = GetUniqueNamespaceAlias(typeNamespace, existingAliases);
            usings.Add(new UsingStatement { Namespace = typeNamespace, Alias = typeNamespaceAlias, IsUsed = true });
        }

        if (componentNamespace != MauiComponentsNamespace)
        {
            usings.Add(new UsingStatement { Namespace = MauiComponentsNamespace, IsUsed = true });
        }

        var assemblyName = typeToGenerate.ContainingAssembly.Name;
        if (assemblyName.Contains('.') && typeNamespace != assemblyName && typeNamespace.StartsWith(assemblyName))
        {
            usings.Add(new UsingStatement { Namespace = assemblyName, Alias = GetUniqueNamespaceAlias(assemblyName, existingAliases) });
        }

        return usings;
    }

    internal static string GetXmlDocContents(ISymbol symbol, string indent)
    {
        var xmlDocString = symbol.GetDocumentationCommentXml();

        if (string.IsNullOrEmpty(xmlDocString))
        {
            return null;
        }

        var xmlDoc = new XmlDocument();

        // Try parse raw doc string (should have root element)
        try
        {
            xmlDoc.LoadXml(xmlDocString);
        }
        catch
        {
            // Fallback to old method if not.
            xmlDoc.LoadXml($"<member>{xmlDocString}</member>");
        }

        var xmlDocNode = xmlDoc.FirstChild;

        var xmlDocContents = string.Empty;
        // Format of XML docs we're looking for in a given property:
        // <member name="P:Xamarin.Forms.ActivityIndicator.Color">
        //     <summary>Gets or sets the <see cref="T:Xamarin.Forms.Color" /> of the ActivityIndicator. This is a bindable property.</summary>
        //     <value>A <see cref="T:Xamarin.Forms.Color" /> used to display the ActivityIndicator. Default is <see cref="P:Xamarin.Forms.Color.Default" />.</value>
        //     <remarks />
        // </member>

        var summaryText = GetXmlDocText(xmlDocNode["summary"]);
        var valueText = GetXmlDocText(xmlDocNode["value"]);

        if (summaryText != null || valueText != null)
        {
            var xmlDocContentBuilder = new StringBuilder();
            if (summaryText != null)
            {
                xmlDocContentBuilder.AppendLine($"{indent}/// <summary>");
                xmlDocContentBuilder.AppendLine($"{indent}/// {summaryText}");
                xmlDocContentBuilder.AppendLine($"{indent}/// </summary>");
            }
            if (valueText != null)
            {
                xmlDocContentBuilder.AppendLine($"{indent}/// <value>");
                xmlDocContentBuilder.AppendLine($"{indent}/// {valueText}");
                xmlDocContentBuilder.AppendLine($"{indent}/// </value>");
            }
            xmlDocContents = xmlDocContentBuilder.ToString();
        }
        return xmlDocContents;

        static string GetXmlDocText(XmlElement xmlDocElement)
        {
            var allText = xmlDocElement?.InnerXml;
            allText = allText?.Replace("To be added.", "").Replace("This is a bindable property.", "");
            allText = allText is null ? null : MultipleSpacesRegex().Replace(allText, " ");
            return string.IsNullOrWhiteSpace(allText) ? null : allText.Trim();
        }
    }

    /// <summary>
    /// Finds the next non-generic base type of the specified type. This matches the Mobile Blazor Bindings
    /// model where there is no need to represent the intermediate generic base classes because they are
    /// generally only containers and have no API functionality that needs to be generated.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static INamedTypeSymbol GetBaseTypeOfInterest(INamedTypeSymbol type)
    {
        do
        {
            type = type.BaseType;
            if (!type.IsGenericType)
            {
                return type;
            }
        }
        while (type != null);

        return null;
    }

    public static string GetIdentifierName(string possibleIdentifier)
    {
        return ReservedKeywords.Contains(possibleIdentifier, StringComparer.Ordinal)
            ? $"@{possibleIdentifier}"
            : possibleIdentifier;
    }

    private static readonly List<string> ReservedKeywords = ["class"];

    private static string GetComponentGroup(INamedTypeSymbol typeToGenerate)
    {
        var nsName = typeToGenerate.ContainingNamespace.GetFullName();
        var parts = nsName.Split('.')
            .Except(["Maui", "Controls", "Views", "UI", "Microsoft"], StringComparer.OrdinalIgnoreCase);

        return string.Join('.', parts);
    }

    private static string GetComponentNamespace(INamedTypeSymbol typeToGenerate)
    {
        var group = GetComponentGroup(typeToGenerate);
        return string.IsNullOrEmpty(group) ? "BlazorBindings.Maui.Elements" : $"BlazorBindings.Maui.Elements.{group}";
    }

    private static string GetNamespaceAlias(string @namespace)
    {
        return new string(@namespace.Split('.').Where(part => part != "Microsoft").Select(part => part[0]).ToArray());
    }

    private static string GetUniqueNamespaceAlias(string @namespace, HashSet<string> existingAliases)
    {
        var baseAlias = GetNamespaceAlias(@namespace);
        var uniqueAlias = baseAlias;
        var counter = 1;

        while (existingAliases.Contains(uniqueAlias))
        {
            uniqueAlias = baseAlias + counter;
            counter++;
        }

        existingAliases.Add(uniqueAlias);
        return uniqueAlias;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MultipleSpacesRegex();
}
