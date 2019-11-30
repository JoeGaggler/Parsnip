// Code Generated via Parsnip Packrat Parser Producer
// Version: 1.23
// Date: 2019-11-29 19:36:30

using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JMG.Parsnip.SyntacticModel.Generated
{
	public interface IParsnipRuleFactory
	{
		ParsnipDefinition Definition1(IReadOnlyList<IParsnipDefinitionItem> t0);
		IParsnipDefinitionItem DefinitionItem1(Rule t0);
		IParsnipDefinitionItem DefinitionItem2(String t0);
		IParsnipDefinitionItem DefinitionItem3();
		Rule Rule1(RuleHead t0, RuleBody t1);
		RuleHead RuleHead1(RuleHeadPrefix t0, ClassIdentifier t1);
		RuleHead RuleHead2(RuleHeadPrefix t0);
		RuleHeadPrefix RuleHeadPrefix1(RuleIdentifier t0);
		RuleBody RuleBody1(IReadOnlyList<Choice> t0);
		Choice Choice1(Union t0);
		Union Union1(IReadOnlyList<Sequence> t0);
		Sequence Sequence1(IReadOnlyList<Segment> t0);
		Segment Special1(IToken t0, IToken t1);
		Segment Special2(Segment t0);
		Segment Segment1(String t0, TokenCardinality t1);
		TokenCardinality Cardinality1(IToken t0, String t1);
		IToken Token1();
		IToken Token2(String t0);
		IToken Token3(RuleIdentifier t0);
		IToken Token4(String t0);
		IToken Token5(Union t0);
		RuleIdentifier RuleIdentifier1(String t0, IReadOnlyList<String> t1);
		ClassIdentifier ClassIdentifier1(IReadOnlyList<String> t0);
		String CsharpIdentifier1(String t0, IReadOnlyList<String> t1);
		String IntrinsicIdentifier1(IReadOnlyList<String> t0);
		String IntrinsicIdentifier2(String t0);
		String Comment1(IReadOnlyList<String> t0);
	}

	public class Parsnip
	{
		private class ParseResult<T>
		{
			public readonly T Node;
			public readonly PackratState State;
			public readonly Int32 Advanced;

			public ParseResult(T node, PackratState state, Int32 advanced)
			{
				this.Node = node;
				this.State = state;
				this.Advanced = advanced;
			}
		}

		private class EmptyNode { private EmptyNode() { } public static EmptyNode Instance = new EmptyNode(); }

		public static ParsnipDefinition Parse(String input, IParsnipRuleFactory factory)
		{
			var states = new PackratState[input.Length + 1];
			Enumerable.Range(0, input.Length + 1).ToList().ForEach(i => states[i] = new PackratState(input, states));
			var state = states[0];
			var result = ParseRule_Definition(0, state, factory);
			if (result == null) return null;
			return result.Node;
		}


		private static ParseResult<T> ParseMaybe<T>(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory, Func<Int32, PackratState, IParsnipRuleFactory, ParseResult<T>> parseAction)
		{
			var result = parseAction(inputPosition, state, factory);
			if (result != null)
			{
				return result;
			}
			return new ParseResult<T>(default(T), state, 0);
		}

		private static ParseResult<IReadOnlyList<T>> ParseStar<T>(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory, Func<Int32, PackratState, IParsnipRuleFactory, ParseResult<T>> parseAction)
		{
			var list = new List<T>();
			var nextResultInputPosition = inputPosition;
			while (true)
			{
				var nextResult = parseAction(nextResultInputPosition, state, factory);
				if (nextResult == null)
				{
					break;
				}
				list.Add(nextResult.Node);
				state = nextResult.State;
				nextResultInputPosition = nextResultInputPosition + nextResult.Advanced;
			}
			return new ParseResult<IReadOnlyList<T>>(list, state, nextResultInputPosition - inputPosition);
		}

		private static ParseResult<IReadOnlyList<T>> ParsePlus<T>(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory, Func<Int32, PackratState, IParsnipRuleFactory, ParseResult<T>> parseAction)
		{
			var list = new List<T>();
			var nextResultInputPosition = inputPosition;
			var firstResult = parseAction(nextResultInputPosition, state, factory);
			if (firstResult == null)
			{
				return null;
			}
			list.Add(firstResult.Node);
			state = firstResult.State;
			nextResultInputPosition = nextResultInputPosition + firstResult.Advanced;
			while (true)
			{
				var nextResult = parseAction(nextResultInputPosition, state, factory);
				if (nextResult == null)
				{
					break;
				}
				list.Add(nextResult.Node);
				state = nextResult.State;
				nextResultInputPosition = nextResultInputPosition + nextResult.Advanced;
			}
			return new ParseResult<IReadOnlyList<T>>(list, state, nextResultInputPosition - inputPosition);
		}

		private static ParseResult<IReadOnlyList<T>> ParseSeries<T, D>(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory, Func<Int32, PackratState, IParsnipRuleFactory, ParseResult<T>> parseAction, Func<Int32, PackratState, IParsnipRuleFactory, ParseResult<D>> parseDelimiterAction)
		{
			var list = new List<T>();
			var nextResultInputPosition = inputPosition;
			var firstResult = parseAction(nextResultInputPosition, state, factory);
			if (firstResult == null)
			{
				return null;
			}
			nextResultInputPosition += firstResult.Advanced;
			list.Add(firstResult.Node);
			state = firstResult.State;
			while (true)
			{
				var delimResult = parseDelimiterAction(nextResultInputPosition, state, factory);
				if (delimResult == null)
				{
					break;
				}
				var nextResult = parseAction(nextResultInputPosition + delimResult.Advanced, delimResult.State, factory);
				if (nextResult == null)
				{
					break;
				}
				list.Add(nextResult.Node);
				state = nextResult.State;
				nextResultInputPosition = nextResultInputPosition + delimResult.Advanced + nextResult.Advanced;
			}
			return new ParseResult<IReadOnlyList<T>>(list, state, nextResultInputPosition - inputPosition);
		}

		private static ParseResult<String> ParseIntrinsic_AnyCharacter(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var input = state.input;
			if (inputPosition >= input.Length)
			{
				return null;
			}
			return new ParseResult<String>(state.input.Substring(inputPosition, 1), state.states[inputPosition + 1], 1);
		}

		private static ParseResult<String> ParseIntrinsic_AnyLetter(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var input = state.input;
			if (inputPosition >= input.Length)
			{
				return null;
			}
			else if (!Char.IsLetter(input[inputPosition]))
			{
				return null;
			}
			return new ParseResult<String>(state.input.Substring(inputPosition, 1), state.states[inputPosition + 1], 1);
		}

		private static ParseResult<String> ParseIntrinsic_AnyDigit(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var input = state.input;
			if (inputPosition >= input.Length)
			{
				return null;
			}
			else if (!Char.IsDigit(input[inputPosition]))
			{
				return null;
			}
			return new ParseResult<String>(state.input.Substring(inputPosition, 1), state.states[inputPosition + 1], 1);
		}

		private static ParseResult<String> ParseIntrinsic_EndOfLine(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var result1 = ParseLexeme(inputPosition, state, "\r\n");
			if (result1 != null)
			{
				return new ParseResult<String>(result1.Node, result1.State, result1.Advanced);
			}
			var result2 = ParseLexeme(inputPosition, state, "\n");
			if (result2 != null)
			{
				return new ParseResult<String>(result2.Node, result2.State, result2.Advanced);
			}
			return null;
		}

		private static ParseResult<EmptyNode> ParseIntrinsic_EndOfStream(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var input = state.input;
			if (inputPosition == input.Length)
			{
				return new ParseResult<EmptyNode>(EmptyNode.Instance, state, 0);
			}
			return null;
		}

		private static ParseResult<EmptyNode> ParseIntrinsic_EndOfLineOrStream(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var input = state.input;
			if (inputPosition == input.Length)
			{
				return new ParseResult<EmptyNode>(EmptyNode.Instance, state, 0);
			}
			var result1 = ParseLexeme(inputPosition, state, "\r\n");
			if (result1 != null)
			{
				return new ParseResult<EmptyNode>(EmptyNode.Instance, result1.State, result1.Advanced);
			}
			var result2 = ParseLexeme(inputPosition, state, "\n");
			if (result2 != null)
			{
				return new ParseResult<EmptyNode>(EmptyNode.Instance, result2.State, result2.Advanced);
			}
			return null;
		}

		private static ParseResult<String> ParseIntrinsic_CString(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var resultStart = ParseLexeme(inputPosition, state, "\"");
			if (resultStart == null) return null;
			var currentState = resultStart.State;
			var nextInputPosition = inputPosition + resultStart.Advanced;
			var sb = new System.Text.StringBuilder();
			while (true)
			{
				var inputPosition2 = nextInputPosition;
				var resultEscape = ParseLexeme(inputPosition2, currentState, "\\");
				if (resultEscape != null)
				{
					inputPosition2 = inputPosition2 + resultEscape.Advanced;
					var resultToken = ParseIntrinsic_AnyCharacter(inputPosition2, resultEscape.State, factory);
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
					inputPosition2 = inputPosition2 + resultToken.Advanced;
					continue;
				}
				var resultEnd = ParseLexeme(inputPosition2, currentState, "\"");
				if (resultEnd != null)
				{
					return new ParseResult<String>(sb.ToString(), resultEnd.State, inputPosition2 + resultEnd.Advanced - inputPosition);
				}
				var resultChar = ParseIntrinsic_AnyCharacter(inputPosition2, currentState, factory);
				if (resultChar == null) return null;
				sb.Append(resultChar.Node);
				currentState = resultChar.State;
				nextInputPosition = inputPosition2 + resultChar.Advanced;
			}
		}

		private static ParseResult<String> ParseIntrinsic_OptionalHorizontalWhitespace(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var input = state.input;
			if (inputPosition >= input.Length)
			{
				return new ParseResult<String>(String.Empty, state, 0);
			}
			var sb = new System.Text.StringBuilder();
			var nextInputPosition = inputPosition;
			while (nextInputPosition < input.Length)
			{
				var ch = input[nextInputPosition];
				if (ch != ' ' && ch != '\t')
				{
					break;
				}
				nextInputPosition++;
			}
			return new ParseResult<String>(state.input.Substring(inputPosition, nextInputPosition - inputPosition), state.states[nextInputPosition], nextInputPosition - inputPosition);
		}

		private static ParseResult<String> ParseLexeme(Int32 inputPosition, PackratState state, String lexeme)
		{
			var lexemeLength = lexeme.Length;
			if (inputPosition + lexemeLength > state.input.Length) return null;
			if (state.input.Substring(inputPosition, lexemeLength) != lexeme) return null;
			return new ParseResult<String>(lexeme, state.states[inputPosition + lexemeLength], lexemeLength);
		}

		// Repetition: definition-item+
		private static ParseResult<ParsnipDefinition> ParseRule_Definition(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_Definition != null) { return state.Mem_ParseRule_Definition; }

			var result = ParsePlus(inputPosition, state, factory, (i, s, f) => ParseRule_DefinitionItem(i, s, f));
			if (result == null) return null;
			return state.Mem_ParseRule_Definition = new ParseResult<ParsnipDefinition>(factory.Definition1(result.Node), result.State, result.Advanced);
		}

		// Selection: rule | comment | (`<EOL>)
		private static ParseResult<IParsnipDefinitionItem> ParseRule_DefinitionItem(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_DefinitionItem != null) { return state.Mem_ParseRule_DefinitionItem; }

			var r1 = ParseRule_Rule(inputPosition, state, factory);
			if (r1 != null) return state.Mem_ParseRule_DefinitionItem = new ParseResult<IParsnipDefinitionItem>(factory.DefinitionItem1(r1.Node), r1.State, r1.Advanced);
			var r2 = ParseRule_Comment(inputPosition, state, factory);
			if (r2 != null) return state.Mem_ParseRule_DefinitionItem = new ParseResult<IParsnipDefinitionItem>(factory.DefinitionItem2(r2.Node), r2.State, r2.Advanced);
			var r3 = ParseRule_DefinitionItem_C3(inputPosition, state, factory);
			if (r3 != null) return state.Mem_ParseRule_DefinitionItem = new ParseResult<IParsnipDefinitionItem>(factory.DefinitionItem3(), r3.State, r3.Advanced);
			return null;
		}

		// Sequence: `<EOL>
		private static ParseResult<EmptyNode> ParseRule_DefinitionItem_C3(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseIntrinsic_EndOfLine(inputPosition, state, factory);
			if (r1 == null) return null;
			return new ParseResult<EmptyNode>(EmptyNode.Instance, r1.State, inputPosition + r1.Advanced - inputPosition);
		}

		// Sequence: rule-head rule-body
		private static ParseResult<Rule> ParseRule_Rule(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_Rule != null) { return state.Mem_ParseRule_Rule; }

			var r1 = ParseRule_RuleHead(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseRule_RuleBody(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			return state.Mem_ParseRule_Rule = new ParseResult<Rule>(factory.Rule1(r1.Node, r2.Node), r2.State, inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		// Selection: (rule-head-prefix `-- class-identifier `-- `<EOL>) | (rule-head-prefix `-- `<EOL>)
		private static ParseResult<RuleHead> ParseRule_RuleHead(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_RuleHead != null) { return state.Mem_ParseRule_RuleHead; }

			var r1 = ParseRule_RuleHead_C1(inputPosition, state, factory);
			if (r1 != null) return state.Mem_ParseRule_RuleHead = new ParseResult<RuleHead>(factory.RuleHead1(r1.Node.Item1, r1.Node.Item2), r1.State, r1.Advanced);
			var r2 = ParseRule_RuleHead_C2(inputPosition, state, factory);
			if (r2 != null) return state.Mem_ParseRule_RuleHead = new ParseResult<RuleHead>(factory.RuleHead2(r2.Node), r2.State, r2.Advanced);
			return null;
		}

		// Sequence: rule-head-prefix `-- class-identifier `-- `<EOL>
		private static ParseResult<(RuleHeadPrefix, ClassIdentifier)> ParseRule_RuleHead_C1(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseRule_RuleHeadPrefix(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			var r3 = ParseRule_ClassIdentifier(inputPosition + r1.Advanced + r2.Advanced, r2.State, factory);
			if (r3 == null) return null;
			var r4 = ParseIntrinsic_OptionalHorizontalWhitespace(inputPosition + r1.Advanced + r2.Advanced + r3.Advanced, r3.State, factory);
			if (r4 == null) return null;
			var r5 = ParseIntrinsic_EndOfLine(inputPosition + r1.Advanced + r2.Advanced + r3.Advanced + r4.Advanced, r4.State, factory);
			if (r5 == null) return null;
			return new ParseResult<(RuleHeadPrefix, ClassIdentifier)>((r1.Node, r3.Node), r5.State, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced + r4.Advanced + r5.Advanced - inputPosition);
		}

		// Sequence: rule-head-prefix `-- `<EOL>
		private static ParseResult<RuleHeadPrefix> ParseRule_RuleHead_C2(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseRule_RuleHeadPrefix(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			var r3 = ParseIntrinsic_EndOfLine(inputPosition + r1.Advanced + r2.Advanced, r2.State, factory);
			if (r3 == null) return null;
			return new ParseResult<RuleHeadPrefix>(r1.Node, r3.State, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Sequence: rule-identifier `-- `":"
		private static ParseResult<RuleHeadPrefix> ParseRule_RuleHeadPrefix(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_RuleHeadPrefix != null) { return state.Mem_ParseRule_RuleHeadPrefix; }

			var r1 = ParseRule_RuleIdentifier(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			var r3 = ParseLexeme(inputPosition + r1.Advanced + r2.Advanced, r2.State, ":");
			if (r3 == null) return null;
			return state.Mem_ParseRule_RuleHeadPrefix = new ParseResult<RuleHeadPrefix>(factory.RuleHeadPrefix1(r1.Node), r3.State, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Repetition: choice+
		private static ParseResult<RuleBody> ParseRule_RuleBody(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_RuleBody != null) { return state.Mem_ParseRule_RuleBody; }

			var result = ParsePlus(inputPosition, state, factory, (i, s, f) => ParseRule_Choice(i, s, f));
			if (result == null) return null;
			return state.Mem_ParseRule_RuleBody = new ParseResult<RuleBody>(factory.RuleBody1(result.Node), result.State, result.Advanced);
		}

		// Sequence: union `-- `<EOLOS>
		private static ParseResult<Choice> ParseRule_Choice(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_Choice != null) { return state.Mem_ParseRule_Choice; }

			var r1 = ParseRule_Union(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			var r3 = ParseIntrinsic_EndOfLineOrStream(inputPosition + r1.Advanced + r2.Advanced, r2.State, factory);
			if (r3 == null) return null;
			return state.Mem_ParseRule_Choice = new ParseResult<Choice>(factory.Choice1(r1.Node), r3.State, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Series: sequence/(-- "|" --)
		private static ParseResult<Union> ParseRule_Union(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_Union != null) { return state.Mem_ParseRule_Union; }

			var result = ParseSeries(inputPosition, state, factory, (i, s, f) => ParseRule_Sequence(i, s, f), (i, s, f) => ParseRule_Union_D(i, s, f));
			if (result == null) return null;
			return state.Mem_ParseRule_Union = new ParseResult<Union>(factory.Union1(result.Node), result.State, result.Advanced);
		}

		// Sequence: -- "|" --
		private static ParseResult<(String, String, String)> ParseRule_Union_D(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseIntrinsic_OptionalHorizontalWhitespace(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseLexeme(inputPosition + r1.Advanced, r1.State, "|");
			if (r2 == null) return null;
			var r3 = ParseIntrinsic_OptionalHorizontalWhitespace(inputPosition + r1.Advanced + r2.Advanced, r2.State, factory);
			if (r3 == null) return null;
			return new ParseResult<(String, String, String)>((r1.Node, r2.Node, r3.Node), r3.State, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Series: special/--
		private static ParseResult<Sequence> ParseRule_Sequence(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_Sequence != null) { return state.Mem_ParseRule_Sequence; }

			var result = ParseSeries(inputPosition, state, factory, (i, s, f) => ParseRule_Special(i, s, f), (i, s, f) => ParseIntrinsic_OptionalHorizontalWhitespace(i, s, f));
			if (result == null) return null;
			return state.Mem_ParseRule_Sequence = new ParseResult<Sequence>(factory.Sequence1(result.Node), result.State, result.Advanced);
		}

		// Selection: (token `"/" token) | segment
		private static ParseResult<Segment> ParseRule_Special(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_Special != null) { return state.Mem_ParseRule_Special; }

			var r1 = ParseRule_Special_C1(inputPosition, state, factory);
			if (r1 != null) return state.Mem_ParseRule_Special = new ParseResult<Segment>(factory.Special1(r1.Node.Item1, r1.Node.Item2), r1.State, r1.Advanced);
			var r2 = ParseRule_Segment(inputPosition, state, factory);
			if (r2 != null) return state.Mem_ParseRule_Special = new ParseResult<Segment>(factory.Special2(r2.Node), r2.State, r2.Advanced);
			return null;
		}

		// Sequence: token `"/" token
		private static ParseResult<(IToken, IToken)> ParseRule_Special_C1(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseRule_Token(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseLexeme(inputPosition + r1.Advanced, r1.State, "/");
			if (r2 == null) return null;
			var r3 = ParseRule_Token(inputPosition + r1.Advanced + r2.Advanced, r2.State, factory);
			if (r3 == null) return null;
			return new ParseResult<(IToken, IToken)>((r1.Node, r3.Node), r3.State, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Sequence: ("`" | "~" | "&")? cardinality
		private static ParseResult<Segment> ParseRule_Segment(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_Segment != null) { return state.Mem_ParseRule_Segment; }

			var r1 = ParseRule_Segment_S1(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseRule_Cardinality(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			return state.Mem_ParseRule_Segment = new ParseResult<Segment>(factory.Segment1(r1.Node, r2.Node), r2.State, inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		// Repetition: ("`" | "~" | "&")?
		private static ParseResult<String> ParseRule_Segment_S1(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var result = ParseMaybe(inputPosition, state, factory, (i, s, f) => ParseRule_Segment_S1_M(i, s, f));
			if (result == null) return null;
			return new ParseResult<String>(result.Node, result.State, result.Advanced);
		}

		// Selection: "`" | "~" | "&"
		private static ParseResult<String> ParseRule_Segment_S1_M(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseLexeme(inputPosition, state, "`");
			if (r1 != null) return new ParseResult<String>(r1.Node, r1.State, r1.Advanced);
			var r2 = ParseLexeme(inputPosition, state, "~");
			if (r2 != null) return new ParseResult<String>(r2.Node, r2.State, r2.Advanced);
			var r3 = ParseLexeme(inputPosition, state, "&");
			if (r3 != null) return new ParseResult<String>(r3.Node, r3.State, r3.Advanced);
			return null;
		}

		// Sequence: token ("+" | "?" | "*")?
		private static ParseResult<TokenCardinality> ParseRule_Cardinality(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_Cardinality != null) { return state.Mem_ParseRule_Cardinality; }

			var r1 = ParseRule_Token(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseRule_Cardinality_S2(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			return state.Mem_ParseRule_Cardinality = new ParseResult<TokenCardinality>(factory.Cardinality1(r1.Node, r2.Node), r2.State, inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		// Repetition: ("+" | "?" | "*")?
		private static ParseResult<String> ParseRule_Cardinality_S2(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var result = ParseMaybe(inputPosition, state, factory, (i, s, f) => ParseRule_Cardinality_S2_M(i, s, f));
			if (result == null) return null;
			return new ParseResult<String>(result.Node, result.State, result.Advanced);
		}

		// Selection: "+" | "?" | "*"
		private static ParseResult<String> ParseRule_Cardinality_S2_M(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseLexeme(inputPosition, state, "+");
			if (r1 != null) return new ParseResult<String>(r1.Node, r1.State, r1.Advanced);
			var r2 = ParseLexeme(inputPosition, state, "?");
			if (r2 != null) return new ParseResult<String>(r2.Node, r2.State, r2.Advanced);
			var r3 = ParseLexeme(inputPosition, state, "*");
			if (r3 != null) return new ParseResult<String>(r3.Node, r3.State, r3.Advanced);
			return null;
		}

		// Selection: (`".") | <CSTRING> | rule-identifier | ((`"<" intrinsic-identifier `">") | "--") | (`"(" union `")")
		private static ParseResult<IToken> ParseRule_Token(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_Token != null) { return state.Mem_ParseRule_Token; }

			var r1 = ParseRule_Token_C1(inputPosition, state, factory);
			if (r1 != null) return state.Mem_ParseRule_Token = new ParseResult<IToken>(factory.Token1(), r1.State, r1.Advanced);
			var r2 = ParseIntrinsic_CString(inputPosition, state, factory);
			if (r2 != null) return state.Mem_ParseRule_Token = new ParseResult<IToken>(factory.Token2(r2.Node), r2.State, r2.Advanced);
			var r3 = ParseRule_RuleIdentifier(inputPosition, state, factory);
			if (r3 != null) return state.Mem_ParseRule_Token = new ParseResult<IToken>(factory.Token3(r3.Node), r3.State, r3.Advanced);
			var r4 = ParseRule_Token_C4(inputPosition, state, factory);
			if (r4 != null) return state.Mem_ParseRule_Token = new ParseResult<IToken>(factory.Token4(r4.Node), r4.State, r4.Advanced);
			var r5 = ParseRule_Token_C5(inputPosition, state, factory);
			if (r5 != null) return state.Mem_ParseRule_Token = new ParseResult<IToken>(factory.Token5(r5.Node), r5.State, r5.Advanced);
			return null;
		}

		// Sequence: `"."
		private static ParseResult<EmptyNode> ParseRule_Token_C1(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseLexeme(inputPosition, state, ".");
			if (r1 == null) return null;
			return new ParseResult<EmptyNode>(EmptyNode.Instance, r1.State, inputPosition + r1.Advanced - inputPosition);
		}

		// Selection: (`"<" intrinsic-identifier `">") | "--"
		private static ParseResult<String> ParseRule_Token_C4(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseRule_Token_C4_C1(inputPosition, state, factory);
			if (r1 != null) return new ParseResult<String>(r1.Node, r1.State, r1.Advanced);
			var r2 = ParseLexeme(inputPosition, state, "--");
			if (r2 != null) return new ParseResult<String>(r2.Node, r2.State, r2.Advanced);
			return null;
		}

		// Sequence: `"<" intrinsic-identifier `">"
		private static ParseResult<String> ParseRule_Token_C4_C1(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseLexeme(inputPosition, state, "<");
			if (r1 == null) return null;
			var r2 = ParseRule_IntrinsicIdentifier(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			var r3 = ParseLexeme(inputPosition + r1.Advanced + r2.Advanced, r2.State, ">");
			if (r3 == null) return null;
			return new ParseResult<String>(r2.Node, r3.State, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Sequence: `"(" union `")"
		private static ParseResult<Union> ParseRule_Token_C5(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseLexeme(inputPosition, state, "(");
			if (r1 == null) return null;
			var r2 = ParseRule_Union(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			var r3 = ParseLexeme(inputPosition + r1.Advanced + r2.Advanced, r2.State, ")");
			if (r3 == null) return null;
			return new ParseResult<Union>(r2.Node, r3.State, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Sequence: <Aa> (<Aa> | "-")*
		private static ParseResult<RuleIdentifier> ParseRule_RuleIdentifier(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_RuleIdentifier != null) { return state.Mem_ParseRule_RuleIdentifier; }

			var r1 = ParseIntrinsic_AnyLetter(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseRule_RuleIdentifier_S2(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			return state.Mem_ParseRule_RuleIdentifier = new ParseResult<RuleIdentifier>(factory.RuleIdentifier1(r1.Node, r2.Node), r2.State, inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		// Repetition: (<Aa> | "-")*
		private static ParseResult<IReadOnlyList<String>> ParseRule_RuleIdentifier_S2(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var result = ParseStar(inputPosition, state, factory, (i, s, f) => ParseRule_RuleIdentifier_S2_M(i, s, f));
			if (result == null) return null;
			return new ParseResult<IReadOnlyList<String>>(result.Node, result.State, result.Advanced);
		}

		// Selection: <Aa> | "-"
		private static ParseResult<String> ParseRule_RuleIdentifier_S2_M(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseIntrinsic_AnyLetter(inputPosition, state, factory);
			if (r1 != null) return new ParseResult<String>(r1.Node, r1.State, r1.Advanced);
			var r2 = ParseLexeme(inputPosition, state, "-");
			if (r2 != null) return new ParseResult<String>(r2.Node, r2.State, r2.Advanced);
			return null;
		}

		// Series: csharp-identifier/"."
		private static ParseResult<ClassIdentifier> ParseRule_ClassIdentifier(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_ClassIdentifier != null) { return state.Mem_ParseRule_ClassIdentifier; }

			var result = ParseSeries(inputPosition, state, factory, (i, s, f) => ParseRule_CsharpIdentifier(i, s, f), (i, s, f) => ParseLexeme(i, s, "."));
			if (result == null) return null;
			return state.Mem_ParseRule_ClassIdentifier = new ParseResult<ClassIdentifier>(factory.ClassIdentifier1(result.Node), result.State, result.Advanced);
		}

		// Sequence: <Aa> (<Aa> | <#>)*
		private static ParseResult<String> ParseRule_CsharpIdentifier(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_CsharpIdentifier != null) { return state.Mem_ParseRule_CsharpIdentifier; }

			var r1 = ParseIntrinsic_AnyLetter(inputPosition, state, factory);
			if (r1 == null) return null;
			var r2 = ParseRule_CsharpIdentifier_S2(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			return state.Mem_ParseRule_CsharpIdentifier = new ParseResult<String>(factory.CsharpIdentifier1(r1.Node, r2.Node), r2.State, inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		// Repetition: (<Aa> | <#>)*
		private static ParseResult<IReadOnlyList<String>> ParseRule_CsharpIdentifier_S2(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var result = ParseStar(inputPosition, state, factory, (i, s, f) => ParseRule_CsharpIdentifier_S2_M(i, s, f));
			if (result == null) return null;
			return new ParseResult<IReadOnlyList<String>>(result.Node, result.State, result.Advanced);
		}

		// Selection: <Aa> | <#>
		private static ParseResult<String> ParseRule_CsharpIdentifier_S2_M(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseIntrinsic_AnyLetter(inputPosition, state, factory);
			if (r1 != null) return new ParseResult<String>(r1.Node, r1.State, r1.Advanced);
			var r2 = ParseIntrinsic_AnyDigit(inputPosition, state, factory);
			if (r2 != null) return new ParseResult<String>(r2.Node, r2.State, r2.Advanced);
			return null;
		}

		// Selection: <Aa>+ | "#"
		private static ParseResult<String> ParseRule_IntrinsicIdentifier(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_IntrinsicIdentifier != null) { return state.Mem_ParseRule_IntrinsicIdentifier; }

			var r1 = ParseRule_IntrinsicIdentifier_C1(inputPosition, state, factory);
			if (r1 != null) return state.Mem_ParseRule_IntrinsicIdentifier = new ParseResult<String>(factory.IntrinsicIdentifier1(r1.Node), r1.State, r1.Advanced);
			var r2 = ParseLexeme(inputPosition, state, "#");
			if (r2 != null) return state.Mem_ParseRule_IntrinsicIdentifier = new ParseResult<String>(factory.IntrinsicIdentifier2(r2.Node), r2.State, r2.Advanced);
			return null;
		}

		// Repetition: <Aa>+
		private static ParseResult<IReadOnlyList<String>> ParseRule_IntrinsicIdentifier_C1(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var result = ParsePlus(inputPosition, state, factory, (i, s, f) => ParseIntrinsic_AnyLetter(i, s, f));
			if (result == null) return null;
			return new ParseResult<IReadOnlyList<String>>(result.Node, result.State, result.Advanced);
		}

		// Sequence: `"//" (~<EOLOS> .)* `<EOLOS>
		private static ParseResult<String> ParseRule_Comment(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			if (state.Mem_ParseRule_Comment != null) { return state.Mem_ParseRule_Comment; }

			var r1 = ParseLexeme(inputPosition, state, "//");
			if (r1 == null) return null;
			var r2 = ParseRule_Comment_S2(inputPosition + r1.Advanced, r1.State, factory);
			if (r2 == null) return null;
			var r3 = ParseIntrinsic_EndOfLineOrStream(inputPosition + r1.Advanced + r2.Advanced, r2.State, factory);
			if (r3 == null) return null;
			return state.Mem_ParseRule_Comment = new ParseResult<String>(factory.Comment1(r2.Node), r3.State, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Repetition: (~<EOLOS> .)*
		private static ParseResult<IReadOnlyList<String>> ParseRule_Comment_S2(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var result = ParseStar(inputPosition, state, factory, (i, s, f) => ParseRule_Comment_S2_M(i, s, f));
			if (result == null) return null;
			return new ParseResult<IReadOnlyList<String>>(result.Node, result.State, result.Advanced);
		}

		// Sequence: ~<EOLOS> .
		private static ParseResult<String> ParseRule_Comment_S2_M(Int32 inputPosition, PackratState state, IParsnipRuleFactory factory)
		{
			var r1 = ParseIntrinsic_EndOfLineOrStream(inputPosition, state, factory);
			if (r1 != null) return null;
			var r2 = ParseIntrinsic_AnyCharacter(inputPosition + r1.Advanced, state, factory);
			if (r2 == null) return null;
			return new ParseResult<String>(r2.Node, r2.State, inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		private class PackratState
		{
			internal readonly string input;
			internal readonly PackratState[] states;

			public PackratState(String input, PackratState[] states)
			{
				this.input = input;
				this.states = states;
			}

			internal ParseResult<ParsnipDefinition> Mem_ParseRule_Definition;
			internal ParseResult<IParsnipDefinitionItem> Mem_ParseRule_DefinitionItem;
			internal ParseResult<Rule> Mem_ParseRule_Rule;
			internal ParseResult<RuleHead> Mem_ParseRule_RuleHead;
			internal ParseResult<RuleHeadPrefix> Mem_ParseRule_RuleHeadPrefix;
			internal ParseResult<RuleBody> Mem_ParseRule_RuleBody;
			internal ParseResult<Choice> Mem_ParseRule_Choice;
			internal ParseResult<Union> Mem_ParseRule_Union;
			internal ParseResult<Sequence> Mem_ParseRule_Sequence;
			internal ParseResult<Segment> Mem_ParseRule_Special;
			internal ParseResult<Segment> Mem_ParseRule_Segment;
			internal ParseResult<TokenCardinality> Mem_ParseRule_Cardinality;
			internal ParseResult<IToken> Mem_ParseRule_Token;
			internal ParseResult<RuleIdentifier> Mem_ParseRule_RuleIdentifier;
			internal ParseResult<ClassIdentifier> Mem_ParseRule_ClassIdentifier;
			internal ParseResult<String> Mem_ParseRule_CsharpIdentifier;
			internal ParseResult<String> Mem_ParseRule_IntrinsicIdentifier;
			internal ParseResult<String> Mem_ParseRule_Comment;
		}
	}
}

