using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace ServiceInjection.SourceGenerator.Tests;

[TestClass]
public class ServiceInjectionSourceGeneratorTests
{
    [TestMethod]
    public void GetHintName_ReturnsCorrectFormat()
    {
        // Arrange
        var namespaceSymbol = Substitute.For<INamespaceSymbol>();
        namespaceSymbol.ToDisplayString().Returns("MyNamespace");

        var symbol = Substitute.For<ISymbol>();
        symbol.ContainingNamespace.Returns(namespaceSymbol);
        symbol.Name.Returns("MyClass");

        // Act
        var result = ServiceInjectionSourceGenerator.GetHintName(symbol);

        // Assert
        Assert.AreEqual("MyNamespace_MyClass.g.cs", result);
    }

    [TestMethod]
    public void CreateInjectionFromMember_ReturnsCorrectInjection()
    {
        // Arrange
        var attributeData = Substitute.For<AttributeData>();
        var injectedType = Substitute.For<ITypeSymbol>();
        injectedType.Name.Returns("InjectedType");


        var member = Substitute.For<ISymbol>();
        member.GetAttributes().Returns(new[] { attributeData }.ToImmutableArray());
        member.Name.Returns("memberName");

        // Act
        var injection = ServiceInjectionSourceGenerator.CreateInjectionFromMember(member);

        // Assert
        Assert.AreEqual("memberName", injection.Name);
        Assert.AreEqual(injectedType, injection.InjectedType);
        // Add more assertions as necessary
    }

    // Additional tests for other methods...

    // Mock helper methods
    private INamedTypeSymbol CreateFakeNamedTypeSymbol()
    {
        var namedTypeSymbol = Substitute.For<INamedTypeSymbol>();
        // Set up the namedTypeSymbol as needed for your tests
        return namedTypeSymbol;
    }
}