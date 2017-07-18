using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class Rule : IParseFunction
	{
		public Rule(String ruleIdentifier, Boolean isMemoized, INodeType returnType)
		{
			this.RuleIdentifier = ruleIdentifier;
			this.IsMemoized = isMemoized;
			this.ReturnType = returnType;
		}

		public String RuleIdentifier { get; }

		public Boolean IsMemoized { get; }

		public INodeType ReturnType { get; }
	}
}
