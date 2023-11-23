using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators
{
    internal class Injection
    {
        public string Name { get; set; }

        public bool IsOptional { get; set; }

        public ITypeSymbol Type { get; set; }

        public ITypeSymbol InjectedType { get; set; }
    }
}