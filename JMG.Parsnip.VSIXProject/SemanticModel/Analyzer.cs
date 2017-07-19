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
			var visitor2 = new CreateParseFunctionVisitor(model);
			foreach (var i in syntacticModel.Items)
			{
				model = i.ApplyVisitor(visitor2);
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
				var ruleName = target.Head.RuleIdentifier.Text;
				var className = target.Head.ClassIdentifier.Text;
				var returnType = GetNodeType(className);
				var rule = new Rule(ruleName, returnType, null);
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

			public CreateParseFunctionVisitor(ParsnipModel model)
			{
				this.model = model;
			}

			public ParsnipModel Visit(SyntacticModel.Rule target)
			{
				var oldRule = model.Rules.First(i => i.RuleIdentifier == target.Head.RuleIdentifier.Text);

				var selection = new Selection(false, new SelectionStep[0], factoryReturnType: null);
				foreach (var choice in target.Body.Choices)
				{
					var func = VisitChoice(choice);
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

			internal static Selection VisitChoice(SyntacticModel.Choice target) => VisitUnion(target.Union);

			internal static Selection VisitUnion(SyntacticModel.Union union)
			{
				var selection = new Selection(false, new SelectionStep[0], factoryReturnType: null);
				foreach (var sequence in union.Sequences)
				{
					var func = VisitSequence(sequence);
					var step = new SelectionStep(func, interfaceMethod: null);
					selection = selection.AddingStep(step);
				}
				return selection;
			}

			internal static Sequence VisitSequence(SyntacticModel.Sequence target)
			{
				var sequence = new Sequence(false, new SequenceStep[0], factoryReturnType: null);
				foreach (var segment in target.Segments)
				{
					IParseFunction func;
					switch (segment.Cardinality)
					{
						case Cardinality.One: func = VisitToken(segment.Item); break;
						case Cardinality.Plus: func = VisitToken(segment.Item); break;  // TODO: THESE ARE NOT IMPLEMENTED YET!
						case Cardinality.Star: func = VisitToken(segment.Item); break;	// TODO: THESE ARE NOT IMPLEMENTED YET!
						case Cardinality.Maybe: func = VisitToken(segment.Item); break;	// TODO: THESE ARE NOT IMPLEMENTED YET!
						default: throw new InvalidOperationException();
					}

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

			internal static IParseFunction VisitToken(IToken item) => item.ApplyVisitor(new TokenVisitor());

			private class TokenVisitor : ITokenFuncVisitor<IParseFunction>
			{
				public IParseFunction Visit(RuleIdentifierToken target) => new ReferencedRule(target.Identifier.Text, ruleNodeType: null);

				public IParseFunction Visit(LiteralStringToken target) => new LiteralString(target.Text);

				public IParseFunction Visit(IntrinsicToken target)
				{
					IntrinsicType type;
					switch (target.Identifier)
					{
						case "EOL": type = IntrinsicType.EndOfLine; break;
						case "EOS": type = IntrinsicType.EndOfStream; break;
						case ".": type = IntrinsicType.AnyCharacter; break;
						case "Aa": type = IntrinsicType.AnyLetter; break;
						case "CSTRING": type = IntrinsicType.CString; break;
						default: throw new NotImplementedException($"Unrecognized intrinsic: {target.Identifier}");
					}

					return new Intrinsic(type);
				}

				public IParseFunction Visit(AnyToken target) => new Intrinsic(IntrinsicType.AnyCharacter);

				public IParseFunction Visit(UnionToken target) => VisitUnion(target.Union);
			}
		}
	}
}
