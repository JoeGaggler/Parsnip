using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.CodeWriting;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject.SerializedModel
{
	internal class GenerateSignaturesVisitor : IParseFunctionActionVisitor<Access>
	{
		private readonly ParsnipCode parsnipCode;
		private readonly String baseName;
		private readonly Boolean isMemoized;
		private readonly Boolean mustAddSignature;

		public GenerateSignaturesVisitor(ParsnipCode parsnipCode, String baseName, Boolean isMemoized, Boolean mustAddSignature)
		{
			this.parsnipCode = parsnipCode;
			this.baseName = baseName;
			this.isMemoized = isMemoized;
			this.mustAddSignature = mustAddSignature;
		}

		private static Invoker CreateInvoker(String baseName) => (i, p, s, f) => $"{baseName}({i}, {p}, {s}, {f})"; // Invocation

		private void AddSignature(IParseFunction target, Access access, Invoker invoker)
		{
			parsnipCode.MapFunctionInvocation(target, invoker);
			parsnipCode.AddSignature(new Signature(baseName, access, target, invoker, isMemoized));
		}

		private void AddInvoker(IParseFunction target, Invoker invoker) => this.parsnipCode.MapFunctionInvocation(target, invoker);

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

				var visitor = new GenerateSignaturesVisitor(parsnipCode, stepBaseName, isMemoized: false, mustAddSignature: false);
				func.ApplyVisitor(visitor, Access.Private);
			}
		}

		public void Visit(Intrinsic target, Access access)
		{
			var methodName = parsnipCode.Intrinsics[target.Type];
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

		public void Visit(LiteralString target, Access access)
		{
			var expanded = target.Text.Replace("\\", "\\\\");
			Invoker invoker = (i, p, s, f) => $"ParseLexeme({i}, {p}, {s}, \"{expanded}\")"; // Invocation
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
			var methodName = parsnipCode.RuleMethodNames[target.Identifier];
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
			var visitor = new GenerateSignaturesVisitor(parsnipCode, baseName + "_M", isMemoized: false, mustAddSignature: false);
			inner.ApplyVisitor(visitor, Access.Private);
		}

		public void Visit(Series target, Access access)
		{
			var invoker = CreateInvoker(baseName);
			AddSignature(target, access, invoker);

			var visitor = new GenerateSignaturesVisitor(parsnipCode, baseName + "_D", isMemoized: false, mustAddSignature: false);
			target.RepeatedToken.ApplyVisitor(visitor, Access.Private);
			target.DelimiterToken.ApplyVisitor(visitor, Access.Private);
		}
	}
}
