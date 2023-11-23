using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators;

[Generator]
public class ServiceInjectionGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var fullName = typeof(ServiceInjectionAttribute).FullName;

        foreach (var typeSymbol in context.Compilation.GlobalNamespace.GetAllTypes())
        {
            var hasAttribute = typeSymbol.GetAttributes().Any(attr => attr.AttributeClass?.ToDisplayString() == fullName);
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
                .Where(m => m.GetAttributes().Any(a => a.AttributeClass?.Name == "InjectedAttribute"))
                .Select(CreateInjectionFromMember)
            ;
    }

    internal static Injection CreateInjectionFromMember(ISymbol member)
    {
        var injectedAttribute = member.GetAttributes()
            .Single(a => a.AttributeClass?.Name == "InjectedAttribute");

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
        var fac = new SourceCodeFactory(symbol, injections)
        {
            DateTimeProvided = DateTime.Now
        };
        return fac.CreateSourceCode();
    }
}