using CodeIX.ServiceInjection.SourceGenerators.GeneratorModel;
using CodeIX.ServiceInjection.SourceGenerators.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ServiceInjection.SourceGenerators.Tests.Helper;

namespace ServiceInjection.SourceGenerators.Tests.GeneratorModel;

[TestClass]
public class ArgumentResolverTests
{
    [TestMethod]
    public void Match_ShouldReturnCorrectInjectionsForTwoDistinctParameters()
    {
        // Arrange
        const string sourceCode = """
                                  public class DummyClass1 {}
                                  public class DummyClass2 {}
                                  public class MyClass {
                                      public MyClass(DummyClass1 p11, DummyClass2 p2) {}
                                  }
                                  """;

        var semanticModel = sourceCode.CreateSemanticModel();
        var parameters = semanticModel.GetParameters("MyClass");
        var injections = new List<Injection>
        {
            new("Injection1", false, semanticModel.GetTypeSymbol("DummyClass1"), null),
            new("Injection2", false, semanticModel.GetTypeSymbol("DummyClass2"), null),
        };

        // Act
        var resolver = new ArgumentResolver(injections, parameters);
        var param1 = resolver.Match(parameters[0]);
        var param2 = resolver.Match(parameters[1]);

        // Assert
        Assert.AreEqual("Injection1", param1);
        Assert.AreEqual("Injection2", param2);
    }

    [TestMethod]
    public void Match_ShouldReturnDefaultWhenNoMatchFound()
    {
        // Arrange
        const string sourceCode = """
                                  public class DummyClass1 {}
                                  public class DummyClass2 {}
                                  public class MyClass {
                                      public MyClass(DummyClass1 p) {}
                                  }
                                  """;

        var semanticModel = sourceCode.CreateSemanticModel();
        var parameters = semanticModel.GetParameters("MyClass");

        var injections = new List<Injection>
        {
            new()
            {
                Name = "Injection",
                Type = semanticModel.GetTypeSymbol("DummyClass2"),
                InjectedType = null
            },
        };

        // Act
        var resolver = new ArgumentResolver(injections, parameters);
        var param1 = resolver.Match(parameters[0]);

        // Assert
        Assert.AreEqual("default", param1);
    }

    [TestMethod]
    public void Match_ShouldReturnMatchForInterface()
    {
        // Arrange
        const string sourceCode = """
                                  public interface IDummyInterface {}
                                  public class DummyClass : IDummyInterface {}
                                  public class MyClass {
                                      public MyClass(IDummyInterface p1) {}
                                  }
                                  """;

        var semanticModel = sourceCode.CreateSemanticModel();
        var parameters = semanticModel.GetParameters("MyClass");

        var injections = new List<Injection>
        {
            new()
            {
                Name = "Injection",
                Type = semanticModel.GetTypeSymbol("DummyClass"),
                InjectedType = null
            },
        };

        // Act
        var resolver = new ArgumentResolver(injections, parameters);
        var param1 = resolver.Match(parameters[0]);

        // Assert
        Assert.AreEqual("Injection", param1);
    }

    [TestMethod]
    public void Match_ShouldReturnDefaultWhenNoDirectMatch()
    {
        // Arrange
        const string sourceCode = """
                                  public interface IDummyInterface {}
                                  public class DummyClass : IDummyInterface {}
                                  public class MyClass {
                                      public MyClass(DummyClass p1) {}
                                  }
                                  """;

        var semanticModel = sourceCode.CreateSemanticModel();
        var parameters = semanticModel.GetParameters("MyClass");

        var injections = new List<Injection>
        {
            new()
            {
                Name = "Injection",
                Type = semanticModel.GetTypeSymbol("IDummyInterface"),
                InjectedType = null
            },
        };

        // Act
        var resolver = new ArgumentResolver(injections, parameters);
        var param1 = resolver.Match(parameters[0]);

        // Assert
        Assert.AreEqual("default", param1);
    }

    [TestMethod]
    public void Match_ShouldReturnCorrectInjectionForMultipleCompatibleTypes()
    {
        // Arrange
        const string sourceCode = """
                                  public interface IDummyInterface {}
                                  public class DummyClass1 : IDummyInterface {}
                                  public class DummyClass2 : IDummyInterface {}
                                  public class MyClass {
                                      public MyClass(IDummyInterface p1, IDummyInterface p2) {}
                                  }
                                  """;

        var semanticModel = sourceCode.CreateSemanticModel();
        var parameters = semanticModel.GetParameters("MyClass");

        var injections = new List<Injection>
        {
            new()
            {
                Name = "_p1",
                Type = semanticModel.GetTypeSymbol("DummyClass1"),
                InjectedType = null
            },
            new()
            {
                Name = "_p2",
                Type = semanticModel.GetTypeSymbol("DummyClass2"),
                InjectedType = null
            },
        };

        // Act
        var resolver = new ArgumentResolver(injections, parameters);
        var param1 = resolver.Match(parameters[0]);
        var param2 = resolver.Match(parameters[1]);

        // Assert
        Assert.AreEqual("_p1", param1);
        Assert.AreEqual("_p2", param2);
    }

    [TestMethod]
    public void Match_ShouldReturnCorrectOrder()
    {
        // Arrange
        const string sourceCode = """
                                  public interface IDummyInterface {}
                                  public class DummyClass1 : IDummyInterface {}
                                  public class DummyClass2 : IDummyInterface {}
                                  public class MyClass {
                                      public MyClass(DummyClass2 p1, DummyClass1 p2) {}
                                  }
                                  """;

        var semanticModel = sourceCode.CreateSemanticModel();
        var parameters = semanticModel.GetParameters("MyClass");

        var injections = new List<Injection>
        {
            new()
            {
                Name = "_p1",
                Type = semanticModel.GetTypeSymbol("DummyClass1"),
                InjectedType = null
            },
            new()
            {
                Name = "_p2",
                Type = semanticModel.GetTypeSymbol("DummyClass2"),
                InjectedType = null
            },
        };

        // Act
        var resolver = new ArgumentResolver(injections, parameters);
        var param1 = resolver.Match(parameters[0]);
        var param2 = resolver.Match(parameters[1]);

        // Assert
        Assert.AreEqual("_p2", param1);
        Assert.AreEqual("_p1", param2);
    }

    [TestMethod]
    public void Match_ShouldReturnInjectionWhenOneAvailableAndNoNameMatch()
    {
        // Arrange
        const string sourceCode = """
                                  public class DummyClass {}
                                  public class MyClass {
                                      public MyClass(DummyClass p1, DummyClass p2) {}
                                  }
                                  """;

        var semanticModel = sourceCode.CreateSemanticModel();
        var parameters = semanticModel.GetParameters("MyClass");

        var injections = new List<Injection>
        {
            new()
            {
                Name = "Injection",
                Type = semanticModel.GetTypeSymbol("DummyClass"),
                InjectedType = null
            }
        };

        // Act
        var resolver = new ArgumentResolver(injections, parameters);
        var param1 = resolver.Match(parameters[0]);
        var param2 = resolver.Match(parameters[1]);

        // Assert
        Assert.AreEqual("Injection", param1);
        Assert.AreEqual("default", param2);
    }

    [TestMethod]
    public void Match_ShouldChooseBestMatchBasedOnName()
    {
        // Arrange
        const string sourceCode = """
                                  public class DummyClass {}
                                  public class MyClass {
                                      public MyClass(DummyClass p1, DummyClass p2) {}
                                  }
                                  """;

        var semanticModel = sourceCode.CreateSemanticModel();
        var parameters = semanticModel.GetParameters("MyClass");

        var injections = new List<Injection>
        {
            new()
            {
                Name = "P2",
                Type = semanticModel.GetTypeSymbol("DummyClass"),
                InjectedType = null
            }
        };

        // Act
        var resolver = new ArgumentResolver(injections, parameters);
        var param1 = resolver.Match(parameters[0]);
        var param2 = resolver.Match(parameters[1]);

        // Assert
        Assert.AreEqual("default", param1);
        Assert.AreEqual("P2", param2);
    }

    [TestMethod]
    public void Match_ShouldReturnDefaultWhenPerfectNameNotTypeCompatible()
    {
        // Arrange
        const string sourceCode = """
                                  public class DummyClass1 {}
                                  public class DummyClass2 {}
                                  public class MyClass {
                                      public MyClass(DummyClass2 injection) {}
                                  }
                                  """;

        var semanticModel = sourceCode.CreateSemanticModel();
        var parameters = semanticModel.GetParameters("MyClass");

        var injections = new List<Injection>
        {
            new()
            {
                Name = "injection",
                Type = semanticModel.GetTypeSymbol("DummyClass1"),
                InjectedType = null
            }
        };

        // Act
        var resolver = new ArgumentResolver(injections, parameters);
        var param1 = resolver.Match(parameters[0]);

        // Assert
        Assert.AreEqual("default", param1);
    }
}