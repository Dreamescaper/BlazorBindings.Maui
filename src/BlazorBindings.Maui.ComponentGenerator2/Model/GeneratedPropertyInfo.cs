using BlazorBindings.Maui.ComponentGenerator.Extensions;
using BlazorBindings.Maui.ComponentGenerator2.Extensions;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace BlazorBindings.Maui.ComponentGenerator.Model;

public abstract class GeneratedPropertyInfo
{
    public abstract ISymbol? MemberSymbol { get; }

    public string ComponentPropertyName { get; set; }
    public string ComponentType { get; set; }
    public bool IsHidingProperty { get; set; }
    public bool IsGeneric { get; }
    public string MauiContainingTypeName { get; }
    public GeneratedTypeInfo ContainingType { get; }
    public Compilation Compilation => ContainingType.Compilation;

    public GeneratedPropertyInfo(GeneratedTypeInfo containingType)
    {
        ContainingType = containingType;
        MauiContainingTypeName = GetTypeNameAndAddNamespace(containingType.MauiType);
    }

    public string GetPropertyDeclaration()
    {
        var newModifier = IsHidingProperty ? "new " : "";

        const string indent = "        ";

        var xmlDocContents = MemberSymbol?.GetXmlDocContents()?.Indent(indent) ?? "";
        return $@"{xmlDocContents}{indent}[Parameter] public {newModifier}{ComponentType} {ComponentPropertyName} {{ get; set; }}
";
    }

    public abstract string GetHandlePropertyCase();

    protected string GetTypeNameAndAddNamespace(ITypeSymbol type)
    {
        return ContainingType.GetTypeNameAndAddNamespace(type);
    }

    protected static bool IsExplicitlyAllowed(IPropertySymbol propertyInfo, GeneratedTypeInfo generatedType)
    {
        return generatedType.Settings.Include.Contains(propertyInfo.Name)
            || propertyInfo.Type.SpecialType == SpecialType.System_Object && generatedType.Settings.GenericProperties.ContainsKey(propertyInfo.Name);
    }

    protected static bool IsPublicMember(ISymbol symbol)
    {
        if (!symbol.IsBrowsable() || symbol.DeclaredAccessibility != Accessibility.Public)
            return false;

        if (symbol is IPropertySymbol propertyInfo)
        {
            return propertyInfo.GetMethod?.DeclaredAccessibility == Accessibility.Public
                && !propertyInfo.IsIndexer;
        }

        return true;
    }

    protected static bool HasPublicSetter(IPropertySymbol propertyInfo)
    {
        return propertyInfo.SetMethod?.DeclaredAccessibility == Accessibility.Public;
    }

    protected static IEnumerable<T> GetMembers<T>(GeneratedTypeInfo generatedType) where T : ISymbol
    {
        var typeSymbol = generatedType.MauiType;
        var includeBaseMemberNames = generatedType.Settings.Include;

        var baseMembers = includeBaseMemberNames
            .Select(member => typeSymbol.GetMember(member, true)!)
            .Where(member => member != null);

        IEnumerable<ISymbol> members = typeSymbol.GetMembers();

        // Since we don't generate components for generic types, we include them here.
        if (typeSymbol.BaseType != null && typeSymbol.BaseType.IsGenericType)
        {
            members = members.Concat(typeSymbol.BaseType.GetMembers());
        }

        return members.Union(baseMembers, SymbolEqualityComparer.Default)
            .Where(m => !m.IsStatic && IsPublicMember(m) && !m.IsObsolete())
            .Where(m => !generatedType.Settings.Exclude.Contains(m.Name))
            .OfType<T>();
    }
}
