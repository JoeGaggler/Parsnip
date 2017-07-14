using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class ParsnipDefinition
	{
		public ParsnipDefinition(IReadOnlyList<IParsnipDefinitionItem> items)
		{
			this.Items = items;
		}

		public IReadOnlyList<IParsnipDefinitionItem> Items { get; }
	}
}
