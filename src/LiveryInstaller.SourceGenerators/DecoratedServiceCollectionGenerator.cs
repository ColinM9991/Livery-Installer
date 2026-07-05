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
        var decoratedServices = context.SyntaxProvider.ForAttributeWithMetadataName("LiveryInstaller.Library.LoggingDecoratorAttribute",
                predicate: static (_, _) => true,
                transform: static (node, _) => (node.TargetSymbol.ContainingNamespace.ToDisplayString(), node.TargetSymbol.Name))
            .Collect();
        
        var combined = context.CompilationProvider
            .Combine(decoratedServices)
            .Select((x, _) =>
            {
                var (compilation, services) = x;
                return (compilation.AssemblyName, services);
            });

        context.RegisterSourceOutput(combined, static (spc, source) =>
        {
            var (assemblyName, services) = source;

            Execute(spc, assemblyName, services);
        });
    }

    private static void Execute(SourceProductionContext context, string assemblyName, ImmutableArray<(string Namespace, string InterfaceName)> symbols)
    {
        const string className = "DecoratedServiceCollectionExtensions";

        if (symbols.IsEmpty) return;

        var lastDot = assemblyName.LastIndexOf('.');
        var assemblyUniqueName = lastDot >= 0
            ? assemblyName.Substring(lastDot + 1)
            : assemblyName;

        var extensionMethodName = $"Add{assemblyUniqueName}DecoratedServices";

        var sb = new IndentingStringBuilder();
        sb.AppendLine("using Microsoft.Extensions.DependencyInjection;")
            .AppendLine()
            .AppendLine($"namespace {assemblyName};")
            .AppendLine()
            .AppendLine("public static class DecoratedServiceCollectionExtensions")
            .AppendLine("{")
            .IncrementIndentation()
            .AppendLine($"public static IServiceCollection {extensionMethodName}(this IServiceCollection services)")
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