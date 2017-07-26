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
		public Choice Choice1(Union t0) => new Choice(t0);

		public ClassIdentifier CID1(IReadOnlyList<String> t0) => new ClassIdentifier(String.Join("", t0));

		public String Comment1(IReadOnlyList<String> t0) => String.Join("", t0);

		public ParsnipDefinition Definition1(IReadOnlyList<IParsnipDefinitionItem> t0) => new ParsnipDefinition(t0.Where(i => i != null).ToList());

		public IParsnipDefinitionItem DefinitionItem1(Rule t0) => t0;

		public IParsnipDefinitionItem DefinitionItem2(String t0) => null;

		public IParsnipDefinitionItem DefinitionItem3() => null;

		public String IID1(IReadOnlyList<String> t0) => String.Join("", t0);

		public RuleIdentifier RID1(String t0, IReadOnlyList<String> t1) => new RuleIdentifier(t0 + String.Join("", t1));

		public Rule Rule1(RuleHead t0, RuleBody t1) => new Rule(t0, t1);

		public RuleBody RuleBody1(IReadOnlyList<Choice> t0) => new RuleBody(t0);

		public RuleHead RuleHead1((RuleHeadPrefix, ClassIdentifier) t0) => new RuleHead(t0.Item1.Id, t0.Item2);

		public RuleHead RuleHead2(RuleHeadPrefix t0) => new RuleHead(t0.Id, null);

		public RuleHeadPrefix RuleHeadPrefix1(RuleIdentifier t0) => new RuleHeadPrefix(t0);

		public Segment Segment1(TokenCardinality t0) => new Segment(MatchAction.Ignore, t0.Item, t0.Cardinality);

		public Segment Segment2(TokenCardinality t0) => new Segment(MatchAction.Fail, t0.Item, t0.Cardinality);

		public Segment Segment3(TokenCardinality t0) => new Segment(MatchAction.Rewind, t0.Item, t0.Cardinality);

		public Segment Segment4(TokenCardinality t0) => new Segment(MatchAction.Consume, t0.Item, t0.Cardinality);

		public TokenCardinality Cardinality1(IToken t0) => new TokenCardinality(t0, Cardinality.Plus);

		public TokenCardinality Cardinality2(IToken t0) => new TokenCardinality(t0, Cardinality.Maybe);

		public TokenCardinality Cardinality3(IToken t0) => new TokenCardinality(t0, Cardinality.Star);

		public TokenCardinality Cardinality4(IToken t0) => new TokenCardinality(t0, Cardinality.One);

		public Sequence Sequence1((Segment, Sequence) t0) => new Sequence(t0.Item2.Segments.Prepending(t0.Item1));

		public Sequence Sequence2(Segment t0) => new Sequence(new[] { t0 });

		public IToken Token1() => new IntrinsicToken(".");

		public IToken Token2(String t0) => new LiteralStringToken(t0);

		public IToken Token3(RuleIdentifier t0) => new RuleIdentifierToken(t0);

		public IToken Token4(String t0) => new IntrinsicToken(t0);

		public IToken Token5(Union t0) => new UnionToken(t0);

		public Union Union1((Sequence, Union) t0) => new Union(t0.Item2.Sequences.Prepending(t0.Item1));

		public Union Union2(Sequence t0) => new Union(new[] { t0 });
	}
}
