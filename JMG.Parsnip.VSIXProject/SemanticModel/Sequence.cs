using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class Sequence : IParseFunction
	{
		public Sequence(Boolean isMemoized, IReadOnlyList<SequenceStep> steps)
		{
			this.IsMemoized = isMemoized;
			this.Steps = steps;
		}

		public IReadOnlyList<SequenceStep> Steps { get; }

		public Boolean IsMemoized { get; }

		public INodeType ReturnType => new SingleNodeType("IDontKnowYet");

		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);
	}

	internal class SequenceStep
	{
		public SequenceStep(IParseFunction parseFunction, MatchAction matchAction)
		{
			this.Function = parseFunction;
			this.MatchAction = matchAction;
		}

		public IParseFunction Function { get; }
		public MatchAction MatchAction { get; }
	}
}
