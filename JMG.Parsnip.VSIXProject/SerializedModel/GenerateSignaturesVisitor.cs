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

		public GenerateSignaturesVisitor(ParsnipCode parsnipCode, String baseName, Boolean isMemoized)
		{
			this.parsnipCode = parsnipCode;
			this.baseName = baseName;
			this.isMemoized = isMemoized;
		}

		public void Visit(Selection target, Access access)
		{
			Invoker invoker = (s, f) => $"{baseName}({s}, {f})";
			parsnipCode.MapFunctionInvocation(target, invoker);
			parsnipCode.AddSignature(new Signature(baseName, access, target, invoker, isMemoized));

			int index = 0;
			foreach (var step in target.Steps)
			{
				index++;
				String stepBaseName = $"{baseName}_C{index}";
				IParseFunction func = step.Function;

				var visitor = new GenerateSignaturesVisitor(parsnipCode, stepBaseName, isMemoized: false);
				func.ApplyVisitor(visitor, Access.Private);
			}
		}

		public void Visit(Sequence target, Access access)
		{
			Invoker invoker = (s, f) => $"{baseName}({s}, {f})";
			parsnipCode.MapFunctionInvocation(target, invoker);
			parsnipCode.AddSignature(new Signature(baseName, access, target, invoker, isMemoized));

			int index = 0;
			foreach (var step in target.Steps)
			{
				index++;
				String stepBaseName = $"{baseName}_S{index}";
				IParseFunction func = step.Function;

				var visitor = new GenerateSignaturesVisitor(parsnipCode, stepBaseName, isMemoized: false);
				func.ApplyVisitor(visitor, Access.Private);
			}
		}

		public void Visit(Intrinsic target, Access access)
		{
			// No need to generate method for Intrinsic
			var methodName = parsnipCode.Intrinsics[target.Type];
			parsnipCode.MapFunctionInvocation(target, (s, f) => $"{methodName}({s}, {f})");
		}

		public void Visit(LiteralString target, Access access)
		{
			var expanded = target.Text.Replace("\\", "\\\\");
			// No need to generate method for LiteralString
			parsnipCode.MapFunctionInvocation(target, (s, f) => $"ParseLexeme({s}, \"{expanded}\")");
		}

		public void Visit(ReferencedRule target, Access access)
		{
			// No need to generate method for ReferencedRule
			var methodName = parsnipCode.RuleMethodNames[target.Identifier];
			parsnipCode.MapFunctionInvocation(target, (s, f) => $"{methodName}({s}, {f})");
		}

		public void Visit(CardinalityFunction target, Access access)
		{
			Invoker invoker = (s, f) => $"{baseName}({s}, {f})";
			parsnipCode.MapFunctionInvocation(target, invoker);
			parsnipCode.AddSignature(new Signature(baseName, access, target, invoker, isMemoized));

			var inner = target.InnerParseFunction;
			var visitor = new GenerateSignaturesVisitor(parsnipCode, baseName + "_M", isMemoized: false);
			inner.ApplyVisitor(visitor, Access.Private);
			var innerInvocation = parsnipCode.Invokers[inner]("s", "f");
		}
	}
}
