using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

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
                predicate: static (_, _) => true,
                transform: static (node, _) => (node.TargetSymbol.ContainingNamespace.ToDisplayString(), node.TargetSymbol.Name))
            .Collect();

        context.RegisterSourceOutput(decoratedServices, Execute);
    }

    private static void Execute(SourceProductionContext context, ImmutableArray<(string Namespace, string InterfaceName)> symbols)
    {
        const string className = "DecoratedServiceCollectionExtensions";

        if (symbols.IsEmpty) return;

        var sb = new IndentingStringBuilder();
        sb.AppendLine("using Microsoft.Extensions.DependencyInjection;")
            .AppendLine()
            .AppendLine($"namespace LiveryInstaller.SourceGenerated;")
            .AppendLine()
            .AppendLine("public static class DecoratedServiceCollectionExtensions")
            .AppendLine("{")
            .IncrementIndentation()
            .AppendLine("public static IServiceCollection AddDecoratedServices(this IServiceCollection services)")
            .AppendLine("{")
            .IncrementIndentation();
        
        foreach (var (namespaceName, interfaceName) in symbols)
        {
            sb.AppendLine($"services.Decorate<{namespaceName}.{interfaceName}, {namespaceName}.Logging{interfaceName.Substring(1)}>();");
        }
        
        sb.AppendLine("return services;")
            .DecrementIndentation()
            .AppendLine("}")
            .DecrementIndentation()
            .AppendLine("}");
        
        context.AddSource($"{className}.g.cs", sb.ToString());
    }
}