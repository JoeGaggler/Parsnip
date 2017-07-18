using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.CodeWriting;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject.SerializedModel
{
	internal class GenerateSignaturesVisitor : IParseFunctionActionVisitor
	{
		private readonly ParsnipCode parsnipCode;
		private readonly String baseName;

		public GenerateSignaturesVisitor(ParsnipCode parsnipCode, String baseName)
		{
			this.parsnipCode = parsnipCode;
			this.baseName = baseName;
		}

		public void Visit(Selection target)
		{
			parsnipCode.AddSignature(new Signature(baseName, Access.Private, target, (s, f) => $"{baseName}({s}, {f})"));

			int index = 0;
			foreach (var step in target.Steps)
			{
				index++;
				String stepBaseName = $"{baseName}_C{index}";
				IParseFunction func = step.Function;

				var visitor = new GenerateSignaturesVisitor(parsnipCode, stepBaseName);
				func.ApplyVisitor(visitor);
			}
		}

		public void Visit(Sequence target)
		{
			parsnipCode.AddSignature(new Signature(baseName, Access.Private, target, (s, f) => $"{baseName}({s}, {f})"));

			int index = 0;
			foreach (var step in target.Steps)
			{
				index++;
				String stepBaseName = $"{baseName}_S{index}";
				IParseFunction func = step.Function;

				var visitor = new GenerateSignaturesVisitor(parsnipCode, stepBaseName);
				func.ApplyVisitor(visitor);
			}
		}

		public void Visit(Intrinsic target)
		{
			// No need to generate method for Intrinsic
			var methodName = parsnipCode.Intrinsics[target.Type].Name;
			parsnipCode.MapFunctionInvocation(target, (s, f) => $"{methodName}({s}, {f})");
		}

		public void Visit(LiteralString target)
		{
			// No need to generate method for LiteralString
			parsnipCode.MapFunctionInvocation(target, (s, f) => $"TODO_LiteralString(s, f)");
		}

		public void Visit(ReferencedRule target)
		{
			// No need to generate method for ReferencedRule
			var methodName = parsnipCode.RuleMethodNames[target.Identifier];
			parsnipCode.MapFunctionInvocation(target, (s, f) => $"{methodName}({s}, {f})");
		}
	}
}
