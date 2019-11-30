﻿using System;
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
			this.intrinsics[IntrinsicType.AnyDigit] = "ParseIntrinsic_AnyDigit";
			this.intrinsics[IntrinsicType.EndOfLine] = "ParseIntrinsic_EndOfLine";
			this.intrinsics[IntrinsicType.EndOfStream] = "ParseIntrinsic_EndOfStream";
			this.intrinsics[IntrinsicType.EndOfLineOrStream] = "ParseIntrinsic_EndOfLineOrStream";
			this.intrinsics[IntrinsicType.CString] = "ParseIntrinsic_CString";
			this.intrinsics[IntrinsicType.OptionalHorizontalWhitespace] = "ParseIntrinsic_OptionalHorizontalWhitespace";
		}

		public void AddSignature(Signature signature) => this.methodItems.Add(signature);

		public void MapFunctionInvocation(IParseFunction function, Invoker invoker) => this.invokers[function] = invoker;

		private IDisposable WriteMethodSignature(CodeWriter writer, IParseFunction target, Access access, String name, String inputPositionName, String statesName, IReadOnlyList<LocalVarDecl> parameters, Boolean isMemoized)
		{
			var returnType = target.ReturnType.GetParseResultTypeString();
			var memName = NameGen.MemoizedFieldName(name);

			var disposable = writer.Method(access, true, returnType, name, parameters);

			if (isMemoized)
			{
				writer.LineOfCode($"if ({statesName}[{inputPositionName}].{memName} is var mem && mem != null) {{ return mem; }}");
				writer.EndOfLine();
			}

			return disposable;
		}

		private static void WriteMemoizedField(CodeWriter writer, IParseFunction target, String name, Boolean isMemoized)
		{
			var returnType = target.ReturnType.GetParseResultTypeString();
			var memName = NameGen.MemoizedFieldName(name);

			if (isMemoized)
			{
				writer.LineOfCode($"internal {returnType} {memName};");
			}
		}

		public void WriteMethods(CodeWriter writer, String interfaceName, ParsnipModel semanticModel)
		{
			// Populate dictionary of rule identifiers to method names
			foreach (var rule in semanticModel.Rules)
			{
				var methodName = NameGen.ParseFunctionMethodName(rule.RuleIdentifier);
				this.ruleMethodNames[rule.RuleIdentifier] = methodName;
			}

			// Generate method signatures
			var methodAccess = Access.Private;
			foreach (var rule in semanticModel.Rules)
			{
				var methodName = this.RuleMethodNames[rule.RuleIdentifier];
				rule.ParseFunction.ApplyVisitor(new GenerateSignaturesVisitor(this, methodName, isMemoized: true, mustAddSignature: true), methodAccess);
			}

			writer.EndOfLine();
			WriteCardinalityMethods(writer, interfaceName);
			writer.EndOfLine();
			WriteDelimiterMethod(writer, interfaceName);
			writer.EndOfLine();
			WriteIntrinsics(writer, interfaceName);
			writer.EndOfLine();
			WriteLexeme(writer);

			var typicalParams = new List<LocalVarDecl> // Invocation
			{
				new LocalVarDecl("String", "input"),
				new LocalVarDecl("Int32", "inputPosition"),
				new LocalVarDecl("PackratState[]", "states"),
				new LocalVarDecl(interfaceName, "factory")
			};

			// Generate methods
			var generateMethods = new GenerateMethodsVisitor(this, writer, interfaceName);
			var commentVisitor = new CommentParseFunctionVisitor(showHeader: true);
			foreach (var methodItem in this.methodItems)
			{
				writer.EndOfLine();

				var target = methodItem.Func;
				writer.Comment(target.ApplyVisitor(commentVisitor));
				var returnType = target.ReturnType.GetParseResultTypeString();
				using (WriteMethodSignature(writer, target, methodItem.Access, methodItem.Name, "inputPosition", "states", typicalParams, methodItem.IsMemoized))
				{
					target.ApplyVisitor(generateMethods, methodItem);
				}
			}
		}

		public void WriteMemoizedFields(CodeWriter writer)
		{
			foreach (var methodItem in this.methodItems)
			{
				var target = methodItem.Func;
				WriteMemoizedField(writer, target, methodItem.Name, methodItem.IsMemoized);
			}
		}

		private static void WriteDelimiterMethod(CodeWriter writer, String interfaceName)
		{
			// Delimited
			var delimParams = new[] {
				new LocalVarDecl("String", "input"), // Invocation
				new LocalVarDecl("Int32", "inputPosition"),
				new LocalVarDecl("PackratState[]", "states"),
				new LocalVarDecl(interfaceName, "factory"),
				new LocalVarDecl($"Func<String, Int32, PackratState[], {interfaceName}, ParseResult<T>>", "parseAction"), // Invocation
				new LocalVarDecl($"Func<String, Int32, PackratState[], {interfaceName}, ParseResult<D>>", "parseDelimiterAction") // Invocation
			};

			using (writer.Method(Access.Private, true, "ParseResult<IReadOnlyList<T>>", "ParseSeries<T, D>", delimParams))
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
				writer.Return("new ParseResult<IReadOnlyList<T>>(list, nextResultInputPosition - inputPosition)");
			}
		}

		private static void WriteCardinalityMethods(CodeWriter writer, String interfaceName)
		{
			// Maybe			
			var cardParams = new[] {
				new LocalVarDecl("String", "input"), // Invocation
				new LocalVarDecl("Int32", "inputPosition"),
				new LocalVarDecl("PackratState[]", "states"),
				new LocalVarDecl(interfaceName, "factory"),
				new LocalVarDecl($"Func<String, Int32, PackratState[], {interfaceName}, ParseResult<T>>", "parseAction") // Invocation
			};
			using (writer.Method(Access.Private, true, "ParseResult<T>", "ParseMaybe<T>", cardParams))
			{
				writer.VarAssign("result", "parseAction(input, inputPosition, states, factory)"); // Invocation
				using (writer.If("result != null"))
				{
					writer.Return("result");
				}
				writer.Return("new ParseResult<T>(default(T), 0)");
			}

			// Star
			writer.EndOfLine();
			using (writer.Method(Access.Private, true, "ParseResult<IReadOnlyList<T>>", "ParseStar<T>", cardParams))
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
				writer.Return("new ParseResult<IReadOnlyList<T>>(list, nextResultInputPosition - inputPosition)");
			}

			// Plus
			writer.EndOfLine();
			using (writer.Method(Access.Private, true, "ParseResult<IReadOnlyList<T>>", "ParsePlus<T>", cardParams))
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
				writer.Return("new ParseResult<IReadOnlyList<T>>(list, nextResultInputPosition - inputPosition)");
			}
		}

		private static void WriteIntrinsics(CodeWriter writer, String interfaceName)
		{
			var typicalParams = new List<LocalVarDecl>
			{
				new LocalVarDecl("String", "input"), // Invocation
				new LocalVarDecl("Int32", "inputPosition"),
				new LocalVarDecl("PackratState[]", "states"),
				new LocalVarDecl(interfaceName, "factory")
			};

			// Any Character
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseIntrinsic_AnyCharacter", typicalParams))
			{
				using (writer.If("inputPosition >= input.Length"))
				{
					writer.Return("null");
				}
				writer.Return("new ParseResult<String>(input.Substring(inputPosition, 1), 1)");
			}

			// Any Letter
			writer.EndOfLine();
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseIntrinsic_AnyLetter", typicalParams))
			{
				using (writer.If("inputPosition >= input.Length"))
				{
					writer.Return("null");
				}
				using (writer.ElseIf("!Char.IsLetter(input[inputPosition])"))
				{
					writer.Return("null");

				}
				writer.Return("new ParseResult<String>(input.Substring(inputPosition, 1), 1)");
			}


			// Any Digit
			writer.EndOfLine();
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseIntrinsic_AnyDigit", typicalParams))
			{
				using (writer.If("inputPosition >= input.Length"))
				{
					writer.Return("null");
				}
				using (writer.ElseIf("!Char.IsDigit(input[inputPosition])"))
				{
					writer.Return("null");

				}
				writer.Return("new ParseResult<String>(input.Substring(inputPosition, 1), 1)");
			}

			// End of Line
			writer.EndOfLine();
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseIntrinsic_EndOfLine", typicalParams))
			{
				writer.VarAssign("result1", "ParseLexeme(input, inputPosition, \"\\r\\n\")"); // Invocation
				using (writer.If("result1 != null"))
				{
					writer.Return("new ParseResult<String>(result1.Node, result1.Advanced)");
				}

				writer.VarAssign("result2", "ParseLexeme(input, inputPosition, \"\\n\")"); // Invocation
				using (writer.If("result2 != null"))
				{
					writer.Return("new ParseResult<String>(result2.Node, result2.Advanced)");
				}

				writer.Return("null");
			}

			// End of Stream
			writer.EndOfLine();
			using (writer.Method(Access.Private, true, "ParseResult<EmptyNode>", "ParseIntrinsic_EndOfStream", typicalParams))
			{
				using (writer.If("inputPosition == input.Length"))
				{
					writer.Return("new ParseResult<EmptyNode>(EmptyNode.Instance, 0)");
				}
				writer.Return("null");
			}

			// End of Line Or Stream
			writer.EndOfLine();
			using (writer.Method(Access.Private, true, "ParseResult<EmptyNode>", "ParseIntrinsic_EndOfLineOrStream", typicalParams))
			{
				using (writer.If("inputPosition == input.Length"))
				{
					writer.Return("new ParseResult<EmptyNode>(EmptyNode.Instance, 0)");
				}

				writer.VarAssign("result1", "ParseLexeme(input, inputPosition, \"\\r\\n\")"); // Invocation
				using (writer.If("result1 != null"))
				{
					writer.Return("new ParseResult<EmptyNode>(EmptyNode.Instance, result1.Advanced)");
				}

				writer.VarAssign("result2", "ParseLexeme(input, inputPosition, \"\\n\")"); // Invocation
				using (writer.If("result2 != null"))
				{
					writer.Return("new ParseResult<EmptyNode>(EmptyNode.Instance, result2.Advanced)");
				}

				writer.Return("null");
			}

			// C-String
			writer.EndOfLine();
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseIntrinsic_CString", typicalParams))
			{
				writer.VarAssign("resultStart", "ParseLexeme(input, inputPosition, \"\\\"\")"); // Invocation
				writer.IfNullReturnNull("resultStart");
				writer.VarAssign("nextInputPosition", "inputPosition + resultStart.Advanced");
				writer.VarAssign("sb", "new System.Text.StringBuilder()");
				using (writer.While("true"))
				{
					writer.VarAssign("inputPosition2", "nextInputPosition");
					writer.VarAssign("resultEscape", "ParseLexeme(input, inputPosition2, \"\\\\\")"); // Invocation
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
					writer.VarAssign("resultEnd", "ParseLexeme(input, inputPosition2, \"\\\"\")"); // Invocation
					using (writer.If("resultEnd != null"))
					{
						writer.Return("new ParseResult<String>(sb.ToString(), inputPosition2 + resultEnd.Advanced - inputPosition)");
					}
					writer.VarAssign("resultChar", "ParseIntrinsic_AnyCharacter(input, inputPosition2, states, factory)"); // Invocation
					writer.IfNullReturnNull("resultChar");
					writer.LineOfCode("sb.Append(resultChar.Node);");
					writer.Assign("nextInputPosition", "inputPosition2 + resultChar.Advanced");
				}
			}

			// Optional Horizontal Whitespace
			writer.EndOfLine();
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseIntrinsic_OptionalHorizontalWhitespace", typicalParams))
			{
				using (writer.If("inputPosition >= input.Length"))
				{
					writer.Return("new ParseResult<String>(String.Empty, 0)");
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
				writer.Return("new ParseResult<String>(input.Substring(inputPosition, nextInputPosition - inputPosition), nextInputPosition - inputPosition)");
			}
		}

		private static void WriteLexeme(CodeWriter writer)
		{
			using (writer.Method(Access.Private, true, "ParseResult<String>", "ParseLexeme", new[] {
				new LocalVarDecl("String", "input"), // Invocation
				new LocalVarDecl("Int32", "inputPosition"),
				new LocalVarDecl("String", "lexeme") }))
			{
				writer.VarAssign("lexemeLength", "lexeme.Length");
				writer.IfTrueReturnNull("inputPosition + lexemeLength > input.Length");
				writer.IfTrueReturnNull("input.Substring(inputPosition, lexemeLength) != lexeme");
				writer.Return("new ParseResult<String>(lexeme, lexemeLength)");
			}
		}
	}
}
