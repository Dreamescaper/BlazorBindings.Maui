using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics.CodeAnalysis;

namespace BlazorBindings.Maui.ComponentGenerator.Model;

internal class RenderFragmentPropertyInfo : GeneratedPropertyInfo
{
    public IPropertySymbol? MauiProperty { get; }

    public ITypeSymbol MauiPropertyType { get; }

    public ITypeSymbol? MauiContentType { get; }

    [MemberNotNullWhen(true, nameof(MauiContentType))]
    public bool IsList { get; }

    public override ISymbol? MemberSymbol => MauiProperty;

    [MemberNotNullWhen(true, nameof(MauiProperty))]
    private bool IsControlTemplate  { get; }

    [MemberNotNullWhen(true, nameof(MauiProperty))]
    private bool IsDataTemplate { get; }

    private bool ForceContent => ContainingType.Settings.ContentProperties.Contains(MauiProperty?.Name!);

    public RenderFragmentPropertyInfo(GeneratedTypeInfo containingType, IPropertySymbol property) : base(containingType)
    {
        MauiProperty = property;
        MauiPropertyType = property.Type;

        ComponentPropertyName = GetComponentPropertyName(containingType, property);
        ComponentType = GetComponentPropertyTypeName(property, containingType, isRenderFragmentProperty: true);

        IsGeneric = containingType.Settings.GenericProperties.TryGetValue(property.Name, out var genericTypeArgument);
        GenericTypeArgument = genericTypeArgument;

        (IsList, MauiContentType) = GetMauiContentType(Compilation, MauiPropertyType);
        IsDataTemplate = IsDataTemplateType(MauiPropertyType);
        IsControlTemplate = IsControlTemplateType(MauiPropertyType);
    }

    public RenderFragmentPropertyInfo(GeneratedTypeInfo containingType) : base(containingType)
    {
        MauiPropertyType = containingType.MauiType;
        ComponentPropertyName = "ChildContent";
        ComponentType = "RenderFragment";

        (IsList, MauiContentType) = GetMauiContentType(Compilation, MauiPropertyType);
    }

    public override string GetHandlePropertyCase()
    {
        return $@"                case nameof({ComponentPropertyName}):
                    {ComponentPropertyName} = ({ComponentType})value;
                    break;
";
    }

    public override string AdditionalContentForProperty()
    {
        // Handling the case when ChildContent property is created for the IList element type itself.
        if (MauiProperty is null && IsList)
        {
            // RenderTreeBuilderHelper.AddListContentProperty<MC.TableSection, MC.Cell>(builder, sequence++, ChildContent, x => x);
            var itemTypeName = GetTypeNameAndAddNamespace(MauiContentType);
            return $"\r\n            RenderTreeBuilderHelper.AddListContentProperty<{MauiContainingTypeName}, {itemTypeName}>(" +
                $"builder, sequence++, {ComponentPropertyName}, x => x);";
        }

        if (IsControlTemplate)
        {
            // RenderTreeBuilderHelper.AddControlTemplateProperty<MC.TemplatedView>(builder, sequence++, ControlTemplate, (x, template) => x.ControlTemplate = template);
            return $"\r\n            RenderTreeBuilderHelper.AddControlTemplateProperty<{MauiContainingTypeName}>(" +
                $"builder, sequence++, {ComponentPropertyName}, (x, template) => x.{MauiProperty.Name} = template);";
        }

        if (IsDataTemplate && !IsGeneric)
        {
            // RenderTreeBuilderHelper.AddDataTemplateProperty<MC.Shell>(builder, sequence++, FlyoutContent, (x, template) => x.FlyoutContentTemplate = template);
            return $"\r\n            RenderTreeBuilderHelper.AddDataTemplateProperty<{MauiContainingTypeName}>(" +
                $"builder, sequence++, {ComponentPropertyName}, (x, template) => x.{MauiProperty.Name} = template);";
        }

        if (IsDataTemplate && IsGeneric)
        {
            // RenderTreeBuilderHelper.AddDataTemplateProperty<MC.ItemsView, T>(builder, sequence++, ItemTemplate, (x, template) => x.ItemTemplate = template);
            var itemTypeName = GenericTypeArgument is null ? "T" : GetTypeNameAndAddNamespace(GenericTypeArgument);
            return $"\r\n            RenderTreeBuilderHelper.AddDataTemplateProperty<{MauiContainingTypeName}, {itemTypeName}>(" +
                $"builder, sequence++, {ComponentPropertyName}, (x, template) => x.{MauiProperty.Name} = template);";
        }

        if (!ForceContent && IsList && !IsElement(Compilation, MauiPropertyType))
        {
            // RenderTreeBuilderHelper.AddListContentProperty<MC.Layout, IView>(builder, sequence++, ChildContent, x => x.Children);
            var itemTypeName = GetTypeNameAndAddNamespace(MauiContentType);
            return $"\r\n            RenderTreeBuilderHelper.AddListContentProperty<{MauiContainingTypeName}, {itemTypeName}>(" +
                $"builder, sequence++, {ComponentPropertyName}, x => x.{MauiProperty.Name});";
        }
        else
        {
            // RenderTreeBuilderHelper.AddContentProperty<MC.ContentPage>(builder, sequence++, ChildContent, (x, value) => x.Content = (MC.View)value);
            var propTypeName = GetTypeNameAndAddNamespace(MauiPropertyType);
            return $"\r\n            RenderTreeBuilderHelper.AddContentProperty<{MauiContainingTypeName}>(" +
                $"builder, sequence++, {ComponentPropertyName}, (x, value) => x.{MauiProperty.Name} = ({propTypeName})value);";
        }
    }

    protected override string? GetXmlDocs()
    {
        var xmlDocs = base.GetXmlDocs();

        if (MauiContentType is not null && !IsDataTemplate && !IsControlTemplate)
        {
            var contentDocs = IsList
                ? $"Accepts one or more {MauiContentType.Name} elements."
                : $"Accepts single {MauiContentType.Name} element.";

            xmlDocs += $"""
            /// <remarks>
            /// {contentDocs}
            /// </remarks>

            """;
        }

        return xmlDocs;
    }

    private static string GetComponentPropertyName(GeneratedTypeInfo containingType, IPropertySymbol mauiProperty)
    {
        if (containingType.Settings.Aliases.TryGetValue(mauiProperty.Name, out var aliasName))
            return aliasName;

        if (mauiProperty.ContainingType.GetAttributes().Any(a => a.AttributeClass?.Name == "ContentPropertyAttribute"
            && Equals(a.ConstructorArguments.FirstOrDefault().Value, mauiProperty.Name)))
        {
            return "ChildContent";
        }

        return mauiProperty.Name.EscapeIdentifierName();
    }

    private static bool IsReferenceProperty(GeneratedTypeInfo containingType, IPropertySymbol prop)
    {
        if (containingType.Settings.ContentProperties.Contains(prop.Name))
            return false;

        // RenderFragment property makes sense when we're creating a new element.
        // However, some properties expect not a new element, but a reference to an existing one.
        // E.g. VisibleViews, SelectedItem, CurrentItem, etc.
        // As for now we exclude such properties.
        var referenceNames = new[] { "Visible", "Selected", "Current" };
        return referenceNames.Any(n => prop.Name.Contains(n));
    }

    private static bool IsRenderFragmentPropertySymbol(GeneratedTypeInfo containingType, IPropertySymbol prop)
    {
        if (containingType.Settings.ContentProperties.Contains(prop.Name))
            return true;

        if (containingType.Settings.NonContentProperties.Contains(prop.Name))
            return false;

        var type = prop.Type;
        if (IsContent(containingType.Compilation, type) && HasPublicSetter(prop))
            return true;

        if (IsIList(type, out var itemType) && IsContent(containingType.Compilation, itemType))
        {
            return true;
        }

        return false;
    }

    private static (bool IsList, ITypeSymbol MauiContentType) GetMauiContentType(Compilation compilation, ITypeSymbol mauiPropertyType)
    {
        if (IsIList(mauiPropertyType, out var itemType) && IsContent(compilation, itemType))
            return (true, itemType);

        if (IsContent(compilation, mauiPropertyType))
            return (false, mauiPropertyType);

        return (false, null);
    }

    private static bool IsContent(Compilation compilation, ITypeSymbol type)
    {
        return ContentTypes.Any(contentType => type.IsAssignableToType(contentType, compilation)
            && !NonContentTypes.Any(nonContentType => type.IsAssignableToType(nonContentType, compilation)));
    }

    private static bool IsElement(Compilation compilation, ITypeSymbol type)
    {
        var elementSymbol = compilation.GetTypeByMetadataName("Microsoft.Maui.Controls.Element")!;
        return compilation.ClassifyConversion(type, elementSymbol) is { IsIdentity: true } or { IsReference: true, IsImplicit: true };
    }

    private static bool IsIList(ITypeSymbol type, [NotNullWhen(true)] out ITypeSymbol? itemType)
    {
        var isList = TypeEqualsIList(type, out var outItemType) || type.AllInterfaces.Any(i => IsIList(i, out outItemType));
        itemType = outItemType;
        return isList;

        static bool TypeEqualsIList(ITypeSymbol type, out ITypeSymbol? itemType)
        {
            if (type is INamedTypeSymbol namedType
                && namedType.IsGenericType
                && namedType.ConstructedFrom.SpecialType == SpecialType.System_Collections_Generic_IList_T)
            {
                itemType = namedType.TypeArguments[0];
                return true;
            }
            else
            {
                itemType = null;
                return false;
            }
        }
    }

    public static void AddContentProperties(List<GeneratedPropertyInfo> generatedProperties, GeneratedTypeInfo containingType)
    {
        var componentInfo = containingType.Settings;
        var propInfos = GetMembers<IPropertySymbol>(containingType)
            .Where(prop => !IsReferenceProperty(containingType, prop))
            .Where(prop => IsRenderFragmentPropertySymbol(containingType, prop))
            .OrderBy(prop => prop.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var prop in propInfos)
        {
            var generatedInfo = new RenderFragmentPropertyInfo(containingType, prop);
            generatedProperties.Add(generatedInfo);
        }

        // If there is no ChildContent property, but the type itself is IList, we add ChildContent property for children.
        if (generatedProperties.All(p => p.ComponentPropertyName != "ChildContent")
            && IsIList(containingType.MauiType, out var itemType)
            && (!IsIList(containingType.MauiBaseType, out _))
            && IsContent(containingType.Compilation, itemType))
        {
            var thisProperty = new RenderFragmentPropertyInfo(containingType);
            generatedProperties.Add(thisProperty);
        }
    }

    private static bool IsDataTemplateType(ITypeSymbol? type) => type?.GetFullName() == "Microsoft.Maui.Controls.DataTemplate";
    private static bool IsControlTemplateType(ITypeSymbol? type) => type?.GetFullName() == "Microsoft.Maui.Controls.ControlTemplate";

    private static readonly string[] ContentTypes =
    [
        "Microsoft.Maui.IElement",
        "Microsoft.Maui.Controls.BindableObject",
        "Microsoft.Maui.Controls.IGestureRecognizer",
        "Microsoft.Maui.Controls.ControlTemplate",
        "Microsoft.Maui.Controls.DataTemplate",
        "Microsoft.Maui.Graphics.IShape",
    ];

    private static readonly string[] NonContentTypes =
    [
        "Microsoft.Maui.Controls.FormattedString",
        "Microsoft.Maui.Controls.ImageSource",
        "Microsoft.Maui.Controls.ItemsLayout"
    ];
}