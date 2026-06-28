using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace LiveryInstaller.SourceGenerators
{
    [Generator(LanguageNames.CSharp)]
    public class LoggingDecoratorGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
                ctx.AddSource("LoggingDecoratorGenerator.g.cs", """
                                                                using System;

                                                                [AttributeUsage(AttributeTargets.Interface)]
                                                                public sealed class LoggingDecoratorAttribute : Attribute { }
                                                                """));

            var syntaxNodes = context.SyntaxProvider.ForAttributeWithMetadataName(
                    "LoggingDecoratorAttribute",
                    predicate: static (node, _) => node is InterfaceDeclarationSyntax,
                    transform: static (node, _) => (INamedTypeSymbol)node.TargetSymbol)
                .Where(symbol => symbol is not null);

            context.RegisterSourceOutput(syntaxNodes, Execute);
        }

        private static void Execute(SourceProductionContext context, INamedTypeSymbol node)
        {
            var namespaceName = node.ContainingNamespace.ToDisplayString();
            var interfaceName = node.Name;
            var desiredClassName = $"Logging{interfaceName.Substring(1)}";

            var classDeclaration = ClassDeclaration(Identifier(desiredClassName))
                .AddBaseListTypes(SimpleBaseType(IdentifierName(interfaceName)))
                .WithModifiers(TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.SealedKeyword)))
                .WithMembers(List(node.GetMembers()
                    .OfType<IMethodSymbol>()
                    .Select(GetMethodDeclaration)))
                .WithParameterList(ParameterList(SeparatedList([
                    Parameter(Identifier("inner")).WithType(IdentifierName(interfaceName)),
                    Parameter(Identifier("logger"))
                        .WithType(GenericName(Identifier("ILogger"))
                            .WithTypeArgumentList(TypeArgumentList(
                                SingletonSeparatedList<TypeSyntax>(
                                    IdentifierName(desiredClassName)))))
                ])));

            var namespaceDeclaration = FileScopedNamespaceDeclaration(
                    ParseName(node.ContainingNamespace.ToDisplayString()))
                .AddMembers(classDeclaration);

            var compilationUnit = CompilationUnit()
                .WithUsings(SingletonList(UsingDirective(IdentifierName("Microsoft.Extensions.Logging"))))
                .AddMembers(namespaceDeclaration);

            context.AddSource($"{namespaceName}.{desiredClassName}.g.cs",
                compilationUnit.NormalizeWhitespace().ToFullString());
        }

        private static MemberDeclarationSyntax GetMethodDeclaration(IMethodSymbol methodSymbol)
        {
            var method = MethodDeclaration(ParseTypeName(methodSymbol.ReturnType.ToDisplayString()), methodSymbol.Name)
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                    ParameterList(
                        SeparatedList(methodSymbol.Parameters.Select(y =>
                            Parameter(Identifier(y.Name))
                                .WithType(ParseTypeName(y.Type.ToDisplayString()))))))
                .WithBody(Block(SingletonList(GetBlockBody(methodSymbol))));

            if (IsTaskLike(methodSymbol.ReturnType))
            {
                method = method.AddModifiers(Token(SyntaxKind.AsyncKeyword));
            }

            return method;
        }

        private static StatementSyntax GetBlockBody(IMethodSymbol methodSymbol)
        {
            List<StatementSyntax> statements =
            [
                GetLoggerExpression("Starting", methodSymbol),
                GetBodyExpression(methodSymbol),
                GetLoggerExpression("Finished", methodSymbol),
            ];

            if (HasNonVoidResult(methodSymbol))
            {
                statements.Add(ReturnStatement(IdentifierName("result")));
            }

            var statement = TryStatement()
                .WithBlock(
                    Block(List(statements)))
                .WithCatches(SingletonList(CatchClause()
                    .WithBlock(
                        Block(
                            List<StatementSyntax>([
                                GetLoggerExpression("Error when executing", methodSymbol),
                                ThrowStatement()
                            ])))));

            return statement;
        }

        private static StatementSyntax GetLoggerExpression(string logPrefix, IMethodSymbol methodSymbol)
        {
            var logArguments = string.Join(" - ", methodSymbol.Parameters.Select(x => $"{{{ToPascalCase(x.Name)}}}"));
            return ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("logger"),
                        IdentifierName("LogInformation")),
                    ArgumentList(SeparatedList(
                    [
                        Argument(LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            Literal($"{logPrefix} {methodSymbol.Name} - {logArguments}"))),
                        ..methodSymbol.Parameters.Select(x => Argument(IdentifierName(x.Name)))
                    ]))));
        }

        private static StatementSyntax GetBodyExpression(IMethodSymbol methodSymbol)
        {
            ExpressionSyntax invocationExpression = InvocationExpression(
                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("inner"),
                    IdentifierName(methodSymbol.Name)), ArgumentList(
                    SeparatedList(methodSymbol.Parameters.Select(x => Argument(IdentifierName(x.Name))))));

            if (IsTaskLike(methodSymbol.ReturnType))
            {
                invocationExpression = AwaitExpression(invocationExpression);
            }

            if (HasNonVoidResult(methodSymbol))
            {
                return LocalDeclarationStatement(VariableDeclaration(IdentifierName("var"))
                    .WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier("result"))
                        .WithInitializer(EqualsValueClause(invocationExpression)))));
            }

            return ExpressionStatement(invocationExpression);
        }

        private static bool IsTaskLike(ITypeSymbol typeSymbol)
        {
            var original = typeSymbol.OriginalDefinition;
            var ns = original.ContainingNamespace?.ToDisplayString();

            if (ns is not "System.Threading.Tasks") return false;

            return original.MetadataName is "Task" or "Task`1" or "ValueTask" or "ValueTask`1";
        }

        private static bool HasNonVoidResult(IMethodSymbol methodSymbol) =>
            methodSymbol.ReturnType.SpecialType != SpecialType.System_Void &&
            methodSymbol.ReturnType.OriginalDefinition.MetadataName is not ("Task" or "ValueTask");

        private static string ToPascalCase(string str) => str.Substring(0, 1).ToUpperInvariant() + str.Substring(1);
    }
}