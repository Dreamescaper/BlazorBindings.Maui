using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorBindings.Maui.ComponentGenerator;

public class GenerateComponentSettings
{
    private bool _isForcedGeneric;

    public string FileHeader { get; set; }
    public string TypeAlias { get; set; }
    public INamedTypeSymbol TypeSymbol { get; set; }
    public HashSet<string> Exclude { get; set; } = new();
    public HashSet<string> Include { get; set; } = new();
    public HashSet<string> ContentProperties { get; set; } = new();
    public HashSet<string> NonContentProperties { get; set; } = new();
    public string[] PropertyChangedEvents { get; set; } = Array.Empty<string>();
    public Dictionary<string, INamedTypeSymbol> GenericProperties { get; set; } = new();
    public Dictionary<string, string> Aliases { get; set; } = new();
    public GenerateComponentSettings BaseTypeInfo { get; set; }
    public bool IsBaseTypeGeneric => BaseTypeInfo?.IsGeneric ?? false;

    public bool IsGeneric
    {
        get => _isForcedGeneric || GenericProperties.Any(p => p.Value == null) || IsBaseTypeGeneric;
        set => _isForcedGeneric = value;
    }
}
