using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	static class StringExtensions
	{
		public static String GetParameterListString(this IReadOnlyList<LocalVarDecl> parameters)
		{
			if (parameters == null)
			{
				return String.Empty;
			}

			return String.Join(", ", parameters.Select(i => $"{i.TypeName} {i.LocalName}"));
		}
	}
}
