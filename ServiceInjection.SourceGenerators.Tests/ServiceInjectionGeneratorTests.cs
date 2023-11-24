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
    [DataRow("TestClass_OneField")]
    [DataRow("TestClass_OneField_OneProperty")]
    public async Task TestMyMethod(string fileName)
    {
        var code = File.ReadAllText($"SourceCode/{fileName}.cs");
        var generated = File.ReadAllText($"SourceCode/{fileName}.g.cs");

        await VerifySourceCode(code, $"{fileName}.g.cs", generated);
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Cleanup code after test method did execute
    }
}