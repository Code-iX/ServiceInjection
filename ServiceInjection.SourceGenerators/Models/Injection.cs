using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators.Models;

internal sealed record Injection
{
    public string Name { get; set; }

    public bool IsOptional { get; set; }

    public ITypeSymbol Type { get; set; }

    public ITypeSymbol InjectedType { get; set; }
}