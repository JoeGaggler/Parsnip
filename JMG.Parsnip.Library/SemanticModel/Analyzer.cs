using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.Extensions;
using JMG.Parsnip.SyntacticModel;
using JMG.Parsnip.Visiting;

namespace JMG.Parsnip.SemanticModel
{
	internal class Analyzer
	{
		public static ParsnipModel Analyze(ParsnipDefinition syntacticModel)
		{
			var model = new ParsnipModel();

			// Capture Lexemes
			var lexVisitor = new LexVisitor(model);
			foreach (var i in syntacticModel.Items)
			{
				model = i.ApplyVisitor(lexVisitor);
			}

			// Populate rules
			var ruleVisitor = new RuleVisitor(model);
			foreach (var i in syntacticModel.Items)
			{
				model = i.ApplyVisitor(ruleVisitor);
			}

			// Populate parse functions
			var funcVisitor = new CreateParseFunctionVisitor(model, isMemoized: true);
			foreach (var i in syntacticModel.Items)
			{
				model = i.ApplyVisitor(funcVisitor);
			}

			return model;
		}

		private static INodeType GetNodeType(String name)
		{
			if (String.IsNullOrEmpty(name))
			{
				return EmptyNodeType.Instance;
			}

			return new SingleNodeType(name);
		}

		private class LexVisitor : IParsnipDefinitionItemFuncVisitor<ParsnipModel>
		{
			private ParsnipModel model;

			public LexVisitor(ParsnipModel model)
			{
				this.model = model;
			}

			public ParsnipModel Visit(SyntacticModel.Rule target) => model;

			public ParsnipModel Visit(Comment target) => model;

			public ParsnipModel Visit(SyntacticModel.LexemeIdentifier target) => model = model.AddingLexemeIdentifier(new LexemeIdentifier(target.Id, interfaceMethod: null));
		}

		private class RuleVisitor : IParsnipDefinitionItemFuncVisitor<ParsnipModel>
		{
			private ParsnipModel model;

			public RuleVisitor(ParsnipModel model)
			{
				this.model = model;
			}

			public ParsnipModel Visit(SyntacticModel.Rule target)
			{
				var ruleID = target.Head.RuleIdentifier.Text;
				var className = target.Head.ClassIdentifier?.Text;
				var returnType = GetNodeType(className);
				var rule = new Rule(ruleID, returnType, null);
				model = model.AddingRule(rule);
				return model;
			}

			public ParsnipModel Visit(SyntacticModel.Comment target)
			{
				return model;
			}

			public ParsnipModel Visit(SyntacticModel.LexemeIdentifier target)
			{
				return model;
			}
		}

		private class CreateParseFunctionVisitor : IParsnipDefinitionItemFuncVisitor<ParsnipModel>
		{
			private ParsnipModel model;
			private readonly Boolean isMemoized;

			public CreateParseFunctionVisitor(ParsnipModel model, Boolean isMemoized = false)
			{
				this.model = model;
				this.isMemoized = isMemoized;
			}

			public ParsnipModel Visit(SyntacticModel.Rule target)
			{
				var oldRule = model.Rules.First(i => i.RuleIdentifier == target.Head.RuleIdentifier.Text);

				var selection = new Selection(new SelectionStep[0]);
				foreach (var choice in target.Body.Choices)
				{
					var func = VisitChoice(choice, false, model);
					var step = new SelectionStep(func, interfaceMethod: null);
					selection = selection.AddingStep(step);
				}

				var newRule = oldRule.WithParseFunction(selection);

				return model = model.ReplacingRule(oldRule, newRule);
			}

			public ParsnipModel Visit(SyntacticModel.Comment target)
			{
				return model;
			}

			internal static Selection VisitChoice(SyntacticModel.Choice target, Boolean isMemoized, ParsnipModel model) => VisitUnion(target.Union, isMemoized, model);

			internal static Selection VisitUnion(SyntacticModel.Union union, Boolean isMemoized, ParsnipModel model)
			{
				var selection = new Selection(new SelectionStep[0]);
				foreach (var sequence in union.Sequences)
				{
					var func = VisitSequence(sequence, false, model);
					var step = new SelectionStep(func, interfaceMethod: null);
					selection = selection.AddingStep(step);
				}
				return selection;
			}

			private static Cardinality Convert(SyntacticModel.Cardinality cardinality)
			{
				switch (cardinality)
				{
					case SyntacticModel.Cardinality.One: return Cardinality.One;
					case SyntacticModel.Cardinality.Maybe: return Cardinality.Maybe;
					case SyntacticModel.Cardinality.Plus: return Cardinality.Plus;
					case SyntacticModel.Cardinality.Star: return Cardinality.Star;
					default: throw new InvalidOperationException($"Found unexpected cardinality: {cardinality}");
				}
			}

			internal static Sequence VisitSequence(SyntacticModel.Sequence target, Boolean isMemoized, ParsnipModel model)
			{
				var sequence = new Sequence(new SequenceStep[0], interfaceMethod: null);
				foreach (var segment in target.Segments)
				{
					var func = VisitCardinality(segment.Item, Convert(segment.Cardinality), false, model);

					MatchAction action;
					switch (segment.Action)
					{
						case SyntacticModel.MatchAction.Consume: action = MatchAction.Consume; break;
						case SyntacticModel.MatchAction.Fail: action = MatchAction.Fail; break;
						case SyntacticModel.MatchAction.Ignore: action = MatchAction.Ignore; break;
						case SyntacticModel.MatchAction.Rewind: action = MatchAction.Rewind; break;
						default: throw new InvalidOperationException();
					}
					var step = new SequenceStep(func, action);
					sequence = sequence.AddingStep(step);
				}
				return sequence;
			}

			internal static IParseFunction VisitCardinality(IToken item, Cardinality cardinality, Boolean isMemoized, ParsnipModel model)
			{
				var func = VisitToken(item, false, model);
				switch (cardinality)
				{
					case Cardinality.One:
					{
						return func;
					}
					case Cardinality.Plus:
					case Cardinality.Star:
					case Cardinality.Maybe:
					{
						return new Repetition(func, cardinality, interfaceMethod: null);
					}
					default:
					{
						throw new InvalidOperationException();
					}
				}
			}

			internal static IParseFunction VisitToken(IToken item, Boolean isMemoized, ParsnipModel model) => item.ApplyVisitor(new TokenVisitor(isMemoized, model));

			public ParsnipModel Visit(SyntacticModel.LexemeIdentifier target) => this.model;

			private class TokenVisitor : ITokenFuncVisitor<IParseFunction>
			{
				private readonly Boolean isMemoized;
				private readonly ParsnipModel model;

				public TokenVisitor(Boolean isMemoized, ParsnipModel model)
				{
					this.isMemoized = isMemoized;
					this.model = model;
				}

				public IParseFunction Visit(RuleIdentifierToken target) => new ReferencedRule(target.Identifier.Text, ruleNodeType: null, interfaceMethod: null);

				public IParseFunction Visit(LiteralStringToken target) => new LiteralString(target.Text, interfaceMethod: null, stringComparison: target.StringComparison);

				public IParseFunction Visit(IntrinsicToken target)
				{
					IntrinsicType type;
					switch (target.Identifier) // Check symbolic and case-sensitive identifiers first
					{
						case ".": type = IntrinsicType.AnyCharacter; break;
						case "--": type = IntrinsicType.OptionalHorizontalWhitespace; break;
						case "#": case "0": type = IntrinsicType.AnyDigit; break;
						case "Aa": type = IntrinsicType.AnyLetter; break;
						case "Aa#": case "Aa0": type = IntrinsicType.AnyLetterOrDigit; break;
						case "A": type = IntrinsicType.UpperLetter; break;
						case "a": type = IntrinsicType.LowerLetter; break;
						default:
						{
							switch (target.Identifier.ToUpperInvariant()) // Case-insensitive identifiers
							{
								case "END":
								case "EOS": type = IntrinsicType.EndOfStream; break;
								case "EOL": type = IntrinsicType.EndOfLine; break;
								case "EOLOS": type = IntrinsicType.EndOfLineOrStream; break;
								case "CSTRING": type = IntrinsicType.CString; break;
								case "TAB": return new LiteralString("\t", interfaceMethod: null, stringComparison: StringComparison.Ordinal);
								case "SP":
								case "SPACE": return new LiteralString(" ", interfaceMethod: null, stringComparison: StringComparison.Ordinal);
								default:
								{
									if (model.LexemeIdentifiers.Any(i => i.Identifier == target.Identifier))
									{
										return new LexemeIdentifier(target.Identifier, interfaceMethod: null);
									}
									else
									{
										throw new NotImplementedException($"Unrecognized intrinsic: {target.Identifier}");
									}
								}
							}
							break;
						}
					}

					return new Intrinsic(type, interfaceMethod: null);
				}

				public IParseFunction Visit(UnionToken target) => VisitUnion(target.Union, isMemoized, model);

				public IParseFunction Visit(SeriesToken target)
				{
					var repeatedFunction = target.RepeatedToken.ApplyVisitor(this);
					var delimiterFunction = target.DelimiterToken.ApplyVisitor(this);
					return new Series(repeatedFunction, delimiterFunction, interfaceMethod: null);
				}
			}
		}
	}
}
