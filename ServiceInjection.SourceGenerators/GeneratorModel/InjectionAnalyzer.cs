using System;
using System.Collections.Generic;
using System.Linq;
using CodeIX.ServiceInjection.SourceGenerators.Models;

using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators.GeneratorModel;

public class InjectionAnalyzer
{
    public bool TryGetGeneratedCode(INamedTypeSymbol typeSymbol, out string source, out string hintName)
    {
        source = null;
        hintName = null;

        if (!typeSymbol.GetAttributes().Any(IsServiceInjection))
            return false;

        var injections = GetInjectedMembers(typeSymbol)
                .OrderBy(i => i.IsOptional)
                .ToList();

        if (injections.Count == 0)
            return false;

        source = GenerateSourceCode(typeSymbol, injections);
        hintName = GetHintName(typeSymbol);
        return true;
    }


    internal static string GetHintName(ISymbol symbol)
    {
        var fullName = symbol.ContainingNamespace.IsGlobalNamespace
            ? symbol.Name
            : $"{symbol.ContainingNamespace.ToDisplayString()}.{symbol.Name}";

        return fullName.Replace('.', '_') + ".g.cs";
    }


    private static IEnumerable<Injection> GetInjectedMembers(INamespaceOrTypeSymbol symbol) =>
        symbol.GetMembers()
            .Select(m => (Member: m, Attribute: m.GetAttributes().FirstOrDefault(IsInjectedAttribute)))
            .Where(x => x.Attribute != null)
            .Select(x => CreateInjectionFromMember(x.Member, x.Attribute));

    private static Injection CreateInjectionFromMember(ISymbol member, AttributeData injectedAttribute) =>
        new()
        {
            Name = GetNameFromMember(member),
            Type = GetTypeFromMember(member),
            IsOptional = GetIsOptionalFromAttribute(injectedAttribute),
            InjectedType = GetInjectedTypeFromAttribute(injectedAttribute)
        };

    private static bool IsServiceInjection(AttributeData attr) =>
        IsAttributeOfType(attr?.AttributeClass, "ServiceInjectionAttribute", "CodeIX.ServiceInjection");

    private static bool IsInjectedAttribute(AttributeData attr) =>
        IsAttributeOfType(attr?.AttributeClass, "InjectedAttribute", "CodeIX.ServiceInjection");

    private static bool IsAttributeOfType(ISymbol symbol, string typeName, string namespaceName) =>
        symbol != null && symbol.Name == typeName && symbol.ContainingNamespace.ToDisplayString() == namespaceName;

    private static string GetNameFromMember(ISymbol member) =>
        member.Name;

    private static ITypeSymbol GetTypeFromMember(ISymbol member) =>
        member switch
        {
            IPropertySymbol property => property.Type,
            IFieldSymbol field => field.Type,
            _ => throw new NotSupportedException()
        };

    private static bool GetIsOptionalFromAttribute(AttributeData attribute) =>
        attribute.ConstructorArguments.FirstOrDefault().Value as bool? ?? false;

    private static ITypeSymbol GetInjectedTypeFromAttribute(AttributeData attribute) =>
        attribute.ConstructorArguments
            .FirstOrDefault(arg => arg.Value is ITypeSymbol)
            .Value as ITypeSymbol;


    private static string GenerateSourceCode(INamedTypeSymbol symbol, IReadOnlyCollection<Injection> injections) =>
        new SourceCodeFactory(symbol, injections).GenerateSourceCode();
}