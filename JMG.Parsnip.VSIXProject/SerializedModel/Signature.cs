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
		public Signature(String name, Access access, SemanticModel.IParseFunction func, Invoker invoker)
		{
			this.Name = name;
			this.Access = access;
			this.Func = func;
			this.Invoker = invoker;
		}

		public String Name { get; }
		public Access Access { get; }
		public IParseFunction Func { get; }
		public Invoker Invoker { get; }
	}

	internal delegate String Invoker(String stateName, String factoryName);
}
