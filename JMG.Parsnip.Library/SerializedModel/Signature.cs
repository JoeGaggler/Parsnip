using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.CodeWriting;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject.SerializedModel
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

	internal delegate String Invoker(String inputName, String inputPositionName, String stateName, String factoryName);
}
