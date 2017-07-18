using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class ParsnipModel
	{
		public ParsnipModel()
		{
			this.Rules = Enumerable.Empty<Rule>().ToList();
		}

		public ParsnipModel(IReadOnlyList<Rule> rules)
		{
			this.Rules = rules;
		}

		public IReadOnlyList<Rule> Rules { get; }
	}
}
