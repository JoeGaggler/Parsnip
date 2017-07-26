using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	public class LiteralStringToken : IToken
	{
		public LiteralStringToken(String text)
		{
			this.Text = text;
		}

		public String Text { get; }

		public void ApplyVisitor(ITokenActionVisitor visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TOutput>(ITokenFuncVisitor<TOutput> visitor) => visitor.Visit(this);
	}
}
