using CodeIX.ServiceInjection.SourceGenerators.Helpers;
using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators.GeneratorModel;

[Generator]
public class ServiceInjectionGenerator : ISourceGenerator
{
    private InjectionAnalyzer _injectionAnalyzer;

    public void Initialize(GeneratorInitializationContext context)
    {
        _injectionAnalyzer = new InjectionAnalyzer();
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;

        foreach (var typeSymbol in context.Compilation.GlobalNamespace.GetAllTypes())
        {
            // Check if the generator has been cancelled. If so, then stop working.
            cancellationToken.ThrowIfCancellationRequested();

            if (!_injectionAnalyzer.TryGetGeneratedCode(typeSymbol, out var source, out var hintName))
                continue;

            context.AddSource(hintName, source);
        }
    }
}