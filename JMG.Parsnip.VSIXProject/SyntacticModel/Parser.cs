using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Extensions;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	/// <summary>
	/// Mediator between the IVsSingleFileGenerator.Generate method, and the 
	/// </summary>
	internal static class Parser
	{
		public static ParsnipDefinition Parse(string wszInputFilePath)
		{
			// Fake the parsing until the parser can be bootstrapped.

			const String definitionRuleName = "definition";
			const String definitionItemRuleName = "definitionItem";

			var root = new ParsnipDefinition(new[] {
				new Rule(
					new RuleHead(
						new RuleIdentifier(definitionRuleName),
						new ClassIdentifier(nameof(ParsnipDefinition))
					),
					new RuleBody(new[] {
						new Union(new[] {
							new Sequence(new[] {
								new Segment(
									MatchAction.Consume,
									new RuleIdentifierToken(new RuleIdentifier(definitionItemRuleName)),
									Cardinality.Plus)
							})
						})
					})
				),
			});

			return root;
		}
	}
}
