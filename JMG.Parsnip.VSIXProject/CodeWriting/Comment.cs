using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public class Comment : IFileScopeItem
	{
		public Comment(String text)
		{
			this.Text = text;
		}

		public String Text { get; }

		public void ApplyVisitor(IFileScopeItemVisitor visitor) => visitor.Visit(this);
	}
}
