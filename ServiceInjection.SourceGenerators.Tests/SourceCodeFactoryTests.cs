﻿using CodeIX.ServiceInjection;

namespace ServiceInjection.SourceGenerators.Tests;

[TestClass]
public class SourceCodeFactoryTests
{
    [TestMethod]
    public void TestGenerateSourceCode()
    {
//        // Arrange
//        // create a INamedTypeSymbol with Namespace and Name
//        var symbol = Substitute.For<INamedTypeSymbol>();
//        var namespaceSymbol = Substitute.For<INamespaceSymbol>();
//        namespaceSymbol.ToDisplayString().Returns("MyNamespace");
//        symbol.ContainingNamespace.Returns(namespaceSymbol);
//        symbol.Constructors.Returns(new List<IMethodSymbol>().ToImmutableArray());
//        symbol.Name.Returns("MyName");

//        var typeSymbol = Substitute.For<ITypeSymbol>();
//        typeSymbol.Name.Returns("MyClass");

//        var injections = new List<Injection>
//        {
//            new()
//            {
//                Name = "MyDependentName",
//                Type = typeSymbol
//            }
//        };

//        // Act
//        var sourceCodeFactory = new SourceCodeFactory(symbol, injections);
//        var sourceCode = sourceCodeFactory.GenerateSourceCode();

//        // Assert
//        Assert.IsNotNull(sourceCode);

//        const string expected = @"// <auto-generated />
//using System;

//namespace MyNamespace
//{
//    partial class MyName
//    {
//        public MyName(MyClass MyDependentName)
//        {
//            this.MyDependentName = MyDependentName ?? throw new ArgumentNullException(nameof(MyDependentName));
//        }
//    }
//}";
//        Assert.AreEqual(expected, sourceCode);
    }
}