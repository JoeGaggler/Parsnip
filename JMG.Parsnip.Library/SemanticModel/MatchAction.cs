﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SemanticModel
{
	internal enum MatchAction
	{
		Consume,
		Ignore,
		Rewind,
		Fail
	}
}
