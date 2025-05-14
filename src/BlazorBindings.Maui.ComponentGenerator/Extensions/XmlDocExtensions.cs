using Microsoft.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace BlazorBindings.Maui.ComponentGenerator2.Extensions;

public static partial class XmlDocExtensions
{
    public static string? GetXmlDocContents(this ISymbol symbol)
    {
        var xmlDocString = symbol.GetDocumentationCommentXml();

        if (string.IsNullOrEmpty(xmlDocString))
        {
            return null;
        }

        var xmlDoc = new XmlDocument();

        // Try parse raw doc string (should have root element)
        try
        {
            xmlDoc.LoadXml(xmlDocString);
        }
        catch
        {
            // Fallback to old method if not.
            xmlDoc.LoadXml($"<member>{xmlDocString}</member>");
        }

        var xmlDocNode = xmlDoc.FirstChild;

        var xmlDocContents = string.Empty;
        // Format of XML docs we're looking for in a given property:
        // <member name="P:Xamarin.Forms.ActivityIndicator.Color">
        //     <summary>Gets or sets the <see cref="T:Xamarin.Forms.Color" /> of the ActivityIndicator. This is a bindable property.</summary>
        //     <value>A <see cref="T:Xamarin.Forms.Color" /> used to display the ActivityIndicator. Default is <see cref="P:Xamarin.Forms.Color.Default" />.</value>
        //     <remarks />
        // </member>

        var summaryText = GetXmlDocText(xmlDocNode["summary"]);
        var valueText = GetXmlDocText(xmlDocNode["value"]);

        if (summaryText != null || valueText != null)
        {
            var xmlDocContentBuilder = new StringBuilder();
            if (summaryText != null)
            {
                xmlDocContentBuilder.AppendLine("/// <summary>");
                xmlDocContentBuilder.AppendLine($"/// {summaryText}");
                xmlDocContentBuilder.AppendLine("/// </summary>");
            }
            if (valueText != null)
            {
                xmlDocContentBuilder.AppendLine("/// <value>");
                xmlDocContentBuilder.AppendLine($"/// {valueText}");
                xmlDocContentBuilder.AppendLine("/// </value>");
            }
            xmlDocContents = xmlDocContentBuilder.ToString();
        }
        return xmlDocContents;

        static string GetXmlDocText(XmlElement xmlDocElement)
        {
            var allText = xmlDocElement?.InnerXml;
            allText = allText?.Replace("To be added.", "").Replace("This is a bindable property.", "");
            allText = allText is null ? null : MultipleSpacesRegex().Replace(allText, " ");
            return string.IsNullOrWhiteSpace(allText) ? null : allText.Trim();
        }
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MultipleSpacesRegex();

}
