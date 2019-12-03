using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SemanticModel
{
	/// <summary>
	/// Represents a factory method that creates a parent token from a set of parsed child tokens
	/// </summary>
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
