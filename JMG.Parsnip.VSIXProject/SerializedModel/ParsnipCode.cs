using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.CodeWriting;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject.SerializedModel
{
	internal class ParsnipCode
	{
		public IReadOnlyDictionary<IParseFunction, Invoker> Invokers => invokers;
		private Dictionary<IParseFunction, Invoker> invokers = new Dictionary<IParseFunction, Invoker>();

		public IReadOnlyDictionary<String, String> RuleMethodNames => ruleMethodNames;
		private Dictionary<String, String> ruleMethodNames = new Dictionary<String, String>();

		public IReadOnlyDictionary<IntrinsicType, String> Intrinsics => intrinsics;
		private Dictionary<IntrinsicType, String> intrinsics = new Dictionary<IntrinsicType, String>();

		private List<Signature> methodItems = new List<Signature>();

		public ParsnipCode()
		{
			this.intrinsics[IntrinsicType.AnyCharacter] = "ParseIntrinsic_AnyCharacter";
			this.intrinsics[IntrinsicType.AnyLetter] = "ParseIntrinsic_AnyLetter";
			this.intrinsics[IntrinsicType.EndOfLine] = "ParseIntrinsic_EndOfLine";
			this.intrinsics[IntrinsicType.EndOfStream] = "ParseIntrinsic_EndOfStream";
			this.intrinsics[IntrinsicType.CString] = "ParseIntrinsic_CString";
		}

		private void AddIntrinsic(IntrinsicType type, String name)
		{
			var sig = new Signature(name, Access.Private, new Intrinsic(type), (s, f) => $"{name}({s}, {f})", isMemoized: false);
			this.methodItems.Add(sig);			
		}

		public void AddSignature(Signature signature) => this.methodItems.Add(signature);
		public void MapFunctionInvocation(IParseFunction function, Invoker invoker) => this.invokers[function] = invoker;

		public void WriteMethods(CodeWriter writer, String interfaceName, ParsnipModel semanticModel)
		{
			// Populate dictionary of rule identifiers to method names
			foreach (var rule in semanticModel.Rules)
			{
				var methodName = NameGen.ParseFunctionMethodName(rule.RuleIdentifier);
				this.ruleMethodNames[rule.RuleIdentifier] = methodName;
			}

			// Generate method signatures
			var methodAccess = Access.Public;
			foreach (var rule in semanticModel.Rules)
			{
				var methodName = this.RuleMethodNames[rule.RuleIdentifier];
				rule.ParseFunction.ApplyVisitor(new GenerateSignaturesVisitor(this, methodName, isMemoized: true), methodAccess);

				// Only the first method is public
				methodAccess = Access.Private;
			}

			writer.LineOfCode("");
			WriteCardinalityMethods(writer, interfaceName);
			writer.LineOfCode("");
			WriteIntrinsics(writer, interfaceName);
			writer.LineOfCode("");
			WriteLexeme(writer);

			// Generate methods
			var generateMethods = new GenerateMethodsVisitor(this, writer, interfaceName);
			var commentVisitor = new CommentParseFunctionVisitor(showHeader: true);
			foreach (var methodItem in this.methodItems)
			{
				writer.EndOfLine();

				var func = methodItem.Func;
				writer.Comment(func.ApplyVisitor(commentVisitor));
				func.ApplyVisitor(generateMethods, methodItem);
			}
		}

		private static void WriteCardinalityMethods(CodeWriter writer, String interfaceName)
		{
			// Maybe			
			var cardParams = new[] {
				new LocalVarDecl("PackratState", "state"),
				new LocalVarDecl(interfaceName, "factory"),
				new LocalVarDecl($"Func<PackratState, {interfaceName}, ParseResult<T>>", "parseAction")
			};
			using (writer.Method(Access.Private, true, "ParseResult<T>", "ParseMaybe<T>", cardParams))
			{
				writer.VarAssign("result", "parseAction(state, factory)");
				using (writer.If("result != null"))
				{
					writer.Return("result");
				}
				writer.Return("new ParseResult<T> { State = state, Node = default(T) }");
			}

			// Star
			writer.LineOfCode("");
			using (writer.Method(Access.Private, true, "ParseResult<IReadOnlyList<T>>", "ParseStar<T>", cardParams))
			{
				writer.VarAssign("list", "new List<T>()");
				using (writer.While("true"))
				{
					writer.VarAssign("nextResult", "parseAction(state, factory)");
					using (writer.If("nextResult == null"))
					{
						writer.SwitchBreak();
					}
					writer.LineOfCode("list.Add(nextResult.Node);");
					writer.Assign("state", "nextResult.State");
				}
				writer.Return("new ParseResult<IReadOnlyList<T>> { State = state, Node = list }");
			}

			// Plus
			writer.LineOfCode("");
			using (writer.Method(Access.Private, true, "ParseResult<IReadOnlyList<T>>", "ParsePlus<T>", cardParams))
			{
				writer.VarAssign("list", "new List<T>()");

				writer.VarAssign("firstResult", "parseAction(state, factory)");
				using (writer.If("firstResult == null"))
				{
					writer.Return("null");
				}
				writer.LineOfCode("list.Add(firstResult.Node);");
				writer.Assign("state", "firstResult.State");

				using (writer.While("true"))
				{
					writer.VarAssign("nextResult", "parseAction(state, factory)");
					using (writer.If("nextResult == null"))
					{
						writer.SwitchBreak();
					}
					writer.LineOfCode("list.Add(nextResult.Node);");
					writer.Assign("state", "nextResult.State");
				}
				writer.Return("new ParseResult<IReadOnlyList<T>> { State = state, Node = list }");
			}
		}

		private static void WriteIntrinsics(CodeWriter writer, String interfaceName)
		{
			var typicalParams = new List<LocalVarDecl>
			{
				new LocalVarDecl("PackratState", "state"),
				new LocalVarDecl(interfaceName, "factory")
			};

			// Any Character
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseIntrinsic_AnyCharacter", typicalParams))
			{
				writer.VarAssign("input", "state.input");
				writer.VarAssign("inputPosition", "state.inputPosition");
				using (writer.If("inputPosition >= input.Length"))
				{
					writer.Return("null");
				}
				writer.Return("new ParseResult<String>() { Node = state.input.Substring(inputPosition, 1), State = state.states[inputPosition + 1] }");
			}

			// Any Letter
			writer.LineOfCode("");
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseIntrinsic_AnyLetter", typicalParams))
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
			}

			// End of Line
			writer.LineOfCode("");
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseIntrinsic_EndOfLine", typicalParams))
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
			}

			// End of Stream
			writer.LineOfCode("");
			using (writer.Method(Access.Private, true, "ParseResult<EmptyNode>", "ParseIntrinsic_EndOfStream", typicalParams))
			{
				writer.VarAssign("input", "state.input");
				writer.VarAssign("inputPosition", "state.inputPosition");
				using (writer.If("inputPosition == input.Length"))
				{
					writer.Return("new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = state }");
				}
				writer.Return("null");
			}

			// C-String
			writer.LineOfCode("");
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseIntrinsic_CString", typicalParams))
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
			}
		}

		private static void WriteLexeme(CodeWriter writer)
		{
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseLexeme", new[] { new LocalVarDecl("PackratState", "state"), new LocalVarDecl("String", "lexeme") }))
			{
				writer.VarAssign("lexemeLength", "lexeme.Length");
				writer.IfTrueReturnNull("state.inputPosition + lexemeLength > state.input.Length");
				writer.IfTrueReturnNull("state.input.Substring(state.inputPosition, lexemeLength) != lexeme");
				writer.Return("new ParseResult<String>() { Node = lexeme, State = state.states[state.inputPosition + lexemeLength] }");
			}
		}
	}
}
