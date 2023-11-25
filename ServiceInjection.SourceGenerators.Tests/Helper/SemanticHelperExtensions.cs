using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ServiceInjection.SourceGenerators.Tests.Helper;

public static class SemanticHelperExtensions
{
    public static SemanticModel CreateSemanticModel(this string sourceCode)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var compilation = CSharpCompilation.Create("DynamicCompilation")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTree);
        return compilation.GetSemanticModel(syntaxTree);
    }

    public static ITypeSymbol GetTypeSymbol(this SemanticModel semanticModel, string typeName)
    {
        var typeSyntax = semanticModel.SyntaxTree.GetRoot().DescendantNodes()
            .OfType<TypeDeclarationSyntax>()
            .FirstOrDefault(node => node.Identifier.ValueText == typeName);

        return typeSyntax != null ? semanticModel.GetDeclaredSymbol(typeSyntax) : null;
    }

    public static ImmutableArray<IParameterSymbol> GetParameters(this SemanticModel semanticModel, string className)
    {
        var classSyntax = semanticModel.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
            .FirstOrDefault(c => c.Identifier.ValueText == className);

        if (classSyntax != null)
        {
            var classSymbol = semanticModel.GetDeclaredSymbol(classSyntax) as INamedTypeSymbol;
            var constructorSymbol = classSymbol?.Constructors.FirstOrDefault();
            return constructorSymbol?.Parameters ?? ImmutableArray<IParameterSymbol>.Empty;
        }

        return ImmutableArray<IParameterSymbol>.Empty;
    }
}