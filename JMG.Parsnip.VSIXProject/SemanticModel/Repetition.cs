using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal enum Cardinality
	{
		One,
		Star,
		Plus,
		Maybe
	}

	internal class Repetition : IParseFunction
	{
		public Repetition(IParseFunction innerParseFunction, Cardinality cardinality, InterfaceMethod interfaceMethod)
		{
			this.InnerParseFunction = innerParseFunction;
			this.Cardinality = cardinality;
			this.InterfaceMethod = interfaceMethod;
		}

		public IParseFunction InnerParseFunction { get; }

		public Cardinality Cardinality { get; }

		public InterfaceMethod InterfaceMethod { get; }

		public INodeType ReturnType
		{
			get
			{
				if (InterfaceMethod != null) return InterfaceMethod.ReturnType;

				var innerType = InnerParseFunction.ReturnType;
				switch (Cardinality)
				{
					case Cardinality.One: return innerType;
					case Cardinality.Maybe: return innerType;
					case Cardinality.Star: return new CollectionNodeType(innerType);
					case Cardinality.Plus: return new CollectionNodeType(innerType);
					default: throw new InvalidOperationException($"Found unexpected cardinality: {Cardinality}");
				}
			}
		}

		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);

		public void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input) => visitor.Visit(this, input);

		public void ApplyVisitor<TInput1, TInput2>(IParseFunctionActionVisitor<TInput1, TInput2> visitor, TInput1 input1, TInput2 input2) => visitor.Visit(this, input1, input2);

		public TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input) => visitor.Visit(this, input);
	}
}
