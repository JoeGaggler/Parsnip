using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.Extensions;

namespace JMG.Parsnip.SyntacticModel.Generated
{
	internal class ParsnipRuleFactory : IParsnipRuleFactory
	{
		Choice IParsnipRuleFactory.Choice1(Union t0) => new Choice(t0);

		ClassIdentifier IParsnipRuleFactory.ClassIdentifier1(IReadOnlyList<String> t0) => new ClassIdentifier(t0.Concat());

		String IParsnipRuleFactory.Comment1(IReadOnlyList<String> t0) => t0.Concat();

		String IParsnipRuleFactory.CsharpIdentifier1(String t0, IReadOnlyList<String> t1) => t0 + t1.Concat();

		ParsnipDefinition IParsnipRuleFactory.Definition1(IReadOnlyList<IParsnipDefinitionItem> t0) => new ParsnipDefinition(t0.Where(i => i != null).ToList());

		IParsnipDefinitionItem IParsnipRuleFactory.DefinitionItem1(Rule t0) => t0;

		IParsnipDefinitionItem IParsnipRuleFactory.DefinitionItem2(String t0) => null;

		IParsnipDefinitionItem IParsnipRuleFactory.DefinitionItem3(String t0) => new LexemeIdentifier(t0);

		IParsnipDefinitionItem IParsnipRuleFactory.DefinitionItem4() => null;

		String IParsnipRuleFactory.IntrinsicIdentifier1(IReadOnlyList<String> t0) => t0.Concat();

		String IParsnipRuleFactory.Lexeme1(String t0) => t0;

		Rule IParsnipRuleFactory.Rule1(RuleHead t0, RuleBody t1) => new Rule(t0, t1);

		RuleBody IParsnipRuleFactory.RuleBody1(IReadOnlyList<Choice> t0) => new RuleBody(t0);

		RuleHead IParsnipRuleFactory.RuleHead1(RuleHeadPrefix t0, ClassIdentifier t1) => new RuleHead(t0.Id, t1);

		RuleHead IParsnipRuleFactory.RuleHead2(RuleHeadPrefix t0) => new RuleHead(t0.Id, null);

		RuleHeadPrefix IParsnipRuleFactory.RuleHeadPrefix1(RuleIdentifier t0) => new RuleHeadPrefix(t0);

		RuleIdentifier IParsnipRuleFactory.RuleIdentifier1(String t0, IReadOnlyList<String> t1) => new RuleIdentifier(t0 + t1.Concat());

		Segment IParsnipRuleFactory.Segment1(String t0, TokenCardinality t1)
		{
			MatchAction matchAction;
			if (t0 == null) { matchAction = MatchAction.Consume; }
			else if (t0 == "`") { matchAction = MatchAction.Ignore; }
			else if (t0 == "~") { matchAction = MatchAction.Fail; }
			else if (t0 == "&") { matchAction = MatchAction.Rewind; }
			else { throw new InvalidOperationException($"Unexpected match action: {t0}"); }

			return new Segment(matchAction, t1.Item, t1.Cardinality);
		}

		TokenCardinality IParsnipRuleFactory.Cardinality1(IToken t0, String t1)
		{
			Cardinality cardinality;
			if (t1 == null) { cardinality = Cardinality.One; }
			else if (t1 == "+") { cardinality = Cardinality.Plus; }
			else if (t1 == "?") { cardinality = Cardinality.Maybe; }
			else if (t1 == "*") { cardinality = Cardinality.Star; }
			else { throw new InvalidOperationException($"Unexpected cardinality: {t1}"); }

			return new TokenCardinality(t0, cardinality);
		}

		Sequence IParsnipRuleFactory.Sequence1(IReadOnlyList<Segment> t0) => new Sequence(t0);

		Segment IParsnipRuleFactory.Special1(IToken t0, IToken t1) => new Segment(MatchAction.Consume, new SeriesToken(t0, t1), Cardinality.One);

		Segment IParsnipRuleFactory.Special2(Segment t0) => t0;

		IToken IParsnipRuleFactory.Token1() => new IntrinsicToken(".");

		IToken IParsnipRuleFactory.Token2(String t0, String t1) => new LiteralStringToken(t0, t1);

		IToken IParsnipRuleFactory.Token3(RuleIdentifier t0) => new RuleIdentifierToken(t0);

		IToken IParsnipRuleFactory.Token4(String t0) => new IntrinsicToken(t0);

		IToken IParsnipRuleFactory.Token5(Union t0) => new UnionToken(t0);

		Union IParsnipRuleFactory.Union1(IReadOnlyList<Sequence> t0) => new Union(t0);
	}
}
