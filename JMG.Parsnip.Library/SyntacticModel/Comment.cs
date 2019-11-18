using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.SyntacticModel
{
	public class Comment : IParsnipDefinitionItem
	{
		public void ApplyVisitor(IParsnipDefinitionItemVisitor visitor) => visitor.Visit(this);
		public TOutput ApplyVisitor<TOutput>(IParsnipDefinitionItemFuncVisitor<TOutput> visitor) => visitor.Visit(this);
	}
}
