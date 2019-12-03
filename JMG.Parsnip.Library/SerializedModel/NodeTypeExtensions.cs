using JMG.Parsnip.SemanticModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SerializedModel
{
	internal static class NodeTypeExtensions
	{
		public static String GetParseResultTypeString(this INodeType returnType) => $"ParseResult<{NameGen.TypeString(returnType)}>";
	}
}
