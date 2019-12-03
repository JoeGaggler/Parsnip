using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	internal class RuleIdentifier
	{
		public RuleIdentifier(String text)
		{
			this.Text = text;
		}

		public String Text { get; }
	}
}
