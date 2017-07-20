// Code Generated via Parsnip Packrat Parser Producer
// Version: unknown
// File: Parsnip.parsnip
// Date: 2017-07-19 20:02:27

using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JMG.Parsnip.VSIXProject.SyntacticModel.Generated
{
	internal interface IParsnipRuleFactory
	{
		ParsnipDefinition Definition1(IReadOnlyList<IParsnipDefinitionItem> t0);
		IParsnipDefinitionItem DefinitionItem1(Rule t0);
		IParsnipDefinitionItem DefinitionItem2(String t0);
		IParsnipDefinitionItem DefinitionItem3();
		Rule Rule1((RuleHead, RuleBody) t0);
		RuleHead RuleHead1((RuleHeadPrefix, ClassIdentifier) t0);
		RuleHead RuleHead2(RuleHeadPrefix t0);
		RuleHeadPrefix RuleHeadPrefix1(RuleIdentifier t0);
		RuleBody RuleBody1(IReadOnlyList<Choice> t0);
		Choice Choice1(Union t0);
		Union Union1((Sequence, Union) t0);
		Union Union2(Sequence t0);
		Sequence Sequence1((Segment, Sequence) t0);
		Sequence Sequence2(Segment t0);
		Segment Segment1(TokenCardinality t0);
		Segment Segment2(TokenCardinality t0);
		Segment Segment3(TokenCardinality t0);
		Segment Segment4(TokenCardinality t0);
		TokenCardinality SegmentCardinality1(IToken t0);
		TokenCardinality SegmentCardinality2(IToken t0);
		TokenCardinality SegmentCardinality3(IToken t0);
		TokenCardinality SegmentCardinality4(IToken t0);
		IToken Token1();
		IToken Token2(String t0);
		IToken Token3(RuleIdentifier t0);
		IToken Token4(String t0);
		IToken Token5(Union t0);
		RuleIdentifier RID1(IReadOnlyList<String> t0);
		ClassIdentifier CID1(IReadOnlyList<String> t0);
		String IID1(IReadOnlyList<String> t0);
		String Comment1(IReadOnlyList<String> t0);
	}

	internal class Parsnip
	{
		private class ParseResult<T> { public T Node; public PackratState State; }

		private class EmptyNode { private EmptyNode() { } public static EmptyNode Instance = new EmptyNode(); }

		public static ParsnipDefinition Parse(String input, IParsnipRuleFactory factory)
		{
			var states = new PackratState[input.Length + 1];
			Enumerable.Range(0, input.Length + 1).ToList().ForEach(i => states[i] = new PackratState(input, i, states, factory));
			var state = states[0];
			var result = PackratState.Definition(state, factory);
			if (result == null) return null;
			return result.Node;
		}

		private class PackratState
		{
			private readonly string input;
			private readonly int inputPosition;
			private readonly PackratState[] states;
			private readonly IParsnipRuleFactory factory;

			public PackratState(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
			{
				this.input = input;
				this.inputPosition = inputPosition;
				this.states = states;
				this.factory = factory;
			}
			
			private static ParseResult<T> ParseMaybe<T>(PackratState state, IParsnipRuleFactory factory, Func<PackratState, IParsnipRuleFactory, ParseResult<T>> parseAction)
			{
				var result = parseAction(state, factory);
				if (result != null)
				{
					return result;
				}
				return new ParseResult<T> { State = state, Node = default(T) };
			}
			
			private static ParseResult<IReadOnlyList<T>> ParseStar<T>(PackratState state, IParsnipRuleFactory factory, Func<PackratState, IParsnipRuleFactory, ParseResult<T>> parseAction)
			{
				var list = new List<T>();
				while (true)
				{
					var nextResult = parseAction(state, factory);
					if (nextResult == null)
					{
						break;
					}
					list.Add(nextResult.Node);
					state = nextResult.State;
				}
				return new ParseResult<IReadOnlyList<T>> { State = state, Node = list };
			}
			
			private static ParseResult<IReadOnlyList<T>> ParsePlus<T>(PackratState state, IParsnipRuleFactory factory, Func<PackratState, IParsnipRuleFactory, ParseResult<T>> parseAction)
			{
				var list = new List<T>();
				var firstResult = parseAction(state, factory);
				if (firstResult == null)
				{
					return null;
				}
				list.Add(firstResult.Node);
				state = firstResult.State;
				while (true)
				{
					var nextResult = parseAction(state, factory);
					if (nextResult == null)
					{
						break;
					}
					list.Add(nextResult.Node);
					state = nextResult.State;
				}
				return new ParseResult<IReadOnlyList<T>> { State = state, Node = list };
			}

			// Intrinsic: .
			private static ParseResult<String> ParseIntrinsic_AnyCharacter(PackratState state, IParsnipRuleFactory factory)
			{
				var input = state.input;
				var inputPosition = state.inputPosition;
				if (inputPosition >= input.Length)
				{
					return null;
				}
				return new ParseResult<String>() { Node = state.input.Substring(inputPosition, 1), State = state.states[inputPosition + 1] };
			}

			// Intrinsic: <Aa>
			private static ParseResult<String> ParseIntrinsic_AnyLetter(PackratState state, IParsnipRuleFactory factory)
			{
				var input = state.input;
				var inputPosition = state.inputPosition;
				if (inputPosition >= input.Length)
				{
					return null;
				}
				else if (!Char.IsLetter(input[inputPosition]))
				{
					return null;
				}
				return new ParseResult<String>() { Node = state.input.Substring(inputPosition, 1), State = state.states[inputPosition + 1] };
			}

			// Intrinsic: <EOL>
			private static ParseResult<String> ParseIntrinsic_EndOfLine(PackratState state, IParsnipRuleFactory factory)
			{
				var result1 = ParseLexeme(state, "\r\n");
				if (result1 != null)
				{
					return new ParseResult<String>() { Node = result1.Node, State = result1.State };
				}
				var result2 = ParseLexeme(state, "\n");
				if (result2 != null)
				{
					return new ParseResult<String>() { Node = result2.Node, State = result2.State };
				}
				return null;
			}

			// Intrinsic: <EOS>
			private static ParseResult<EmptyNode> ParseIntrinsic_EndOfStream(PackratState state, IParsnipRuleFactory factory)
			{
				var input = state.input;
				var inputPosition = state.inputPosition;
				if (inputPosition == input.Length)
				{
					return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = state };
				}
				return null;
			}

			// Intrinsic: <CSTRING>
			private static ParseResult<String> ParseIntrinsic_CString(PackratState state, IParsnipRuleFactory factory)
			{
				var resultStart = ParseLexeme(state, "\"");
				if (resultStart == null) return null;
				var currentState = resultStart.State;
				var sb = new System.Text.StringBuilder();
				while (true)
				{
					var resultEscape = ParseLexeme(currentState, "\\");
					if (resultEscape != null)
					{
						var resultToken = ParseIntrinsic_AnyCharacter(resultEscape.State, factory);
						if (resultToken == null) return null;
						switch (resultToken.Node)
						{
							case "\\":
							case "\"":
							case "t":
							case "r":
							case "n":
							{
								sb.Append("\\");
								sb.Append(resultToken.Node);
								break;
							}
							default:
							{
								return null;
							}
						}
						currentState = resultToken.State;
						continue;
					}
					var resultEnd = ParseLexeme(currentState, "\"");
					if (resultEnd != null)
					{
						return new ParseResult<String>() { Node = sb.ToString(), State = resultEnd.State };
					}
					var resultChar = ParseIntrinsic_AnyCharacter(currentState, factory);
					if (resultChar == null) return null;
					sb.Append(resultChar.Node);
					currentState = resultChar.State;
				}
			}

			// Intrinsic: "HACK: PLACEHOLDER"
			private static ParseResult<String> ParseLexeme(PackratState state, String lexeme)
			{
				var lexemeLength = lexeme.Length;
				if (state.inputPosition + lexemeLength > state.input.Length) return null;
				if (state.input.Substring(state.inputPosition, lexemeLength) != lexeme) return null;
				return new ParseResult<String>() { Node = lexeme, State = state.states[state.inputPosition + lexemeLength] };
			}

			// Selection: ((definitionItem+))
			public static ParseResult<ParsnipDefinition> Definition(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Definition_C1(state, factory);
				if (r1 != null) return new ParseResult<ParsnipDefinition>() { Node = factory.Definition1(r1.Node), State = r1.State };
				return null;
			}

			// Selection: (definitionItem+)
			private static ParseResult<IReadOnlyList<IParsnipDefinitionItem>> Definition_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Definition_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<IReadOnlyList<IParsnipDefinitionItem>>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: definitionItem+
			private static ParseResult<IReadOnlyList<IParsnipDefinitionItem>> Definition_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParsePlus(state, factory, (s, f) => DefinitionItem(s, f));
				if (r1 == null) return null;
				return new ParseResult<IReadOnlyList<IParsnipDefinitionItem>>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ((rule)) | ((comment)) | ((`<EOL>))
			private static ParseResult<IParsnipDefinitionItem> DefinitionItem(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = DefinitionItem_C1(state, factory);
				if (r1 != null) return new ParseResult<IParsnipDefinitionItem>() { Node = factory.DefinitionItem1(r1.Node), State = r1.State };
				var r2 = DefinitionItem_C2(state, factory);
				if (r2 != null) return new ParseResult<IParsnipDefinitionItem>() { Node = factory.DefinitionItem2(r2.Node), State = r2.State };
				var r3 = DefinitionItem_C3(state, factory);
				if (r3 != null) return new ParseResult<IParsnipDefinitionItem>() { Node = factory.DefinitionItem3(), State = r3.State };
				return null;
			}

			// Selection: (rule)
			private static ParseResult<Rule> DefinitionItem_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = DefinitionItem_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<Rule>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: rule
			private static ParseResult<Rule> DefinitionItem_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Rule(state, factory);
				if (r1 == null) return null;
				return new ParseResult<Rule>() { Node = r1.Node, State = r1.State };
			}

			// Selection: (comment)
			private static ParseResult<String> DefinitionItem_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = DefinitionItem_C2_C1(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: comment
			private static ParseResult<String> DefinitionItem_C2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Comment(state, factory);
				if (r1 == null) return null;
				return new ParseResult<String>() { Node = r1.Node, State = r1.State };
			}

			// Selection: (`<EOL>)
			private static ParseResult<EmptyNode> DefinitionItem_C3(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = DefinitionItem_C3_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				return null;
			}

			// Sequence: `<EOL>
			private static ParseResult<EmptyNode> DefinitionItem_C3_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseIntrinsic_EndOfLine(state, factory);
				if (r1 == null) return null;
				return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
			}

			// Selection: ((ruleHead ruleBody))
			private static ParseResult<Rule> Rule(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Rule_C1(state, factory);
				if (r1 != null) return new ParseResult<Rule>() { Node = factory.Rule1(r1.Node), State = r1.State };
				return null;
			}

			// Selection: (ruleHead ruleBody)
			private static ParseResult<(RuleHead, RuleBody)> Rule_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Rule_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<(RuleHead, RuleBody)>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: ruleHead ruleBody
			private static ParseResult<(RuleHead, RuleBody)> Rule_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RuleHead(state, factory);
				if (r1 == null) return null;
				var r2 = RuleBody(r1.State, factory);
				if (r2 == null) return null;
				return new ParseResult<(RuleHead, RuleBody)>() { Node = (r1.Node, r2.Node), State = r2.State };
			}

			// Selection: ((ruleHeadPrefix `OHS CID `OHS `<EOL>)) | ((ruleHeadPrefix `OHS `<EOL>))
			private static ParseResult<RuleHead> RuleHead(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RuleHead_C1(state, factory);
				if (r1 != null) return new ParseResult<RuleHead>() { Node = factory.RuleHead1(r1.Node), State = r1.State };
				var r2 = RuleHead_C2(state, factory);
				if (r2 != null) return new ParseResult<RuleHead>() { Node = factory.RuleHead2(r2.Node), State = r2.State };
				return null;
			}

			// Selection: (ruleHeadPrefix `OHS CID `OHS `<EOL>)
			private static ParseResult<(RuleHeadPrefix, ClassIdentifier)> RuleHead_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RuleHead_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<(RuleHeadPrefix, ClassIdentifier)>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: ruleHeadPrefix `OHS CID `OHS `<EOL>
			private static ParseResult<(RuleHeadPrefix, ClassIdentifier)> RuleHead_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RuleHeadPrefix(state, factory);
				if (r1 == null) return null;
				var r2 = OHS(r1.State, factory);
				if (r2 == null) return null;
				var r3 = CID(r2.State, factory);
				if (r3 == null) return null;
				var r4 = OHS(r3.State, factory);
				if (r4 == null) return null;
				var r5 = ParseIntrinsic_EndOfLine(r4.State, factory);
				if (r5 == null) return null;
				return new ParseResult<(RuleHeadPrefix, ClassIdentifier)>() { Node = (r1.Node, r3.Node), State = r5.State };
			}

			// Selection: (ruleHeadPrefix `OHS `<EOL>)
			private static ParseResult<RuleHeadPrefix> RuleHead_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RuleHead_C2_C1(state, factory);
				if (r1 != null) return new ParseResult<RuleHeadPrefix>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: ruleHeadPrefix `OHS `<EOL>
			private static ParseResult<RuleHeadPrefix> RuleHead_C2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RuleHeadPrefix(state, factory);
				if (r1 == null) return null;
				var r2 = OHS(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseIntrinsic_EndOfLine(r2.State, factory);
				if (r3 == null) return null;
				return new ParseResult<RuleHeadPrefix>() { Node = r1.Node, State = r3.State };
			}

			// Selection: ((RID `OHS `":"))
			private static ParseResult<RuleHeadPrefix> RuleHeadPrefix(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RuleHeadPrefix_C1(state, factory);
				if (r1 != null) return new ParseResult<RuleHeadPrefix>() { Node = factory.RuleHeadPrefix1(r1.Node), State = r1.State };
				return null;
			}

			// Selection: (RID `OHS `":")
			private static ParseResult<RuleIdentifier> RuleHeadPrefix_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RuleHeadPrefix_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<RuleIdentifier>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: RID `OHS `":"
			private static ParseResult<RuleIdentifier> RuleHeadPrefix_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RID(state, factory);
				if (r1 == null) return null;
				var r2 = OHS(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseLexeme(r2.State, ":");
				if (r3 == null) return null;
				return new ParseResult<RuleIdentifier>() { Node = r1.Node, State = r3.State };
			}

			// Selection: ((choice+))
			private static ParseResult<RuleBody> RuleBody(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RuleBody_C1(state, factory);
				if (r1 != null) return new ParseResult<RuleBody>() { Node = factory.RuleBody1(r1.Node), State = r1.State };
				return null;
			}

			// Selection: (choice+)
			private static ParseResult<IReadOnlyList<Choice>> RuleBody_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RuleBody_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<IReadOnlyList<Choice>>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: choice+
			private static ParseResult<IReadOnlyList<Choice>> RuleBody_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParsePlus(state, factory, (s, f) => Choice(s, f));
				if (r1 == null) return null;
				return new ParseResult<IReadOnlyList<Choice>>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ((union `OHS `EOLOS))
			private static ParseResult<Choice> Choice(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Choice_C1(state, factory);
				if (r1 != null) return new ParseResult<Choice>() { Node = factory.Choice1(r1.Node), State = r1.State };
				return null;
			}

			// Selection: (union `OHS `EOLOS)
			private static ParseResult<Union> Choice_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Choice_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<Union>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: union `OHS `EOLOS
			private static ParseResult<Union> Choice_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Union(state, factory);
				if (r1 == null) return null;
				var r2 = OHS(r1.State, factory);
				if (r2 == null) return null;
				var r3 = EOLOS(r2.State, factory);
				if (r3 == null) return null;
				return new ParseResult<Union>() { Node = r1.Node, State = r3.State };
			}

			// Selection: ((sequence `OHS `"|" `OHS union)) | ((sequence))
			private static ParseResult<Union> Union(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Union_C1(state, factory);
				if (r1 != null) return new ParseResult<Union>() { Node = factory.Union1(r1.Node), State = r1.State };
				var r2 = Union_C2(state, factory);
				if (r2 != null) return new ParseResult<Union>() { Node = factory.Union2(r2.Node), State = r2.State };
				return null;
			}

			// Selection: (sequence `OHS `"|" `OHS union)
			private static ParseResult<(Sequence, Union)> Union_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Union_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<(Sequence, Union)>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: sequence `OHS `"|" `OHS union
			private static ParseResult<(Sequence, Union)> Union_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Sequence(state, factory);
				if (r1 == null) return null;
				var r2 = OHS(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseLexeme(r2.State, "|");
				if (r3 == null) return null;
				var r4 = OHS(r3.State, factory);
				if (r4 == null) return null;
				var r5 = Union(r4.State, factory);
				if (r5 == null) return null;
				return new ParseResult<(Sequence, Union)>() { Node = (r1.Node, r5.Node), State = r5.State };
			}

			// Selection: (sequence)
			private static ParseResult<Sequence> Union_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Union_C2_C1(state, factory);
				if (r1 != null) return new ParseResult<Sequence>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: sequence
			private static ParseResult<Sequence> Union_C2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Sequence(state, factory);
				if (r1 == null) return null;
				return new ParseResult<Sequence>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ((segment `OHS sequence)) | ((segment))
			private static ParseResult<Sequence> Sequence(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Sequence_C1(state, factory);
				if (r1 != null) return new ParseResult<Sequence>() { Node = factory.Sequence1(r1.Node), State = r1.State };
				var r2 = Sequence_C2(state, factory);
				if (r2 != null) return new ParseResult<Sequence>() { Node = factory.Sequence2(r2.Node), State = r2.State };
				return null;
			}

			// Selection: (segment `OHS sequence)
			private static ParseResult<(Segment, Sequence)> Sequence_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Sequence_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<(Segment, Sequence)>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: segment `OHS sequence
			private static ParseResult<(Segment, Sequence)> Sequence_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Segment(state, factory);
				if (r1 == null) return null;
				var r2 = OHS(r1.State, factory);
				if (r2 == null) return null;
				var r3 = Sequence(r2.State, factory);
				if (r3 == null) return null;
				return new ParseResult<(Segment, Sequence)>() { Node = (r1.Node, r3.Node), State = r3.State };
			}

			// Selection: (segment)
			private static ParseResult<Segment> Sequence_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Sequence_C2_C1(state, factory);
				if (r1 != null) return new ParseResult<Segment>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: segment
			private static ParseResult<Segment> Sequence_C2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Segment(state, factory);
				if (r1 == null) return null;
				return new ParseResult<Segment>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ((`"`" segmentCardinality)) | ((`"~" segmentCardinality)) | ((`"&" segmentCardinality)) | ((segmentCardinality))
			private static ParseResult<Segment> Segment(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Segment_C1(state, factory);
				if (r1 != null) return new ParseResult<Segment>() { Node = factory.Segment1(r1.Node), State = r1.State };
				var r2 = Segment_C2(state, factory);
				if (r2 != null) return new ParseResult<Segment>() { Node = factory.Segment2(r2.Node), State = r2.State };
				var r3 = Segment_C3(state, factory);
				if (r3 != null) return new ParseResult<Segment>() { Node = factory.Segment3(r3.Node), State = r3.State };
				var r4 = Segment_C4(state, factory);
				if (r4 != null) return new ParseResult<Segment>() { Node = factory.Segment4(r4.Node), State = r4.State };
				return null;
			}

			// Selection: (`"`" segmentCardinality)
			private static ParseResult<TokenCardinality> Segment_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Segment_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<TokenCardinality>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: `"`" segmentCardinality
			private static ParseResult<TokenCardinality> Segment_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "`");
				if (r1 == null) return null;
				var r2 = SegmentCardinality(r1.State, factory);
				if (r2 == null) return null;
				return new ParseResult<TokenCardinality>() { Node = r2.Node, State = r2.State };
			}

			// Selection: (`"~" segmentCardinality)
			private static ParseResult<TokenCardinality> Segment_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Segment_C2_C1(state, factory);
				if (r1 != null) return new ParseResult<TokenCardinality>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: `"~" segmentCardinality
			private static ParseResult<TokenCardinality> Segment_C2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "~");
				if (r1 == null) return null;
				var r2 = SegmentCardinality(r1.State, factory);
				if (r2 == null) return null;
				return new ParseResult<TokenCardinality>() { Node = r2.Node, State = r2.State };
			}

			// Selection: (`"&" segmentCardinality)
			private static ParseResult<TokenCardinality> Segment_C3(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Segment_C3_C1(state, factory);
				if (r1 != null) return new ParseResult<TokenCardinality>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: `"&" segmentCardinality
			private static ParseResult<TokenCardinality> Segment_C3_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "&");
				if (r1 == null) return null;
				var r2 = SegmentCardinality(r1.State, factory);
				if (r2 == null) return null;
				return new ParseResult<TokenCardinality>() { Node = r2.Node, State = r2.State };
			}

			// Selection: (segmentCardinality)
			private static ParseResult<TokenCardinality> Segment_C4(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Segment_C4_C1(state, factory);
				if (r1 != null) return new ParseResult<TokenCardinality>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: segmentCardinality
			private static ParseResult<TokenCardinality> Segment_C4_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = SegmentCardinality(state, factory);
				if (r1 == null) return null;
				return new ParseResult<TokenCardinality>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ((token `"+")) | ((token `"?")) | ((token `"*")) | ((token))
			private static ParseResult<TokenCardinality> SegmentCardinality(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = SegmentCardinality_C1(state, factory);
				if (r1 != null) return new ParseResult<TokenCardinality>() { Node = factory.SegmentCardinality1(r1.Node), State = r1.State };
				var r2 = SegmentCardinality_C2(state, factory);
				if (r2 != null) return new ParseResult<TokenCardinality>() { Node = factory.SegmentCardinality2(r2.Node), State = r2.State };
				var r3 = SegmentCardinality_C3(state, factory);
				if (r3 != null) return new ParseResult<TokenCardinality>() { Node = factory.SegmentCardinality3(r3.Node), State = r3.State };
				var r4 = SegmentCardinality_C4(state, factory);
				if (r4 != null) return new ParseResult<TokenCardinality>() { Node = factory.SegmentCardinality4(r4.Node), State = r4.State };
				return null;
			}

			// Selection: (token `"+")
			private static ParseResult<IToken> SegmentCardinality_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = SegmentCardinality_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<IToken>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: token `"+"
			private static ParseResult<IToken> SegmentCardinality_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Token(state, factory);
				if (r1 == null) return null;
				var r2 = ParseLexeme(r1.State, "+");
				if (r2 == null) return null;
				return new ParseResult<IToken>() { Node = r1.Node, State = r2.State };
			}

			// Selection: (token `"?")
			private static ParseResult<IToken> SegmentCardinality_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = SegmentCardinality_C2_C1(state, factory);
				if (r1 != null) return new ParseResult<IToken>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: token `"?"
			private static ParseResult<IToken> SegmentCardinality_C2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Token(state, factory);
				if (r1 == null) return null;
				var r2 = ParseLexeme(r1.State, "?");
				if (r2 == null) return null;
				return new ParseResult<IToken>() { Node = r1.Node, State = r2.State };
			}

			// Selection: (token `"*")
			private static ParseResult<IToken> SegmentCardinality_C3(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = SegmentCardinality_C3_C1(state, factory);
				if (r1 != null) return new ParseResult<IToken>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: token `"*"
			private static ParseResult<IToken> SegmentCardinality_C3_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Token(state, factory);
				if (r1 == null) return null;
				var r2 = ParseLexeme(r1.State, "*");
				if (r2 == null) return null;
				return new ParseResult<IToken>() { Node = r1.Node, State = r2.State };
			}

			// Selection: (token)
			private static ParseResult<IToken> SegmentCardinality_C4(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = SegmentCardinality_C4_C1(state, factory);
				if (r1 != null) return new ParseResult<IToken>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: token
			private static ParseResult<IToken> SegmentCardinality_C4_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Token(state, factory);
				if (r1 == null) return null;
				return new ParseResult<IToken>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ((`".")) | ((<CSTRING>)) | ((RID)) | ((`"<" IID `">")) | ((`"(" union `")"))
			private static ParseResult<IToken> Token(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Token_C1(state, factory);
				if (r1 != null) return new ParseResult<IToken>() { Node = factory.Token1(), State = r1.State };
				var r2 = Token_C2(state, factory);
				if (r2 != null) return new ParseResult<IToken>() { Node = factory.Token2(r2.Node), State = r2.State };
				var r3 = Token_C3(state, factory);
				if (r3 != null) return new ParseResult<IToken>() { Node = factory.Token3(r3.Node), State = r3.State };
				var r4 = Token_C4(state, factory);
				if (r4 != null) return new ParseResult<IToken>() { Node = factory.Token4(r4.Node), State = r4.State };
				var r5 = Token_C5(state, factory);
				if (r5 != null) return new ParseResult<IToken>() { Node = factory.Token5(r5.Node), State = r5.State };
				return null;
			}

			// Selection: (`".")
			private static ParseResult<EmptyNode> Token_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Token_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				return null;
			}

			// Sequence: `"."
			private static ParseResult<EmptyNode> Token_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, ".");
				if (r1 == null) return null;
				return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
			}

			// Selection: (<CSTRING>)
			private static ParseResult<String> Token_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Token_C2_C1(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: <CSTRING>
			private static ParseResult<String> Token_C2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseIntrinsic_CString(state, factory);
				if (r1 == null) return null;
				return new ParseResult<String>() { Node = r1.Node, State = r1.State };
			}

			// Selection: (RID)
			private static ParseResult<RuleIdentifier> Token_C3(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Token_C3_C1(state, factory);
				if (r1 != null) return new ParseResult<RuleIdentifier>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: RID
			private static ParseResult<RuleIdentifier> Token_C3_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RID(state, factory);
				if (r1 == null) return null;
				return new ParseResult<RuleIdentifier>() { Node = r1.Node, State = r1.State };
			}

			// Selection: (`"<" IID `">")
			private static ParseResult<String> Token_C4(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Token_C4_C1(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: `"<" IID `">"
			private static ParseResult<String> Token_C4_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "<");
				if (r1 == null) return null;
				var r2 = IID(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseLexeme(r2.State, ">");
				if (r3 == null) return null;
				return new ParseResult<String>() { Node = r2.Node, State = r3.State };
			}

			// Selection: (`"(" union `")")
			private static ParseResult<Union> Token_C5(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Token_C5_C1(state, factory);
				if (r1 != null) return new ParseResult<Union>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: `"(" union `")"
			private static ParseResult<Union> Token_C5_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "(");
				if (r1 == null) return null;
				var r2 = Union(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseLexeme(r2.State, ")");
				if (r3 == null) return null;
				return new ParseResult<Union>() { Node = r2.Node, State = r3.State };
			}

			// Selection: ((<Aa>+))
			private static ParseResult<RuleIdentifier> RID(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RID_C1(state, factory);
				if (r1 != null) return new ParseResult<RuleIdentifier>() { Node = factory.RID1(r1.Node), State = r1.State };
				return null;
			}

			// Selection: (<Aa>+)
			private static ParseResult<IReadOnlyList<String>> RID_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = RID_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<IReadOnlyList<String>>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: <Aa>+
			private static ParseResult<IReadOnlyList<String>> RID_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParsePlus(state, factory, (s, f) => ParseIntrinsic_AnyLetter(s, f));
				if (r1 == null) return null;
				return new ParseResult<IReadOnlyList<String>>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ((<Aa>+))
			private static ParseResult<ClassIdentifier> CID(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = CID_C1(state, factory);
				if (r1 != null) return new ParseResult<ClassIdentifier>() { Node = factory.CID1(r1.Node), State = r1.State };
				return null;
			}

			// Selection: (<Aa>+)
			private static ParseResult<IReadOnlyList<String>> CID_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = CID_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<IReadOnlyList<String>>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: <Aa>+
			private static ParseResult<IReadOnlyList<String>> CID_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParsePlus(state, factory, (s, f) => ParseIntrinsic_AnyLetter(s, f));
				if (r1 == null) return null;
				return new ParseResult<IReadOnlyList<String>>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ((<Aa>+))
			private static ParseResult<String> IID(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = IID_C1(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = factory.IID1(r1.Node), State = r1.State };
				return null;
			}

			// Selection: (<Aa>+)
			private static ParseResult<IReadOnlyList<String>> IID_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = IID_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<IReadOnlyList<String>>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: <Aa>+
			private static ParseResult<IReadOnlyList<String>> IID_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParsePlus(state, factory, (s, f) => ParseIntrinsic_AnyLetter(s, f));
				if (r1 == null) return null;
				return new ParseResult<IReadOnlyList<String>>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ((HS?))
			private static ParseResult<EmptyNode> OHS(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = OHS_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				return null;
			}

			// Selection: (HS?)
			private static ParseResult<EmptyNode> OHS_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = OHS_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				return null;
			}

			// Sequence: HS?
			private static ParseResult<EmptyNode> OHS_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseMaybe(state, factory, (s, f) => HS(s, f));
				if (r1 == null) return null;
				return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
			}

			// Selection: ((WS HS)) | ((WS))
			private static ParseResult<EmptyNode> HS(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = HS_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				var r2 = HS_C2(state, factory);
				if (r2 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r2.State };
				return null;
			}

			// Selection: (WS HS)
			private static ParseResult<EmptyNode> HS_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = HS_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				return null;
			}

			// Sequence: WS HS
			private static ParseResult<EmptyNode> HS_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = WS(state, factory);
				if (r1 == null) return null;
				var r2 = HS(r1.State, factory);
				if (r2 == null) return null;
				return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r2.State };
			}

			// Selection: (WS)
			private static ParseResult<EmptyNode> HS_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = HS_C2_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				return null;
			}

			// Sequence: WS
			private static ParseResult<EmptyNode> HS_C2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = WS(state, factory);
				if (r1 == null) return null;
				return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
			}

			// Selection: ((" ")) | (("\t"))
			private static ParseResult<EmptyNode> WS(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = WS_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				var r2 = WS_C2(state, factory);
				if (r2 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r2.State };
				return null;
			}

			// Selection: (" ")
			private static ParseResult<String> WS_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = WS_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: " "
			private static ParseResult<String> WS_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, " ");
				if (r1 == null) return null;
				return new ParseResult<String>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ("\t")
			private static ParseResult<String> WS_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = WS_C2_C1(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: "\t"
			private static ParseResult<String> WS_C2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "\\t");
				if (r1 == null) return null;
				return new ParseResult<String>() { Node = r1.Node, State = r1.State };
			}

			// Selection: ((`<EOL>)) | ((<EOS>))
			private static ParseResult<EmptyNode> EOLOS(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = EOLOS_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				var r2 = EOLOS_C2(state, factory);
				if (r2 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r2.State };
				return null;
			}

			// Selection: (`<EOL>)
			private static ParseResult<EmptyNode> EOLOS_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = EOLOS_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				return null;
			}

			// Sequence: `<EOL>
			private static ParseResult<EmptyNode> EOLOS_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseIntrinsic_EndOfLine(state, factory);
				if (r1 == null) return null;
				return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
			}

			// Selection: (<EOS>)
			private static ParseResult<EmptyNode> EOLOS_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = EOLOS_C2_C1(state, factory);
				if (r1 != null) return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
				return null;
			}

			// Sequence: <EOS>
			private static ParseResult<EmptyNode> EOLOS_C2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseIntrinsic_EndOfStream(state, factory);
				if (r1 == null) return null;
				return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
			}

			// Selection: ((`"//" ((~EOLOS .))* `EOLOS))
			private static ParseResult<String> Comment(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Comment_C1(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = factory.Comment1(r1.Node), State = r1.State };
				return null;
			}

			// Selection: (`"//" ((~EOLOS .))* `EOLOS)
			private static ParseResult<IReadOnlyList<String>> Comment_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Comment_C1_C1(state, factory);
				if (r1 != null) return new ParseResult<IReadOnlyList<String>>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: `"//" ((~EOLOS .))* `EOLOS
			private static ParseResult<IReadOnlyList<String>> Comment_C1_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "//");
				if (r1 == null) return null;
				var r2 = ParseStar(r1.State, factory, (s, f) => Comment_C1_C1_S2(s, f));
				if (r2 == null) return null;
				var r3 = EOLOS(r2.State, factory);
				if (r3 == null) return null;
				return new ParseResult<IReadOnlyList<String>>() { Node = r2.Node, State = r3.State };
			}

			// Selection: (~EOLOS .)
			private static ParseResult<String> Comment_C1_C1_S2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = Comment_C1_C1_S2_C1(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				return null;
			}

			// Sequence: ~EOLOS .
			private static ParseResult<String> Comment_C1_C1_S2_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = EOLOS(state, factory);
				if (r1 != null) return null;
				var r2 = ParseIntrinsic_AnyCharacter(r1.State, factory);
				if (r2 == null) return null;
				return new ParseResult<String>() { Node = r2.Node, State = r2.State };
			}
		}
	}

}
