using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	public enum MatchAction
	{
		Consume,
		Ignore,
		Rewind,
		Fail
	}
}
