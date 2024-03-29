﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.Visiting;

namespace JMG.Parsnip.SemanticModel
{
	/// <summary>
	/// Interface for a type that implements a parsing step
	/// </summary>
	internal interface IParseFunction
	{
		InterfaceMethod InterfaceMethod { get; }

		INodeType ReturnType { get; }

		void ApplyVisitor(IParseFunctionActionVisitor visitor);

		void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input);

		void ApplyVisitor<TInput1, TInput2>(IParseFunctionActionVisitor<TInput1, TInput2> visitor, TInput1 input1, TInput2 input2);

		TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor);

		TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input);
	}

	internal interface IParseFunctionActionVisitor :
		IActionVisitor<Selection>,
		IActionVisitor<Sequence>,
		IActionVisitor<Intrinsic>,
		IActionVisitor<LiteralString>,
		IActionVisitor<ReferencedRule>,
		IActionVisitor<Repetition>,
		IActionVisitor<Series>,
		IActionVisitor<LexemeIdentifier>
	{

	}

	internal interface IParseFunctionActionVisitor<TInput> :
		IActionVisitor<Selection, TInput>,
		IActionVisitor<Sequence, TInput>,
		IActionVisitor<Intrinsic, TInput>,
		IActionVisitor<LiteralString, TInput>,
		IActionVisitor<ReferencedRule, TInput>,
		IActionVisitor<Repetition, TInput>,
		IActionVisitor<Series, TInput>,
		IActionVisitor<LexemeIdentifier, TInput>
	{

	}

	internal interface IParseFunctionActionVisitor<T0, T1> :
		IActionVisitor<Selection, T0, T1>,
		IActionVisitor<Sequence, T0, T1>,
		IActionVisitor<Intrinsic, T0, T1>,
		IActionVisitor<LiteralString, T0, T1>,
		IActionVisitor<ReferencedRule, T0, T1>,
		IActionVisitor<Repetition, T0, T1>,
		IActionVisitor<Series, T0, T1>,
		IActionVisitor<LexemeIdentifier, T0, T1>
	{

	}

	internal interface IParseFunctionFuncVisitor<TOutput> :
		IFuncVisitor<Selection, TOutput>,
		IFuncVisitor<Sequence, TOutput>,
		IFuncVisitor<Intrinsic, TOutput>,
		IFuncVisitor<LiteralString, TOutput>,
		IFuncVisitor<ReferencedRule, TOutput>,
		IFuncVisitor<Repetition, TOutput>,
		IFuncVisitor<Series, TOutput>,
		IFuncVisitor<LexemeIdentifier, TOutput>
	{

	}

	internal interface IParseFunctionFuncVisitor<TInput, TOutput> :
		IFuncVisitor<Selection, TInput, TOutput>,
		IFuncVisitor<Sequence, TInput, TOutput>,
		IFuncVisitor<Intrinsic, TInput, TOutput>,
		IFuncVisitor<LiteralString, TInput, TOutput>,
		IFuncVisitor<ReferencedRule, TInput, TOutput>,
		IFuncVisitor<Repetition, TInput, TOutput>,
		IFuncVisitor<Series, TInput, TOutput>,
		IFuncVisitor<LexemeIdentifier, TInput, TOutput>
	{

	}
}
