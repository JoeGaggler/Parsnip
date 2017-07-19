using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel.Transformations
{
	internal static class AssignRuleFactoryMethods
	{
		public static ParsnipModel Go(ParsnipModel model)
		{
			var vis = new Visitor();

			var oldRules = model.Rules;
			foreach (var oldRule in oldRules)
			{
				var newFunc = oldRule.ParseFunction.ApplyVisitor(vis, oldRule.ReturnType);
				var newRule = oldRule.WithParseFunction(newFunc);
				model = model.ReplacingRule(oldRule, newRule);
			}
			return model;
		}

		private class Visitor : IParseFunctionFuncVisitor<INodeType, IParseFunction>
		{
			public IParseFunction Visit(Selection target, INodeType input)
			{
				return target.WithFactoryReturnType(input);
			}

			public IParseFunction Visit(Sequence target, INodeType input)
			{
				return target.WithFactoryReturnType(input);
			}

			public IParseFunction Visit(Intrinsic target, INodeType input)
			{
				throw new NotImplementedException();
			}

			public IParseFunction Visit(LiteralString target, INodeType input)
			{
				throw new NotImplementedException();
			}

			public IParseFunction Visit(ReferencedRule target, INodeType input)
			{
				throw new NotImplementedException();
			}
		}
	}
}
