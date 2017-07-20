using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal interface IParsnipDefinitionItem
	{
		void ApplyVisitor(IParsnipDefinitionItemVisitor visitor);

		TOutput ApplyVisitor<TOutput>(IParsnipDefinitionItemFuncVisitor<TOutput> visitor);
	}

	internal interface IParsnipDefinitionItemVisitor :
		IActionVisitor<Rule>,
		IActionVisitor<Comment>
	{

	}

	internal interface IParsnipDefinitionItemFuncVisitor<TOutput> :
		IFuncVisitor<Rule, TOutput>,
		IFuncVisitor<Comment, TOutput>
	{

	}
}
