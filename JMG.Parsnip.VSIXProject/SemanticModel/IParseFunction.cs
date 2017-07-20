using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal interface IParseFunction
	{
		Boolean IsMemoized { get; }

		INodeType ReturnType { get; }

		void ApplyVisitor(IParseFunctionActionVisitor visitor);

		void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input);

		TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor);

		TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input);
	}

	internal interface IParseFunctionActionVisitor :
		IActionVisitor<Selection>,
		IActionVisitor<Sequence>,
		IActionVisitor<Intrinsic>,
		IActionVisitor<LiteralString>,
		IActionVisitor<ReferencedRule>,
		IActionVisitor<CardinalityFunction>
	{

	}

	internal interface IParseFunctionActionVisitor<TInput> :
		IActionVisitor<Selection, TInput>,
		IActionVisitor<Sequence, TInput>,
		IActionVisitor<Intrinsic, TInput>,
		IActionVisitor<LiteralString, TInput>,
		IActionVisitor<ReferencedRule, TInput>,
		IActionVisitor<CardinalityFunction, TInput>
	{

	}

	internal interface IParseFunctionFuncVisitor<TOutput> :
		IFuncVisitor<Selection, TOutput>,
		IFuncVisitor<Sequence, TOutput>,
		IFuncVisitor<Intrinsic, TOutput>,
		IFuncVisitor<LiteralString, TOutput>,
		IFuncVisitor<ReferencedRule, TOutput>,
		IFuncVisitor<CardinalityFunction, TOutput>
	{

	}

	internal interface IParseFunctionFuncVisitor<TInput, TOutput> :
		IFuncVisitor<Selection, TInput, TOutput>,
		IFuncVisitor<Sequence, TInput, TOutput>,
		IFuncVisitor<Intrinsic, TInput, TOutput>,
		IFuncVisitor<LiteralString, TInput, TOutput>,
		IFuncVisitor<ReferencedRule, TInput, TOutput>,
		IFuncVisitor<CardinalityFunction, TInput, TOutput>
	{

	}
}
