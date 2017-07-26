using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	public class Rule : IParsnipDefinitionItem
	{
		public Rule(RuleHead head, RuleBody body)
		{
			this.Head = head;
			this.Body = body;
		}

		public RuleHead Head { get; }
		public RuleBody Body { get; }

		public void ApplyVisitor(IParsnipDefinitionItemVisitor visitor) => visitor.Visit(this);
		public TOutput ApplyVisitor<TOutput>(IParsnipDefinitionItemFuncVisitor<TOutput> visitor) => visitor.Visit(this);
	}
}
