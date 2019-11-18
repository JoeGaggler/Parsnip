using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class Rule
	{
		public Rule(String ruleIdentifier, INodeType returnType, IParseFunction parseFunction)
		{
			this.RuleIdentifier = ruleIdentifier;
			this.ReturnType = returnType;
			this.ParseFunction = parseFunction;
		}

		public String RuleIdentifier { get; }

		public INodeType ReturnType { get; }
		public IParseFunction ParseFunction { get; }
	}
}
