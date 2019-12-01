using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SemanticModel
{
	internal class ParsnipModel
	{
		public ParsnipModel()
		{
			this.Rules = Enumerable.Empty<Rule>().ToList();
			this.InterfaceMethods = Enumerable.Empty<InterfaceMethod>().ToList();
		}

		public ParsnipModel(IReadOnlyList<Rule> rules, IReadOnlyList<InterfaceMethod> interfaceMethods)
		{
			this.Rules = rules;
			this.InterfaceMethods = interfaceMethods;
		}

		public IReadOnlyList<Rule> Rules { get; }

		public IReadOnlyList<InterfaceMethod> InterfaceMethods { get; }
	}
}
