using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class InterfaceMethod
	{
		public InterfaceMethod(INodeType returnType, String name, IReadOnlyList<INodeType> parameterTypes)
		{
			this.ReturnType = returnType;
			this.Name = name;
			this.ParameterTypes = parameterTypes;
		}

		public INodeType ReturnType { get; }

		public String Name { get; }

		public IReadOnlyList<INodeType> ParameterTypes { get; }
	}
}
