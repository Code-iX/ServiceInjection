using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators;

[Generator]
public class ServiceInjectionGenerator : ISourceGenerator
{
    private static readonly string InjectedAttributeName = nameof(InjectedAttribute);
    private static readonly string ServiceInjectionAttributeName = nameof(ServiceInjectionAttribute);

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var typeSymbol in context.Compilation.GlobalNamespace.GetAllTypes())
        {
            var hasAttribute = typeSymbol.GetAttributes().Any(attr => attr.AttributeClass?.Name == ServiceInjectionAttributeName);
            if (!hasAttribute)
                continue;

            GenerateCodeForClass(context, typeSymbol);
        }
    }

    internal void GenerateCodeForClass(GeneratorExecutionContext context, INamedTypeSymbol symbol)
    {
        var injections = GetInjectedMembers(symbol)
                .OrderBy(i => !i.Required)
                .ToList()
            ;

        if (injections.Count == 0)
            return;

        var source = GenerateSourceCode(symbol, injections);
        var hintName = GetHintName(symbol);

        context.AddSource(hintName, source);
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
            Required = GetRequiredFromAttribute(injectedAttribute),
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

    internal static bool GetRequiredFromAttribute(AttributeData attribute) =>
        attribute.ConstructorArguments.FirstOrDefault().Value as bool? ?? true;

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