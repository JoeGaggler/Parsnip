using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.SyntacticModel
{
	public interface IParsnipDefinitionItem
	{
		void ApplyVisitor(IParsnipDefinitionItemVisitor visitor);

		TOutput ApplyVisitor<TOutput>(IParsnipDefinitionItemFuncVisitor<TOutput> visitor);
	}

	public interface IParsnipDefinitionItemVisitor :
		IActionVisitor<Rule>,
		IActionVisitor<Comment>
	{

	}

	public interface IParsnipDefinitionItemFuncVisitor<TOutput> :
		IFuncVisitor<Rule, TOutput>,
		IFuncVisitor<Comment, TOutput>
	{

	}
}
