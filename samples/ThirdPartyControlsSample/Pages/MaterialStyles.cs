namespace ThirdPartyControlsSample.Pages;

public static class MaterialStyles
{
    public static Style ElevatedButton => GetStyle("ElevatedButtonStyle");
    public static Style FilledButton => GetStyle("FilledButtonStyle");
    public static Style FilledTonalButton => GetStyle("FilledTonalButtonStyle");
    public static Style OutlinedButton => GetStyle("OutlinedButtonStyle");
    public static Style TextButton => GetStyle("TextButtonStyle");
    public static Style FilledComboBox => GetStyle("FilledComboBoxStyle");
    public static Style OutlinedComboBox => GetStyle("OutlinedComboBoxStyle");
    public static Style FilledCard => GetStyle("FilledCardStyle");
    public static Style ElevatedCard => GetStyle("ElevatedCardStyle");
    public static Style OutlinedCard => GetStyle("OutlinedCardStyle");

    public static Style GetStyle(string key)
    {
        return Application.Current.Resources.TryGetValue(key, out var style) ? (Style)style : null;
    }
}
