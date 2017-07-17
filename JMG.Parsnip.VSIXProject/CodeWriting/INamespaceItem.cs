using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public interface INamespaceItem : IActionVisitable<INamespaceItemVisitor>
	{
	}

	public interface INamespaceItemVisitor :
		IActionVisitor<Class>
	{

	}
}
