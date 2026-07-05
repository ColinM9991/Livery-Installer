using System.Linq;
using Microsoft.CodeAnalysis;

namespace LiveryInstaller.SourceGenerators;

internal record InterfaceModel(
    string Namespace,
    string Name,
    string FullyQualifiedName,
    EquatableArray<MethodModel> Methods)
{
    public static InterfaceModel Create(INamedTypeSymbol symbol) =>
        new(
            symbol.ContainingNamespace.ToDisplayString(),
            symbol.Name,
            symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            symbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(static y => y.MethodKind == MethodKind.Ordinary)
                .Select(MethodModel.Create)
                .ToEquatableArray());
}