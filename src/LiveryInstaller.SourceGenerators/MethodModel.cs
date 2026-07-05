using System.Linq;
using Microsoft.CodeAnalysis;

namespace LiveryInstaller.SourceGenerators;

internal record MethodModel(
    string ReturnType,
    ReturnKind ReturnKind,
    string Name,
    EquatableArray<ParameterModel> Parameters)
{
    public static MethodModel Create(IMethodSymbol methodSymbol) =>
        new(
            methodSymbol.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            methodSymbol.ReturnType switch
            {
                { SpecialType: SpecialType.System_Void } => ReturnKind.Void,
                INamedTypeSymbol { IsGenericType: false, Name: "Task" } => ReturnKind.Task,
                INamedTypeSymbol { IsGenericType: false, Name: "ValueTask" } => ReturnKind
                    .ValueTask,
                INamedTypeSymbol { IsGenericType: true, Name: "Task" } => ReturnKind
                    .GenericTask,
                INamedTypeSymbol { IsGenericType: true, Name: "ValueTask" } => ReturnKind
                    .GenericTask,
                _ => ReturnKind.NonVoid
            },
            methodSymbol.Name,
            methodSymbol.Parameters
                .Select(static z => new ParameterModel(
                    z.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    z.Name))
                .ToEquatableArray());
}