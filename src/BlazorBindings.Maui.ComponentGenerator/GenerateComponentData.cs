using Microsoft.CodeAnalysis;

namespace BlazorBindings.Maui.ComponentGenerator;

public class GenerateComponentSettings
{
    public string FileHeader { get; set; }
    public string? TypeAlias { get; set; }
    public INamedTypeSymbol TypeSymbol { get; set; }
    public HashSet<string> Exclude { get; set; } = [];
    public HashSet<string> Include { get; set; } = [];
    public HashSet<string> ContentProperties { get; set; } = [];
    public HashSet<string> NonContentProperties { get; set; } = [];
    public string[] PropertyChangedEvents { get; set; } = [];
    public Dictionary<string, INamedTypeSymbol?> GenericProperties { get; set; } = [];
    public Dictionary<string, string> Aliases { get; set; } = [];
    public bool IsGeneric { get; set; }
    public bool? MakeItemsGeneric { get; set; }
}
