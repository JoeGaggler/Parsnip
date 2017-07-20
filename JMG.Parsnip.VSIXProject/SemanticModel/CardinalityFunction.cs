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

	internal class CardinalityFunction : IParseFunction
	{
		public CardinalityFunction(IParseFunction innerParseFunction, Cardinality cardinality)
		{
			this.InnerParseFunction = innerParseFunction;
			this.Cardinality = cardinality;
		}

		public IParseFunction InnerParseFunction { get; }

		public Cardinality Cardinality { get; }

		public Boolean IsMemoized => false;

		public INodeType ReturnType
		{
			get
			{
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

		public TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input) => visitor.Visit(this, input);
	}
}
