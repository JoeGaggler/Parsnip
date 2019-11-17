using System;
using System.IO;
using System.Linq;
using JMG.Parsnip.VSIXProject.CodeWriting;
using JMG.Parsnip.VSIXProject.SerializedModel;

namespace JMG.Parsnip.VSIXProject
{
    public static class ParsnipGenerator
    {
        public static String Generate(String inputString, String outputNamespace, String fileName, String versionString)
        {
            try
            {
                var fileBaseName = Path.GetFileNameWithoutExtension(fileName);
                var className = NameGen.ClassName(fileBaseName);

                var syntacticModel = SyntacticModel.Generated.Parsnip.Parse(inputString, new SyntacticModel.Generated.ParsnipRuleFactory());
                var semanticModel = SemanticModel.Analyzer.Analyze(syntacticModel);

                // TRANSFORMATIONS
                semanticModel = SemanticModel.Transformations.Collapsing.Go(semanticModel);
                semanticModel = SemanticModel.Transformations.AssignRuleReferenceTypes.Go(semanticModel);
                semanticModel = SemanticModel.Transformations.AssignRuleFactoryMethods.Go(semanticModel);

                var writer = new CodeWriter();

                writer.Comment("Code Generated via Parsnip Packrat Parser Producer");
                writer.Comment($"Version: {versionString}");
                writer.Comment($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                writer.EndOfLine();
                writer.UsingNamespace("System");
                writer.UsingNamespace("System.Linq");
                writer.UsingNamespace("System.Diagnostics");
                writer.UsingNamespace("System.Threading.Tasks");
                writer.UsingNamespace("System.Collections.Generic");
                writer.EndOfLine();

                using (writer.Namespace(outputNamespace))
                {
                    var interfaceName = $"I{className}RuleFactory";
                    using (writer.Interface(interfaceName, Access.Public))
                    {
                        foreach (var method in semanticModel.InterfaceMethods)
                        {
                            var returnType = NameGen.TypeString(method.ReturnType);
                            var name = method.Name;
                            var parameterTypes = method.ParameterTypes.Select((i, j) => $"{NameGen.TypeString(i)} t{j}");
                            var parameterTypeList = String.Join(", ", parameterTypes);
                            writer.LineOfCode($"{returnType} {name}({parameterTypeList});");
                        }
                    }
                    writer.EndOfLine();

                    using (writer.Class(className, Access.Public))
                    {
                        writer.LineOfCode("private class ParseResult<T> { public T Node; public PackratState State; }");
                        writer.EndOfLine();
                        writer.LineOfCode("private class EmptyNode { private EmptyNode() { } public static EmptyNode Instance = new EmptyNode(); }");
                        writer.EndOfLine();
                        var firstRule = semanticModel.Rules[0];
                        var firstRuleReturnType = NameGen.TypeString(firstRule.ReturnType);
                        var firstRuleParseMethodName = NameGen.ParseFunctionMethodName(firstRule.RuleIdentifier);
                        using (writer.Method(Access.Public, true, firstRuleReturnType, "Parse", new[] {
                            new LocalVarDecl("String", "input"),
                            new LocalVarDecl(interfaceName, "factory"),
                        }))
                        {
                            writer.Assign("var states", "new PackratState[input.Length + 1]");
                            writer.LineOfCode("Enumerable.Range(0, input.Length + 1).ToList().ForEach(i => states[i] = new PackratState(input, i, states, factory));");
                            writer.Assign("var state", "states[0]");
                            writer.Assign("var result", $"PackratState.{firstRuleParseMethodName}(state, factory)");
                            writer.LineOfCode("if (result == null) return null;");
                            writer.Return("result.Node");
                        }
                        writer.EndOfLine();
                        var packratStateClassName = "PackratState";
                        using (writer.Class(packratStateClassName, Access.Private))
                        {
                            writer.LineOfCode("private readonly string input;");
                            writer.LineOfCode("private readonly int inputPosition;");
                            writer.LineOfCode("private readonly PackratState[] states;");
                            writer.LineOfCode($"private readonly {interfaceName} factory;");
                            writer.EndOfLine();
                            using (writer.Constructor(Access.Public, packratStateClassName, new[] {
                                new LocalVarDecl("String", "input"),
                                new LocalVarDecl("Int32", "inputPosition"),
                                new LocalVarDecl("PackratState[]", "states"),
                                new LocalVarDecl(interfaceName, "factory")
                            }))
                            {
                                writer.Assign("this.input", "input");
                                writer.Assign("this.inputPosition", "inputPosition");
                                writer.Assign("this.states", "states");
                                writer.Assign("this.factory", "factory");
                            }

                            var parsnipCode = new ParsnipCode();
                            parsnipCode.WriteMethods(writer, interfaceName, semanticModel);
                        }
                    }
                }

                return writer.GetText();
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }
        }
    }
}
