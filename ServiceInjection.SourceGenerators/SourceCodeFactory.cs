﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators;

internal class SourceCodeFactory
{
    private readonly INamedTypeSymbol _symbol;
    private readonly IEnumerable<Injection> _injections;
    public DateTime DateTimeProvided { get; set; }

    public SourceCodeFactory(INamedTypeSymbol symbol, IEnumerable<Injection> injections)
    {
        _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        _injections = injections ?? throw new ArgumentNullException(nameof(injections));
        DateTimeProvided = DateTime.Now; // Default value
    }

    public string CreateSourceCode()
    {
        var sb = new StringBuilder();

        // Append Header, Usings, Namespace, Class, Constructor, Body, and Close Class/Namespace
        AppendHeader(sb);
        AppendUsings(sb);
        sb.AppendLine($"namespace {_symbol.ContainingNamespace.ToDisplayString()}");
        sb.AppendLine("{");
        sb.AppendLine($"    partial class {_symbol.Name}");
        sb.AppendLine("    {");
        AppendCtor(sb, "        ");
        AppendCall(sb, "            ");
        sb.AppendLine("        {");
        AppendBody(sb, "            ");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private void AppendHeader(StringBuilder sb)
    {
        sb.AppendLine($"// <auto-generated date=\"{DateTimeProvided:s}\">");
        sb.AppendLine("// This code was generated by ServiceInjection.");
        sb.AppendLine("// Changes to this file may cause incorrect behavior");
        sb.AppendLine("// and will be lost if the code is regenerated.");
        sb.AppendLine("// </auto-generated>");
    }

    private void AppendUsings(StringBuilder sb)
    {
        var namespaces = new HashSet<string>();
        foreach (var injection in _injections)
        {
            AddNamespace(namespaces, injection.Type);
            AddNamespace(namespaces, injection.InjectedType);
        }
        foreach (var ns in namespaces)
        {
            sb.AppendLine($"using {ns};");
        }
        sb.AppendLine();
    }

    private static void AddNamespace(ISet<string> namespaces, ISymbol typeSymbol)
    {
        if (typeSymbol == null) return;
        var ns = typeSymbol.ContainingNamespace?.ToDisplayString();
        if (!string.IsNullOrEmpty(ns) && ns != "<global namespace>")
        {
            namespaces.Add(ns);
        }
    }

    private void AppendCall(StringBuilder sb, string indent)
    {
        var constructors = _symbol.Constructors.Where(c => !c.IsImplicitlyDeclared).ToList();
        switch (constructors.Count)
        {
            case 0:
                // No constructors, so we can't call one
                break;
            case 1:
                var constructor = constructors.First();
                sb.AppendLine($"{indent}: this(");
                sb.Append($"{indent}    ");
                sb.Append(GetParameterList(constructor));
                sb.AppendLine(")");
                break;
            case > 1:
                // TODO: Handle multiple constructors with parameters
                // right now, we don't know which one to call
                break;
        }
    }

    private string GetParameterList(IMethodSymbol constructor)
    {
        return string.Join(", ", constructor.Parameters.Select(p => $"{p.Name}: {MatchParameterList(p)}"));
    }

    private string MatchParameterList(IParameterSymbol parameter)
    {
        // match by class type
        var matchByType = _injections.FirstOrDefault(i => SymbolEqualityComparer.Default.Equals(i.Type, parameter.Type));
        if (matchByType != null)
            return matchByType.Name;

        // match by injected class type
        var matchByInjectedType = _injections.FirstOrDefault(i => SymbolEqualityComparer.Default.Equals(i.InjectedType, parameter.Type));
        if (matchByInjectedType != null)
            return matchByInjectedType.Name;

        // match by name
        var matchByName = _injections.FirstOrDefault(i => i.Name == parameter.Name);
        if (matchByName != null)
            return matchByName.Name;

        return "default";
    }

    private void AppendCtor(StringBuilder sb, string indent)
    {
        sb.Append(indent + $"public {_symbol.Name}(");
        var skipFirst = true;
        foreach (var injection in _injections)
        {
            if (skipFirst) skipFirst = false;
            else sb.Append(", ");
            var typeToUse = injection.InjectedType ?? injection.Type;
            if (injection.Required) sb.Append($"{typeToUse.Name} {injection.Name}");
            else sb.Append($"{typeToUse.Name} {injection.Name} = null");
        }
        sb.AppendLine(")");
    }

    private void AppendBody(StringBuilder sb, string indent)
    {
        foreach (var injection in _injections)
        {
            sb.Append(indent);
            sb.Append($"this.{injection.Name} = {injection.Name}");

            if (injection.Required)
            {
                sb.Append($" ?? throw new ArgumentNullException(nameof({injection.Name}))");
            }

            sb.AppendLine(";");
        }
    }
}