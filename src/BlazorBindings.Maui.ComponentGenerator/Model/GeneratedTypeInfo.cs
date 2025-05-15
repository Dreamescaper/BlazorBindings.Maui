using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace BlazorBindings.Maui.ComponentGenerator.Model;

public record GeneratedTypeInfo(
    Compilation Compilation,
    GenerateComponentSettings Settings,
    string ComponentGroup,
    string Namespace,
    string TypeName,
    string BaseTypeName,
    INamedTypeSymbol MauiType,
    GeneratedTypeInfo? GeneratedBaseType,
    INamedTypeSymbol? BaseTypeSymbol,
    INamedTypeSymbol MauiBaseType,
    IList<UsingStatement> Usings)
{
    const string MauiComponentsNamespace = "BlazorBindings.Maui.Elements";

    private List<GeneratedPropertyInfo>? properties;
    public List<GeneratedPropertyInfo> Properties => properties ??= GetGeneratedProperties();

    public bool IsAbstract => MauiType.IsAbstract || !MauiType.Constructors.Any(c => c.DeclaredAccessibility == Accessibility.Public && c.Parameters.Length == 0);

    public bool IsGeneric => Settings.IsGeneric || Settings.GenericProperties.Any(p => p.Value == null) || IsBaseTypeGeneric;
    public bool IsBaseTypeGeneric => GeneratedBaseType?.IsGeneric ?? false;

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

    public bool BaseTypesHaveProperty(string propertyName)
    {
        if (BaseTypeSymbol != null)
        {
            return BaseTypeSymbol.GetProperty(propertyName, includeBaseTypes: true) != null;
        }

        if (GeneratedBaseType != null)
        {
            if (GeneratedBaseType.Properties.Any(p => p.ComponentPropertyName == propertyName))
                return true;

            return GeneratedBaseType.BaseTypesHaveProperty(propertyName);
        }

        return false;
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

    public static GeneratedTypeInfo Create(Compilation compilation, GenerateComponentSettings generatedInfo, List<GeneratedTypeInfo> generatedTypes)
    {
        var typeToGenerate = generatedInfo.TypeSymbol;

        var componentGroup = GetComponentGroup(typeToGenerate);
        var componentName = generatedInfo.TypeAlias ?? typeToGenerate.Name;
        var componentNamespace = GetComponentNamespace(typeToGenerate);

        var mauiBaseType = GetBaseTypeOfInterest(typeToGenerate);
        var generatedBaseType = generatedTypes.FirstOrDefault(t => SymbolEqualityComparer.Default.Equals(t.MauiType, mauiBaseType));
        var componentBaseName = generatedBaseType?.TypeName ?? mauiBaseType.Name;
        var baseTypeNamespace = GetComponentNamespace(mauiBaseType);

        var baseTypeSymbol = generatedBaseType != null
            ? null
            : compilation.GetTypeByMetadataName($"{baseTypeNamespace}.{componentBaseName}");

        // If baseTypeSymbol is from generated file from current assembly - that means that
        // it was generated previously by this tool, and should be ignored.
        if (baseTypeSymbol != null
            && SymbolEqualityComparer.Default.Equals(baseTypeSymbol.ContainingAssembly, compilation.Assembly)
            && baseTypeSymbol.Locations.Any(l => l.SourceTree?.FilePath.EndsWith("generated.cs") == true))
        {
            baseTypeSymbol = null;
        }

        // Generate base type if not included explicitly
        if (generatedBaseType is null && baseTypeSymbol is null)
        {
            var baseGeneratedInfo = new GenerateComponentSettings
            {
                FileHeader = generatedInfo.FileHeader,
                TypeSymbol = mauiBaseType
            };
            generatedBaseType = Create(compilation, baseGeneratedInfo, generatedTypes);

            generatedTypes.Add(generatedBaseType);
        }

        if (componentNamespace != baseTypeNamespace)
            componentBaseName = $"{baseTypeNamespace}.{componentBaseName}";

        var usings = GetDefaultUsings(typeToGenerate, componentNamespace);

        return new GeneratedTypeInfo(
            compilation,
            generatedInfo,
            componentGroup,
            componentNamespace,
            componentName,
            componentBaseName,
            typeToGenerate,
            generatedBaseType,
            baseTypeSymbol,
            mauiBaseType,
            usings);
    }

    private static string GetComponentNamespace(INamedTypeSymbol typeToGenerate)
    {
        var group = GetComponentGroup(typeToGenerate);
        return string.IsNullOrEmpty(group) ? "BlazorBindings.Maui.Elements" : $"BlazorBindings.Maui.Elements.{group}";
    }

    private static string GetComponentGroup(INamedTypeSymbol typeToGenerate)
    {
        var nsName = typeToGenerate.ContainingNamespace.GetFullName();
        var parts = nsName.Split('.')
            .Except(["Maui", "Controls", "Views", "UI", "Microsoft"], StringComparer.OrdinalIgnoreCase);

        return string.Join('.', parts);
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
            .Select(u => u.Alias!)
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
}
