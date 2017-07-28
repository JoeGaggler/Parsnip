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

		private static Invoker CreateInvoker(String baseName) => (s, f) => $"{baseName}({s}, {f})";

		private void AddSignature(IParseFunction target, Access access, Invoker invoker)
		{
			parsnipCode.MapFunctionInvocation(target, invoker);
			parsnipCode.AddSignature(new Signature(baseName, access, target, invoker, isMemoized));
		}

		private void AddInvoker(IParseFunction target, Invoker invoker) => this.parsnipCode.MapFunctionInvocation(target, invoker);

		public void Visit(Selection target, Access access)
		{
			var invoker = CreateInvoker(baseName);
			AddSignature(target, access, invoker);

			int index = 0;
			foreach (var step in target.Steps)
			{
				index++;
				String stepBaseName = $"{baseName}_C{index}";
				IParseFunction func = step.Function;

				var visitor = new GenerateSignaturesVisitor(parsnipCode, stepBaseName, isMemoized: false, mustAddSignature: false);
				func.ApplyVisitor(visitor, Access.Private);
			}
		}

		public void Visit(Sequence target, Access access)
		{
			var invoker = CreateInvoker(baseName);
			AddSignature(target, access, invoker);

			int index = 0;
			foreach (var step in target.Steps)
			{
				index++;
				String stepBaseName = $"{baseName}_S{index}";
				IParseFunction func = step.Function;

				var visitor = new GenerateSignaturesVisitor(parsnipCode, stepBaseName, isMemoized: false, mustAddSignature: false);
				func.ApplyVisitor(visitor, Access.Private);
			}
		}

		public void Visit(Intrinsic target, Access access)
		{
			var methodName = parsnipCode.Intrinsics[target.Type];
			Invoker invoker = (s, f) => $"{methodName}({s}, {f})";
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
			Invoker invoker = (s, f) => $"ParseLexeme({s}, \"{expanded}\")";
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
			Invoker invoker = (s, f) => $"{methodName}({s}, {f})";
			if (this.mustAddSignature)
			{
				AddSignature(target, access, invoker);
			}
			else
			{
				AddInvoker(target, invoker);
			}
		}

		public void Visit(CardinalityFunction target, Access access)
		{
			var invoker = CreateInvoker(baseName);
			AddSignature(target, access, invoker);

			var inner = target.InnerParseFunction;
			var visitor = new GenerateSignaturesVisitor(parsnipCode, baseName + "_M", isMemoized: false, mustAddSignature: false);
			inner.ApplyVisitor(visitor, Access.Private);
			var innerInvocation = parsnipCode.Invokers[inner]("s", "f");
		}
	}
}
