using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators
{
    internal static class StaticExtensions
    {
        public static IEnumerable<INamedTypeSymbol> GetAllTypes(this INamespaceOrTypeSymbol symbol)
        {
            switch (symbol)
            {
                case INamespaceSymbol namespaceSymbol:
                    foreach (var member in namespaceSymbol.GetMembers())
                    {
                        foreach (var type in GetAllTypes(member))
                        {
                            yield return type;
                        }
                    }

                    break;
                case INamedTypeSymbol namedTypeSymbol:
                    yield return namedTypeSymbol;

                    foreach (var type in namedTypeSymbol.GetTypeMembers().SelectMany(GetAllTypes))
                    {
                        yield return type;
                    }

                    break;
            }
        }
    }
}