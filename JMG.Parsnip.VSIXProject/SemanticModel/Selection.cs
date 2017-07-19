using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class Selection : IParseFunction
	{
		public Selection(Boolean isMemoized, IReadOnlyList<SelectionStep> steps)
		{
			this.IsMemoized = isMemoized;
			this.Steps = steps;
		}

		public IReadOnlyList<SelectionStep> Steps { get; }

		public Boolean IsMemoized { get; }

		public INodeType ReturnType => new SingleNodeType("IDontKnowYet");

		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);

		public void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input) => visitor.Visit(this, input);

		public TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor) => visitor.Visit(this);
	}

	internal class SelectionStep
	{
		public SelectionStep(IParseFunction parseFunction)
		{
			this.Function = parseFunction;
		}

		public IParseFunction Function { get; }
	}
}
