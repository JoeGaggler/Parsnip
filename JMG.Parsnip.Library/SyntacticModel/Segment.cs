using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	public class Segment
	{
		public Segment(MatchAction action, IToken token, Cardinality cardinality)
		{
			this.Action = action;
			this.Item = token;
			this.Cardinality = cardinality;
		}

		public MatchAction Action { get; }
		public IToken Item { get; }
		public Cardinality Cardinality { get; }
	}
}
