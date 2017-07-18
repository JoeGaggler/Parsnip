using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Extensions;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal static class AppendingExtensions
	{
		public static ParsnipModel AddingRule(this ParsnipModel model, Rule rule) => new ParsnipModel(model.Rules.Appending(rule));

		public static ParsnipModel ReplacingRule(this ParsnipModel model, Rule oldRule, Rule newRule) => new ParsnipModel(model.Rules.Replacing(oldRule, newRule));

		public static Rule WithParseFunction(this Rule rule, IParseFunction func) => new Rule(rule.RuleIdentifier, rule.ReturnType, func);

		public static Sequence AddingStep(this Sequence sequence, SequenceStep step) => new Sequence(sequence.IsMemoized, sequence.Steps.Appending(step));

		public static Selection AddingStep(this Selection selection, SelectionStep step) => new Selection(selection.IsMemoized, selection.Steps.Appending(step));


	}
}
