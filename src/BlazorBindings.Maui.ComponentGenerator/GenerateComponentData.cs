using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace BlazorBindings.Maui.ComponentGenerator
{
    public class GenerateComponentSettings
    {
        public string FileHeader { get; set; }
        public INamedTypeSymbol TypeSymbol { get; set; }
        public HashSet<string> Exclude { get; set; }
        public HashSet<string> Include { get; set; }
        public HashSet<string> ContentProperties { get; set; }
        public string[] PropertyChangedEvents { get; set; }
        public Dictionary<string, INamedTypeSymbol> GenericProperties { get; set; }
        public Dictionary<string, string> Aliases { get; set; }
        public GenerateComponentSettings BaseTypeInfo { get; set; }
        public bool IsBaseTypeGeneric => BaseTypeInfo?.IsGeneric ?? false;
        public bool IsGeneric => GenericProperties.Any(p => p.Value == null) || IsBaseTypeGeneric;
    }
}
