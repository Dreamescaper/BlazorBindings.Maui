﻿using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace BlazorBindings.Maui.ComponentGenerator;

public partial class GeneratedPropertyInfo
{
    private IPropertySymbol _bindedProperty;

    private bool IsPropertyChangedEvent => MauiPropertyName == "PropertyChanged";
    private ITypeSymbol EventArgsType => _propertyType.GetMethod("Invoke")?.Parameters[1].Type;

    public string GetHandleEventCallbackProperty()
    {
        /* 
            case nameof(OnClick):
                if (!Equals(OnClick, value))
                {
                    void Clicked(object sender, System.EventArgs e) => OnClick.InvokeAsync();

                    OnClick = (EventCallback)value;
                    NativeControl.Clicked -= Clicked;
                    NativeControl.Clicked += Clicked;
                }

                return true; */

        var eventName = MauiPropertyName;

        var localFunctionName = $"NativeControl{eventName}";

        var localFunctionBody = GetLocalHandlerFunctionBody();

        return $@"                case nameof({ComponentPropertyName}):
                    if (!Equals({ComponentPropertyName}, value))
                    {{
                        void {localFunctionName}(object sender, {GetTypeNameAndAddNamespace(EventArgsType)} e){localFunctionBody}

                        {ComponentPropertyName} = ({ComponentType})value;
                        NativeControl.{eventName} -= {localFunctionName};
                        NativeControl.{eventName} += {localFunctionName};
                    }}
                    break;
";
    }

    private string GetLocalHandlerFunctionBody()
    {
        string argument;

        if (_bindedProperty != null)
        {
            var componentPropertyType = GetComponentPropertyTypeName(_bindedProperty, ContainingType);
            var mauiPropertyType = GetTypeNameAndAddNamespace(_bindedProperty.Type);

            argument = componentPropertyType == mauiPropertyType
                ? $"NativeControl.{_bindedProperty.Name}"
                : $"NativeControl.{_bindedProperty.Name} is {componentPropertyType} item ? item : default({componentPropertyType})";
        }
        else
        {
            argument = GetEventArgType(_propertyType).Name != nameof(EventArgs) ? "e" : "";
        }

        if (_bindedProperty != null && IsPropertyChangedEvent)
        {
            return $@"
                        {{
                            if (e.PropertyName == nameof(NativeControl.{_bindedProperty.Name}))
                            {{
                                var value = {argument};
                                {_bindedProperty.Name} = value;
                                InvokeEventCallback({ComponentPropertyName}, value);
                            }}
                        }}";
        }

        if (_bindedProperty != null)
        {
            return $@"
                        {{
                            var value = {argument};
                            {_bindedProperty.Name} = value;
                            InvokeEventCallback({ComponentPropertyName}, value);
                        }}";
        }

        return string.IsNullOrEmpty(argument)
            ? $" => InvokeEventCallback({ComponentPropertyName});"
            : $" => InvokeEventCallback({ComponentPropertyName}, {argument});";
    }

    internal static GeneratedPropertyInfo[] GetEventCallbackProperties(GeneratedTypeInfo containingType)
    {
        var componentInfo = containingType.Settings;
        var componentType = componentInfo.TypeSymbol;

        var propertyChangedEvents = componentInfo.PropertyChangedEvents
            .Select(propertyForEvent =>
            {
                var propertyInfo = componentType.GetProperty(propertyForEvent)
                    ?? throw new Exception($"Cannot find property {componentType.Name}.{propertyForEvent}.");

                var eventInfo = componentType.GetEvent("PropertyChanged", includeBaseTypes: true);

                var componentEventName = $"{propertyInfo.Name}Changed";

                var generatedPropertyInfo = new GeneratedPropertyInfo(
                    containingType,
                    eventInfo.Type,
                    "PropertyChanged",
                    containingType.GetTypeNameAndAddNamespace(componentType),
                    componentEventName,
                    GetEventCallbackType(containingType, null, propertyInfo),
                    GeneratedPropertyKind.EventCallback);

                generatedPropertyInfo._bindedProperty = propertyInfo;
                return generatedPropertyInfo;
            });

        var inferredEvents = GetMembers<IEventSymbol>(componentInfo.TypeSymbol, containingType.Settings.Include)
            .Where(e => !componentInfo.Exclude.Contains(e.Name))
            .Where(e => e.DeclaredAccessibility == Accessibility.Public && e.IsBrowsable())
            .Select(eventInfo =>
            {
                var isBindEvent = IsBindEvent(eventInfo, containingType, out var bindedProperty);

                var eventCallbackName = isBindEvent ? $"{bindedProperty.Name}Changed" : GetEventCallbackName(eventInfo);

                var generatedPropertyInfo = new GeneratedPropertyInfo(
                    containingType,
                    eventInfo.Type,
                    eventInfo.Name,
                    containingType.GetTypeNameAndAddNamespace(componentType),
                    eventCallbackName,
                    GetEventCallbackType(containingType, eventInfo, bindedProperty),
                    GeneratedPropertyKind.EventCallback);

                generatedPropertyInfo._bindedProperty = bindedProperty;
                generatedPropertyInfo.IsHidingProperty = eventInfo.IsHidingMember() || IsHidingCustomViewEvents(eventInfo);
                return generatedPropertyInfo;
            })
                .Where(e => e != null);

        return propertyChangedEvents.Concat(inferredEvents).ToArray();
    }

    private static string GetEventCallbackType(GeneratedTypeInfo containingType, IEventSymbol eventInfo, IPropertySymbol bindedProperty)
    {
        if (bindedProperty != null)
        {
            var typeName = GetComponentPropertyTypeName(bindedProperty, containingType);
            return $"EventCallback<{typeName}>";
        }

        var eventArgType = GetEventArgType(eventInfo.Type);
        if (eventArgType.Name != nameof(EventArgs))
        {
            return $"EventCallback<{containingType.GetTypeNameAndAddNamespace(eventArgType)}>";
        }
        else
        {
            return "EventCallback";
        }
    }

    private static string GetEventCallbackName(IEventSymbol eventSymbol)
    {
        return eventSymbol.Name switch
        {
            "Clicked" => "OnClick",
            "Pressed" => "OnPress",
            "Released" => "OnRelease",
            _ => $"On{eventSymbol.Name}"
        };
    }

    private static bool IsBindEvent(IEventSymbol eventSymbol, GeneratedTypeInfo containingType, out IPropertySymbol property)
    {
        var properties = eventSymbol.ContainingType.GetMembers()
            .OfType<IPropertySymbol>()
            .Where(p => IsValueProperty(p, containingType));

        property = properties.FirstOrDefault(p =>
            eventSymbol.Name == $"{p.Name}Changed"  // e.g. Value - ValueChanged
            || eventSymbol.Name == $"{p.Name}Selected"  // e.g. Date - DateSelected
            || eventSymbol.Name == $"Is{p.Name}Changed"  // e.g. Selected - IsSelectedChanged
            || $"Is{eventSymbol.Name}" == $"{p.Name}Changed"  // e.g. IsSelected - SelectedChanged
            || $"Is{eventSymbol.Name}" == $"{p.Name}");  // e.g. IsToggled - Toggled

        return property != null;
    }

    private static ITypeSymbol GetEventArgType(ITypeSymbol eventHandlerType)
    {
        return eventHandlerType.GetMethod("Invoke").Parameters[1].Type;
    }

    private static bool IsHidingCustomViewEvents(IEventSymbol eventSymbol)
    {
        // View has some events defines by us. We need to add 'new' to them as well.
        return eventSymbol.ContainingType.GetBaseTypes().Any(t => t.Name == "View") &&
            eventSymbol.Name is "Tap" or "DoubleTap" or "Swipe" or "PinchUpdate" or "PanUpdate";
    }
}
