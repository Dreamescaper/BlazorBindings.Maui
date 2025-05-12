using BlazorBindings.Maui.ComponentGenerator.Extensions;
using BlazorBindings.Maui.ComponentGenerator.Model;
using Microsoft.CodeAnalysis;
using System.Text;

namespace BlazorBindings.Maui.ComponentGenerator;

public record GeneratedTypeInfo(
    Compilation Compilation,
    GenerateComponentSettings Settings,
    string TypeName,
    string BaseTypeName,
    INamedTypeSymbol MauiType,
    INamedTypeSymbol MauiBaseType,
    IList<UsingStatement> Usings)
{
    private List<GeneratedPropertyInfo>? properties;
    public List<GeneratedPropertyInfo> Properties => properties ??= GetGeneratedProperties();

    public string GetTypeNameAndAddNamespace(string @namespace, string typeName)
    {
        var @using = Usings.FirstOrDefault(u => u.Namespace == @namespace);

        if (@using != null)
        {
            @using.IsUsed = true;
            return @using.Alias == null ? typeName : $"{@using.Alias}.{typeName}";
        }

        var partialAliasUsing = Usings.FirstOrDefault(u => u.Alias != null && @namespace.StartsWith(u.Namespace + "."));
        if (partialAliasUsing != null)
        {
            partialAliasUsing.IsUsed = true;
            var aliasedNs = @namespace.Replace(partialAliasUsing.Namespace, partialAliasUsing.Alias);
            return $"{aliasedNs}.{typeName}";
        }

        // Adding random usings might cause conflicts with global usings.
        // Therefore, we only add usings for commonly used namespaces.
        if (@using == null && (@namespace.StartsWith("System.") || @namespace.StartsWith("Microsoft.")))
        {
            @using = new UsingStatement { Namespace = @namespace, IsUsed = true };
            Usings.Add(@using);
            return typeName;
        }

        return $"global::{@namespace}.{typeName}";
    }

    public string GetTypeNameAndAddNamespace(ITypeSymbol type)
    {
        if (type is IArrayTypeSymbol arrayType)
        {
            return $"{GetTypeNameAndAddNamespace(arrayType.ElementType)}[]";
        }

        var typeName = type.GetCSharpTypeName();
        if (typeName != null)
        {
            return typeName;
        }

        if (type is not INamedTypeSymbol namedType)
            return type.Name;

        if (type.ContainingType != null)
        {
            return $"{GetTypeNameAndAddNamespace(type.ContainingType)}.{FormatTypeName(namedType)}";
        }

        var nsName = type.ContainingNamespace.GetFullName();

        return GetTypeNameAndAddNamespace(nsName, FormatTypeName(namedType));
    }

    private string FormatTypeName(INamedTypeSymbol namedType)
    {
        if (!namedType.IsGenericType)
        {
            return namedType.Name;
        }
        var typeNameBuilder = new StringBuilder();
        typeNameBuilder.Append(namedType.Name);
        typeNameBuilder.Append('<');
        var genericArgs = namedType.TypeArguments;
        for (var i = 0; i < genericArgs.Length; i++)
        {
            if (i > 0)
            {
                typeNameBuilder.Append(", ");
            }
            typeNameBuilder.Append(GetTypeNameAndAddNamespace(genericArgs[i]));

        }
        typeNameBuilder.Append('>');
        return typeNameBuilder.ToString();
    }

    private List<GeneratedPropertyInfo> GetGeneratedProperties()
    {
        var properties = new List<GeneratedPropertyInfo>();

        RenderFragmentPropertyInfo.AddContentProperties(properties, this);
        ValuePropertyInfo.AddValueProperties(properties, this);
        EventCallbackPropertyInfo.AddEventCallbackProperties(properties, this);

        properties = properties.Where(p => p is ValuePropertyInfo).OrderBy(p => p.ComponentPropertyName)
            .Concat(properties.OfType<RenderFragmentPropertyInfo>())
            .Concat(properties.OfType<EventCallbackPropertyInfo>())
            .ToList();

        return properties;
    }
}
