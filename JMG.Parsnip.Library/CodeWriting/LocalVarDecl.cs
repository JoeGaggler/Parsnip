using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	internal class LocalVarDecl
	{
		public LocalVarDecl(String typeName, String localName)
		{
			this.TypeName = typeName;
			this.LocalName = localName;
		}

		public String TypeName { get; }
		public String LocalName { get; }
	}
}
