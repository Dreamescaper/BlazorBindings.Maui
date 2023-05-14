using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;

namespace BlazorBindings.Maui.ComponentGenerator;

public partial class GeneratedPropertyInfo
{
    private static readonly (string TypeName, bool AllowDescendantTypes)[] ContentTypes = new[]
    {
        ("Microsoft.Maui.IView", false),
        ("Microsoft.Maui.Controls.VisualElement", true),
        ("Microsoft.Maui.Controls.BaseMenuItem", true),
        ("Microsoft.Maui.Controls.Brush", true),
        ("Microsoft.Maui.Controls.Shadow", false),
        ("Microsoft.Maui.Controls.ControlTemplate", false),
        ("Microsoft.Maui.Controls.DataTemplate",false),
        ("Microsoft.Maui.Controls.Shapes.Shape", true),
        ("Microsoft.Maui.Graphics.IShape",false)
    };

    public bool IsRenderFragmentProperty => Kind == GeneratedPropertyKind.RenderFragment;
    public bool IsControlTemplate => _propertyInfo.Type.GetFullName() == "Microsoft.Maui.Controls.ControlTemplate";
    public bool IsDataTemplate => _propertyInfo.Type.GetFullName() == "Microsoft.Maui.Controls.DataTemplate";
    public bool ForceContent => ContainingType.Settings.ContentProperties.Contains(_propertyInfo.Name);

    public string GetHandleContentProperty()
    {
        return $@"                case nameof({ComponentPropertyName}):
                    {ComponentPropertyName} = ({ComponentType})value;
                    break;
";
    }

    public string RenderContentProperty()
    {
        var type = (INamedTypeSymbol)_propertyInfo.Type;

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
        else if (!ForceContent && IsIList(type, out var itemType))
        {
            // RenderTreeBuilderHelper.AddListContentProperty<MC.Layout, IView>(builder, sequence++, ChildContent, x => x.Children);
            var itemTypeName = GetTypeNameAndAddNamespace(itemType);
            return $"\r\n            RenderTreeBuilderHelper.AddListContentProperty<{MauiContainingTypeName}, {itemTypeName}>(builder, sequence++, {ComponentPropertyName}, x => x.{_propertyInfo.Name});";
        }
        else
        {
            // RenderTreeBuilderHelper.AddContentProperty<MC.ContentPage>(builder, sequence++, ChildContent, (x, value) => x.Content = (MC.View)value);
            var propTypeName = GetTypeNameAndAddNamespace(type);
            return $"\r\n            RenderTreeBuilderHelper.AddContentProperty<{MauiContainingTypeName}>(builder, sequence++, {ComponentPropertyName}, (x, value) => x.{_propertyInfo.Name} = ({propTypeName})value);";
        }
    }

    internal static GeneratedPropertyInfo[] GetContentProperties(GeneratedTypeInfo containingType)
    {
        var componentInfo = containingType.Settings;
        var propInfos = GetMembers<IPropertySymbol>(componentInfo.TypeSymbol, containingType.Settings.Include)
            .Where(e => !componentInfo.Exclude.Contains(e.Name))
            .Where(IsPublicProperty)
            .Where(prop => !IsReferenceProperty(containingType, prop))
            .Where(prop => IsRenderFragmentPropertySymbol(containingType, prop))
            .OrderBy(prop => prop.Name, StringComparer.OrdinalIgnoreCase);

        return propInfos.Select(prop => new GeneratedPropertyInfo(containingType, prop, GeneratedPropertyKind.RenderFragment)).ToArray();
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

        var type = prop.Type;
        if (IsContent(type) && HasPublicSetter(prop))
            return true;

        if (IsIList(type, out var itemType) && IsContent(itemType))
        {
            return true;
        }

        return false;

        bool IsContent(ITypeSymbol type) => ContentTypes.Any(t =>
        {
            var compilation = containingType.Compilation;
            var contentTypeSymbol = compilation.GetTypeByMetadataName(t.TypeName);
            var conversion = compilation.ClassifyConversion(type, contentTypeSymbol);
            return conversion is { IsIdentity: true }
                || t.AllowDescendantTypes && conversion is { IsReference: true, IsImplicit: true };
        });
    }

    private static bool IsIList(ITypeSymbol type, out ITypeSymbol itemType)
    {
        var isList = TypeEqualsIList(type, out var outItemType) || type.AllInterfaces.Any(i => IsIList(i, out outItemType));
        itemType = outItemType;
        return isList;

        static bool TypeEqualsIList(ITypeSymbol type, out ITypeSymbol itemType)
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
}
