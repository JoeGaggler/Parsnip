using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class RuleIdentifierToken : IToken
	{
		public RuleIdentifierToken(RuleIdentifier ruleIdentifier)
		{
			this.Identifier = ruleIdentifier;
		}

		public RuleIdentifier Identifier { get; }
	}
}
