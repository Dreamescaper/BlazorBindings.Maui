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

        var lines = str
            .Split(["\r\n", "\r", "\n"], StringSplitOptions.TrimEntries)
            .Select(line => line.Length > 0 ? indent + line : line);

        return string.Join(Environment.NewLine, lines);
    }
}
