using BlazorBindings.Core;

namespace BlazorBindings.UnitTests;

public class NativeControlComponentBaseTests
{
    [Test]
    public void CastParameterReturnsTypedValue()
    {
        var value = TestComponent.CastParameterValue<int?>(42, "Count");

        Assert.That(value, Is.EqualTo(42));
    }

    [Test]
    public void CastParameterAllowsNullForNullableTypes()
    {
        var value = TestComponent.CastParameterValue<int?>(null, "Count");

        Assert.That(value, Is.Null);
    }

    [Test]
    public void CastParameterThrowsWithParameterDetails()
    {
        var ex = Assert.Throws<ArgumentException>(() => TestComponent.CastParameterValue<int>("abc", "Count"));

        Assert.Multiple(() =>
        {
            Assert.That(ex.ParamName, Is.EqualTo("Count"));
            Assert.That(ex.Message, Does.Contain("Count"));
            Assert.That(ex.Message, Does.Contain("abc"));
            Assert.That(ex.Message, Does.Contain("System.Int32"));
            Assert.That(ex.Message, Does.Contain("System.String"));
        });
    }

    private sealed class TestComponent : NativeControlComponentBase
    {
        public static T CastParameterValue<T>(object value, string parameterName)
            => CastParameter<T>(value, parameterName);
    }
}
