using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class LiteralStringToken : IToken
	{
		public LiteralStringToken(String text)
		{
			this.Text = text;
		}

		public String Text { get; }
	}
}
