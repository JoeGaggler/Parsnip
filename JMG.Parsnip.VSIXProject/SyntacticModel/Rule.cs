using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class Rule : IParsnipDefinitionItem
	{
		public Rule(RuleHead head, RuleBody body)
		{
			this.Head = head;
			this.Body = body;
		}

		public RuleHead Head { get; }
		public RuleBody Body { get; }
	}
}
