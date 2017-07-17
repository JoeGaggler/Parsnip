using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public class UsingNamespace : IFileScopeItem
	{
		public UsingNamespace(String identifier)
		{
			this.Identifier = identifier;
		}

		public String Identifier { get; }

		public void ApplyVisitor(IFileScopeItemVisitor visitor) => visitor.Visit(this);
	}
}
