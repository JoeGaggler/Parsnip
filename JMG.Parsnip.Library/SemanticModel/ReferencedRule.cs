using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class ReferencedRule : IParseFunction
	{
		public ReferencedRule(String identifier, INodeType ruleNodeType, InterfaceMethod interfaceMethod)
		{
			this.Identifier = identifier;			
			this.RuleNodeType = ruleNodeType ?? new SingleNodeType("UNRESOLVED_RULE");
			this.InterfaceMethod = interfaceMethod;
		}

		public INodeType RuleNodeType { get; }

		public String Identifier { get; }

		public INodeType ReturnType
		{
			get
			{
				if (InterfaceMethod != null) return InterfaceMethod.ReturnType;

				return RuleNodeType;
			}
		}

		public InterfaceMethod InterfaceMethod { get; }

		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);

		public void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input) => visitor.Visit(this, input);

		public void ApplyVisitor<TInput1, TInput2>(IParseFunctionActionVisitor<TInput1, TInput2> visitor, TInput1 input1, TInput2 input2) => visitor.Visit(this, input1, input2);

		public TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input) => visitor.Visit(this, input);
	}
}
