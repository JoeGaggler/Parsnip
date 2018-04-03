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
			var model = new ParsnipModel();

			// Populate rules
			var ruleVisitor = new RuleVisitor();
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

		private class RuleVisitor : IParsnipDefinitionItemFuncVisitor<ParsnipModel>
		{
			private ParsnipModel model = new ParsnipModel();

			public RuleVisitor()
			{
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
					var func = VisitChoice(choice, false);
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

			internal static Selection VisitChoice(SyntacticModel.Choice target, Boolean isMemoized) => VisitUnion(target.Union, isMemoized);

			internal static Selection VisitUnion(SyntacticModel.Union union, Boolean isMemoized)
			{
				var selection = new Selection(new SelectionStep[0]);
				foreach (var sequence in union.Sequences)
				{
					var func = VisitSequence(sequence, false);
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

			internal static Sequence VisitSequence(SyntacticModel.Sequence target, Boolean isMemoized)
			{
				var sequence = new Sequence(new SequenceStep[0], interfaceMethod: null);
				foreach (var segment in target.Segments)
				{
					var func = VisitCardinality(segment.Item, Convert(segment.Cardinality), false);

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

			internal static IParseFunction VisitCardinality(IToken item, Cardinality cardinality, Boolean isMemoized)
			{
				var func = VisitToken(item, false);
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
						return new CardinalityFunction(func, cardinality, interfaceMethod: null);
					}
					default:
					{
						throw new InvalidOperationException();
					}
				}
			}

			internal static IParseFunction VisitToken(IToken item, Boolean isMemoized) => item.ApplyVisitor(new TokenVisitor(isMemoized));

			private class TokenVisitor : ITokenFuncVisitor<IParseFunction>
			{
				private readonly Boolean isMemoized;

				public TokenVisitor(Boolean isMemoized)
				{
					this.isMemoized = isMemoized;
				}

				public IParseFunction Visit(RuleIdentifierToken target) => new ReferencedRule(target.Identifier.Text, ruleNodeType: null, interfaceMethod: null);

				public IParseFunction Visit(LiteralStringToken target) => new LiteralString(target.Text, interfaceMethod: null);

				public IParseFunction Visit(IntrinsicToken target)
				{
					IntrinsicType type;
					switch (target.Identifier) // Check symbolic and case-sensitive identifiers first
					{
						case ".": type = IntrinsicType.AnyCharacter; break;
						case "--": type = IntrinsicType.OptionalHorizontalWhitespace; break;
						case "#": type = IntrinsicType.AnyDigit; break;
						case "Aa": type = IntrinsicType.AnyLetter; break;
						default:
						{
							switch (target.Identifier.ToUpperInvariant()) // Case-insensitive identifiers
							{
								case "END":
								case "EOS": type = IntrinsicType.EndOfStream; break;
								case "EOL": type = IntrinsicType.EndOfLine; break;
								case "EOLOS": type = IntrinsicType.EndOfLineOrStream; break;
								case "CSTRING": type = IntrinsicType.CString; break;
								case "TAB": return new LiteralString("\t", interfaceMethod: null);
								case "SP":
								case "SPACE": return new LiteralString(" ", interfaceMethod: null);
								default:
								{

									throw new NotImplementedException($"Unrecognized intrinsic: {target.Identifier}");
								}
							}
							break;
						}
					}

					return new Intrinsic(type, interfaceMethod: null);
				}

				public IParseFunction Visit(AnyToken target) => new Intrinsic(IntrinsicType.AnyCharacter, interfaceMethod: null);

				public IParseFunction Visit(UnionToken target) => VisitUnion(target.Union, isMemoized);
			}
		}
	}
}
