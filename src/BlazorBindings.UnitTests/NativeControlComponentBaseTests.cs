using BlazorBindings.Core;

namespace BlazorBindings.UnitTests;

public class NativeControlComponentBaseTests
{
    [Test]
    public void CastParameterReturnsTypedValue()
    {
        var value = new TestComponent().CastParameterValue<int?>(42, "Count");

        Assert.That(value, Is.EqualTo(42));
    }

    [Test]
    public void CastParameterAllowsNullForNullableTypes()
    {
        var value = new TestComponent().CastParameterValue<int?>(null, "Count");

        Assert.That(value, Is.Null);
    }

    [Test]
    public void CastParameterThrowsWithParameterDetails()
    {
        var ex = Assert.Throws<ArgumentException>(() => new TestComponent().CastParameterValue<int>("abc", "Count"));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex.ParamName, Is.EqualTo("Count"));
            Assert.That(ex.Message, Does.Contain("Count"));
            Assert.That(ex.Message, Does.Contain("abc"));
            Assert.That(ex.Message, Does.Contain("TestComponent"));
            Assert.That(ex.Message, Does.Contain("Int32"));
            Assert.That(ex.Message, Does.Contain("String"));
        }
    }

    private sealed class TestComponent : NativeControlComponentBase
    {
        public T CastParameterValue<T>(object value, string parameterName)
            => CastParameter<T>(value, parameterName);
    }
}
