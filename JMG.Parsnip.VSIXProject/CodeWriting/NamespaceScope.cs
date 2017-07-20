using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	internal class NamespaceScope : IDisposable
	{
		private CodeWriter writer;
		private IDisposable braceScope;

		public NamespaceScope(CodeWriter writer, String namespaceIdentifer)
		{
			this.writer = writer;

			this.writer.LineOfCode($"namespace {namespaceIdentifer}");
			this.braceScope = this.writer.BraceScope();
		}

		public void Dispose()
		{
			this.braceScope.Dispose();
		}
	}
}
