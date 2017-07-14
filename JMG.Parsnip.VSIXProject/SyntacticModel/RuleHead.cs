using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class RuleHead
	{
		public RuleHead(RuleIdentifier ruleIdentifier, ClassIdentifier classIdentifier)
		{
			this.RuleIdentifier = ruleIdentifier;
			this.ClassIdentifier = classIdentifier;
		}

		public RuleIdentifier RuleIdentifier { get; }
		public ClassIdentifier ClassIdentifier { get; }
	}
}
