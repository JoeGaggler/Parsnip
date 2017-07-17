using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	internal static class TextWriterExtensions
	{
		public static void WriteIndentation(this TextWriter writer, Int32 depth)
		{
			writer.Write(new String('\t', depth));
		}
	}
}
