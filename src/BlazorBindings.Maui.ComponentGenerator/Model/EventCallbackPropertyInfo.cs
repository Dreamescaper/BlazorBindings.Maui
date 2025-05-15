using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace BlazorBindings.Maui.ComponentGenerator.Model;

internal class EventCallbackPropertyInfo : GeneratedPropertyInfo
{
    public bool PropertyChangedEvent { get; private init; }
    public IEventSymbol EventSymbol { get; private init; }
    public IPropertySymbol? BindProperty { get; private init; }

    public override ISymbol? MemberSymbol => EventSymbol;

    public EventCallbackPropertyInfo(
        GeneratedTypeInfo containingType,
        IEventSymbol eventSymbol,
        IPropertySymbol? bindProperty,
        bool propertyChangedEvent = false
        ) : base(containingType)
    {
        PropertyChangedEvent = propertyChangedEvent;
        EventSymbol = eventSymbol;
        BindProperty = bindProperty;

        ComponentType = GetEventCallbackType(containingType, eventSymbol, bindProperty);

        if (BindProperty != null)
        {
            ComponentPropertyName = $"{BindProperty.Name}Changed";
        }
        else
        {
            ComponentPropertyName = GetEventCallbackName(eventSymbol);
        }
    }

    public override string GetHandlePropertyCase()
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

        var eventName = EventSymbol.Name;

        var localFunctionName = $"NativeControl{eventName}";

        var localFunctionBody = GetLocalHandlerFunctionBody();

        return $@"                case nameof({ComponentPropertyName}):
                    if (!Equals({ComponentPropertyName}, value))
                    {{
                        void {localFunctionName}(object sender, {GetTypeNameAndAddNamespace(GetEventArgType(EventSymbol.Type))} e){localFunctionBody}

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

        if (BindProperty != null)
        {
            var componentPropertyType = GetComponentPropertyTypeName(BindProperty, ContainingType);
            var mauiPropertyType = GetTypeNameAndAddNamespace(BindProperty.Type);

            argument = componentPropertyType == mauiPropertyType
                ? $"NativeControl.{BindProperty.Name}"
                : $"NativeControl.{BindProperty.Name} is {componentPropertyType} item ? item : default({componentPropertyType})";
        }
        else
        {
            argument = GetEventArgType(EventSymbol.Type).Name != nameof(EventArgs) ? "e" : "";
        }

        if (BindProperty != null && PropertyChangedEvent)
        {
            return $@"
                        {{
                            if (e.PropertyName == nameof(NativeControl.{BindProperty.Name}))
                            {{
                                var value = {argument};
                                {BindProperty.Name} = value;
                                InvokeEventCallback({ComponentPropertyName}, value);
                            }}
                        }}";
        }

        if (BindProperty != null)
        {
            return $@"
                        {{
                            var value = {argument};
                            {BindProperty.Name} = value;
                            InvokeEventCallback({ComponentPropertyName}, value);
                        }}";
        }

        return string.IsNullOrEmpty(argument)
            ? $" => InvokeEventCallback({ComponentPropertyName});"
            : $" => InvokeEventCallback({ComponentPropertyName}, {argument});";
    }

    protected override string GetXmlDocs()
    {
        return PropertyChangedEvent ? string.Empty : base.GetXmlDocs();
    }

    public static void AddEventCallbackProperties(List<GeneratedPropertyInfo> generatedProperties, GeneratedTypeInfo containingType)
    {
        var componentInfo = containingType.Settings;
        var componentType = componentInfo.TypeSymbol;

        foreach (var propertyForEvent in componentInfo.PropertyChangedEvents)
        {
            var propertyInfo = componentType.GetProperty(propertyForEvent)
                ?? throw new Exception($"Cannot find property {componentType.Name}.{propertyForEvent}.");

            var eventInfo = componentType.GetEvent("PropertyChanged", includeBaseTypes: true)!;

            var property = new EventCallbackPropertyInfo(containingType, eventInfo, propertyInfo, propertyChangedEvent: true);
            generatedProperties.Add(property);
        }

        foreach (var eventInfo in GetMembers<IEventSymbol>(containingType))
        {
            var bindProperty = GetBindProperty(eventInfo, generatedProperties);
            var property = new EventCallbackPropertyInfo(containingType, eventInfo, bindProperty);
            generatedProperties.Add(property);
        }
    }


    private static string GetEventCallbackType(GeneratedTypeInfo containingType, IEventSymbol eventInfo, IPropertySymbol? bindedProperty)
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

    private static IPropertySymbol? GetBindProperty(IEventSymbol eventSymbol, List<GeneratedPropertyInfo> generatedProperties)
    {
        return generatedProperties
            .OfType<ValuePropertyInfo>()
            .Select(p => p.MauiProperty)
            .FirstOrDefault(p =>
                eventSymbol.Name == $"{p.Name}Changed"  // e.g. Value - ValueChanged
                || eventSymbol.Name == $"{p.Name}Selected"  // e.g. Date - DateSelected
                || eventSymbol.Name == $"Is{p.Name}Changed"  // e.g. Selected - IsSelectedChanged
                || $"Is{eventSymbol.Name}" == $"{p.Name}Changed"  // e.g. IsSelected - SelectedChanged
                || $"Is{eventSymbol.Name}" == $"{p.Name}");  // e.g. IsToggled - Toggled
    }

    private static ITypeSymbol GetEventArgType(ITypeSymbol eventHandlerType)
    {
        return eventHandlerType.GetMethod("Invoke")!.Parameters[1].Type;
    }
}
