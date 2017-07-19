using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class ReferencedRule : IParseFunction
	{
		public ReferencedRule(String identifier, INodeType ruleNodeType)
		{
			this.Identifier = identifier;
			
			this.ReturnType = ruleNodeType ?? new SingleNodeType("UNRESOLVED_RULE");
		}

		public String Identifier { get; }

		public Boolean IsMemoized => false;

		public INodeType ReturnType { get; }

		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);

		public void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input) => visitor.Visit(this, input);

		public TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input) => visitor.Visit(this, input);
	}
}
