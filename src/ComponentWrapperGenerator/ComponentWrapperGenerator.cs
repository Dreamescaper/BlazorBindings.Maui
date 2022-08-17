﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace ComponentWrapperGenerator
{
#pragma warning disable CA1724 // Type name conflicts with namespace name
    public partial class ComponentWrapperGenerator
#pragma warning restore CA1724 // Type name conflicts with namespace name
    {
        private GeneratorSettings Settings { get; }
        private IList<XmlDocument> XmlDocs { get; }
        private IList<string> ElementNamespaces { get; }

        public ComponentWrapperGenerator(GeneratorSettings settings, IList<XmlDocument> xmlDocs, IList<string> namespaces)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            XmlDocs = xmlDocs ?? throw new ArgumentNullException(nameof(xmlDocs));
            ElementNamespaces = namespaces ?? throw new ArgumentNullException(nameof(namespaces));
        }

        public void GenerateComponentWrapper(Type typeToGenerate, string outputFolder)
        {
            typeToGenerate = typeToGenerate ?? throw new ArgumentNullException(nameof(typeToGenerate));

            GenerateComponentFile(typeToGenerate, outputFolder);
        }

        private void GenerateComponentFile(Type typeToGenerate, string outputFolder)
        {
            var subPath = GetSubPath(typeToGenerate);
            var fileName = Path.Combine(outputFolder, subPath, $"{typeToGenerate.Name}.generated.cs");
            var directoryName = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            Console.WriteLine($"Generating component for type '{typeToGenerate.FullName}' into file '{fileName}'.");

            var componentName = typeToGenerate.Name;
            var componentHandlerName = $"{componentName}Handler";

            var baseType = GetBaseTypeOfInterest(typeToGenerate);
            var componentBaseName = GetComponentNamespace(typeToGenerate) == GetComponentNamespace(baseType)
                ? baseType.Name
                : $"{GetComponentNamespace(baseType)}.{baseType.Name}";

            var componentNamespace = GetComponentNamespace(typeToGenerate);

            // header
            var headerText = Settings.FileHeader;

            // usings
            var usings = new List<UsingStatement>
            {
                new UsingStatement { Namespace = "Microsoft.AspNetCore.Components", IsUsed = true, },
                new UsingStatement { Namespace = "BlazorBindings.Core", IsUsed = true, },
                new UsingStatement { Namespace = "System.Threading.Tasks", IsUsed = true, },
                new UsingStatement { Namespace = "Microsoft.Maui.Controls", Alias = "MC", IsUsed = true },
                new UsingStatement { Namespace = "Microsoft.Maui.Controls.Compatibility", Alias = "MCC" },
                new UsingStatement { Namespace = "Microsoft.Maui.Controls.Shapes", Alias = "MCS" },
                //new UsingStatement { Namespace = "Xamarin.Forms.DualScreen", Alias = "XFD" },
            };

            if (componentNamespace != Settings.RootNamespace)
            {
                usings.Add(new UsingStatement { Namespace = Settings.RootNamespace, IsUsed = true });
            }

            var componentNamespacePrefix = GetNamespacePrefix(typeToGenerate, usings);

            // props
            var valueProperties = GeneratedPropertyInfo.GetValueProperties(typeToGenerate, usings);
            var contentProperties = GeneratedPropertyInfo.GetContentProperties(typeToGenerate, usings);
            var eventCallbackProperties = GeneratedPropertyInfo.GetEventCallbackProperties(typeToGenerate, usings);
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

            var isComponentAbstract = typeToGenerate.IsAbstract || typeToGenerate.GetConstructor(Array.Empty<Type>()) is null;
            var classModifiers = string.Empty;
            if (isComponentAbstract)
            {
                classModifiers += "abstract ";
            }

            var staticConstructorBody = "";
            foreach (var prop in contentProperties)
            {
                staticConstructorBody += prop.GetContentHandlerRegistration();
            }
            staticConstructorBody += "\r\n            RegisterAdditionalHandlers();";

            var createNativeElement = isComponentAbstract ? "" : $@"
        protected override MC.Element CreateNativeElement() => new {componentNamespacePrefix}{componentName}();";

            var handleParameter = !allProperties.Any() ? "" : $@"
        protected override void HandleParameter(string name, object value)
        {{
            switch (name)
            {{
                {handleProperties.Trim()}

                default:
                    base.HandleParameter(name, value);
                    break;
            }}
        }}";

            var renderAdditionalElementContent = !contentProperties.Any() ? "" : $@"

        protected override void RenderAdditionalElementContent({GetTypeNameAndAddNamespace("Microsoft.AspNetCore.Components.Rendering", "RenderTreeBuilder", usings)} builder, ref int sequence)
        {{
            base.RenderAdditionalElementContent(builder, ref sequence);{string.Concat(contentProperties.Select(prop => prop.RenderContentProperty()))};
        }}";


            var usingsText = string.Join(
                Environment.NewLine,
                usings
                    .Distinct()
                    .Where(u => u.Namespace != componentNamespace)
                    .Where(u => u.IsUsed)
                    .OrderBy(u => u.ComparableString)
                    .Select(u => u.UsingText));

            var outputBuilder = new StringBuilder();
            outputBuilder.Append($@"{headerText}
{usingsText}

namespace {componentNamespace}
{{
    public {classModifiers}partial class {componentName} : {componentBaseName}
    {{
        static {componentName}()
        {{
            {staticConstructorBody.Trim()}
        }}
{propertyDeclarations}
        public new {componentNamespacePrefix}{componentName} NativeControl => ({componentNamespacePrefix}{componentName})((Element)this).NativeControl;
{createNativeElement}
{handleParameter}{renderAdditionalElementContent}

        static partial void RegisterAdditionalHandlers();
    }}
}}
");

            File.WriteAllText(fileName, outputBuilder.ToString());
        }

        private static string GetNamespacePrefix(Type type, List<UsingStatement> usings)
        {
            // Check if there's a 'using' already. If so, check if it has an alias. If not, add a new 'using'.
            var namespaceAlias = string.Empty;

            var existingUsing = usings.FirstOrDefault(u => u.Namespace == type.Namespace);
            if (existingUsing == null)
            {
                usings.Add(new UsingStatement { Namespace = type.Namespace, IsUsed = true, });
                return string.Empty;
            }
            else
            {
                existingUsing.IsUsed = true;
                if (existingUsing.Alias != null)
                {
                    return existingUsing.Alias + ".";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private static string GetXmlDocText(XmlElement xmlDocElement)
        {
            var allText = xmlDocElement?.InnerXml;
            allText = allText.Replace("To be added.", string.Empty, StringComparison.Ordinal);
            if (string.IsNullOrWhiteSpace(allText))
            {
                return null;
            }
            return allText;
        }

        private string GetXmlDocContents(PropertyInfo prop, string indent)
        {
            foreach (var xmlDoc in XmlDocs)
            {

                var xmlDocContents = string.Empty;
                // Format of XML docs we're looking for in a given property:
                // <member name="P:Xamarin.Forms.ActivityIndicator.Color">
                //     <summary>Gets or sets the <see cref="T:Xamarin.Forms.Color" /> of the ActivityIndicator. This is a bindable property.</summary>
                //     <value>A <see cref="T:Xamarin.Forms.Color" /> used to display the ActivityIndicator. Default is <see cref="P:Xamarin.Forms.Color.Default" />.</value>
                //     <remarks />
                // </member>
                var xmlDocNodeName = $"P:{prop.DeclaringType.Namespace}.{prop.DeclaringType.Name}.{prop.Name}";
                var xmlDocNode = xmlDoc.SelectSingleNode($"//member[@name='{xmlDocNodeName}']");
                if (xmlDocNode != null)
                {
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
                }
            }

            return null;
        }

        internal static string GetTypeNameAndAddNamespace(string @namespace, string typeName, IList<UsingStatement> usings)
        {
            var @using = usings.FirstOrDefault(u => u.Namespace == @namespace);
            if (@using == null)
            {
                @using = new UsingStatement { Namespace = @namespace, IsUsed = true };
                usings.Add(@using);
            }
            else
            {
                @using.IsUsed = true;
            }

            return @using.Alias == null ? typeName : $"{@using.Alias}.{typeName}";
        }

        internal static string GetTypeNameAndAddNamespace(Type type, IList<UsingStatement> usings)
        {
            var typeName = GetCSharpType(type);
            if (typeName != null)
            {
                return typeName;
            }

            // Check if there's a 'using' already. If so, check if it has an alias. If not, add a new 'using'.
            var namespaceAlias = string.Empty;

            var existingUsing = usings.FirstOrDefault(u => u.Namespace == type.Namespace);
            if (existingUsing == null)
            {
                usings.Add(new UsingStatement { Namespace = type.Namespace, IsUsed = true, });
            }
            else
            {
                existingUsing.IsUsed = true;
                if (existingUsing.Alias != null)
                {
                    namespaceAlias = existingUsing.Alias + ".";
                }
            }
            typeName = namespaceAlias + FormatTypeName(type, usings);
            return typeName;
        }

        private static string FormatTypeName(Type type, IList<UsingStatement> usings)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }
            var typeNameBuilder = new StringBuilder();
            typeNameBuilder.Append(type.Name.Substring(0, type.Name.IndexOf('`', StringComparison.Ordinal)));
            typeNameBuilder.Append('<');
            var genericArgs = type.GetGenericArguments();
            for (var i = 0; i < genericArgs.Length; i++)
            {
                if (i > 0)
                {
                    typeNameBuilder.Append(", ");
                }
                typeNameBuilder.Append(GetTypeNameAndAddNamespace(genericArgs[i], usings));

            }
            typeNameBuilder.Append('>');
            return typeNameBuilder.ToString();
        }


        private static readonly Dictionary<Type, string> TypeToCSharpName = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(object), "object" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(string), "string" },
        };

        private static string GetCSharpType(Type propertyType)
        {
            return TypeToCSharpName.TryGetValue(propertyType, out var typeName) ? typeName : null;
        }

        /// <summary>
        /// Finds the next non-generic base type of the specified type. This matches the Mobile Blazor Bindings
        /// model where there is no need to represent the intermediate generic base classes because they are
        /// generally only containers and have no API functionality that needs to be generated.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type GetBaseTypeOfInterest(Type type)
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

        private static readonly List<string> ReservedKeywords = new List<string>
            { "class", };

        private string GetNamespacePart(Type typeToGenerate)
        {
            var rootNamespace = ElementNamespaces.First(n => typeToGenerate.Namespace.StartsWith(n, StringComparison.Ordinal));

            var remainingNamespacePart = typeToGenerate.Namespace == rootNamespace
                ? ""
                : typeToGenerate.Namespace[(rootNamespace.Length + 1)..];

            return remainingNamespacePart;
        }

        private string GetComponentNamespace(Type typeToGenerate)
        {
            var namespacePart = GetNamespacePart(typeToGenerate);
            var componentNamespace = string.IsNullOrEmpty(namespacePart)
                ? Settings.RootNamespace
                : $"{Settings.RootNamespace}.{namespacePart}";

            return componentNamespace;
        }

        private string GetSubPath(Type typeToGenerate)
        {
            return GetNamespacePart(typeToGenerate).Replace('.', Path.DirectorySeparatorChar);
        }
    }
}
