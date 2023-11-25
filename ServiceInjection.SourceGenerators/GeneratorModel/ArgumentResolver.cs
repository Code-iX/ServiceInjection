using System;
using System.Collections.Generic;
using System.Linq;

using CodeIX.ServiceInjection.SourceGenerators.Models;

using Microsoft.CodeAnalysis;

namespace CodeIX.ServiceInjection.SourceGenerators.GeneratorModel;

internal class ArgumentResolver
{
    private readonly IList<Injection> _injections;
    private readonly IList<IParameterSymbol> _parameters;

    public ArgumentResolver(IEnumerable<Injection> injections, IEnumerable<IParameterSymbol> parameters)
    {
        _injections = injections.ToList();
        _parameters = parameters.ToList();
    }
    public string Match(IParameterSymbol parameter)
    {
        // Create a list of possible matches with their score
        var possibleMatches = _injections.Select(injection => new
        {
            Injection = injection,
            Score = GetMatchScore(parameter, injection)
        })
            .Where(x => x.Score > 0) // Filter out incompatible injections
            .OrderByDescending(x => x.Score)
            .ToList();

        // Check if there is a match with a higher score than the current parameter
        foreach (var match in possibleMatches)
        {
            // If there is a match with a higher score, we can't use this parameter
            if (_parameters.Any(p => p != parameter && GetMatchScore(p, match.Injection) > match.Score))
                continue;

            _injections.Remove(match.Injection);
            return match.Injection.Name;
        }


        return "default";
    }
    private static bool IsTypeCompatible(Injection injection, IParameterSymbol parameter)
    {
        if (injection.Type == null)
            return false;

        if (SymbolEqualityComparer.Default.Equals(injection.Type, parameter.Type) ||  (injection.InjectedType != null && SymbolEqualityComparer.Default.Equals(injection.InjectedType, parameter.Type)))
            return true;

        if (parameter.Type.TypeKind != TypeKind.Interface)
            return false;

        var interfaceType = parameter.Type;
        var injectionTypeMembers = injection.Type.AllInterfaces;
        return injectionTypeMembers.Any(intf => SymbolEqualityComparer.Default.Equals(intf, interfaceType));
    }

    private static float GetMatchScore(ISymbol parameter, Injection injection)
    {
        if (!IsTypeCompatible(injection, (IParameterSymbol)parameter))
            return -1;

        if (string.Equals(parameter.Name, injection.Name, StringComparison.Ordinal))
            return 1;

        if (string.Equals(parameter.Name, injection.Name, StringComparison.OrdinalIgnoreCase))
            return 0.9f;

        var trimmedInjectionName = injection.Name.TrimStart('_');
        var trimmedParameterName = parameter.Name.TrimStart('_');

        if (string.Equals(trimmedInjectionName, trimmedParameterName, StringComparison.Ordinal))
            return 0.8f;

        if (string.Equals(trimmedInjectionName, trimmedParameterName, StringComparison.OrdinalIgnoreCase))
            return 0.7f;

        return 0.5f;
    }
}