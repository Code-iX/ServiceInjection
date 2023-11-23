using Microsoft.CodeAnalysis;

namespace ServiceInjection.SourceGenerator
{
    internal class Injection
    {
        public string Name { get; set; }
        public bool Required { get; set; }

        public ITypeSymbol Type { get; set; }

        public ITypeSymbol InjectedType { get; set; }
    }
}