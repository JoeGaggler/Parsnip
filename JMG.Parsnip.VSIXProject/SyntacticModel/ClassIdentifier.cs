using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	public class ClassIdentifier
	{
		public ClassIdentifier(String text)
		{
			this.Text = text;
		}

		public String Text { get; }
	}
}
