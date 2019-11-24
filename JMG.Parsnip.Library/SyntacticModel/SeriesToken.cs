using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	public class SeriesToken : IToken
	{
		public SeriesToken(IToken repeatedToken, IToken delimiterToken)
		{
			this.RepeatedToken = repeatedToken;
			this.DelimiterToken = delimiterToken;
		}

		public IToken RepeatedToken { get; }
		public IToken DelimiterToken { get; }

		public void ApplyVisitor(ITokenActionVisitor visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TOutput>(ITokenFuncVisitor<TOutput> visitor) => visitor.Visit(this);
	}
}
