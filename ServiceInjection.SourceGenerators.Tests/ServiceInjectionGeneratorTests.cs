using static ServiceInjection.SourceGenerators.Tests.Helper.Verifier;

namespace ServiceInjection.SourceGenerators.Tests;

[TestClass]
public class ServiceInjectionGeneratorTests
{
    private const string SourceCodeDirectory = "SourceCode/";
    private const string FileSuffix = ".cs";
    private const string GeneratedFileSuffix = ".g" + FileSuffix;

    [TestMethod]
    [DataRow("TestClass_OneField")]
    [DataRow("TestClass_OneField_OneProperty")]
    [DataRow("MyNamespace_TestClass_OneField_WithNamespace")]
    public async Task TestSourceGenerator(string fileName)
    {
        var originalFilePath = GetFullFilePath(fileName, false);
        var generatedFilePath = GetFullFilePath(fileName, true);

        var originalCode = ReadFileContent(originalFilePath);
        var generatedCode = ReadFileContent(generatedFilePath);

        await VerifySourceCode(originalCode, generatedFilePath, generatedCode);
    }

    private static string GetFullFilePath(string fileName, bool isGenerated)
    {
        var fileExtension = isGenerated ? GeneratedFileSuffix : FileSuffix;
        return $"{fileName}{fileExtension}";
    }

    private static string ReadFileContent(string filePath)
    {
        filePath = Path.Combine(SourceCodeDirectory, filePath);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var content = File.ReadAllText(filePath)
            .Replace("\r\n", "\n")
            .Replace("\n", "\r\n");
        return content;
    }
}