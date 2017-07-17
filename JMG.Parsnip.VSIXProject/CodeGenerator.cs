using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.CodeWriting;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject
{
	internal class CodeGenerator
	{
		public static IReadOnlyList<IFileScopeItem> FileScopeItemsForSemanticModel(ParsnipModel model, String classNamespace, String className)
		{
			var i = new IClassScopeItem[] { };

			var c = new Class(AccessModifier.Public, className, i);

			var ns = new Namespace(classNamespace, new[] { c });

			return new IFileScopeItem[] { ns };
		}
	}
}
