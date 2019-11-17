using System;
using System.Collections.Generic;
using System.Linq;
using ParsnipCLI.Syntax;

namespace ParsnipCLI
{
    public class MyFactory : IMyParserFactory
    {
        TokenCardinality IMyParserFactory.Cardinality1(IToken t0, String t1)
        {
            Cardinality cardinality;
            if (t1 == null) { cardinality = Cardinality.One; }
            else if (t1 == "+") { cardinality = Cardinality.Plus; }
            else if (t1 == "?") { cardinality = Cardinality.Maybe; }
            else if (t1 == "*") { cardinality = Cardinality.Star; }
            else { throw new InvalidOperationException($"Unexpected cardinality: {t1}"); }

            return new TokenCardinality(token: t0, cardinality);
        }

        ParsnipChoice IMyParserFactory.Choice1(ParsnipUnion t0) => new ParsnipChoice(union: t0);

        ParsnipClassIdentifier IMyParserFactory.ClassIdentifier1(List<String> t0) => new ParsnipClassIdentifier(id: t0.Concat());

        String IMyParserFactory.Comment1(IReadOnlyList<String> t0) => t0.Concat();

        String IMyParserFactory.CsharpIdentifier1(String t0, IReadOnlyList<String> t1) => t0 + t1.Concat();

        ParsnipDefinition IMyParserFactory.Definition1(List<ParsnipDefinitionItem> t0) => new ParsnipDefinition(items: t0.Where(i => i != null).ToList());

        ParsnipDefinitionItem IMyParserFactory.DefinitionItem1(ParsnipRule t0) => t0;

        ParsnipDefinitionItem IMyParserFactory.DefinitionItem2(String node) => null; // comment

        ParsnipDefinitionItem IMyParserFactory.DefinitionItem3() => null; // eol

        String IMyParserFactory.IntrinsicIdentifier1(IReadOnlyList<String> t0) => t0.Concat();

        String IMyParserFactory.IntrinsicIdentifier2(String node) => node;

        ParsnipRule IMyParserFactory.Rule1(ParsnipRuleHead t0, ParsnipRuleBody t1) => new ParsnipRule(head: t0, body: t1);

        ParsnipRuleBody IMyParserFactory.RuleBody1(List<ParsnipChoice> t0) => new ParsnipRuleBody(choices: t0);

        ParsnipRuleHead IMyParserFactory.RuleHead1((ParsnipRuleHeadPrefix, ParsnipClassIdentifier) t0) => new ParsnipRuleHead(prefix: t0.Item1, classId: t0.Item2);

        ParsnipRuleHead IMyParserFactory.RuleHead2(ParsnipRuleHeadPrefix node) => new ParsnipRuleHead(prefix: node, classId: null);

        ParsnipRuleHeadPrefix IMyParserFactory.RuleHeadPrefix(ParsnipRuleIdentifier node) => new ParsnipRuleHeadPrefix(id: node.Id);

        ParsnipRuleIdentifier IMyParserFactory.RuleIdentifier1(String t0, IReadOnlyList<String> t1) => new ParsnipRuleIdentifier(id: t0 + t1.Concat());

        ParsnipSegment IMyParserFactory.Segment1(String t0, TokenCardinality t1)
        {
            MatchAction matchAction;
            if (t0 == null) { matchAction = MatchAction.Consume; }
            else if (t0 == "`") { matchAction = MatchAction.Ignore; }
            else if (t0 == "~") { matchAction = MatchAction.Fail; }
            else if (t0 == "&") { matchAction = MatchAction.Rewind; }
            else { throw new InvalidOperationException($"Unexpected match action: {t0}"); }

            return new ParsnipSegment(action: matchAction, token: t1.Item, cardinality: t1.Cardinality);
        }

        ParsnipSequence IMyParserFactory.Sequence1(List<ParsnipSegment> node) => new ParsnipSequence(segments: node);

        ParsnipSegment IMyParserFactory.Special1((IToken, IToken) t0) => new ParsnipSegment(MatchAction.Consume, new SeriesToken(t0, t1), Cardinality.One);

        ParsnipSegment IMyParserFactory.Special2(ParsnipSegment node)
        {
            throw new NotImplementedException();
        }

        IToken IMyParserFactory.Token1()
        {
            throw new NotImplementedException();
        }

        IToken IMyParserFactory.Token2(String node)
        {
            throw new NotImplementedException();
        }

        IToken IMyParserFactory.Token3(ParsnipRuleIdentifier node)
        {
            throw new NotImplementedException();
        }

        IToken IMyParserFactory.Token4(String node)
        {
            throw new NotImplementedException();
        }

        IToken IMyParserFactory.Token5(ParsnipUnion node)
        {
            throw new NotImplementedException();
        }

        ParsnipUnion IMyParserFactory.Union1(List<ParsnipSequence> node)
        {
            throw new NotImplementedException();
        }
    }
}
