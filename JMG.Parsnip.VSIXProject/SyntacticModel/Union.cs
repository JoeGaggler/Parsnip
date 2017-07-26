using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	public class Union
	{
		public Union(IReadOnlyList<Sequence> sequences)
		{
			this.Sequences = sequences;
		}

		public IReadOnlyList<Sequence> Sequences { get; }
	}
}
