using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.CodeWriting;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject.SerializedModel
{
	internal class GenerateMethodsVisitor : IParseFunctionActionVisitor<Signature>
	{
		private readonly CodeWriter writer;
		private readonly String interfaceName;
		private readonly ParsnipCode parsnipCode;

		private readonly IReadOnlyList<LocalVarDecl> typicalParams;

		public GenerateMethodsVisitor(ParsnipCode parsnipCode, CodeWriter writer, String interfaceName)
		{
			this.writer = writer;
			this.interfaceName = interfaceName;
			this.parsnipCode = parsnipCode;
			this.typicalParams = new List<LocalVarDecl>
			{
				new LocalVarDecl("PackratState", "state"),
				new LocalVarDecl(interfaceName, "factory")
			};
		}

		private static String GetParseResultTypeString(IParseFunction target) => $"ParseResult<{NameGen.TypeString(target.ReturnType)}>";

		public void Visit(Selection target, Signature input)
		{
			var returnType = GetParseResultTypeString(target);
			using (writer.Method(input.Access, true, returnType, input.Name, typicalParams))
			{
				int stepIndex = 0;
				foreach (var step in target.Steps)
				{
					stepIndex++;
					var func = step.Function;
					var invoker = this.parsnipCode.Invokers[func];
					var resultName = $"r{stepIndex}";

					writer.VarAssign(resultName, invoker("state", "factory"));

					var nodeReference = $"{resultName}.Node";
					if (target.ReturnType == EmptyNodeType.Instance)
					{
						nodeReference = "EmptyNode.Instance";
					}
					else if (step.InterfaceMethod is InterfaceMethod method)
					{
						var param = nodeReference;
						if (func.ReturnType == EmptyNodeType.Instance)
						{
							param = String.Empty;
						}

						nodeReference = $"factory.{method.Name}({param})";
					}
					writer.LineOfCode($"if ({resultName} != null) return new {returnType}() {{ Node = {nodeReference}, State = {resultName}.State }};");
				}

				// No choices matched
				writer.Return("null");
			}
		}

		public void Visit(Sequence target, Signature input)
		{
			var returnType = GetParseResultTypeString(target);
			using (writer.Method(input.Access, true, returnType, input.Name, typicalParams))
			{
				var returnedResults = new List<String>();

				int stepIndex = 0;
				var currentState = "state";
				foreach (var step in target.Steps)
				{
					stepIndex++;
					var func = step.Function;
					var invoker = this.parsnipCode.Invokers[func];
					var resultName = $"r{stepIndex}";
					var nodeName = $"{resultName}.Node";
					var nextState = $"{resultName}.State";

					if (step.IsReturned)
					{
						returnedResults.Add(nodeName);
					}

					writer.VarAssign(resultName, invoker(currentState, "factory"));

					switch (step.MatchAction)
					{
						case MatchAction.Consume: writer.IfNullReturnNull(resultName); currentState = nextState; break;
						case MatchAction.Fail: writer.IfNotNullReturnNull(resultName); currentState = nextState; break;
						case MatchAction.Rewind: writer.IfNullReturnNull(resultName); break;
						case MatchAction.Ignore: writer.IfNullReturnNull(resultName); currentState = nextState; break;
					}
				}

				String nodeReference;
				if (returnedResults.Count == 0)
				{
					nodeReference = "EmptyNode.Instance";
				}
				else if (returnedResults.Count == 1)
				{
					nodeReference = returnedResults[0];
				}
				else
				{
					nodeReference = $"({String.Join(", ", returnedResults)})";
				}

				writer.Return($"new {returnType}() {{ Node = {nodeReference}, State = {currentState} }}");
			}
		}

		public void Visit(Intrinsic target, Signature input)
		{
			var returnType = GetParseResultTypeString(target);
			using (writer.Method(input.Access, true, returnType, input.Name, typicalParams))
			{
				switch (target.Type)
				{
					case IntrinsicType.AnyCharacter:
					{
						writer.VarAssign("input", "state.input");
						writer.VarAssign("inputPosition", "state.inputPosition");
						using (writer.If("inputPosition >= input.Length"))
						{
							writer.Return("null");
						}
						writer.Return("new ParseResult<String>() { Node = state.input.Substring(inputPosition, 1), State = state.states[inputPosition + 1] }");
						break;
					}
					case IntrinsicType.AnyLetter:
					{
						writer.VarAssign("input", "state.input");
						writer.VarAssign("inputPosition", "state.inputPosition");
						using (writer.If("inputPosition >= input.Length"))
						{
							writer.Return("null");
						}
						using (writer.ElseIf("!Char.IsLetter(input[inputPosition])"))
						{
							writer.Return("null");

						}
						writer.Return("new ParseResult<String>() { Node = state.input.Substring(inputPosition, 1), State = state.states[inputPosition + 1] }");
						break;
					}
					case IntrinsicType.EndOfLine:
					{
						writer.VarAssign("result1", "ParseLexeme(state, \"\\r\\n\")");
						using (writer.If("result1 != null"))
						{
							writer.Return("new ParseResult<String>() { Node = result1.Node, State = result1.State }");
						}

						writer.VarAssign("result2", "ParseLexeme(state, \"\\n\")");
						using (writer.If("result2 != null"))
						{
							writer.Return("new ParseResult<String>() { Node = result2.Node, State = result2.State }");
						}

						writer.Return("null");
						break;
					}
					case IntrinsicType.EndOfStream:
					{
						writer.VarAssign("input", "state.input");
						writer.VarAssign("inputPosition", "state.inputPosition");
						using (writer.If("inputPosition == input.Length"))
						{
							writer.Return("new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = state }");
						}
						writer.Return("null");
						break;
					}
					case IntrinsicType.CString:
					{
						writer.VarAssign("resultStart", "ParseLexeme(state, \"\\\"\")");
						writer.IfNullReturnNull("resultStart");
						writer.VarAssign("currentState", "resultStart.State");
						writer.VarAssign("sb", "new System.Text.StringBuilder()");
						using (writer.While("true"))
						{
							writer.VarAssign("resultEscape", "ParseLexeme(currentState, \"\\\\\")");
							using (writer.If("resultEscape != null"))
							{
								writer.VarAssign("resultToken", "ParseIntrinsic_AnyCharacter(resultEscape.State, factory)");
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
								writer.Assign("currentState", "resultToken.State");
								writer.Continue();
							}
							writer.VarAssign("resultEnd", "ParseLexeme(currentState, \"\\\"\")");
							using (writer.If("resultEnd != null"))
							{
								writer.Return("new ParseResult<String>() { Node = sb.ToString(), State = resultEnd.State }");
							}
							writer.VarAssign("resultChar", "ParseIntrinsic_AnyCharacter(currentState, factory)");
							writer.IfNullReturnNull("resultChar");
							writer.LineOfCode("sb.Append(resultChar.Node);");
							writer.Assign("currentState", "resultChar.State");
						}
						break;
					}
					default:
					{
						writer.Comment("TODO: Intrinsic");
						break;
					}
				}
			}
		}

		public void Visit(LiteralString target, Signature input)
		{
			var returnType = GetParseResultTypeString(target);
			using (writer.Method(input.Access, true, returnType, input.Name, new[] { new LocalVarDecl("PackratState", "state"), new LocalVarDecl("String", "lexeme") }))
			{
				writer.VarAssign("lexemeLength", "lexeme.Length");
				writer.IfTrueReturnNull("state.inputPosition + lexemeLength > state.input.Length");
				writer.IfTrueReturnNull("state.input.Substring(state.inputPosition, lexemeLength) != lexeme");
				writer.Return("new ParseResult<String>() { Node = lexeme, State = state.states[state.inputPosition + lexemeLength] }");
			}
		}

		public void Visit(ReferencedRule target, Signature input)
		{
			writer.Comment("TODO: ReferencedRule");
		}

		public void Visit(CardinalityFunction target, Signature input)
		{
			writer.Comment("TODO: CardinalityFunction");
		}
	}
}
