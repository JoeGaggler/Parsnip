﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	internal class RuleBody
	{
		public RuleBody(IReadOnlyList<Choice> choices)
		{
			this.Choices = choices;
		}

		public IReadOnlyList<Choice> Choices { get; }
	}
}
