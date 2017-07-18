using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class ReferencedRule : IParseFunction
	{
		public ReferencedRule(String identifier)
		{
			this.Identifier = identifier;
		}

		public String Identifier { get; }

		public Boolean IsMemoized => false;

		public INodeType ReturnType => throw new NotImplementedException();

		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);
	}
}
