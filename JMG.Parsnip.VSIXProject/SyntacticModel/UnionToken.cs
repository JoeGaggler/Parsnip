using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class UnionToken : IToken
	{
		public UnionToken(Union union)
		{
			this.Union = union;
		}

		public Union Union { get; }
	}
}
