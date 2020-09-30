using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	internal class LiteralStringToken : IToken
	{
		public LiteralStringToken(String text, String stringComparisonMode)
		{
			this.Text = text;

			if (String.IsNullOrEmpty(stringComparisonMode))
			{
				this.StringComparison = StringComparison.Ordinal;
			}
			else if (String.Equals(stringComparisonMode, "i", StringComparison.OrdinalIgnoreCase))
			{
				this.StringComparison = StringComparison.OrdinalIgnoreCase;
			}
			else
			{
				throw new InvalidOperationException($"Unexpected string comparison mode: {stringComparisonMode}");
			}
		}

		public StringComparison StringComparison { get; }

		public String Text { get; }

		public void ApplyVisitor(ITokenActionVisitor visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TOutput>(ITokenFuncVisitor<TOutput> visitor) => visitor.Visit(this);
	}
}
