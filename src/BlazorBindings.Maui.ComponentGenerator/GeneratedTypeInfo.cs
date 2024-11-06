using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace BlazorBindings.Maui.ComponentGenerator;

public class GeneratedTypeInfo
{
    public GeneratedTypeInfo(Compilation compilation, GenerateComponentSettings settings, string typeName, string baseTypeName, ITypeSymbol mauiType, IList<UsingStatement> usings)
    {
        TypeName = typeName;
        BaseTypeName = baseTypeName;
        IsGeneric = settings.IsGeneric;
        MauiType = mauiType;
        Usings = usings;
        Compilation = compilation;
        Settings = settings;
    }

    public string TypeName { get; }
    public string BaseTypeName { get; }
    public bool IsGeneric { get; }
    public Compilation Compilation { get; }
    public GenerateComponentSettings Settings { get; }
    public ITypeSymbol MauiType { get; }
    public IList<UsingStatement> Usings { get; }

    public string GetTypeNameAndAddNamespace(string @namespace, string typeName)
    {
        // Adding random usings might cause conflicts with global usings.
        // Therefore, we only add usings for commonly used namespaces.

        var @using = Usings.FirstOrDefault(u => u.Namespace == @namespace);
        if (@using == null && (@namespace.StartsWith("System.") || @namespace.StartsWith("Microsoft.")))
        {
            @using = new UsingStatement { Namespace = @namespace, IsUsed = true };
            Usings.Add(@using);
        }

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

        return $"global::{@namespace}.{typeName}";
    }

    public string GetTypeNameAndAddNamespace(ITypeSymbol type)
    {
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
}
