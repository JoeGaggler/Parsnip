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
			const String ruleRuleName = "rule";
			const String commentRuleName = "comment";
			const String ruleHeadRuleName = "ruleHead";
			const String ruleBodyRuleName = "ruleBody";
			const String ruleHeadPrefixName = "ruleHeadPrefix";
			const String classIdRuleName = "CID";
			const String ruleIdRuleName = "RID";
			const String intrinsicIdRuleName = "IID";
			const String choiceRuleName = "choice";
			const String unionRuleName = "union";
			const String sequenceRuleName = "sequence";
			const String segmentRuleName = "segment";
			const String segmentCardinalityRuleName = "segmentCardinality";
			const String tokenRuleName = "token";


			const String eolIntrinsic = "EOL";
			const String eosIntrinsic = "EOS";
			const String stringIntrinsic = "CSTRING";


			var definition = new ParsnipDefinition(new[] {
				// definition
				new Rule(
					SyntaxFactory.RuleHead(definitionRuleName, nameof(ParsnipDefinition)),
					SyntaxFactory.RuleBodyForSequence(new[] {
						new Segment(
							MatchAction.Consume,
							new RuleIdentifierToken(new RuleIdentifier(definitionItemRuleName)),
							Cardinality.Plus)
					})
				),

				// definition item
				new Rule(
					SyntaxFactory.RuleHead(definitionItemRuleName, nameof(IParsnipDefinitionItem)),
					new RuleBody(new[] {
						SyntaxFactory.ChoiceForSegment(
							new Segment(
								MatchAction.Consume,
								new RuleIdentifierToken(new RuleIdentifier(ruleRuleName)),
								Cardinality.One)
						),
						SyntaxFactory.ChoiceForSegment(
							new Segment(
								MatchAction.Consume,
								new RuleIdentifierToken(new RuleIdentifier(commentRuleName)),
								Cardinality.One)
						),
						SyntaxFactory.ChoiceForSegment(
							new Segment(
								MatchAction.Ignore,
								new IntrinsicToken(eolIntrinsic),
								Cardinality.One)
						)
					})
				),

				// rule
				new Rule(
					SyntaxFactory.RuleHead(ruleRuleName, nameof(Rule)),
					SyntaxFactory.RuleBodyForSequence(new[] {
						new Segment(
							MatchAction.Consume,
							new RuleIdentifierToken(new RuleIdentifier(ruleHeadRuleName)),
							Cardinality.One),
						new Segment(
							MatchAction.Consume,
							new RuleIdentifierToken(new RuleIdentifier(ruleBodyRuleName)),
							Cardinality.One)
					})
				),

				// ruleHead
				new Rule(
					SyntaxFactory.RuleHead(ruleHeadRuleName, nameof(RuleHead)),
					new RuleBody(new[] {
						SyntaxFactory.ChoiceForSequence(new[] {
							new Segment(
								MatchAction.Consume,
								new RuleIdentifierToken(new RuleIdentifier(ruleHeadPrefixName)),
								Cardinality.One),
							SyntaxFactory.OHS(),
							new Segment(
								MatchAction.Consume,
								new RuleIdentifierToken(new RuleIdentifier(classIdRuleName)),
								Cardinality.One),
							SyntaxFactory.OHS(),
							SyntaxFactory.EOL()
						}),
						SyntaxFactory.ChoiceForSequence(new[] {
							new Segment(
								MatchAction.Consume,
								new RuleIdentifierToken(new RuleIdentifier(ruleHeadPrefixName)),
								Cardinality.One),
							SyntaxFactory.OHS(),
							SyntaxFactory.EOL()
						})
					})
				),

				// ruleHeadPrefix
				new Rule(
					SyntaxFactory.RuleHead(ruleHeadPrefixName, nameof(RuleHeadPrefix)),
					SyntaxFactory.RuleBodyForSequence(new[] {
						new Segment(
							MatchAction.Consume,
							new RuleIdentifierToken(new RuleIdentifier(ruleIdRuleName)),
							Cardinality.One),
						SyntaxFactory.OHS(),
						new Segment(
							MatchAction.Ignore,
							new LiteralStringToken(":"),
							Cardinality.One)
					})
				),

				// ruleBody
				new Rule(
					SyntaxFactory.RuleHead(ruleBodyRuleName, nameof(RuleBody)),
					SyntaxFactory.RuleBodyForSequence(new[] {
						SyntaxFactory.PlusConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(choiceRuleName)))
					})
				),

				// choice
				new Rule(
					SyntaxFactory.RuleHead(choiceRuleName, nameof(Choice)),
					SyntaxFactory.RuleBodyForSequence(new[] {
						SyntaxFactory.PlusConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(unionRuleName))),
						SyntaxFactory.OHS(),
						SyntaxFactory.EOLOS()
					})
				),

				// union
				new Rule(
					SyntaxFactory.RuleHead(unionRuleName, nameof(Union)),
					new RuleBody(new[] {
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(sequenceRuleName))),
							SyntaxFactory.OHS(),
							SyntaxFactory.IgnoreSegment(new LiteralStringToken("|")),
							SyntaxFactory.OHS(),
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(unionRuleName))),
						}),
						SyntaxFactory.ChoiceForSegment(
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(sequenceRuleName)))
						)
					})
				),

				// sequence
				new Rule(
					SyntaxFactory.RuleHead(sequenceRuleName, nameof(Sequence)),
					new RuleBody(new[] {
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(segmentRuleName))),
							SyntaxFactory.OHS(),
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(sequenceRuleName))),
						}),
						SyntaxFactory.ChoiceForSegment(
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(segmentRuleName)))
						)
					})
				),

				// segment
				new Rule(
					SyntaxFactory.RuleHead(segmentRuleName, nameof(Segment)),
					new RuleBody(new[] {
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.IgnoreSegment(new LiteralStringToken("`")),
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(segmentCardinalityRuleName)))
						}),
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.IgnoreSegment(new LiteralStringToken("~")),
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(segmentCardinalityRuleName)))
						}),
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.IgnoreSegment(new LiteralStringToken("&")),
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(segmentCardinalityRuleName)))
						}),
						SyntaxFactory.ChoiceForSegment(
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(segmentCardinalityRuleName)))
						)
					})
				),

				// segmentCardinality
				new Rule(
					SyntaxFactory.RuleHead(segmentCardinalityRuleName, nameof(TokenCardinality)),
					new RuleBody(new[] {
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(tokenRuleName))),
							SyntaxFactory.IgnoreSegment(new LiteralStringToken("+"))
						}),
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(tokenRuleName))),
							SyntaxFactory.IgnoreSegment(new LiteralStringToken("?"))
						}),
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(tokenRuleName))),
							SyntaxFactory.IgnoreSegment(new LiteralStringToken("*"))
						}),
						SyntaxFactory.ChoiceForSegment(
							SyntaxFactory.ConsumeSegment(new RuleIdentifierToken(new RuleIdentifier(tokenRuleName)))
						)
					})
				),

				// token
				new Rule(
					SyntaxFactory.RuleHead(segmentCardinalityRuleName, nameof(IToken)),
					new RuleBody(new[] {
						SyntaxFactory.ChoiceForSegment(SyntaxFactory.IgnoreSegment(new LiteralStringToken("."))),
						SyntaxFactory.ChoiceForSegment(SyntaxFactory.ConsumeSegment(new IntrinsicToken(stringIntrinsic))),
						SyntaxFactory.ChoiceForSegment(SyntaxFactory.ConsumeSegment(SyntaxFactory.RuleIdToken(ruleIdRuleName))),
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.IgnoreSegment(new LiteralStringToken("<")),
							SyntaxFactory.ConsumeSegment(SyntaxFactory.RuleIdToken(intrinsicIdRuleName)),
							SyntaxFactory.IgnoreSegment(new LiteralStringToken(">"))
						}),
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.IgnoreSegment(new LiteralStringToken("(")),
							SyntaxFactory.ConsumeSegment(SyntaxFactory.RuleIdToken(unionRuleName)),
							SyntaxFactory.IgnoreSegment(new LiteralStringToken(")"))
						})
					})
				),

				// rule id
				new Rule(
					SyntaxFactory.RuleHead(ruleIdRuleName, nameof(RuleIdentifier)),
					SyntaxFactory.RuleBodyForSegment(SyntaxFactory.PlusConsumeSegment(new IntrinsicToken("Aa")))
				),

				// class id
				new Rule(
					SyntaxFactory.RuleHead(classIdRuleName, nameof(ClassIdentifier)),
					SyntaxFactory.RuleBodyForSegment(SyntaxFactory.PlusConsumeSegment(new IntrinsicToken("Aa")))
				),

				// intrinsic id
				new Rule(
					SyntaxFactory.RuleHead(intrinsicIdRuleName, nameof(String)),
					SyntaxFactory.RuleBodyForSegment(SyntaxFactory.PlusConsumeSegment(new IntrinsicToken("Aa")))
				),

				// Optional Horizontal Whitespace
				new Rule(
					SyntaxFactory.RuleHead(SyntaxFactory.optionalHorizontalWhitespaceRuleName, null),
					SyntaxFactory.RuleBodyForSegment(SyntaxFactory.MaybeConsumeSegment(SyntaxFactory.RuleIdToken(SyntaxFactory.horizontalWhitespaceRuleName)))
				),

				// Horizontal Whitespace
				new Rule(
					SyntaxFactory.RuleHead(SyntaxFactory.horizontalWhitespaceRuleName, null),
					new RuleBody(new[] {
						SyntaxFactory.ChoiceForSequence(new[] {
							SyntaxFactory.ConsumeSegment(SyntaxFactory.RuleIdToken(SyntaxFactory.whitespaceRuleName)),
							SyntaxFactory.ConsumeSegment(SyntaxFactory.RuleIdToken(SyntaxFactory.horizontalWhitespaceRuleName))
						}),
						SyntaxFactory.ChoiceForSegment(
							SyntaxFactory.ConsumeSegment(SyntaxFactory.RuleIdToken(SyntaxFactory.whitespaceRuleName))
						)
					})
				),

				// Whitespace
				new Rule(
					SyntaxFactory.RuleHead(SyntaxFactory.optionalHorizontalWhitespaceRuleName, null),
					new RuleBody(new[] {
						SyntaxFactory.ChoiceForSegment(SyntaxFactory.ConsumeSegment(new LiteralStringToken(" "))),
						SyntaxFactory.ChoiceForSegment(SyntaxFactory.ConsumeSegment(new LiteralStringToken("\\t"))),
					})
				),

				// EOLOS
				new Rule(
					SyntaxFactory.RuleHead(SyntaxFactory.endOfLineOrWhitespaceRuleName, null),
					new RuleBody(new[] {
						SyntaxFactory.ChoiceForSegment(SyntaxFactory.IgnoreSegment(new IntrinsicToken(eolIntrinsic))),
						SyntaxFactory.ChoiceForSegment(SyntaxFactory.ConsumeSegment(new IntrinsicToken(eosIntrinsic))),
					})
				),

				// comment
				new Rule(
					SyntaxFactory.RuleHead(commentRuleName, nameof(String)),
					SyntaxFactory.RuleBodyForSequence(new[] {
						SyntaxFactory.IgnoreSegment(new LiteralStringToken("//")),
						SyntaxFactory.StarConsumeSegment(new UnionToken(new Union(new[] { new Sequence(new[]
						{
							new Segment(MatchAction.Fail, SyntaxFactory.RuleIdToken(SyntaxFactory.endOfLineOrWhitespaceRuleName), Cardinality.One),
							new Segment(MatchAction.Consume, new IntrinsicToken("."), Cardinality.One),
						})}))),
						SyntaxFactory.IgnoreSegment(SyntaxFactory.RuleIdToken(SyntaxFactory.endOfLineOrWhitespaceRuleName)),
					})
				),
			});

			return definition;
		}
	}
}
