using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.Extensions;

namespace JMG.Parsnip.SemanticModel
{
	internal static class ParsnipModelExtensions
	{
		public static ParsnipModel AddingRule(this ParsnipModel model, Rule rule) => new ParsnipModel(model.Rules.Appending(rule), model.InterfaceMethods, model.LexemeIdentifiers);

		public static ParsnipModel AddingLexemeIdentifier(this ParsnipModel model, LexemeIdentifier id) => new ParsnipModel(model.Rules, model.InterfaceMethods, model.LexemeIdentifiers.Appending(id));

		public static ParsnipModel ReplacingRule(this ParsnipModel model, Rule oldRule, Rule newRule) => new ParsnipModel(model.Rules.Replacing(oldRule, newRule), model.InterfaceMethods, model.LexemeIdentifiers);

		public static ParsnipModel AddingInterfaceMethod(this ParsnipModel model, InterfaceMethod method) => new ParsnipModel(model.Rules, model.InterfaceMethods.Appending(method), model.LexemeIdentifiers);

		public static Rule WithParseFunction(this Rule rule, IParseFunction func) => new Rule(rule.RuleIdentifier, rule.ReturnType, func);

		public static Sequence AddingStep(this Sequence sequence, SequenceStep step) => new Sequence(sequence.Steps.Appending(step), sequence.InterfaceMethod);

		public static Selection AddingStep(this Selection selection, SelectionStep step) => new Selection(selection.Steps.Appending(step));

		public static Selection ReplacingSteps(this Selection target, IReadOnlyList<SelectionStep> steps) => new Selection(steps);

		public static Sequence ReplacingSteps(this Sequence target, IReadOnlyList<SequenceStep> steps) => new Sequence(steps, target.InterfaceMethod);
	}
}
