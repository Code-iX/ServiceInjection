using System.Text;
using CodeIX.ServiceInjection;
using CodeIX.ServiceInjection.SourceGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ServiceInjection.SourceGenerators.Tests;

public static class Verifier
{
    public static async Task VerifySourceCode(string code, string filename, string generated)
    {
       var test = new CSharpSourceGeneratorVerifier<ServiceInjectionGenerator>.Test
        {
            TestState =
            {
                Sources =
                {
                    code
                },
                GeneratedSources =
                {
                    (typeof(ServiceInjectionGenerator), filename, SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                },
                AdditionalReferences =
                {
                    MetadataReference.CreateFromFile(typeof(ServiceInjectionAttribute).Assembly.Location)
                },
            },
        };

        await test.RunAsync();
    }
}