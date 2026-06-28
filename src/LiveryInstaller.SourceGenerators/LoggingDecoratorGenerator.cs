using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

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
                predicate: static (_, _) => true,
                transform: static (ctx, _) =>
                {
                    var symbol = (INamedTypeSymbol)ctx.TargetSymbol;

                    return new InterfaceModel(
                        symbol.ContainingNamespace.ToDisplayString(),
                        symbol.Name,
                        symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                        symbol.GetMembers()
                            .OfType<IMethodSymbol>()
                            .Where(static y => y.MethodKind == MethodKind.Ordinary)
                            .Select(y => new MethodModel(
                                y.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                                y.ReturnType switch
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
                                y.Name,
                                y.Parameters
                                    .Select(z => new ParameterModel(
                                        z.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                                        z.Name))
                                    .ToEquatableArray()))
                            .ToEquatableArray());
                });

            context.RegisterSourceOutput(syntaxNodes, Execute);
        }

        private static void Execute(SourceProductionContext context, InterfaceModel interfaceModel)
        {
            const string loggerService = "ILogger<{Type}> logger";
            const string implementationTemplate = """
                                                  using System;
                                                  using Microsoft.Extensions.Logging;
                                                  namespace {Namespace};

                                                  internal sealed class {Name}({Services}) : {Interface}
                                                  {
                                                  {Code}}
                                                  """;

            var name =
                $"Logging{interfaceModel.Name.Substring(1)}";

            var sb = new StringBuilder();
            foreach (var method in interfaceModel.Methods.Array)
            {
                HandleMethodImplementation(method, sb);
            }

            var code = implementationTemplate
                .Replace("{Namespace}", interfaceModel.Namespace)
                .Replace("{Services}",
                    string.Join(", ",
                    [
                        loggerService.Replace("{Type}", name),
                        $"{interfaceModel.FullyQualifiedName} inner"
                    ]))
                .Replace("{Name}", name)
                .Replace("{Interface}", interfaceModel.FullyQualifiedName)
                .Replace("{Code}", sb.ToString());

            context.AddSource($"{name}.g.cs", code);
        }

        private static void HandleMethodImplementation(MethodModel method, StringBuilder sb)
        {
            var isTask = method.ReturnKind is ReturnKind.Task or ReturnKind.ValueTask or ReturnKind.GenericTask
                or ReturnKind.GenericValueTask;

            sb.Append("    public ");
            if (isTask)
                sb.Append("async ");
            sb.Append(
                $"{method.ReturnType} {method.Name}({string.Join(", ", method.Parameters.Array.Select(x => $"{x.Type} {x.Name}"))}) ");
            sb.AppendLine();
            HandleMethodBody(method, sb, isTask);
        }

        private static void HandleMethodBody(MethodModel method, StringBuilder sb, bool isTask)
        {
            var isVoid = method.ReturnKind is ReturnKind.Void or ReturnKind.Task or ReturnKind.ValueTask;
            var parameterNames = string.Join(" - ", method.Parameters.Array.Select(x => $"{{{ToPascalCase(x.Name)}}}"));
            var args = string.Join(", ", method.Parameters.Array.Select(x => x.Name));

            var logMessageEnding = args.Length > 0 ? $" - {parameterNames}\", {args}" : "\"";

            sb.AppendLine("    {");
            sb.AppendLine("        try");
            sb.AppendLine("        {");
            sb.AppendLine($"            logger.LogInformation(\"Starting {method.Name}{logMessageEnding});");
            sb.Append("            ");
            if (!isVoid) sb.Append("var result = ");
            if (isTask) sb.Append("await ");
            sb.AppendLine($"inner.{method.Name}({string.Join(", ", args)});");
            sb.AppendLine($"            logger.LogInformation(\"Finished {method.Name}{logMessageEnding});");
            if (!isVoid) sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine("        catch (Exception ex)");
            sb.AppendLine("        {");
            sb.AppendLine(
                $"            logger.LogError(ex, \"An error occured during {method.Name}{logMessageEnding});");
            sb.AppendLine("            throw;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
        }

        private static string ToPascalCase(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}