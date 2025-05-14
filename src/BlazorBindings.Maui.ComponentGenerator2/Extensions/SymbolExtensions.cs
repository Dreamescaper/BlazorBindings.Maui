using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.ComponentModel;

namespace BlazorBindings.Maui.ComponentGenerator.Extensions;

internal static class SymbolExtensions
{
    public static IMethodSymbol? GetMethod(this ITypeSymbol typeSymbol, string method)
    {
        return typeSymbol.GetMembers(method).FirstOrDefault() as IMethodSymbol;
    }

    public static IEnumerable<INamedTypeSymbol> GetBaseTypes(this ITypeSymbol typeSymbol)
    {
        var baseType = typeSymbol.BaseType;
        while (baseType != null)
        {
            yield return baseType;
            baseType = baseType.BaseType;
        }
    }

    public static ISymbol? GetMember(this ITypeSymbol typeSymbol, string memberName, bool includeBaseTypes)
    {
        var currentType = typeSymbol;

        while (currentType != null)
        {
            var symbol = currentType.GetMembers(memberName).FirstOrDefault();

            if (symbol != null || !includeBaseTypes)
                return symbol;

            currentType = currentType.BaseType;
        }


        return null;
    }

    public static IEventSymbol? GetEvent(this ITypeSymbol typeSymbol, string eventName, bool includeBaseTypes)
    {
        return typeSymbol.GetMember(eventName, includeBaseTypes) as IEventSymbol;
    }

    public static IPropertySymbol? GetProperty(this ITypeSymbol typeSymbol, string propName, bool includeBaseTypes = false)
    {
        return typeSymbol.GetMember(propName, includeBaseTypes) as IPropertySymbol;
    }

    public static IEnumerable<INamedTypeSymbol> GetAllTypes(this INamespaceSymbol namespaceSymbol)
    {
        foreach (var nsOrType in namespaceSymbol.GetMembers())
        {
            if (nsOrType is INamedTypeSymbol namedType)
                yield return namedType;

            if (nsOrType is INamespaceSymbol ns)
            {
                foreach (var type in ns.GetAllTypes())
                    yield return type;
            }
        }
    }

    public static string GetFullName(this INamespaceOrTypeSymbol namespaceOrType)
    {
        var stack = new Stack<string>();

        stack.Push(GetName(namespaceOrType));

        if (namespaceOrType.ContainingType != null)
        {
            stack.Push(GetName(namespaceOrType.ContainingType));
        }

        var currentNamespace = namespaceOrType.ContainingNamespace;
        while (currentNamespace != null && !currentNamespace.IsGlobalNamespace)
        {
            stack.Push(currentNamespace.Name);
            currentNamespace = currentNamespace.ContainingNamespace;
        }

        return string.Join(".", stack);
    }

    /// <summary>
    /// Returns name with generic type arguments (if any).
    /// </summary>
    private static string GetName(INamespaceOrTypeSymbol namespaceOrType)
    {
        if (namespaceOrType is INamedTypeSymbol namedType && namedType.IsGenericType)
        {
            var genericTypesNames = string.Join(", ", namedType.TypeArguments.Select(GetFullName));
            return $"{namedType.Name}<{genericTypesNames}>";
        }
        else
        {
            return namespaceOrType.CanBeReferencedByName
                ? namespaceOrType.Name
                : namespaceOrType.ToString();
        }
    }

    public static bool IsNullableStruct(this INamedTypeSymbol symbol)
    {
        return symbol.IsGenericType && symbol.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T;
    }

    public static bool IsObsolete(this ISymbol symbol)
    {
        return symbol.GetAttributes().Any(a => a.AttributeClass.Name == nameof(ObsoleteAttribute));
    }

    public static bool IsBrowsable(this ISymbol propInfo)
    {
        // [EditorBrowsable(EditorBrowsableState.Never)]
        return !propInfo.GetAttributes().Any(a => a.AttributeClass.Name == nameof(EditorBrowsableAttribute)
            && a.ConstructorArguments.FirstOrDefault().Value?.Equals((int)EditorBrowsableState.Never) == true);
    }

    public static bool IsAssignableToType(this ITypeSymbol type, ITypeSymbol toType, Compilation compilation)
    {
        return compilation.ClassifyConversion(type, toType)
            is { IsIdentity: true }
            or { IsReference: true, IsImplicit: true };
    }

    public static bool IsAssignableToType(this ITypeSymbol typeSymbol, string toTypeName, Compilation compilation)
    {
        var toType = compilation.GetTypeByMetadataName(toTypeName);
        return toType != null && typeSymbol.IsAssignableToType(toType, compilation);
    }


    private static readonly Dictionary<SpecialType, string> TypeToCSharpName = new()
    {
        { SpecialType.System_Boolean, "bool" },
        { SpecialType.System_Byte, "byte" },
        { SpecialType.System_SByte, "sbyte" },
        { SpecialType.System_Char, "char" },
        { SpecialType.System_Decimal, "decimal" },
        { SpecialType.System_Double, "double" },
        { SpecialType.System_Single, "float" },
        { SpecialType.System_Int32, "int" },
        { SpecialType.System_UInt32, "uint" },
        { SpecialType.System_Int64, "long" },
        { SpecialType.System_UInt64, "ulong" },
        { SpecialType.System_Object, "object" },
        { SpecialType.System_Int16, "short" },
        { SpecialType.System_UInt16, "ushort" },
        { SpecialType.System_String, "string" },
    };

    public static string GetCSharpTypeName(this ITypeSymbol typeSymbol)
    {
        return TypeToCSharpName.TryGetValue(typeSymbol.SpecialType, out var typeName) ? typeName : null;
    }

    public static int GetInheritanceDepth(this ITypeSymbol typeSymbol)
    {
        var depth = 0;
        var currentType = typeSymbol;

        while (currentType != null)
        {
            depth++;
            currentType = currentType.BaseType;
        }

        return depth;
    }
}