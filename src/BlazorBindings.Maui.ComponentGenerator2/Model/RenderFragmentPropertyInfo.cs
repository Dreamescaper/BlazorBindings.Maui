using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace BlazorBindings.Maui.ComponentGenerator.Model;

internal class RenderFragmentPropertyInfo : GeneratedPropertyInfo
{
    public IPropertySymbol? MauiProperty { get; internal set; }
    public ITypeSymbol MauiPropertyType { get; internal set; }

    public override ISymbol? MemberSymbol => MauiProperty;

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

    private bool IsControlTemplate => _propertyType?.GetFullName() == "Microsoft.Maui.Controls.ControlTemplate";
    private bool IsDataTemplate => _propertyType?.GetFullName() == "Microsoft.Maui.Controls.DataTemplate";
    private bool ForceContent => ContainingType.Settings.ContentProperties.Contains(_propertyInfo.Name);

    public override string GetHandlePropertyCase()
    {
        return $@"                case nameof({ComponentPropertyName}):
                    {ComponentPropertyName} = ({ComponentType})value;
                    break;
";
    }

    public string RenderContentProperty()
    {
        // Handling the case when ChildContent property is created for the IList element type itself.
        if (_propertyInfo is null && IsIList(_propertyType, out var itemType))
        {
            // RenderTreeBuilderHelper.AddListContentProperty<MC.TableSection, MC.Cell>(builder, sequence++, ChildContent, x => x);
            var itemTypeName = GetTypeNameAndAddNamespace(itemType);
            return $"\r\n            RenderTreeBuilderHelper.AddListContentProperty<{MauiContainingTypeName}, {itemTypeName}>(builder, sequence++, {ComponentPropertyName}, x => x);";
        }

        if (IsControlTemplate)
        {
            // RenderTreeBuilderHelper.AddControlTemplateProperty<MC.TemplatedView>(builder, sequence++, ControlTemplate, (x, template) => x.ControlTemplate = template);
            return $"\r\n            RenderTreeBuilderHelper.AddControlTemplateProperty<{MauiContainingTypeName}>(builder, sequence++, {ComponentPropertyName}, (x, template) => x.{_propertyInfo.Name} = template);";
        }
        else if (IsDataTemplate && !IsGeneric)
        {
            // RenderTreeBuilderHelper.AddDataTemplateProperty<MC.Shell>(builder, sequence++, FlyoutContent, (x, template) => x.FlyoutContentTemplate = template);
            return $"\r\n            RenderTreeBuilderHelper.AddDataTemplateProperty<{MauiContainingTypeName}>(builder, sequence++, {ComponentPropertyName}, (x, template) => x.{_propertyInfo.Name} = template);";
        }
        else if (IsDataTemplate && IsGeneric)
        {
            // RenderTreeBuilderHelper.AddDataTemplateProperty<MC.ItemsView, T>(builder, sequence++, ItemTemplate, (x, template) => x.ItemTemplate = template);
            var itemTypeName = GenericTypeArgument is null ? "T" : GetTypeNameAndAddNamespace(GenericTypeArgument);
            return $"\r\n            RenderTreeBuilderHelper.AddDataTemplateProperty<{MauiContainingTypeName}, {itemTypeName}>(builder, sequence++, {ComponentPropertyName}, (x, template) => x.{_propertyInfo.Name} = template);";
        }
        else if (!ForceContent && IsIList(_propertyType, out itemType) && !IsElement(Compilation, _propertyType))
        {
            // RenderTreeBuilderHelper.AddListContentProperty<MC.Layout, IView>(builder, sequence++, ChildContent, x => x.Children);
            var itemTypeName = GetTypeNameAndAddNamespace(itemType);
            return $"\r\n            RenderTreeBuilderHelper.AddListContentProperty<{MauiContainingTypeName}, {itemTypeName}>(builder, sequence++, {ComponentPropertyName}, x => x.{_propertyInfo.Name});";
        }
        else
        {
            // RenderTreeBuilderHelper.AddContentProperty<MC.ContentPage>(builder, sequence++, ChildContent, (x, value) => x.Content = (MC.View)value);
            var propTypeName = GetTypeNameAndAddNamespace(_propertyType);
            return $"\r\n            RenderTreeBuilderHelper.AddContentProperty<{MauiContainingTypeName}>(builder, sequence++, {ComponentPropertyName}, (x, value) => x.{_propertyInfo.Name} = ({propTypeName})value);";
        }
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

    private static bool IsIList(ITypeSymbol type, out ITypeSymbol? itemType)
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

    private static bool IfChildContentHiding(ITypeSymbol baseType, Compilation compilation)
    {
        var currentType = baseType;

        while (currentType != null)
        {
            if (currentType.GetAttributes().Any(a => a.AttributeClass?.Name == "ContentPropertyAttribute"))
            {
                return true;
            }

            currentType = currentType.BaseType;
        }

        if (IsIList(baseType, out var itemType) && IsContent(compilation, itemType))
        {
            return true;
        }

        return false;
    }
}
