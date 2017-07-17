using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	internal static class NameGen
	{
		/// <summary>
		/// Generates a C# class identifier from the input string
		/// </summary>
		/// <param name="identifier">Input to translate into a class name</param>
		/// <returns>C# class identifier</returns>
		public static String ClassName(String identifier) => identifier[0].ToString().ToUpperInvariant() + identifier.Substring(1);
	}
}
