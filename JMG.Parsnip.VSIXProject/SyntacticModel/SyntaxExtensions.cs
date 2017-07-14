using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Extensions;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal static class SyntaxExtensions
	{
		public static ParsnipDefinition AddingItem(this ParsnipDefinition definition, IParsnipDefinitionItem item)
		{
			return new ParsnipDefinition(definition.Items.Appending(item));
		}
	}
}
