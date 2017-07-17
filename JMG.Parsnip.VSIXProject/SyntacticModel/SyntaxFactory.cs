using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	internal class SyntaxFactory
	{
		internal const String whitespaceRuleName = "WS";
		internal const String horizontalWhitespaceRuleName = "HS";
		internal const String optionalHorizontalWhitespaceRuleName = "OHS";
		internal const String endOfLineOrWhitespaceRuleName = "EOLOS";

		public static RuleHead RuleHead(String ruleName, String typeName)
			=> new RuleHead(new RuleIdentifier(ruleName), new ClassIdentifier(typeName));

		public static RuleBody RuleBodyForSequence(IReadOnlyList<Segment> segments) =>
			new RuleBody(new[] {
				new Choice(
					new Union(new[] {
						new Sequence(segments)
					})
				)
			});

		public static RuleBody RuleBodyForSegment(Segment segment) =>
			new RuleBody(new[] {
				new Choice(
					new Union(new[] {
						new Sequence(new[] {
							segment
						})
					})
				)
			});

		public static Choice ChoiceForSegment(Segment segment) =>
			new Choice(
				new Union(new[] {
					new Sequence(new[] {
						segment
					})
				})
			);

		public static Choice ChoiceForSequence(IReadOnlyList<Segment> segments) =>
			new Choice(
				new Union(new[] {
					new Sequence(segments)
				})
			);

		public static Segment EOLOS() => new Segment(MatchAction.Ignore, new RuleIdentifierToken(new RuleIdentifier(endOfLineOrWhitespaceRuleName)), Cardinality.One);
		public static Segment OHS() => new Segment(MatchAction.Ignore, new RuleIdentifierToken(new RuleIdentifier(optionalHorizontalWhitespaceRuleName)), Cardinality.One);
		public static Segment EOL() => new Segment(MatchAction.Ignore, new IntrinsicToken("EOL"), Cardinality.One);

		public static RuleIdentifierToken RuleIdToken(String ruleName) => new RuleIdentifierToken(new RuleIdentifier(ruleName));

		public static Segment ConsumeSegment(IToken token) => new Segment(MatchAction.Consume, token, Cardinality.One);
		public static Segment IgnoreSegment(IToken token) => new Segment(MatchAction.Ignore, token, Cardinality.One);
		public static Segment PlusConsumeSegment(IToken token) => new Segment(MatchAction.Consume, token, Cardinality.Plus);
		public static Segment MaybeConsumeSegment(IToken token) => new Segment(MatchAction.Consume, token, Cardinality.Maybe);
		public static Segment StarConsumeSegment(IToken token) => new Segment(MatchAction.Consume, token, Cardinality.Star);
	}
	
}
