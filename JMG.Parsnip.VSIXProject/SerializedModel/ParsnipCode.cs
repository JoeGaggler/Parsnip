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

		public IReadOnlyDictionary<IntrinsicType, Signature> Intrinsics => intrinsics;
		private Dictionary<IntrinsicType, Signature> intrinsics = new Dictionary<IntrinsicType, Signature>();

		private List<Signature> methodItems = new List<Signature>();

		public ParsnipCode()
		{
			AddIntrinsic(IntrinsicType.AnyCharacter, "ParseIntrinsic_AnyCharacter");
			AddIntrinsic(IntrinsicType.AnyLetter, "ParseIntrinsic_AnyLetter");
			AddIntrinsic(IntrinsicType.EndOfLine, "ParseIntrinsic_EndOfLine");
			AddIntrinsic(IntrinsicType.EndOfStream, "ParseIntrinsic_EndOfStream");
			AddIntrinsic(IntrinsicType.CString, "ParseIntrinsic_CString");

			// Lexeme
			var lexemeName = "ParseLexeme";
			var sig = new Signature(lexemeName, Access.Private, new LiteralString("HACK: PLACEHOLDER"), (s, f) => $"{lexemeName}({s}, {f})");
			this.methodItems.Add(sig);
		}

		private void AddIntrinsic(IntrinsicType type, String name)
		{
			var sig = new Signature(name, Access.Private, new Intrinsic(type), (s, f) => $"{name}({s}, {f})");
			this.methodItems.Add(sig);
			this.intrinsics[type] = sig;
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
				rule.ParseFunction.ApplyVisitor(new GenerateSignaturesVisitor(this, methodName), methodAccess);

				// Only the first method is public
				methodAccess = Access.Private;
			}

			// Maybe
			writer.LineOfCode("");
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

			// Generate methods
			var vis = new GenerateMethodsVisitor(this, writer, interfaceName);
			var commentVisitor = new CommentParseFunctionVisitor(showHeader: true);
			foreach (var methodItem in this.methodItems)
			{
				writer.EndOfLine();

				var func = methodItem.Func;
				writer.Comment(func.ApplyVisitor(commentVisitor));
				func.ApplyVisitor(vis, methodItem);
			}
		}
	}
}
