using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;

namespace BlazorBindings.Maui.ComponentGenerator.Model;

public abstract class GeneratedPropertyInfo
{
    public abstract ISymbol? MemberSymbol { get; }

    public string ComponentPropertyName { get; init; }
    public string ComponentType { get; init; }
    public bool IsGeneric { get; init; }
    public INamedTypeSymbol? GenericTypeArgument { get; init; }
    public string MauiContainingTypeName { get; }
    public GeneratedTypeInfo ContainingType { get; }
    public Compilation Compilation => ContainingType.Compilation;

    public bool IsHidingProperty => ContainingType.BaseTypesHaveProperty(ComponentPropertyName);

    protected GeneratedPropertyInfo(GeneratedTypeInfo containingType)
    {
        ContainingType = containingType;
        MauiContainingTypeName = GetTypeNameAndAddNamespace(containingType.MauiType);
    }

    public string GetPropertyDeclaration()
    {
        const string indent = "        ";

        var newModifier = IsHidingProperty ? "new " : "";
        var xmlDocs = GetXmlDocs()?.Indent(indent) ?? "";
        return $@"{xmlDocs}{indent}[Parameter] public {newModifier}{ComponentType} {ComponentPropertyName} {{ get; set; }}
";
    }

    protected virtual string? GetXmlDocs()
    {
        return MemberSymbol?.GetXmlDocContents();
    }

    public abstract string GetHandlePropertyCase();

    public virtual string AdditionalContentForProperty() => "";

    protected string GetTypeNameAndAddNamespace(ITypeSymbol type)
    {
        return ContainingType.GetTypeNameAndAddNamespace(type);
    }

    protected static bool IsExplicitlyAllowed(IPropertySymbol propertyInfo, GeneratedTypeInfo generatedType)
    {
        return generatedType.Settings.Include.Contains(propertyInfo.Name)
            || propertyInfo.Type.SpecialType == SpecialType.System_Object && generatedType.MakePropertyGeneric(propertyInfo.Name, out _);
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

    protected static string GetComponentPropertyTypeName(IPropertySymbol propertySymbol, GeneratedTypeInfo containingType, bool isRenderFragmentProperty = false, bool makeNullable = false)
    {
        var typeSymbol = propertySymbol.Type;
        var isGeneric = containingType.MakePropertyGeneric(propertySymbol.Name, out var typeArgument);
        var typeArgumentName = typeArgument is null ? "T" : containingType.GetTypeNameAndAddNamespace(typeArgument);

        if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return containingType.GetTypeNameAndAddNamespace(typeSymbol);
        }
        else if (namedTypeSymbol.IsGenericType
            && namedTypeSymbol.ConstructedFrom.SpecialType == SpecialType.System_Collections_Generic_IList_T
            && namedTypeSymbol.TypeArguments[0].SpecialType == SpecialType.System_String)
        {
            // Lists of strings are special-cased because they are handled specially by the handlers as a comma-separated list
            return "string";
        }
        else if (isRenderFragmentProperty)
        {
            return isGeneric ? $"RenderFragment<{typeArgumentName}>" : "RenderFragment";
        }
        else if (makeNullable && namedTypeSymbol.IsValueType && !namedTypeSymbol.IsNullableStruct())
        {
            return containingType.GetTypeNameAndAddNamespace(typeSymbol) + "?";
        }
        else if (namedTypeSymbol.SpecialType == SpecialType.System_Collections_IEnumerable && isGeneric)
        {
            return containingType.GetTypeNameAndAddNamespace("System.Collections.Generic", $"IEnumerable<{typeArgumentName}>");
        }
        else if (namedTypeSymbol.GetFullName() == "System.Collections.IList" && isGeneric)
        {
            return containingType.GetTypeNameAndAddNamespace("System.Collections.Generic", $"IList<{typeArgumentName}>");
        }
        else if (namedTypeSymbol.GetFullName() == "Microsoft.Maui.Controls.BindingBase" && isGeneric)
        {
            return "Func<T, string>";
        }
        else if (namedTypeSymbol.SpecialType == SpecialType.System_Object && isGeneric)
        {
            // Some third-party components (e.g. Syncfusion DropDownListBase) define ItemsSource as object.
            // We map it as IList<T>, unless type is requested explicitly.
            if (propertySymbol.Name == "ItemsSource" && typeArgument is null)
            {
                return containingType.GetTypeNameAndAddNamespace("System.Collections.Generic", $"IList<{typeArgumentName}>");
            }

            return typeArgumentName;
        }
        else
        {
            return containingType.GetTypeNameAndAddNamespace(namedTypeSymbol);
        }
    }

}
