using static ServiceInjection.SourceGenerators.Tests.Verifier;

namespace ServiceInjection.SourceGenerators.Tests;

[TestClass]
public class ServiceInjectionGeneratorTests
{
    [TestInitialize]
    public void Init()
    {
        // Initialization code before test method is executed
    }

    [TestMethod]
    [DataRow("Test1", "MyNamespace_MyName")]
    [DataRow("Test2", "TestClass_OneField_OneProperty")]
    public async Task TestMyMethod(string testName, string generatedFileName)
    {
        var code = File.ReadAllText($"SourceCode/{testName}.cs");
        var generated = File.ReadAllText($"SourceCode/{testName}.g.cs");

        await VerifySourceCode(code, $"{generatedFileName}.g.cs", generated);
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Cleanup code after test method did execute
    }
}