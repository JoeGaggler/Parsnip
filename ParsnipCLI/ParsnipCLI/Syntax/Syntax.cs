using System;
using System.Collections.Generic;
using System.Text;

namespace ParsnipCLI.Syntax
{
    public class ParsnipDefinition
    {
        private List<ParsnipDefinitionItem> items;

        public ParsnipDefinition(List<ParsnipDefinitionItem> items)
        {
            this.items = items;
        }
    }

    public abstract class ParsnipDefinitionItem
    {

    }

    public class ParsnipRule : ParsnipDefinitionItem
    {
        private ParsnipRuleHead head;
        private ParsnipRuleBody body;

        public ParsnipRule(ParsnipRuleHead head, ParsnipRuleBody body)
        {
            this.head = head;
            this.body = body;
        }
    }

    public class ParsnipRuleHead
    {
        private ParsnipRuleHeadPrefix prefix;
        private ParsnipClassIdentifier? classId;

        public ParsnipRuleHead(ParsnipRuleHeadPrefix prefix, ParsnipClassIdentifier? classId)
        {
            this.prefix = prefix;
            this.classId = classId;
        }
    }

    public class ParsnipRuleHeadPrefix
    {
        private String id;

        public ParsnipRuleHeadPrefix(String id)
        {
            this.id = id;
        }
    }

    public class ParsnipRuleBody
    {
        private List<ParsnipChoice> choices;

        public ParsnipRuleBody(List<ParsnipChoice> choices)
        {
            this.choices = choices;
        }
    }

    public class ParsnipChoice
    {
        private ParsnipUnion union;

        public ParsnipChoice(ParsnipUnion union)
        {
            this.union = union;
        }
    }

    public class ParsnipUnion
    {

    }

    public class ParsnipSequence
    {
        private List<ParsnipSegment> segments;

        public ParsnipSequence(List<ParsnipSegment> segments)
        {
            this.segments = segments;
        }
    }

    public class ParsnipSegment
    {

    }

    public class ParsnipClassIdentifier
    {
        private String id;

        public ParsnipClassIdentifier(String id)
        {
            this.id = id;
        }
    }

    public class ParsnipRuleIdentifier
    {
        public readonly String Id;

        public ParsnipRuleIdentifier(Object id)
        {
            this.Id = id;
        }
    }

    public interface IToken
    {

    }

    public class TokenCardinality
    {
        private IToken token;
        private Cardinality cardinality;

        public TokenCardinality(IToken token, Cardinality cardinality)
        {
            this.token = token;
            this.cardinality = cardinality;
        }
    }

    public enum Cardinality
    {
        One,
        Plus,
        Maybe,
        Star
    }

    public enum MatchAction
    {
        Consume,
        Ignore,
        Fail,
        Rewind
    }
}
