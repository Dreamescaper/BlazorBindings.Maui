using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;

namespace BlazorBindings.Maui.ComponentGenerator.Model;

internal class CommandEventCallbackPropertyInfo : GeneratedPropertyInfo
{
    /// <summary>Name of the ICommand property on the MAUI type (e.g. "ThresholdCommand").</summary>
    public string CommandPropertyName { get; }

    public override ISymbol? MemberSymbol => null;

    public CommandEventCallbackPropertyInfo(
        GeneratedTypeInfo containingType,
        string commandPropertyName,
        string eventCallbackName) : base(containingType)
    {
        CommandPropertyName = commandPropertyName;
        ComponentPropertyName = eventCallbackName;
        ComponentType = "EventCallback";
    }

    public override string GetHandlePropertyCase()
    {
        /*
            case nameof(OnThreshold):
                if (!Equals(OnThreshold, value))
                {
                    OnThreshold = CastParameter<EventCallback>(value, name);
                    NativeControl.ThresholdCommand = OnThreshold.HasDelegate
                        ? new MC.Command(() => InvokeEventCallback(OnThreshold))
                        : null;
                }
                break;
        */

        return $@"                case nameof({ComponentPropertyName}):
                    if (!Equals({ComponentPropertyName}, value))
                    {{
                        {ComponentPropertyName} = CastParameter<EventCallback>(value, name);
                        NativeControl.{CommandPropertyName} = {ComponentPropertyName}.HasDelegate
                            ? new MC.Command(() => InvokeEventCallback({ComponentPropertyName}))
                            : null;
                    }}
                    break;
";
    }

    public static void AddCommandEventCallbackProperties(List<GeneratedPropertyInfo> generatedProperties, GeneratedTypeInfo containingType)
    {
        foreach (var (commandPropName, eventCallbackName) in containingType.Settings.CommandEvents)
        {
            generatedProperties.Add(new CommandEventCallbackPropertyInfo(containingType, commandPropName, eventCallbackName));
        }
    }
}
