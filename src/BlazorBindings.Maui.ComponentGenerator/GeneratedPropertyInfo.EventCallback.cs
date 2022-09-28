using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using System.Reflection;

namespace BlazorBindings.Maui.ComponentGenerator
{
    public partial class GeneratedPropertyInfo
    {
        private INamedTypeSymbol _eventHandlerType;
        private IPropertySymbol _bindedProperty;

        private bool IsPropertyChangedEvent => MauiPropertyName == "PropertyChanged";
        private ITypeSymbol EventArgsType => _eventHandlerType.GetMethod("Invoke")?.Parameters[1].Type;

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
                var cast = componentPropertyType == mauiPropertyType ? "" : $"({componentPropertyType})";

                argument = $"{cast}NativeControl.{_bindedProperty.Name}";
            }
            else
            {
                argument = _eventHandlerType.IsGenericType ? "e" : "";
            }

            if (_bindedProperty != null && IsPropertyChangedEvent)
            {
                return $@"
                        {{
                            if (e.PropertyName == nameof(NativeControl.{_bindedProperty.Name}))
                            {{
                                var value = {argument};
                                {_bindedProperty.Name} = value;
                                {ComponentPropertyName}.InvokeAsync(value);
                            }}
                        }}";
            }

            if (_bindedProperty != null)
            {
                return $@"
                        {{
                            var value = {argument};
                            {_bindedProperty.Name} = value;
                            {ComponentPropertyName}.InvokeAsync(value);
                        }}";
            }

            return $" => {ComponentPropertyName}.InvokeAsync({argument});";
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
                        "PropertyChanged",
                        containingType.GetTypeNameAndAddNamespace(componentType),
                        componentEventName,
                        GetRenderFragmentType(containingType, null, propertyInfo),
                        GeneratedPropertyKind.EventCallback);

                    generatedPropertyInfo._bindedProperty = propertyInfo;
                    generatedPropertyInfo._eventHandlerType = (INamedTypeSymbol)eventInfo.Type;
                    return generatedPropertyInfo;
                });

            var inferredEvents = componentType.GetMembers().OfType<IEventSymbol>()
                .Where(e => !componentInfo.Exclude.Contains(e.Name))
                .Where(e => e.DeclaredAccessibility == Accessibility.Public && IsBrowsable(e))
                .Select(eventInfo =>
                {
                    var isBindEvent = IsBindEvent(eventInfo, out var bindedProperty);

                    var eventCallbackName = isBindEvent ? $"{bindedProperty.Name}Changed" : GetEventCallbackName(eventInfo);

                    var generatedPropertyInfo = new GeneratedPropertyInfo(
                        containingType,
                        eventInfo.Name,
                        containingType.GetTypeNameAndAddNamespace(componentType),
                        eventCallbackName,
                        GetRenderFragmentType(containingType, eventInfo, bindedProperty),
                        GeneratedPropertyKind.EventCallback);

                    generatedPropertyInfo._bindedProperty = bindedProperty;
                    generatedPropertyInfo._eventHandlerType = (INamedTypeSymbol)eventInfo.Type;
                    return generatedPropertyInfo;
                });

            return propertyChangedEvents.Concat(inferredEvents).ToArray();
        }

        private static string GetRenderFragmentType(GeneratedTypeInfo containingType, IEventSymbol eventInfo, IPropertySymbol bindedProperty)
        {
            if (bindedProperty != null)
            {
                var typeName = GetComponentPropertyTypeName(bindedProperty, containingType);
                return $"EventCallback<{typeName}>";
            }

            var eventArgType = eventInfo.Type.GetMethod("Invoke").Parameters[1].Type;
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

        private static bool IsBindEvent(IEventSymbol eventSymbol, out IPropertySymbol property)
        {
            var properties = eventSymbol.ContainingType.GetMembers().OfType<IPropertySymbol>()
                .Where(p => IsPublicProperty(p) && HasPublicSetter(p));

            property = properties.FirstOrDefault(p =>
                eventSymbol.Name == $"{p.Name}Changed"  // e.g. Value - ValueChanged
                || eventSymbol.Name == $"{p.Name}Selected"  // e.g. Date - DateSelected
                || eventSymbol.Name == $"Is{p.Name}Changed"  // e.g. Selected - IsSelectedChanged
                || $"Is{eventSymbol.Name}" == $"{p.Name}Changed"  // e.g. IsSelected - SelectedChanged
                || $"Is{eventSymbol.Name}" == $"{p.Name}");  // e.g. IsToggled - Toggled

            return property != null;
        }
    }
}
