using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class Choice
	{
		public Choice(Union union)
		{
			this.Union = union;
		}

		public Union Union { get; }
	}
}
