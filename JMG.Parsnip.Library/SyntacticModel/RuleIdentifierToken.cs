﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SyntacticModel
{
	internal class RuleIdentifierToken : IToken
	{
		public RuleIdentifierToken(RuleIdentifier ruleIdentifier)
		{
			this.Identifier = ruleIdentifier;
		}

		public RuleIdentifier Identifier { get; }

		public void ApplyVisitor(ITokenActionVisitor visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TOutput>(ITokenFuncVisitor<TOutput> visitor) => visitor.Visit(this);
	}
}
