using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class RuleHeadPrefix
	{
		public RuleHeadPrefix(RuleIdentifier id)
		{
			this.Id = id;
		}

		public RuleIdentifier Id { get; }
	}
}
