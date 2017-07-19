using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Extensions;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class Selection : IParseFunction, IParseFunctionWithFactoryType
	{
		public Selection(Boolean isMemoized, IReadOnlyList<SelectionStep> steps, INodeType factoryReturnType)
		{
			this.IsMemoized = isMemoized;
			this.Steps = steps;
			this.FactoryReturnType = factoryReturnType;
		}

		public IReadOnlyList<SelectionStep> Steps { get; }

		public Boolean IsMemoized { get; }

		public INodeType FactoryReturnType { get; }

		public INodeType ReturnType
		{
			get
			{
				if (FactoryReturnType != null) return FactoryReturnType;

				var optionTypes = this.Steps.Select(i => i.Function.ReturnType).ToList();
				if (optionTypes.Select(i => NameGen.TypeString(i)).ToList().AllEqual())
				{
					return optionTypes[0];
				}
				else
				{
					return new SingleNodeType("AMBIGUOUS_SELECTION");
				}
			}
		}

		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);

		public void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input) => visitor.Visit(this, input);

		public TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input) => visitor.Visit(this, input);
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
