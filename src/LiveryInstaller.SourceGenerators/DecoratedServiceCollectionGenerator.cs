using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace LiveryInstaller.SourceGenerators;

/// <summary>
/// A source generator that adds an extension method to IServiceCollection to register decorated logging service.
/// </summary>
/// <remarks>
/// For example:
///
/// <code>
/// public class LoggingDecoratorAttribute : Attribute {}
///
/// 
/// [LoggingDecorator]
/// public interface IMyService {}
/// </code>
///
/// This source generator will generate the following class
///
/// <code>
/// public static class DecoratedServiceCollectionExtensions
/// {
///     public static IServiceCollection AddDecoratedServices(this IServiceCollection services)
///     {
///         services.Decorate&lt;IMyService, LoggingMyService&gt;();
///         return services;
///     }
/// }
/// </code>
/// </remarks>
[Generator(LanguageNames.CSharp)]
public sealed class DecoratedServiceCollectionGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var decoratedServices = context.SyntaxProvider.ForAttributeWithMetadataName("LoggingDecoratorAttribute",
                predicate: static (node, _) => node is InterfaceDeclarationSyntax,
                transform: static (node, _) => (INamedTypeSymbol)node.TargetSymbol)
            .Collect();

        context.RegisterSourceOutput(decoratedServices, Execute);
    }

    private static void Execute(SourceProductionContext context, ImmutableArray<INamedTypeSymbol> symbols)
    {
        const string namespaceName = "LiveryInstaller.UI.Extensions";
        const string className = "DecoratedServiceCollectionExtensions";

        if (symbols.IsEmpty) return;

        var decoratedExpressions = symbols.Select(decoratedService => ExpressionStatement(InvocationExpression(
            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("services"),
                GenericName(Identifier("Decorate"),
                    TypeArgumentList(SeparatedList([
                        ParseTypeName(decoratedService.Name),
                        ParseTypeName($"Logging{decoratedService.Name.Substring(1)}")
                    ])))))));

        var methodDeclaration =
            MethodDeclaration(ParseTypeName("IServiceCollection"), Identifier("AddDecoratedServices"))
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(Identifier("services"))
                                .WithModifiers(TokenList(Token(SyntaxKind.ThisKeyword)))
                                .WithType(ParseName("IServiceCollection")))))
                .WithBody(Block(List(decoratedExpressions))
                    .AddStatements(ReturnStatement(IdentifierName("services"))));

        var compilationUnit = CompilationUnit()
            .WithUsings(
                List([
                    UsingDirective(ParseName("Microsoft.Extensions.DependencyInjection")),
                    ..symbols.Select(x => x.ContainingNamespace.ToDisplayString())
                        .Distinct()
                        .Select(x => UsingDirective(ParseName(x)))
                ]))
            .AddMembers(
                FileScopedNamespaceDeclaration(ParseName(namespaceName))
                    .AddMembers(
                        ClassDeclaration(Identifier(className))
                            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                            .AddMembers(methodDeclaration)));

        context.AddSource($"{namespaceName}.{className}.g.cs", compilationUnit.NormalizeWhitespace().ToFullString());
    }
}