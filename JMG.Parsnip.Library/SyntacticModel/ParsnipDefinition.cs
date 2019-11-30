﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	public class ParsnipDefinition
	{
		public ParsnipDefinition(IReadOnlyList<IParsnipDefinitionItem> items)
		{
			this.Items = items;
		}

		public IReadOnlyList<IParsnipDefinitionItem> Items { get; }
	}
}