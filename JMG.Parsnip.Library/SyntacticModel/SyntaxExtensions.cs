using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.Extensions;

namespace JMG.Parsnip.SyntacticModel
{
	internal static class SyntaxExtensions
	{
		public static ParsnipDefinition AddingItem(this ParsnipDefinition definition, IParsnipDefinitionItem item)
		{
			return new ParsnipDefinition(definition.Items.Appending(item));
		}
	}
}
