using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace BlazorBindings.Maui.ComponentGenerator.Model;

internal class ValuePropertyInfo : GeneratedPropertyInfo
{
    public IPropertySymbol MauiProperty { get; internal set; }
    public override ISymbol? MemberSymbol => MauiProperty;

    public ValuePropertyInfo(GeneratedTypeInfo generatedType, IPropertySymbol property) : base(generatedType)
    {
        MauiProperty = property;
        ComponentPropertyName = GetComponentPropertyName(generatedType, property);
        ComponentType = GetComponentPropertyTypeName(property, generatedType, makeNullable: true);

        IsGeneric = generatedType.Settings.GenericProperties.TryGetValue(property.Name, out var genericTypeArgument);
        GenericTypeArgument = genericTypeArgument;
    }

    public override string GetHandlePropertyCase()
    {
        var propName = ComponentPropertyName;
        var mauiPropertyName = MauiProperty.Name.EscapeIdentifierName();

        return $@"                case nameof({propName}):
                    if (!Equals({propName}, value))
                    {{
                        {propName} = ({ComponentType})value;
                        NativeControl.{mauiPropertyName} = {GetConvertedProperty(MauiProperty.Type, propName)};
                    }}
                    break;
";

        string GetConvertedProperty(ITypeSymbol propertyType, string propName)
        {
            if (propertyType is INamedTypeSymbol namedType)
            {
                if (namedType.IsGenericType
                    && namedType.ConstructedFrom.SpecialType == SpecialType.System_Collections_Generic_IList_T
                    && namedType.TypeArguments[0].SpecialType == SpecialType.System_String)
                {
                    return $"AttributeHelper.GetStringList({propName})";
                }

                if (namedType.IsValueType && !namedType.IsNullableStruct())
                {
                    var hasBindingProperty = !MauiProperty.ContainingType.GetMembers($"{propName}Property").IsEmpty;
                    var defaultValue = hasBindingProperty
                        ? $"({GetTypeNameAndAddNamespace(propertyType)}){MauiContainingTypeName}.{propName}Property.DefaultValue"
                        : "default";

                    return $"{propName} ?? {defaultValue}";
                }

                if (IsGeneric && namedType.GetFullName() == "Microsoft.Maui.Controls.BindingBase")
                {
                    return $"AttributeHelper.GetBinding({propName})";
                }

                if (IsGeneric && namedType.GetFullName() == "System.Collections.IList")
                {
                    return $"AttributeHelper.GetIList({propName})";
                }
            }

            return propName;
        }
    }

    public static void AddValueProperties(List<GeneratedPropertyInfo> generatedProperties, GeneratedTypeInfo generatedType)
    {
        var properties = GetMembers<IPropertySymbol>(generatedType).Where(prop => IsValueProperty(prop, generatedType));

        foreach (var prop in properties)
        {
            // Brush properties are already added as RenderFragment properties, but we want add add them as colors as well
            // (it is much more convenient and setting color is very common).
            if (prop.Type.GetFullName() == "Microsoft.Maui.Controls.Brush")
            {
                var propName = prop.Name.Replace("Brush", "") + "Color";

                if (prop.ContainingType.GetProperty(propName, includeBaseTypes: true) != null)
                    continue;

                generatedProperties.Add(new ValuePropertyInfo(generatedType, prop)
                {
                    ComponentPropertyName = propName,
                    ComponentType = generatedType.GetTypeNameAndAddNamespace("Microsoft.Maui.Graphics", "Color")
                });

                continue;
            }

            // Check if already added as RenderFragment or EventCallback.
            if (generatedProperties.Any(p => p.MemberSymbol?.Name == prop.Name))
                continue;

            generatedProperties.Add(new ValuePropertyInfo(generatedType, prop));
        }
    }

    private static bool IsValueProperty(IPropertySymbol prop, GeneratedTypeInfo generatedType)
    {
        return HasPublicSetter(prop)
            && (IsExplicitlyAllowed(prop, generatedType) || (!IsDisallowed(prop) && !IsCommandParameter(prop)));

        static bool IsDisallowed(IPropertySymbol prop) => DisallowedComponentTypes.Contains(prop.Type.GetFullName());
        static bool IsCommandParameter(IPropertySymbol prop) => prop.Type.SpecialType == SpecialType.System_Object && prop.Name.EndsWith("CommandParameter");
    }


    private static string GetComponentPropertyName(GeneratedTypeInfo containingType, IPropertySymbol mauiProperty)
    {
        if (containingType.Settings.Aliases.TryGetValue(mauiProperty.Name, out var aliasName))
            return aliasName;

        return mauiProperty.Name.EscapeIdentifierName();
    }


    private static readonly List<string> DisallowedComponentTypes =
    [
        "Microsoft.Maui.Controls.Button.ButtonContentLayout", // TODO: This is temporary; should be possible to add support later
        "Microsoft.Maui.Controls.ColumnDefinitionCollection",
        "Microsoft.Maui.Controls.PointCollection",
        "Microsoft.Maui.Controls.DoubleCollection",
        "Microsoft.Maui.Controls.ControlTemplate",
        "Microsoft.Maui.Controls.DataTemplate",
        "Microsoft.Maui.Controls.Element",
        "Microsoft.Maui.Font", // TODO: This is temporary; should be possible to add support later
        "Microsoft.Maui.Graphics.Font", // TODO: This is temporary; should be possible to add support later
        "Microsoft.Maui.Controls.FormattedString",
        "Microsoft.Maui.Controls.Shapes.Geometry",
        "Microsoft.Maui.Controls.GradientStopCollection",
        "System.Windows.Input.ICommand",
        "Microsoft.Maui.Controls.Command",
        "Microsoft.Maui.Controls.Page",
        "Microsoft.Maui.Controls.RowDefinitionCollection",
        "Microsoft.Maui.Controls.Shadow",
        "Microsoft.Maui.Controls.ShellContent",
        "Microsoft.Maui.Controls.ShellItem",
        "Microsoft.Maui.Controls.ShellSection",
        "Microsoft.Maui.Controls.IVisual",
        "Microsoft.Maui.Controls.View",
        "Microsoft.Maui.Graphics.IShape",
        "Microsoft.Maui.IView",
        "Microsoft.Maui.IViewHandler"
    ];
}
