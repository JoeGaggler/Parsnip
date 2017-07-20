using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class Sequence
	{
		public Sequence(IReadOnlyList<Segment> segments)
		{
			this.Segments = segments;
		}

		public IReadOnlyList<Segment> Segments { get; }
	}
}
