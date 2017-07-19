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

		public GenerateSignaturesVisitor(ParsnipCode parsnipCode, String baseName)
		{
			this.parsnipCode = parsnipCode;
			this.baseName = baseName;
		}

		public void Visit(Selection target, Access access)
		{
			parsnipCode.AddSignature(new Signature(baseName, access, target, (s, f) => $"{baseName}({s}, {f})"));

			int index = 0;
			foreach (var step in target.Steps)
			{
				index++;
				String stepBaseName = $"{baseName}_C{index}";
				IParseFunction func = step.Function;

				var visitor = new GenerateSignaturesVisitor(parsnipCode, stepBaseName);
				func.ApplyVisitor(visitor, Access.Private);
			}
		}

		public void Visit(Sequence target, Access access)
		{
			parsnipCode.AddSignature(new Signature(baseName, access, target, (s, f) => $"{baseName}({s}, {f})"));

			int index = 0;
			foreach (var step in target.Steps)
			{
				index++;
				String stepBaseName = $"{baseName}_S{index}";
				IParseFunction func = step.Function;

				var visitor = new GenerateSignaturesVisitor(parsnipCode, stepBaseName);
				func.ApplyVisitor(visitor, Access.Private);
			}
		}

		public void Visit(Intrinsic target, Access access)
		{
			// No need to generate method for Intrinsic
			var methodName = parsnipCode.Intrinsics[target.Type].Name;
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
	}
}
