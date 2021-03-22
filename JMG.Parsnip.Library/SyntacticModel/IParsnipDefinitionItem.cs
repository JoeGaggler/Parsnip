using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.Visiting;

namespace JMG.Parsnip.SyntacticModel
{
	internal interface IParsnipDefinitionItem
	{
		void ApplyVisitor(IParsnipDefinitionItemVisitor visitor);

		TOutput ApplyVisitor<TOutput>(IParsnipDefinitionItemFuncVisitor<TOutput> visitor);
	}

	internal interface IParsnipDefinitionItemVisitor :
		IActionVisitor<Rule>,
		IActionVisitor<Comment>,
		IActionVisitor<LexemeIdentifier>
	{

	}

	internal interface IParsnipDefinitionItemFuncVisitor<TOutput> :
		IFuncVisitor<Rule, TOutput>,
		IFuncVisitor<Comment, TOutput>,
		IFuncVisitor<LexemeIdentifier, TOutput>
	{

	}
}
