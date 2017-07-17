using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public class Namespace : IFileScopeItem
	{
		public Namespace(String identifier, IReadOnlyList<INamespaceItem> items)
		{
			this.Identifier = identifier;
			this.Items = items;
		}

		public String Identifier { get; }
		public IReadOnlyList<INamespaceItem> Items { get; }

		public void ApplyVisitor(IFileScopeItemVisitor visitor) => visitor.Visit(this);
	}
}
