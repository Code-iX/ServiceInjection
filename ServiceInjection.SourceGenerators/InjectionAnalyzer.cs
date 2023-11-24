using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators;

public class InjectionAnalyzer
{
    private const string InjectedAttributeName = nameof(InjectedAttribute);
    private const string ServiceInjectionAttributeName = nameof(ServiceInjectionAttribute);

    public bool TryGetGeneratedCode(INamedTypeSymbol typeSymbol, out string source, out string hintName)
    {
        source = null;
        hintName = null;

        var hasAttribute = typeSymbol.GetAttributes().Any(attr => attr.AttributeClass?.Name == ServiceInjectionAttributeName);
        if (!hasAttribute)
            return false;

        var injections = GetInjectedMembers(typeSymbol)
                .OrderBy(i => i.IsOptional)
                .ToList()
            ;

        if (injections.Count == 0)
            return false;

        source = GenerateSourceCode(typeSymbol, injections);
        hintName = GetHintName(typeSymbol);
        return true;
    }

    internal static string GetHintName(ISymbol symbol)
    {
        var fullName = $"{symbol.ContainingNamespace.ToDisplayString()}.{symbol.Name}";

        var safeFileName = fullName.Replace('.', '_');

        return safeFileName + ".g.cs";
    }


    internal IEnumerable<Injection> GetInjectedMembers(INamespaceOrTypeSymbol symbol)
    {
        return symbol.GetMembers()
                .Where(m => m.GetAttributes().Any(a => a.AttributeClass?.Name == InjectedAttributeName))
                .Select(CreateInjectionFromMember)
            ;
    }

    internal static Injection CreateInjectionFromMember(ISymbol member)
    {
        var injectedAttribute = member.GetAttributes()
            .Single(a => a.AttributeClass?.Name == InjectedAttributeName);

        return new Injection
        {
            Name = GetNameFromMember(member),
            Type = GetTypeFromMember(member),
            IsOptional = GetIsOptionalFromAttribute(injectedAttribute),
            InjectedType = GetInjectedTypeFromAttribute(injectedAttribute)
        };
    }

    internal static string GetNameFromMember(ISymbol member) =>
        member.Name;

    internal static ITypeSymbol GetTypeFromMember(ISymbol member) =>
        member switch
        {
            IPropertySymbol property => property.Type,
            IFieldSymbol field => field.Type,
            _ => throw new NotSupportedException()
        };

    internal static bool GetIsOptionalFromAttribute(AttributeData attribute) =>
        attribute.ConstructorArguments.FirstOrDefault().Value as bool? ?? false;

    internal static ITypeSymbol GetInjectedTypeFromAttribute(AttributeData attribute) =>
        attribute.ConstructorArguments
            .FirstOrDefault(arg => arg.Value is ITypeSymbol)
            .Value as ITypeSymbol;


    internal static string GenerateSourceCode(INamedTypeSymbol symbol, IReadOnlyCollection<Injection> injections)
    {
        var sourceCodeFactory = new SourceCodeFactory(symbol, injections);
        return sourceCodeFactory.GenerateSourceCode();
    }
}