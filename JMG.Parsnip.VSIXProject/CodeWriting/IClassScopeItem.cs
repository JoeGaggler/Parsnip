using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public interface IClassScopeItem : IActionVisitable<IClassScopeItemVisitor>
	{
	}

	public interface IClassScopeItemVisitor
	{

	}
}
