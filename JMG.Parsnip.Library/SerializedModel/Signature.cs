using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.CodeWriting;
using JMG.Parsnip.SemanticModel;

namespace JMG.Parsnip.SerializedModel
{
	internal class Signature
	{
		public Signature(String name, Access access, SemanticModel.IParseFunction func, Invoker invoker, Boolean isMemoized)
		{
			this.Name = name;
			this.Access = access;
			this.Func = func;
			this.Invoker = invoker;
			this.IsMemoized = isMemoized;
		}

		public String Name { get; }
		public Access Access { get; }
		public IParseFunction Func { get; }
		public Invoker Invoker { get; }
		public Boolean IsMemoized { get; }
	}

	/// <summary>
	/// Delegate that creates an parse function invocation from the available context
	/// </summary>
	/// <param name="inputName">Reference to the input</param>
	/// <param name="inputPositionName">Reference to the input</param>
	/// <param name="statesName">Reference to the states</param>
	/// <param name="factoryName">Reference to the factory</param>
	/// <returns>C# expression (as a String) that invokes a parse function</returns>
	internal delegate String Invoker(String inputName, String inputPositionName, String statesName, String factoryName);
}
