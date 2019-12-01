using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SemanticModel.Transformations
{
	internal static class AssignRuleReferenceTypes
	{
		public static ParsnipModel Go(ParsnipModel model)
		{
			var vis = new ReplacingVisitor(model);

			vis.ReferencedRule = old =>
			{
				var rule = model.Rules.FirstOrDefault(i => i.RuleIdentifier == old.Identifier);
				if (rule == null)
				{
					throw new InvalidOperationException($"No such rule: {old.Identifier}");
				}
				return new ReferencedRule(old.Identifier, rule.ReturnType, interfaceMethod: null);
			};

			var oldRules = model.Rules;
			foreach (var oldRule in oldRules)
			{
				var newFunc = oldRule.ParseFunction.ApplyVisitor(vis);
				var newRule = oldRule.WithParseFunction(newFunc);
				model = model.ReplacingRule(oldRule, newRule);
			}
			return model;
		}
	}
}
