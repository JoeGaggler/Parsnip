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
	}

	internal interface IParseFunctionActionVisitor :
		IActionVisitor<Selection>,
		IActionVisitor<Sequence>,
		IActionVisitor<Intrinsic>,
		IActionVisitor<LiteralString>,
		IActionVisitor<ReferencedRule>
	{

	}

	internal interface IParseFunctionActionVisitor<TInput> :
		IActionVisitor<Selection, TInput>,
		IActionVisitor<Sequence, TInput>,
		IActionVisitor<Intrinsic, TInput>,
		IActionVisitor<LiteralString, TInput>,
		IActionVisitor<ReferencedRule, TInput>
	{

	}

	internal interface IParseFunctionFuncVisitor<TOutput> :
		IFuncVisitor<Selection, TOutput>,
		IFuncVisitor<Sequence, TOutput>,
		IFuncVisitor<Intrinsic, TOutput>,
		IFuncVisitor<LiteralString, TOutput>,
		IFuncVisitor<ReferencedRule, TOutput>
	{

	}
}
