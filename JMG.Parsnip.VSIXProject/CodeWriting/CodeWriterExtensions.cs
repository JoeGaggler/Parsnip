using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public static class CodeWriterExtensions
	{
		public static void Assign(this CodeWriter writer, String left, String right) => writer.LineOfCode($"{left} = {right};");
	}
}
