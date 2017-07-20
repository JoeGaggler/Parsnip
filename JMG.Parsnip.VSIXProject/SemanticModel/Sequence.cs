using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class Sequence : IParseFunction, IParseFunctionWithFactoryType
	{
		public Sequence(Boolean isMemoized, IReadOnlyList<SequenceStep> steps, INodeType factoryReturnType)
		{
			this.IsMemoized = isMemoized;
			this.Steps = steps;
			this.FactoryReturnType = factoryReturnType;
		}

		public IReadOnlyList<SequenceStep> Steps { get; }

		public INodeType FactoryReturnType { get; }

		public Boolean IsMemoized { get; }

		public INodeType ReturnType
		{
			get
			{
				if (FactoryReturnType != null) return FactoryReturnType;

				var list = StepTypes;
				if (list.Count == 0)
				{
					return EmptyNodeType.Instance;
				}
				else if (list.Count == 1)
				{
					return list[0];
				}
				else
				{
					return new TupleNodeType(list);
				}
			}
		}

		public IReadOnlyList<INodeType> StepTypes
		{
			get
			{
				var list = new List<INodeType>();
				foreach (var step in this.Steps)
				{
					if (!step.IsReturned) continue;
					var funcType = step.Function.ReturnType;
					list.Add(funcType);
				}

				return list;
			}
		}


		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);

		public void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input) => visitor.Visit(this, input);

		public TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input) => visitor.Visit(this, input);
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
		public Boolean IsReturned => MatchAction == MatchAction.Consume && Function.ReturnType != EmptyNodeType.Instance;
	}
}
