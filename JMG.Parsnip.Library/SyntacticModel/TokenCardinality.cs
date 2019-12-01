using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	internal class TokenCardinality
	{
		public TokenCardinality(IToken token, Cardinality cardinality)
		{
			this.Item = token;
			this.Cardinality = cardinality;
		}

		public IToken Item { get; }
		public Cardinality Cardinality { get; }
	}
}
