using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public interface IFileScopeItem : IActionVisitable<IFileScopeItemVisitor>
	{
	}

	public interface IFileScopeItemVisitor :
		IActionVisitor<UsingNamespace>,
		IActionVisitor<EmptyLine>,
		IActionVisitor<Namespace>,
		IActionVisitor<Comment>
	{
	}
}
