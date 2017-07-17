using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.Visiting
{
	public interface IActionVisitor<TVisitee>
	{
		void Visit(TVisitee target);
	}
}
