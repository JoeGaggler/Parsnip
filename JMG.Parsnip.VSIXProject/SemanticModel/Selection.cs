using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Extensions;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class Selection : IParseFunction
	{
		public Selection(IReadOnlyList<SelectionStep> steps)
		{
			this.Steps = steps;
		}

		public IReadOnlyList<SelectionStep> Steps { get; }

		public INodeType ReturnType
		{
			get
			{
				var optionTypes = this.Steps.Select(i => i.InterfaceMethod?.ReturnType ?? i.Function.ReturnType).ToList();
				if (optionTypes.Select(i => NameGen.TypeString(i)).ToList().AllEqual())
				{
					return optionTypes[0];
				}
				else
				{
					// Union-types are not yet supported
					return new SingleNodeType("AMBIGUOUS_SELECTION");
				}
			}
		}

		// Selections cannot have an interface method until union-types are supported
		public InterfaceMethod InterfaceMethod => null;

		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);

		public void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input) => visitor.Visit(this, input);

		public void ApplyVisitor<TInput1, TInput2>(IParseFunctionActionVisitor<TInput1, TInput2> visitor, TInput1 input1, TInput2 input2) => visitor.Visit(this, input1, input2);

		public TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input) => visitor.Visit(this, input);
	}

	internal class SelectionStep
	{
		public SelectionStep(IParseFunction parseFunction, InterfaceMethod interfaceMethod)
		{
			this.Function = parseFunction;
			this.InterfaceMethod = interfaceMethod;
		}

		public IParseFunction Function { get; }
		public InterfaceMethod InterfaceMethod { get; }
	}
}
