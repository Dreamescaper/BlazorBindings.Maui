using System.Diagnostics;

#pragma warning disable CS9113 // Parameter is unread. Type is used by Component Generator.

namespace BlazorBindings.Maui.ComponentGenerator;

[Conditional("CODE_ANALYSIS")]
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class GenerateComponentAttribute(Type typeToGenerate) : Attribute
{
    /// <summary>
    /// Exclude members from generation.
    /// </summary>
    public string[] Exclude { get; set; }

    /// <summary>
    /// Include members to generation which would usually be excluded.
    /// </summary>
    public string[] Include { get; set; }

    /// <summary>
    /// Generate EventCallback for property based on PropertyChanged event.
    /// </summary>
    public string[] PropertyChangedEvents { get; set; }

    /// <summary>
    /// Generates generic component with generic properties.
    /// </summary>
    public string[] GenericProperties { get; set; }

    /// <summary>
    /// Generates property as RenderFragment.
    /// </summary>
    public string[] ContentProperties { get; set; }

    /// <summary>
    /// Makes property to be generated with original type,
    /// when it usually would be generated as RenderFragment.
    /// </summary>
    public string[] NonContentProperties { get; set; }

    /// <summary>
    /// Generate parameters with different name. Format: "{MauiName}:{GeneratedName}".
    /// </summary>
    public string[] Aliases { get; set; }

    /// <summary>
    /// Generate generic component type.
    /// </summary>
    public bool IsGeneric { get; set; }
}