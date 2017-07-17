using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public class Class : INamespaceItem
	{
		public Class(AccessModifier access, String name, IReadOnlyList<IClassScopeItem> items)
		{
			this.Access = access;
			this.Name = name;
			this.Items = items;
		}

		public AccessModifier Access { get; }
		public String Name { get; }
		public IReadOnlyList<IClassScopeItem> Items { get; }

		public void ApplyVisitor(INamespaceItemVisitor visitor) => visitor.Visit(this);
	}
}
