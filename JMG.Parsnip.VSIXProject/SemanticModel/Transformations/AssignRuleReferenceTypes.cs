using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel.Transformations
{
	internal static class AssignRuleReferenceTypes
	{
		public static ParsnipModel Go(ParsnipModel model)
		{
			var vis = new ReplacingVisitor(model);
			vis.ReferencedRule = old => new ReferencedRule(old.Identifier, model.Rules.First(i => i.RuleIdentifier == old.Identifier).ReturnType);

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
