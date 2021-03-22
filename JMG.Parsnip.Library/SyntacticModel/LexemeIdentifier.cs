using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	internal class LexemeIdentifier : IParsnipDefinitionItem
	{
		public readonly String Id;

		public LexemeIdentifier(String id)
		{
			this.Id = id;
		}

		public void ApplyVisitor(IParsnipDefinitionItemVisitor visitor) => visitor.Visit(this);
		public TOutput ApplyVisitor<TOutput>(IParsnipDefinitionItemFuncVisitor<TOutput> visitor) => visitor.Visit(this);
	}
}
