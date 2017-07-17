using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	internal class BraceScope : IDisposable
	{
		private readonly TextWriter writer;
		private readonly Int32 depth;

		public BraceScope(TextWriter writer, Int32 depth)
		{
			this.writer = writer;
			this.depth = depth;

			writer.WriteIndentation(depth);
			writer.WriteLine("{");
		}

		public Int32 Depth => depth + 1;

		public void Dispose()
		{
			writer.WriteIndentation(depth);
			writer.WriteLine("}");
		}
	}
}
