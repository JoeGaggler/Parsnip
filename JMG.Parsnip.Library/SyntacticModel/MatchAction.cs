using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	internal enum MatchAction
	{
		Consume,
		Ignore,
		Rewind,
		Fail
	}
}
