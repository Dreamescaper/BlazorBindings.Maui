namespace BlazorBindings.Maui.ComponentGenerator;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
#pragma warning disable CS9113 // Parameter is unread. Type is used by Component Generator.
public class GenerateComponentsFromAssemblyAttribute(Type containingType) : Attribute
#pragma warning restore CS9113 // Parameter is unread.
{
    public string TypeNamePrefix { get; set; }
}