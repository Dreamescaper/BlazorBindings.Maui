using BlazorBindings.Maui;
using System.Globalization;

namespace BlazorBindings.UnitTests;

public class StringConverterTests
{
    private static readonly Guid _testGuid = Guid.NewGuid();

    public static IEnumerable<TestCaseData> TryParseTestData
    {
        get
        {
            yield return new TestCaseData("s", typeof(string), "s", true).SetArgDisplayNames("Parse valid string");
            yield return new TestCaseData("5", typeof(int), 5, true).SetArgDisplayNames("Parse valid int");
            yield return new TestCaseData("5", typeof(int?), 5, true).SetArgDisplayNames("Parse valid int?");
            yield return new TestCaseData("invalid text", typeof(int), null, false).SetArgDisplayNames("Parse invalid int");
            yield return new TestCaseData("B", typeof(TestEnum), TestEnum.B, true).SetArgDisplayNames("Parse valid enum.");
            yield return new TestCaseData("B", typeof(TestEnum?), TestEnum.B, true).SetArgDisplayNames("Parse valid nullable enum.");
            yield return new TestCaseData("-1", typeof(TestEnum), null, false).SetArgDisplayNames("Parse invalid enum.");
            yield return new TestCaseData("2020-05-20", typeof(DateTime), new DateTime(2020, 05, 20), true).SetArgDisplayNames("Parse valid date");
            yield return new TestCaseData("invalid text", typeof(DateTime), null, false).SetArgDisplayNames("Parse invalid date");
            yield return new TestCaseData("2020-05-20", typeof(DateOnly), new DateOnly(2020, 05, 20), true).SetArgDisplayNames("Parse valid DateOnly");
            yield return new TestCaseData("invalid text", typeof(DateOnly), null, false).SetArgDisplayNames("Parse invalid DateOnly");
            yield return new TestCaseData("12:36:04", typeof(TimeOnly), new TimeOnly(12, 36, 04), true).SetArgDisplayNames("Parse valid TimeOnly");
            yield return new TestCaseData("invalid text", typeof(TimeOnly), null, false).SetArgDisplayNames("Parse invalid DateOnly");
            yield return new TestCaseData("true", typeof(bool), true, true).SetArgDisplayNames("Parse valid bool");
            yield return new TestCaseData("Yes", typeof(bool), null, false).SetArgDisplayNames("Parse invalid bool");
            yield return new TestCaseData(_testGuid.ToString(), typeof(Guid), _testGuid, true).SetArgDisplayNames("Parse valid GUID");
            yield return new TestCaseData(_testGuid.ToString(), typeof(Guid?), _testGuid, true).SetArgDisplayNames("Parse valid GUID?");
            yield return new TestCaseData("invalid text", typeof(Guid), null, false).SetArgDisplayNames("Parse invalid GUID");
            yield return new TestCaseData("{'value': '5'}", typeof(object), null, false).SetArgDisplayNames("Parse POCO should find null operation");
        }
    }

    [TestCaseSource(nameof(TryParseTestData))]
    public void TryParseTest(string s, Type type, object expectedResult, bool expectedSuccess)
    {
        var success = StringConverter.TryParse(type, s, CultureInfo.InvariantCulture, out var result);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(expectedResult));
            Assert.That(success, Is.EqualTo(expectedSuccess));
        });
    }

    private enum TestEnum { A, B, C }
}
