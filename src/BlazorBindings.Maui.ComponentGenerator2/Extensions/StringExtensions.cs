namespace BlazorBindings.Maui.ComponentGenerator.Extensions;

internal static class StringExtensions
{
    private static readonly List<string> ReservedKeywords = ["class"];

    public static string EscapeIdentifierName(this string name)
    {
        return ReservedKeywords.Contains(name, StringComparer.Ordinal)
            ? $"@{name}"
            : name;
    }

    public static string? Indent(this string str, string indent)
    {
        if (str is null)
        {
            return null;
        }

        return indent + str.Replace("\n", "\n" + indent);
    }
}
