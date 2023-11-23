using CodeIX.ServiceInjection.SourceGenerators;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceInjection.SourceGenerators.Tests;

[TestClass]
public class InjectionTests
{
    [TestMethod]
    [Description("Test the default values of the Injection class.")]
    public void TestMyMethod()
    {
        // Arrange
        var injection = new Injection();

        // Act

        // Assert
        Assert.IsNotNull(injection);
        Assert.IsFalse(injection.Required);
        Assert.IsNull(injection.Type);
        Assert.IsNull(injection.InjectedType);
    }
}