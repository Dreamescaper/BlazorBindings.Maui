using BlazorBindings.Maui.ComponentGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlazorBindings.Maui.ComponentGenerator
{
    public class GeneratedTypeInfo
    {
        public GeneratedTypeInfo(Compilation compilation, GenerateComponentSettings settings, string typeName, string baseTypeName, ITypeSymbol mauiType, IList<UsingStatement> usings)
        {
            TypeName = typeName;
            BaseTypeName = baseTypeName;
            IsGeneric = settings.IsGeneric;
            MauiType = mauiType;
            Usings = usings;
            Compilation = compilation;
            Settings = settings;
        }

        public string TypeName { get; }
        public string BaseTypeName { get; }
        public bool IsGeneric { get; }
        public Compilation Compilation { get; }
        public GenerateComponentSettings Settings { get; }
        public ITypeSymbol MauiType { get; }
        public IList<UsingStatement> Usings { get; }

        public string GetTypeNameAndAddNamespace(string @namespace, string typeName)
        {
            var @using = Usings.FirstOrDefault(u => u.Namespace == @namespace);
            if (@using == null)
            {
                @using = new UsingStatement { Namespace = @namespace, IsUsed = true };
                Usings.Add(@using);
            }
            else
            {
                @using.IsUsed = true;
            }

            return @using.Alias == null ? typeName : $"{@using.Alias}.{typeName}";
        }

        public string GetTypeNameAndAddNamespace(ITypeSymbol type)
        {
            var typeName = type.GetCSharpTypeName();
            if (typeName != null)
            {
                return typeName;
            }

            if (type.ContainingType != null)
            {
                return $"{GetTypeNameAndAddNamespace(type.ContainingType)}.{FormatTypeName(type)}";
            }

            // Check if there's a 'using' already. If so, check if it has an alias. If not, add a new 'using'.
            var namespaceAlias = string.Empty;

            var nsName = type.ContainingNamespace.GetFullName();
            var existingUsing = Usings.FirstOrDefault(u => u.Namespace == nsName);
            if (existingUsing == null)
            {
                Usings.Add(new UsingStatement { Namespace = nsName, IsUsed = true, });
            }
            else
            {
                existingUsing.IsUsed = true;
                if (existingUsing.Alias != null)
                {
                    namespaceAlias = existingUsing.Alias + ".";
                }
            }
            typeName = namespaceAlias + FormatTypeName(type);
            return typeName;
        }

        private string FormatTypeName(ITypeSymbol type)
        {
            if (type is not INamedTypeSymbol namedType || !namedType.IsGenericType)
            {
                return type.Name;
            }
            var typeNameBuilder = new StringBuilder();
            typeNameBuilder.Append(type.Name);
            typeNameBuilder.Append('<');
            var genericArgs = namedType.TypeArguments;
            for (var i = 0; i < genericArgs.Length; i++)
            {
                if (i > 0)
                {
                    typeNameBuilder.Append(", ");
                }
                typeNameBuilder.Append(GetTypeNameAndAddNamespace(genericArgs[i]));

            }
            typeNameBuilder.Append('>');
            return typeNameBuilder.ToString();
        }
    }
}
