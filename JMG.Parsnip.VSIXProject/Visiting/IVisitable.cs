using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.Visiting
{
	public interface IActionVisitable<TVisitor>
	{
		void ApplyVisitor(TVisitor visitor);
	}
}
