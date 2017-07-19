using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject
{
	internal static class NameGen
	{
		/// <summary>
		/// Generates a C# class identifier from the input string
		/// </summary>
		/// <param name="identifier">Input to translate into a class name</param>
		/// <returns>C# class identifier</returns>
		public static String ClassName(String identifier) => identifier[0].ToString().ToUpperInvariant() + identifier.Substring(1);

		public static String ParseFunctionMethodName(String ruleIdentifier) => ruleIdentifier[0].ToString().ToUpperInvariant() + ruleIdentifier.Substring(1);

		public static String TypeString(INodeType returnType) => returnType.ApplyVisitor(new TypeStringNodeTypeVisitor());

		private class TypeStringNodeTypeVisitor : INodeTypeFuncVisitor<String>
		{
			public String Visit(EmptyNodeType target) => "EmptyNode";

			public String Visit(SingleNodeType target) => target.Name;

			public String Visit(TupleNodeType target) => $"({String.Join(", ", target.Types.Select(i => i.ApplyVisitor(this)))})";
		}
	}
}
