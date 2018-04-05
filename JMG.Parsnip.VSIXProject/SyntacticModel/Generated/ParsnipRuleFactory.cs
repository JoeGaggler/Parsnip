using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Extensions;

namespace JMG.Parsnip.VSIXProject.SyntacticModel.Generated
{
	internal class ParsnipRuleFactory : IParsnipRuleFactory
	{
		Choice IParsnipRuleFactory.Choice1(Union t0) => new Choice(t0);

		ClassIdentifier IParsnipRuleFactory.ClassIdentifier1(IReadOnlyList<String> t0) => new ClassIdentifier(String.Join("", t0));

		String IParsnipRuleFactory.Comment1(IReadOnlyList<String> t0) => String.Join("", t0);

		String IParsnipRuleFactory.CsharpIdentifier1(String t0, IReadOnlyList<String> t1) => t0 + String.Join("", t1);

		ParsnipDefinition IParsnipRuleFactory.Definition1(IReadOnlyList<IParsnipDefinitionItem> t0) => new ParsnipDefinition(t0.Where(i => i != null).ToList());

		IParsnipDefinitionItem IParsnipRuleFactory.DefinitionItem1(Rule t0) => t0;

		IParsnipDefinitionItem IParsnipRuleFactory.DefinitionItem2(String t0) => null;

		IParsnipDefinitionItem IParsnipRuleFactory.DefinitionItem3() => null;

		String IParsnipRuleFactory.IntrinsicIdentifier1(IReadOnlyList<String> t0) => String.Join("", t0);

		String IParsnipRuleFactory.IntrinsicIdentifier2(String t0) => t0;

		Rule IParsnipRuleFactory.Rule1(RuleHead t0, RuleBody t1) => new Rule(t0, t1);

		RuleBody IParsnipRuleFactory.RuleBody1(IReadOnlyList<Choice> t0) => new RuleBody(t0);

		RuleHead IParsnipRuleFactory.RuleHead1(RuleHeadPrefix t0, ClassIdentifier t1) => new RuleHead(t0.Id, t1);

		RuleHead IParsnipRuleFactory.RuleHead2(RuleHeadPrefix t0) => new RuleHead(t0.Id, null);

		RuleHeadPrefix IParsnipRuleFactory.RuleHeadPrefix1(RuleIdentifier t0) => new RuleHeadPrefix(t0);

		RuleIdentifier IParsnipRuleFactory.RuleIdentifier1(String t0, IReadOnlyList<String> t1) => new RuleIdentifier(t0 + String.Join("", t1));

		Segment IParsnipRuleFactory.Segment1(TokenCardinality t0) => new Segment(MatchAction.Ignore, t0.Item, t0.Cardinality);

		Segment IParsnipRuleFactory.Segment2(TokenCardinality t0) => new Segment(MatchAction.Fail, t0.Item, t0.Cardinality);

		Segment IParsnipRuleFactory.Segment3(TokenCardinality t0) => new Segment(MatchAction.Rewind, t0.Item, t0.Cardinality);

		Segment IParsnipRuleFactory.Segment4(TokenCardinality t0) => new Segment(MatchAction.Consume, t0.Item, t0.Cardinality);

		TokenCardinality IParsnipRuleFactory.Cardinality1(IToken t0) => new TokenCardinality(t0, Cardinality.Plus);

		TokenCardinality IParsnipRuleFactory.Cardinality2(IToken t0) => new TokenCardinality(t0, Cardinality.Maybe);

		TokenCardinality IParsnipRuleFactory.Cardinality3(IToken t0) => new TokenCardinality(t0, Cardinality.Star);

		TokenCardinality IParsnipRuleFactory.Cardinality4(IToken t0) => new TokenCardinality(t0, Cardinality.One);

		Sequence IParsnipRuleFactory.Sequence1(IReadOnlyList<Segment> t0) => new Sequence(t0);

		Segment IParsnipRuleFactory.Special1(IToken t0, IToken t1) => new Segment(MatchAction.Consume, new SeriesToken(t0, t1), Cardinality.One);

		Segment IParsnipRuleFactory.Special2(Segment t0) => t0;

		IToken IParsnipRuleFactory.Token1() => new IntrinsicToken(".");

		IToken IParsnipRuleFactory.Token2(String t0) => new LiteralStringToken(t0);

		IToken IParsnipRuleFactory.Token3(RuleIdentifier t0) => new RuleIdentifierToken(t0);

		IToken IParsnipRuleFactory.Token4(String t0) => new IntrinsicToken(t0);

		IToken IParsnipRuleFactory.Token5(Union t0) => new UnionToken(t0);

		Union IParsnipRuleFactory.Union1(IReadOnlyList<Sequence> t0) => new Union(t0);
	}
}
