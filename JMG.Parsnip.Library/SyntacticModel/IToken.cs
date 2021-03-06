﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.Visiting;

namespace JMG.Parsnip.SyntacticModel
{
	/// <summary>
	/// Interface for a type that represents a Parsnip syntax token
	/// </summary>
	internal interface IToken
	{
		void ApplyVisitor(ITokenActionVisitor visitor);

		TOutput ApplyVisitor<TOutput>(ITokenFuncVisitor<TOutput> visitor);
	}

	internal interface ITokenActionVisitor :
		IActionVisitor<RuleIdentifierToken>,
		IActionVisitor<LiteralStringToken>,
		IActionVisitor<IntrinsicToken>,
		IActionVisitor<UnionToken>,
		IActionVisitor<SeriesToken>
	{

	}

	internal interface ITokenFuncVisitor<TOutput> :
		IFuncVisitor<RuleIdentifierToken, TOutput>,
		IFuncVisitor<LiteralStringToken, TOutput>,
		IFuncVisitor<IntrinsicToken, TOutput>,
		IFuncVisitor<UnionToken, TOutput>,
		IFuncVisitor<SeriesToken, TOutput>
	{

	}
}
