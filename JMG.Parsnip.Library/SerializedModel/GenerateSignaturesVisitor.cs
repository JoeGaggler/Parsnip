using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.CodeWriting;
using JMG.Parsnip.SemanticModel;

namespace JMG.Parsnip.SerializedModel
{
	/// <summary>
	/// Parse function visitor that generates the method signature
	/// </summary>
	internal class GenerateSignaturesVisitor : IParseFunctionActionVisitor<Access>
	{
		private readonly String baseName;
		private readonly Boolean isMemoized;
		private readonly Boolean mustAddSignature;
		private readonly IReadOnlyDictionary<String, String> ruleMethodNames;
		private readonly List<Signature> signatures;
		private readonly Dictionary<IParseFunction, Invoker> invokers;

		public GenerateSignaturesVisitor(
			String baseName, 
			Boolean isMemoized, 
			Boolean mustAddSignature,
			IReadOnlyDictionary<String, String> ruleMethodNames,
			List<Signature> signatures,
			Dictionary<IParseFunction, Invoker> invokers)
		{
			this.baseName = baseName;
			this.isMemoized = isMemoized;
			this.mustAddSignature = mustAddSignature;
			this.ruleMethodNames = ruleMethodNames;
			this.signatures = signatures;
			this.invokers = invokers;
		}

		private static Invoker CreateInvoker(String baseName) => (i, p, s, f) => $"{baseName}({i}, {p}, {s}, {f})"; // Invocation

		private void AddSignature(IParseFunction target, Access access, Invoker invoker)
		{
			this.invokers[target] = invoker;
			this.signatures.Add(new Signature(baseName, access, target, invoker, isMemoized));
		}

		private void AddInvoker(IParseFunction target, Invoker invoker) => this.invokers[target] = invoker;

		public void Visit(Selection target, Access access)
		{
			VisitFunctionWithSteps(target, access, "C", t => t.Steps.Select(i => i.Function).ToArray());
		}

		public void Visit(Sequence target, Access access)
		{
			VisitFunctionWithSteps(target, access, "S", t => t.Steps.Select(i => i.Function).ToArray());
		}

		private void VisitFunctionWithSteps<T>(T target, Access access, String letter, Func<T, IReadOnlyList<IParseFunction>> stepsFunctions) where T : IParseFunction
		{
			var invoker = CreateInvoker(baseName);
			AddSignature(target, access, invoker);

			int index = 0;
			foreach (var func in stepsFunctions(target))
			{
				index++;
				String stepBaseName = $"{baseName}_{letter}{index}";

				var visitor = new GenerateSignaturesVisitor(stepBaseName, isMemoized: false, mustAddSignature: false, ruleMethodNames, this.signatures, this.invokers);
				func.ApplyVisitor(visitor, Access.Private);
			}
		}

		public void Visit(Intrinsic target, Access access)
		{
			var methodName = target.Type switch
			{
				IntrinsicType.AnyCharacter => "ParseIntrinsic_AnyCharacter",
				IntrinsicType.AnyLetter => "ParseIntrinsic_AnyLetter",
				IntrinsicType.AnyDigit => "ParseIntrinsic_AnyDigit",
				IntrinsicType.EndOfLine => "ParseIntrinsic_EndOfLine",
				IntrinsicType.EndOfStream => "ParseIntrinsic_EndOfStream",
				IntrinsicType.EndOfLineOrStream => "ParseIntrinsic_EndOfLineOrStream",
				IntrinsicType.CString => "ParseIntrinsic_CString",
				IntrinsicType.OptionalHorizontalWhitespace => "ParseIntrinsic_OptionalHorizontalWhitespace",
			};

			Invoker invoker = CreateInvoker(methodName);
			if (this.mustAddSignature)
			{
				AddSignature(target, access, invoker);
			}
			else
			{
				AddInvoker(target, invoker);
			}
		}

		public void Visit(LiteralString target, Access access)
		{
			var expanded = target.Text.Replace("\\", "\\\\");
			Invoker invoker = (i, p, s, f) => $"ParseLexeme({i}, {p}, \"{expanded}\")"; // Invocation
			if (this.mustAddSignature)
			{
				AddSignature(target, access, invoker);
			}
			else
			{
				AddInvoker(target, invoker);
			}
		}

		public void Visit(ReferencedRule target, Access access)
		{
			var methodName = this.ruleMethodNames[target.Identifier];
			Invoker invoker = (i, p, s, f) => $"{methodName}({i}, {p}, {s}, {f})"; // Invocation
			if (this.mustAddSignature)
			{
				AddSignature(target, access, invoker);
			}
			else
			{
				AddInvoker(target, invoker);
			}
		}

		public void Visit(Repetition target, Access access)
		{
			var invoker = CreateInvoker(baseName);
			AddSignature(target, access, invoker);

			var inner = target.InnerParseFunction;
			var visitor = new GenerateSignaturesVisitor(baseName + "_M", isMemoized: false, mustAddSignature: false, ruleMethodNames, this.signatures, this.invokers);
			inner.ApplyVisitor(visitor, Access.Private);
		}

		public void Visit(Series target, Access access)
		{
			var invoker = CreateInvoker(baseName);
			AddSignature(target, access, invoker);

			var visitor = new GenerateSignaturesVisitor(baseName + "_D", isMemoized: false, mustAddSignature: false, ruleMethodNames, this.signatures, this.invokers);
			target.RepeatedToken.ApplyVisitor(visitor, Access.Private);
			target.DelimiterToken.ApplyVisitor(visitor, Access.Private);
		}
	}
}
