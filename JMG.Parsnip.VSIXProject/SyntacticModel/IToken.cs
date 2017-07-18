using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal interface IToken
	{
		void ApplyVisitor(ITokenActionVisitor visitor);

		TOutput ApplyVisitor<TOutput>(ITokenFuncVisitor<TOutput> visitor);
	}

	internal interface ITokenActionVisitor :
		IActionVisitor<RuleIdentifierToken>,
		IActionVisitor<LiteralStringToken>,
		IActionVisitor<IntrinsicToken>,
		IActionVisitor<AnyToken>,
		IActionVisitor<UnionToken>
	{

	}

	internal interface ITokenFuncVisitor<TOutput> :
		IFuncVisitor<RuleIdentifierToken, TOutput>,
		IFuncVisitor<LiteralStringToken, TOutput>,
		IFuncVisitor<IntrinsicToken, TOutput>,
		IFuncVisitor<AnyToken, TOutput>,
		IFuncVisitor<UnionToken, TOutput>
	{

	}
}
