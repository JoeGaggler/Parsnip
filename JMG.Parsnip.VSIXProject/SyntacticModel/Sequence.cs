using JMG.Parsnip.VSIXProject.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	public class Sequence
	{
		public Sequence(IReadOnlyList<Segment> segments)
		{
			this.Segments = segments;
		}

		public IReadOnlyList<Segment> Segments { get; private set; }

		//internal void HackReplaceSegment(Segment oldSegment, Segment newSegment)
		//{
		//	this.Segments = this.Segments.Replacing(oldSegment, newSegment);
		//}
	}
}
