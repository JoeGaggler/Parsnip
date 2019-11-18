using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	public enum MatchAction
	{
		Consume,
		Ignore,
		Rewind,
		Fail
	}
}
