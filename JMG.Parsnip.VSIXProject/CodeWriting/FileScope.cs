using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	internal class FileScope
	{
		public FileScope(IReadOnlyList<IFileScopeItem> items)
		{
			this.Items = items;
		}

		public IReadOnlyList<IFileScopeItem> Items { get; }
	}
}
