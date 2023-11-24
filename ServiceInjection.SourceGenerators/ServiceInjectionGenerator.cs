using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators;

[Generator]
public class ServiceInjectionGenerator : ISourceGenerator
{
    private readonly InjectionAnalyzer _injectionAnalyzer = new();

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;

        foreach (var typeSymbol in context.Compilation.GlobalNamespace.GetAllTypes())
        {
            // Check if the generator has been cancelled. If so, then stop working.
            cancellationToken.ThrowIfCancellationRequested();

            if (_injectionAnalyzer.TryGetGeneratedCode(typeSymbol, out var source, out var hintName))
            {
                context.AddSource(hintName, source);
            }
        }
    }
}