﻿using CodeIX.ServiceInjection.SourceGenerators.Models;

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
        Assert.IsFalse(injection.IsOptional);
        Assert.IsNull(injection.Type);
        Assert.IsNull(injection.InjectedType);
    }
}