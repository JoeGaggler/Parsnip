using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JMG.Parsnip.CodeWriting;
using JMG.Parsnip.SerializedModel;

namespace JMG.Parsnip
{
	/// <summary>
	/// Parser generator
	/// </summary>
	public static class ParsnipGenerator
	{
		/// <summary>
		/// Generates a parser
		/// </summary>
		/// <param name="inputString">Specification file</param>
		/// <param name="outputNamespace">Namespace for the generated class</param>
		/// <param name="fileName">Specification file name</param>
		/// <param name="versionString">Version number to print in the header</param>
		/// <returns>Generated output</returns>
		public static String Generate(String inputString, String outputNamespace, String fileName, String versionString)
		{
			try
			{
				if (String.IsNullOrEmpty(inputString))
				{
					throw new ArgumentNullException(nameof(inputString), "Unable to generate parser for empty input");
				}

				var fileBaseName = Path.GetFileNameWithoutExtension(fileName);
				var className = NameGen.ClassName(fileBaseName);

				var syntacticModel = SyntacticModel.Generated.Parsnip.Parse(inputString, new SyntacticModel.Generated.ParsnipRuleFactory());
				var semanticModel = SemanticModel.Analyzer.Analyze(syntacticModel);

				// TRANSFORMATIONS
				semanticModel = SemanticModel.Transformations.Collapsing.Go(semanticModel);
				semanticModel = SemanticModel.Transformations.AssignRuleReferenceTypes.Go(semanticModel);
				semanticModel = SemanticModel.Transformations.AssignRuleFactoryMethods.Go(semanticModel);

				// Reusables
				var interfaceName = $"I{className}RuleFactory";
				var packratStateClassName = "PackratState";
				var parseResultClassName = "ParseResult";
				var parseResultClassNameT = $"{parseResultClassName}<T>";
				var typicalParams = new List<LocalVarDecl>
				{
					new LocalVarDecl("String", "input"), // Invocation
					new LocalVarDecl("Int32", "inputPosition"),
					new LocalVarDecl($"{packratStateClassName}[]", "states"),
					new LocalVarDecl(interfaceName, "factory")
				};
				var cardParams = new[] {
					new LocalVarDecl("String", "input"), // Invocation
					new LocalVarDecl("Int32", "inputPosition"),
					new LocalVarDecl($"{packratStateClassName}[]", "states"),
					new LocalVarDecl(interfaceName, "factory"),
					new LocalVarDecl($"Func<String, Int32, {packratStateClassName}[], {interfaceName}, {parseResultClassNameT}>", "parseAction") // Invocation
				};
				var delimParams = new[] {
					new LocalVarDecl("String", "input"), // Invocation
					new LocalVarDecl("Int32", "inputPosition"),
					new LocalVarDecl($"{packratStateClassName}[]", "states"),
					new LocalVarDecl(interfaceName, "factory"),
					new LocalVarDecl($"Func<String, Int32, {packratStateClassName}[], {interfaceName}, {parseResultClassName}<T>>", "parseAction"), // Invocation
					new LocalVarDecl($"Func<String, Int32, {packratStateClassName}[], {interfaceName}, {parseResultClassName}<D>>", "parseDelimiterAction") // Invocation
				};

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
					using (writer.Interface(interfaceName, Access.Internal))
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

					using (writer.Class(className, Access.Internal))
					{
						using (writer.Class(parseResultClassNameT, Access.Private))
						{
							writer.LineOfCode("public readonly T Node;");
							writer.LineOfCode("public readonly Int32 Advanced;");
							writer.EndOfLine();
							using (writer.Constructor(Access.Public, parseResultClassName, new[] {
							   new LocalVarDecl("T", "node"),
							   new LocalVarDecl("Int32", "advanced") }))
							{
								writer.Assign("this.Node", "node");
								writer.Assign("this.Advanced", "advanced");
							}
						}
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
							writer.Assign("var states", $"new {packratStateClassName}[input.Length + 1]");
							writer.LineOfCode($"Enumerable.Range(0, input.Length + 1).ToList().ForEach(i => states[i] = new {packratStateClassName}());");
							writer.Assign("var result", $"{firstRuleParseMethodName}(input, 0, states, factory)"); // Invocation
							writer.Return("result?.Node");
						}

						// Populate dictionary of rule identifiers to method names
						var ruleMethodNames = semanticModel.Rules.ToDictionary(i => i.RuleIdentifier, i => NameGen.ParseFunctionMethodName(i.RuleIdentifier));

						// Generate method signatures
						var signatures = new List<Signature>();
						var invokers = new Dictionary<SemanticModel.IParseFunction, Invoker>();
						foreach (var rule in semanticModel.Rules)
						{
							var methodName = ruleMethodNames[rule.RuleIdentifier];
							var visitor = new GenerateSignaturesVisitor(methodName, isMemoized: true, mustAddSignature: true, ruleMethodNames, signatures, invokers);
							rule.ParseFunction.ApplyVisitor(visitor, Access.Private);
						}
						writer.EndOfLine();

						// Maybe
						using (writer.Method(Access.Private, true, parseResultClassNameT, "ParseMaybe<T>", cardParams))
						{
							writer.VarAssign("result", "parseAction(input, inputPosition, states, factory)"); // Invocation
							using (writer.If("result != null"))
							{
								writer.Return("result");
							}
							writer.Return($"new {parseResultClassNameT}(default(T), 0)");
						}

						// Star
						writer.EndOfLine();
						using (writer.Method(Access.Private, true, $"{parseResultClassName}<IReadOnlyList<T>>", "ParseStar<T>", cardParams))
						{
							writer.VarAssign("list", "new List<T>()");
							writer.VarAssign("nextResultInputPosition", "inputPosition");
							using (writer.While("true"))
							{
								writer.VarAssign("nextResult", "parseAction(input, nextResultInputPosition, states, factory)"); // Invocation
								using (writer.If("nextResult == null"))
								{
									writer.SwitchBreak();
								}
								writer.LineOfCode("list.Add(nextResult.Node);");
								writer.Assign("nextResultInputPosition", "nextResultInputPosition + nextResult.Advanced");
							}
							writer.Return($"new {parseResultClassName}<IReadOnlyList<T>>(list, nextResultInputPosition - inputPosition)");
						}

						// Plus
						writer.EndOfLine();
						using (writer.Method(Access.Private, true, $"{parseResultClassName}<IReadOnlyList<T>>", "ParsePlus<T>", cardParams))
						{
							writer.VarAssign("list", "new List<T>()");
							writer.VarAssign("nextResultInputPosition", "inputPosition");

							writer.VarAssign("firstResult", "parseAction(input, nextResultInputPosition, states, factory)"); // Invocation
							using (writer.If("firstResult == null"))
							{
								writer.Return("null");
							}
							writer.LineOfCode("list.Add(firstResult.Node);");
							writer.Assign("nextResultInputPosition", "nextResultInputPosition + firstResult.Advanced");

							using (writer.While("true"))
							{
								writer.VarAssign("nextResult", "parseAction(input, nextResultInputPosition, states, factory)"); // Invocation
								using (writer.If("nextResult == null"))
								{
									writer.SwitchBreak();
								}
								writer.LineOfCode("list.Add(nextResult.Node);");
								writer.Assign("nextResultInputPosition", "nextResultInputPosition + nextResult.Advanced");
							}
							writer.Return($"new {parseResultClassName}<IReadOnlyList<T>>(list, nextResultInputPosition - inputPosition)");
						}
						writer.EndOfLine();

						// Delimited
						using (writer.Method(Access.Private, true, $"{parseResultClassName}<IReadOnlyList<T>>", "ParseSeries<T, D>", delimParams))
						{
							writer.VarAssign("list", "new List<T>()");
							writer.VarAssign("nextResultInputPosition", "inputPosition");

							writer.VarAssign("firstResult", "parseAction(input, nextResultInputPosition, states, factory)"); // Invocation
							using (writer.If("firstResult == null"))
							{
								writer.Return("null");
							}
							writer.LineOfCode("nextResultInputPosition += firstResult.Advanced;");
							writer.LineOfCode("list.Add(firstResult.Node);");

							using (writer.While("true"))
							{
								writer.VarAssign("delimResult", "parseDelimiterAction(input, nextResultInputPosition, states, factory)"); // Invocation
								using (writer.If("delimResult == null"))
								{
									writer.SwitchBreak();
								}

								writer.VarAssign("nextResult", "parseAction(input, nextResultInputPosition + delimResult.Advanced, states, factory)"); // Invocation
								using (writer.If("nextResult == null"))
								{
									writer.SwitchBreak();
								}

								writer.LineOfCode("list.Add(nextResult.Node);");
								writer.Assign("nextResultInputPosition", "nextResultInputPosition + delimResult.Advanced + nextResult.Advanced");
							}
							writer.Return($"new {parseResultClassName}<IReadOnlyList<T>>(list, nextResultInputPosition - inputPosition)");
						}

						writer.EndOfLine();

						// Any Character
						using (writer.InlineMethod(Access.Private, true, $"{parseResultClassName}<String>", "ParseIntrinsic_AnyCharacter", typicalParams))
						{
							writer.LineOfCode($"(inputPosition < input.Length) ?");
							writer.LineOfCode($"new ParseResult<String>(input.Substring(inputPosition, 1), 1) :");
							writer.LineOfCode($"null;");
						}

						// Any Letter
						writer.EndOfLine();
						using (writer.InlineMethod(Access.Private, true, $"{parseResultClassName}<String>", "ParseIntrinsic_AnyLetter", typicalParams))
						{
							writer.LineOfCode($"(inputPosition < input.Length && Char.IsLetter(input[inputPosition])) ?");
							writer.LineOfCode($"new ParseResult<String>(input.Substring(inputPosition, 1), 1) :");
							writer.LineOfCode($"null;");
						}

						// Upper Letter
						writer.EndOfLine();
						using (writer.InlineMethod(Access.Private, true, $"{parseResultClassName}<String>", "ParseIntrinsic_UpperLetter", typicalParams))
						{
							writer.LineOfCode($"(inputPosition < input.Length && Char.IsUpper(input[inputPosition])) ?");
							writer.LineOfCode($"new ParseResult<String>(input.Substring(inputPosition, 1), 1) :");
							writer.LineOfCode($"null;");
						}

						// Lower Letter
						writer.EndOfLine();
						using (writer.InlineMethod(Access.Private, true, $"{parseResultClassName}<String>", "ParseIntrinsic_LowerLetter", typicalParams))
						{
							writer.LineOfCode($"(inputPosition < input.Length && Char.IsLower(input[inputPosition])) ?");
							writer.LineOfCode($"new ParseResult<String>(input.Substring(inputPosition, 1), 1) :");
							writer.LineOfCode($"null;");
						}

						// Any Digit
						writer.EndOfLine();
						using (writer.InlineMethod(Access.Private, true, $"{parseResultClassName}<String>", "ParseIntrinsic_AnyDigit", typicalParams))
						{
							writer.LineOfCode($"(inputPosition < input.Length && Char.IsDigit(input[inputPosition])) ?");
							writer.LineOfCode($"new ParseResult<String>(input.Substring(inputPosition, 1), 1) :");
							writer.LineOfCode($"null;");
						}

						// Any Letter or Digit
						writer.EndOfLine();
						using (writer.InlineMethod(Access.Private, true, $"{parseResultClassName}<String>", "ParseIntrinsic_AnyLetterOrDigit", typicalParams))
						{
							writer.LineOfCode($"(inputPosition < input.Length && Char.IsLetterOrDigit(input[inputPosition])) ?");
							writer.LineOfCode($"new ParseResult<String>(input.Substring(inputPosition, 1), 1) :");
							writer.LineOfCode($"null;");
						}

						// End of Line
						writer.EndOfLine();
						using (writer.Method(Access.Private, true, $"{parseResultClassName}<String>", "ParseIntrinsic_EndOfLine", typicalParams))
						{
							writer.VarAssign("result1", "ParseLexeme(input, inputPosition, \"\\r\\n\", StringComparison.Ordinal)"); // Invocation
							using (writer.If("result1 != null"))
							{
								writer.Return($"new {parseResultClassName}<String>(result1.Node, result1.Advanced)");
							}

							writer.VarAssign("result2", "ParseLexeme(input, inputPosition, \"\\n\", StringComparison.Ordinal)"); // Invocation
							using (writer.If("result2 != null"))
							{
								writer.Return($"new {parseResultClassName}<String>(result2.Node, result2.Advanced)");
							}

							writer.Return("null");
						}

						// End of Stream
						writer.EndOfLine();
						using (writer.Method(Access.Private, true, $"{parseResultClassName}<EmptyNode>", "ParseIntrinsic_EndOfStream", typicalParams))
						{
							using (writer.If("inputPosition == input.Length"))
							{
								writer.Return($"new {parseResultClassName}<EmptyNode>(EmptyNode.Instance, 0)");
							}
							writer.Return("null");
						}

						// End of Line Or Stream
						writer.EndOfLine();
						using (writer.Method(Access.Private, true, $"{parseResultClassName}<EmptyNode>", "ParseIntrinsic_EndOfLineOrStream", typicalParams))
						{
							using (writer.If("inputPosition == input.Length"))
							{
								writer.Return($"new {parseResultClassName}<EmptyNode>(EmptyNode.Instance, 0)");
							}

							writer.VarAssign("result1", "ParseLexeme(input, inputPosition, \"\\r\\n\", StringComparison.Ordinal)"); // Invocation
							using (writer.If("result1 != null"))
							{
								writer.Return($"new {parseResultClassName}<EmptyNode>(EmptyNode.Instance, result1.Advanced)");
							}

							writer.VarAssign("result2", "ParseLexeme(input, inputPosition, \"\\n\", StringComparison.Ordinal)"); // Invocation
							using (writer.If("result2 != null"))
							{
								writer.Return($"new {parseResultClassName}<EmptyNode>(EmptyNode.Instance, result2.Advanced)");
							}

							writer.Return("null");
						}

						// C-String
						writer.EndOfLine();
						using (writer.Method(Access.Private, true, $"{parseResultClassName}<String>", "ParseIntrinsic_CString", typicalParams))
						{
							writer.VarAssign("resultStart", "ParseLexeme(input, inputPosition, \"\\\"\", StringComparison.Ordinal)"); // Invocation
							writer.IfNullReturnNull("resultStart");
							writer.VarAssign("nextInputPosition", "inputPosition + resultStart.Advanced");
							writer.VarAssign("sb", "new System.Text.StringBuilder()");
							using (writer.While("true"))
							{
								writer.VarAssign("inputPosition2", "nextInputPosition");
								writer.VarAssign("resultEscape", "ParseLexeme(input, inputPosition2, \"\\\\\", StringComparison.Ordinal)"); // Invocation
								using (writer.If("resultEscape != null"))
								{
									writer.Assign("inputPosition2", "inputPosition2 + resultEscape.Advanced");
									writer.VarAssign("resultToken", "ParseIntrinsic_AnyCharacter(input, inputPosition2, states, factory)"); // Invocation
									writer.IfNullReturnNull("resultToken");
									using (writer.Switch("resultToken.Node"))
									{
										writer.LineOfCode("case \"\\\\\":");
										writer.LineOfCode("case \"\\\"\":");
										writer.LineOfCode("case \"t\":");
										writer.LineOfCode("case \"r\":");
										writer.LineOfCode("case \"n\":");
										using (writer.BraceScope())
										{
											writer.LineOfCode("sb.Append(\"\\\\\");");
											writer.LineOfCode("sb.Append(resultToken.Node);");
											writer.SwitchBreak();
										}
										using (writer.SwitchDefault())
										{
											writer.Return("null");
										}
									}
									writer.Assign("inputPosition2", "inputPosition2 + resultToken.Advanced");

									writer.Continue();
								}
								writer.VarAssign("resultEnd", "ParseLexeme(input, inputPosition2, \"\\\"\", StringComparison.Ordinal)"); // Invocation
								using (writer.If("resultEnd != null"))
								{
									writer.Return($"new {parseResultClassName}<String>(sb.ToString(), inputPosition2 + resultEnd.Advanced - inputPosition)");
								}
								writer.VarAssign("resultChar", "ParseIntrinsic_AnyCharacter(input, inputPosition2, states, factory)"); // Invocation
								writer.IfNullReturnNull("resultChar");
								writer.LineOfCode("sb.Append(resultChar.Node);");
								writer.Assign("nextInputPosition", "inputPosition2 + resultChar.Advanced");
							}
						}

						// Optional Horizontal Whitespace
						writer.EndOfLine();
						using (writer.Method(Access.Private, true, $"{parseResultClassName}<String>", "ParseIntrinsic_OptionalHorizontalWhitespace", typicalParams))
						{
							using (writer.If("inputPosition >= input.Length"))
							{
								writer.Return($"new {parseResultClassName}<String>(String.Empty, 0)");
							}
							writer.VarAssign("sb", "new System.Text.StringBuilder()");
							writer.VarAssign("nextInputPosition", "inputPosition");
							using (writer.While("nextInputPosition < input.Length"))
							{
								writer.VarAssign("ch", "input[nextInputPosition]");
								using (writer.If("ch != \' \' && ch != \'\\t\'"))
								{
									writer.LineOfCode("break;");
								}
								writer.LineOfCode("nextInputPosition++;");
							}
							writer.Return($"new {parseResultClassName}<String>(input.Substring(inputPosition, nextInputPosition - inputPosition), nextInputPosition - inputPosition)");
						}
						writer.EndOfLine();

						// Write Lexeme
						using (writer.Method(Access.Private, true, $"{parseResultClassName}<String>", "ParseLexeme", new[] {
							new LocalVarDecl("String", "input"), // Invocation
							new LocalVarDecl("Int32", "inputPosition"),
							new LocalVarDecl("String", "lexeme"),
							new LocalVarDecl("StringComparison", "stringComparison")}))
						{
							writer.VarAssign("lexemeLength", "lexeme.Length");
							writer.IfTrueReturnNull("inputPosition + lexemeLength > input.Length");
							writer.IfTrueReturnNull("!String.Equals(input.Substring(inputPosition, lexemeLength), lexeme, stringComparison)"); 
							writer.Return($"new {parseResultClassName}<String>(lexeme, lexemeLength)");
						}

						// Generate methods
						var generateMethods = new GenerateMethodsVisitor(writer, interfaceName, invokers);
						var commentVisitor = new CommentParseFunctionVisitor(showHeader: true);
						foreach (var methodItem in signatures)
						{
							writer.EndOfLine();

							var target = methodItem.Func;
							writer.Comment(target.ApplyVisitor(commentVisitor));
							var returnType = target.ReturnType.GetParseResultTypeString();
							var name = methodItem.Name;

							using (writer.Method(methodItem.Access, true, returnType, name, typicalParams))
							{
								if (methodItem.IsMemoized)
								{
									var memName = NameGen.MemoizedFieldName(name);
									writer.LineOfCode($"if (states[inputPosition].{memName} is var mem && mem != null) {{ return mem; }}");
									writer.EndOfLine();
								}

								target.ApplyVisitor(generateMethods, methodItem);
							}
						}
						writer.EndOfLine();

						using (writer.Class(packratStateClassName, Access.Private))
						{
							foreach (var methodItem in signatures)
							{
								var returnType = methodItem.Func.ReturnType.GetParseResultTypeString();
								var memName = NameGen.MemoizedFieldName(methodItem.Name);

								if (methodItem.IsMemoized)
								{
									writer.MemberVariable(memName, returnType, Access.Internal);
								}
							}
						}
					}
				}

				return writer.GetText();
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
			{
				return exception.ToString();
			}
		}
	}
}
