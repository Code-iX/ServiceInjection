using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators.Models;

internal sealed record Injection
{
    public Injection()
    {
    }

    public Injection(string name, bool isOptional, ITypeSymbol type, ITypeSymbol injectedType)
    {
        Name = name;
        Type = type;
        InjectedType = injectedType;
        IsOptional = isOptional;
    }

    public string Name { get; set; }

    public bool IsOptional { get; set; }

    public ITypeSymbol Type { get; set; }

    public ITypeSymbol InjectedType { get; set; }
}