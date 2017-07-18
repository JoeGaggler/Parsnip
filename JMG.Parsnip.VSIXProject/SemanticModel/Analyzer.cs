using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Extensions;
using JMG.Parsnip.VSIXProject.SyntacticModel;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal class Analyzer
	{
		public static ParsnipModel Analyze(ParsnipDefinition syntacticModel)
		{
			var visitor = new RuleVisitor();
			var rules = syntacticModel.Items.Select(i => i.ApplyVisitor(visitor)).Where(i => i != null).ToList();

			return new ParsnipModel(rules);
		}

		private static INodeType GetNodeType(String name)
		{
			if (String.IsNullOrEmpty(name))
			{
				return new EmptyNodeType();
			}

			return new SingleNodeType(name);
		}

		private class RuleVisitor : IParsnipDefinitionItemFuncVisitor<Rule>
		{
			public Rule Visit(SyntacticModel.Rule target)
			{
				var ruleName = target.Head.RuleIdentifier.Text;
				var className = target.Head.ClassIdentifier.Text;
				var returnType = GetNodeType(className);
				return new Rule(ruleName, true, returnType);
			}

			public Rule Visit(Comment target)
			{
				return null;
			}
		}
	}
}
