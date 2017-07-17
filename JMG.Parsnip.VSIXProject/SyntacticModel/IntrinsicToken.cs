using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class IntrinsicToken : IToken
	{
		public IntrinsicToken(String identifier)
		{
			this.Identifier = identifier;
		}

		public String Identifier { get; }
	}
}
