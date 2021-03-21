using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SemanticModel.Transformations
{
	/// <summary>
	/// Simplifies the grammar to collapse redundant steps
	/// </summary>
	internal static class Collapsing
	{
		public static ParsnipModel Go(ParsnipModel model)
		{
			var vis = new Visitor();

			var oldRules = model.Rules;
			foreach (var oldRule in oldRules)
			{
				var newFunc = oldRule.ParseFunction.ApplyVisitor(vis);
				var newRule = oldRule.WithParseFunction(newFunc);
				model = model.ReplacingRule(oldRule, newRule);
			}
			return model;
		}

		private class Visitor : IParseFunctionFuncVisitor<IParseFunction>
		{
			public IParseFunction Visit(Selection target)
			{
				var steps = target.Steps;
				var firstStep = steps[0];

				// Cannot collapse multiselection functions, only children
				// Unable to propagate interface methods. Keep this transformation ahead of <AssignRuleReferenceTypes>
				if (steps.Count > 1 || firstStep.InterfaceMethod != null)
				{
					var newSteps = steps.Select(i => new SelectionStep(i.Function.ApplyVisitor(this), interfaceMethod: null)).ToList();
					return new Selection(newSteps);
				}

				return firstStep.Function.ApplyVisitor(this);
			}

			public IParseFunction Visit(Sequence target)
			{
				var steps = target.Steps;
				var firstStep = steps[0];

				// Cannot collapse multiselection functions, only children
				// Unable to propagate unconsumed segments. Keep this transformation ahead of <AssignRuleReferenceTypes>
				if (steps.Count > 1 || firstStep.MatchAction != MatchAction.Consume)
				{
					var newSteps = steps.Select(i => new SequenceStep(i.Function.ApplyVisitor(this), i.MatchAction)).ToList();
					return new Sequence(newSteps, target.InterfaceMethod);
				}

				return firstStep.Function.ApplyVisitor(this);
			}

			public IParseFunction Visit(Intrinsic target) => target;

			public IParseFunction Visit(LiteralString target) => target;

			public IParseFunction Visit(LexemeIdentifier target) => target;

			public IParseFunction Visit(ReferencedRule target) => target;

			public IParseFunction Visit(Repetition target)
			{
				return new Repetition(target.InnerParseFunction.ApplyVisitor(this), target.Cardinality, target.InterfaceMethod);
			}

			public IParseFunction Visit(Series target)
			{
				return new Series(target.RepeatedToken.ApplyVisitor(this), target.DelimiterToken.ApplyVisitor(this), target.InterfaceMethod);
			}
		}
	}
}
