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

		public static Sequence AddingStep(this Sequence sequence, SequenceStep step) => new Sequence(sequence.IsMemoized, sequence.Steps.Appending(step), sequence.FactoryReturnType);

		public static Selection AddingStep(this Selection selection, SelectionStep step) => new Selection(selection.IsMemoized, selection.Steps.Appending(step), selection.FactoryReturnType);

		public static Selection WithFactoryReturnType(this Selection target, INodeType nodeType) => new Selection(target.IsMemoized, target.Steps, nodeType);

		public static Sequence WithFactoryReturnType(this Sequence target, INodeType nodeType) => new Sequence(target.IsMemoized, target.Steps, nodeType);

		public static Selection ReplacingSteps(this Selection target, IReadOnlyList<SelectionStep> steps) => new Selection(target.IsMemoized, steps, target.FactoryReturnType);

		public static Sequence ReplacingSteps(this Sequence target, IReadOnlyList<SequenceStep> steps) => new Sequence(target.IsMemoized, steps, target.FactoryReturnType);
	}
}
