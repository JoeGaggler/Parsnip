﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SemanticModel
{
	internal class LexemeIdentifier : IParseFunction
	{
		public readonly String Identifier;

		public LexemeIdentifier(String id, InterfaceMethod interfaceMethod)
		{
			this.Identifier = id;
			this.InterfaceMethod = interfaceMethod;
		}

		public InterfaceMethod InterfaceMethod { get; }

		public INodeType ReturnType => new SingleNodeType("String");

		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);

		public void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input) => visitor.Visit(this, input);

		public void ApplyVisitor<TInput1, TInput2>(IParseFunctionActionVisitor<TInput1, TInput2> visitor, TInput1 input1, TInput2 input2) => visitor.Visit(this, input1, input2);

		public TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input) => visitor.Visit(this, input);
	}
}
