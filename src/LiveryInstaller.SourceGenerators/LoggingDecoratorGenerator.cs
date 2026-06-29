using System.Linq;
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
                                                                
                                                                namespace LiveryInstaller.UI;

                                                                [AttributeUsage(AttributeTargets.Interface)]
                                                                public sealed class LoggingDecoratorAttribute : Attribute { }
                                                                """));

            var syntaxNodes = context.SyntaxProvider.ForAttributeWithMetadataName(
                "LiveryInstaller.UI.LoggingDecoratorAttribute",
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
                            .Select(static y => new MethodModel(
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
                                    .Select(static z => new ParameterModel(
                                        z.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                                        z.Name))
                                    .ToEquatableArray()))
                            .ToEquatableArray());
                });

            context.RegisterSourceOutput(syntaxNodes, Execute);
        }

        private static void Execute(SourceProductionContext context, InterfaceModel interfaceModel)
        {
            if (interfaceModel == null || interfaceModel.Methods.IsEmpty) return;
            
            var name =
                $"Logging{interfaceModel.Name.Substring(1)}";

            var sb = new IndentingStringBuilder();
            
            sb.AppendLine("using System;")
                .AppendLine("using Microsoft.Extensions.Logging;")
                .AppendLine($"namespace {interfaceModel.Namespace};")
                .AppendLine()
                .AppendLine($"internal sealed class {name}(ILogger<{name}> logger, {interfaceModel.FullyQualifiedName} inner) : {interfaceModel.FullyQualifiedName}")
                .AppendLine("{")
                .IncrementIndentation();
            
            foreach (var method in interfaceModel.Methods)
            {
                HandleMethodImplementation(method, sb);
            }
            
            sb.DecrementIndentation()
                .AppendLine("}");

            context.AddSource($"{name}.g.cs", sb.ToString());
        }

        private static void HandleMethodImplementation(MethodModel method, IndentingStringBuilder sb)
        {
            var isTask = method.ReturnKind is ReturnKind.Task or ReturnKind.ValueTask or ReturnKind.GenericTask
                or ReturnKind.GenericValueTask;

            sb.Append("public ");
            if (isTask)
                sb.Append("async ");
            sb.Append(
                $"{method.ReturnType} {method.Name}({string.Join(", ", method.Parameters.Select(x => $"{x.Type} {x.Name}"))}) ");
            sb.AppendLine();
            
            HandleMethodBody(method, sb, isTask);
        }

        private static void HandleMethodBody(MethodModel method, IndentingStringBuilder sb, bool isTask)
        {
            var isVoid = method.ReturnKind is ReturnKind.Void or ReturnKind.Task or ReturnKind.ValueTask;
            var parameterNames = string.Join(" - ", method.Parameters.Select(x => $"{{{ToPascalCase(x.Name)}}}"));
            var args = string.Join(", ", method.Parameters.Select(x => x.Name));

            var logMessageEnding = args.Length > 0 ? $" - {parameterNames}\", {args}" : "\"";

            sb.AppendLine("{")
                .IncrementIndentation()
                .AppendLine("try")
                .AppendLine("{")
                .IncrementIndentation()
                .AppendLine($"logger.LogInformation(\"Starting {method.Name}{logMessageEnding});");
            
            if (!isVoid) sb.Append("var result = ");
            if (isTask) sb.Append("await ");
            
            sb.AppendLine($"inner.{method.Name}({string.Join(", ", args)});")
                .AppendLine($"logger.LogInformation(\"Finished {method.Name}{logMessageEnding});");
            
            if (!isVoid) sb.AppendLine("return result;");
            
            sb.DecrementIndentation()
                .AppendLine("}")
                .AppendLine("catch (Exception ex)")
                .AppendLine("{")
                .IncrementIndentation()
                .AppendLine($"logger.LogError(ex, \"An error occured during {method.Name}{logMessageEnding});")
                .AppendLine("throw;")
                .DecrementIndentation()
                .AppendLine("}")
                .DecrementIndentation()
                .AppendLine("}");
        }

        private static string ToPascalCase(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}