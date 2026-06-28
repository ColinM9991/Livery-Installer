using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                transform: static (node, _) => (node.TargetSymbol.ContainingNamespace.ToDisplayString(), node.TargetSymbol.Name))
            .Collect();

        context.RegisterSourceOutput(decoratedServices, Execute);
    }

    private static void Execute(SourceProductionContext context, ImmutableArray<(string Namespace, string InterfaceName)> symbols)
    {
        const string className = "DecoratedServiceCollectionExtensions";

        const string template = """
                                using Microsoft.Extensions.DependencyInjection;

                                namespace LiveryInstaller.UI.Extensions;
                                public static class DecoratedServiceCollectionExtensions
                                {
                                    public static IServiceCollection AddDecoratedServices(this IServiceCollection services)
                                    {
                                        {Code}
                                        return services;
                                    }
                                }
                                """;

        if (symbols.IsEmpty) return;

        var sb = new StringBuilder();
        foreach (var (namespaceName, interfaceName) in symbols)
        {
            sb.AppendLine($"        services.Decorate<{namespaceName}.{interfaceName}, {namespaceName}.Logging{interfaceName.Substring(1)}>();");
        }

        
        context.AddSource($"{className}.g.cs", template.Replace("{Code}", sb.ToString()));
    }
}