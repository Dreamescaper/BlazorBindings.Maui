using Microsoft.CodeAnalysis;
using System.ComponentModel;

namespace BlazorBindings.Maui.ComponentGenerator.Extensions;

internal static class SymbolExtensions
{
    public static IMethodSymbol GetMethod(this ITypeSymbol typeSymbol, string method)
    {
        return typeSymbol.GetMembers(method).FirstOrDefault() as IMethodSymbol;
    }

    public static ISymbol GetMember(this ITypeSymbol typeSymbol, string memberName, bool includeBaseTypes)
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

    public static IEventSymbol GetEvent(this ITypeSymbol typeSymbol, string eventName, bool includeBaseTypes)
    {
        return GetMember(typeSymbol, eventName, includeBaseTypes) as IEventSymbol;
    }

    public static IPropertySymbol GetProperty(this ITypeSymbol typeSymbol, string propName, bool includeBaseTypes = false)
    {
        return GetMember(typeSymbol, propName, includeBaseTypes) as IPropertySymbol;
    }

    public static IEnumerable<INamedTypeSymbol> GetAllTypes(this INamespaceSymbol namespaceSymbol)
    {
        foreach (var nsOrType in namespaceSymbol.GetMembers())
        {
            if (nsOrType is INamedTypeSymbol namedType)
                yield return namedType;

            if (nsOrType is INamespaceSymbol ns)
            {
                foreach (var type in GetAllTypes(ns))
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
        while (!currentNamespace.IsGlobalNamespace)
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
            return namespaceOrType.Name;
        }
    }

    public static bool IsHidingMember(this IPropertySymbol symbol)
    {
        var currentType = symbol.ContainingType?.BaseType;
        var baseProperty = currentType.GetProperty(symbol.Name, includeBaseTypes: true);
        return baseProperty != null
            && baseProperty.DeclaredAccessibility == symbol.DeclaredAccessibility
            && SymbolEqualityComparer.Default.Equals(baseProperty.Type, symbol.Type);
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
}