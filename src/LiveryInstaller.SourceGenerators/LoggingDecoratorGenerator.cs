using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                    predicate: (node, _) => node is InterfaceDeclarationSyntax,
                    transform: (node, _) => (INamedTypeSymbol)node.TargetSymbol)
                .Where(symbol => symbol is not null);

            context.RegisterSourceOutput(syntaxNodes, Execute);
        }

        private void Execute(SourceProductionContext context, INamedTypeSymbol node)
        {
            var namespaceName = node.ContainingNamespace.ToDisplayString();
            var interfaceName = node.Name;
            var desiredClassName = $"Logging{interfaceName.Substring(1)}";

            var methods = node.GetMembers().OfType<IMethodSymbol>();

            var sb = new StringBuilder();
            sb.AppendLine($$"""
                            using System;
                            using Microsoft.Extensions.Logging;

                            namespace {{namespaceName}};
                            
                            public sealed class {{desiredClassName}}(
                                {{interfaceName}} inner,
                                ILogger<{{desiredClassName}}> logger) : {{interfaceName}}
                            {
                            """);
            foreach (var method in methods)
            {
                var isAsync = method.IsAsync || IsTaskLike(method.ReturnType);
                var isVoid = method.ReturnsVoid ||
                             method.ReturnType.OriginalDefinition.MetadataName is "Task" or "ValueTask";
                var methodName = method.Name;
                var parameters = string.Join(", ", method.Parameters.Select(x => $"{x.Type} {x.Name}"));
                var arguments = string.Join(", ", method.Parameters.Select(x => x.Name));
                
                var logArguments = string.Join("- ", method.Parameters.Select(x => $"{{{ToPascalCase(x.Name)}}}"));

                sb.AppendLine($$"""
                                    public{{(isAsync ? " async" : string.Empty)}} {{method.ReturnType.ToDisplayString()}} {{methodName}}({{parameters}})
                                    {
                                        try
                                        {
                                            logger.LogInformation("Starting {{methodName}} - {{logArguments}}", {{arguments}});
                                            {{(isVoid ? string.Empty : "var result = ")}}{{(isAsync ? "await " : string.Empty)}}inner.{{methodName}}({{arguments}});
                                            logger.LogInformation("Finished {{methodName}} - {{logArguments}}", {{arguments}});
                                            {{(isVoid ? string.Empty : "return result;")}}
                                        } catch (Exception e) {
                                            logger.LogError(e, "Error executing {{methodName}} - {{logArguments}}", {{arguments}});
                                            throw;
                                        }
                                    }
                                """);
            }

            sb.AppendLine("}");

            context.AddSource($"{namespaceName}.{desiredClassName}.g.cs", sb.ToString());
        }

        private static bool IsTaskLike(ITypeSymbol typeSymbol)
        {
            var original = typeSymbol.OriginalDefinition;
            var ns = original.ContainingNamespace?.ToDisplayString();

            if (ns is not "System.Threading.Tasks") return false;

            return original.MetadataName is "Task" or "Task`1" or "ValueTask" or "ValueTask`1";
        }
        
        private static string ToPascalCase(string str) => str.Substring(0, 1).ToUpperInvariant() + str.Substring(1);
    }
}