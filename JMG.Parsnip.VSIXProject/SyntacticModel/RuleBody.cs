using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class RuleBody
	{
		public RuleBody(IReadOnlyList<Union> unions)
		{
			this.Unions = unions;
		}

		public IReadOnlyList<Union> Unions { get; }
	}
}
