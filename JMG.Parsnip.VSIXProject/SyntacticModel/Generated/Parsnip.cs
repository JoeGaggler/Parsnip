// Code Generated via Parsnip Packrat Parser Producer
// Version: 1.20.0.0
// File: Parsnip.parsnip
// Date: 2019-11-17 13:47:44

using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JMG.Parsnip.VSIXProject.SyntacticModel.Generated
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
		private class ParseResult<T> { public T Node; public PackratState State; }

		private class EmptyNode { private EmptyNode() { } public static EmptyNode Instance = new EmptyNode(); }

		public static ParsnipDefinition Parse(String input, IParsnipRuleFactory factory)
		{
			var states = new PackratState[input.Length + 1];
			Enumerable.Range(0, input.Length + 1).ToList().ForEach(i => states[i] = new PackratState(input, i, states, factory));
			var state = states[0];
			var result = PackratState.ParseRule_Definition(state, factory);
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

			private static ParseResult<IReadOnlyList<T>> ParseSeries<T, D>(PackratState state, IParsnipRuleFactory factory, Func<PackratState, IParsnipRuleFactory, ParseResult<T>> parseAction, Func<PackratState, IParsnipRuleFactory, ParseResult<D>> parseDelimiterAction)
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
					var delimResult = parseDelimiterAction(state, factory);
					if (delimResult == null)
					{
						break;
					}
					var nextResult = parseAction(delimResult.State, factory);
					if (nextResult == null)
					{
						break;
					}
					list.Add(nextResult.Node);
					state = nextResult.State;
				}
				return new ParseResult<IReadOnlyList<T>> { State = state, Node = list };
			}

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

			private static ParseResult<String> ParseIntrinsic_AnyDigit(PackratState state, IParsnipRuleFactory factory)
			{
				var input = state.input;
				var inputPosition = state.inputPosition;
				if (inputPosition >= input.Length)
				{
					return null;
				}
				else if (!Char.IsDigit(input[inputPosition]))
				{
					return null;
				}
				return new ParseResult<String>() { Node = state.input.Substring(inputPosition, 1), State = state.states[inputPosition + 1] };
			}

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

			private static ParseResult<EmptyNode> ParseIntrinsic_EndOfLineOrStream(PackratState state, IParsnipRuleFactory factory)
			{
				var input = state.input;
				var inputPosition = state.inputPosition;
				if (inputPosition == input.Length)
				{
					return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = state };
				}
				var result1 = ParseLexeme(state, "\r\n");
				if (result1 != null)
				{
					return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = result1.State };
				}
				var result2 = ParseLexeme(state, "\n");
				if (result2 != null)
				{
					return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = result2.State };
				}
				return null;
			}

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

			private static ParseResult<String> ParseIntrinsic_OptionalHorizontalWhitespace(PackratState state, IParsnipRuleFactory factory)
			{
				var input = state.input;
				var inputPosition = state.inputPosition;
				if (inputPosition >= input.Length)
				{
					return new ParseResult<String>() { Node = String.Empty, State = state };
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
				return new ParseResult<String>() { Node = state.input.Substring(inputPosition, nextInputPosition - inputPosition), State = state.states[nextInputPosition] };
			}

			private static ParseResult<String> ParseLexeme(PackratState state, String lexeme)
			{
				var lexemeLength = lexeme.Length;
				if (state.inputPosition + lexemeLength > state.input.Length) return null;
				if (state.input.Substring(state.inputPosition, lexemeLength) != lexeme) return null;
				return new ParseResult<String>() { Node = lexeme, State = state.states[state.inputPosition + lexemeLength] };
			}

			// Repetition: definition-item+
			private ParseResult<ParsnipDefinition> Mem_ParseRule_Definition;
			public static ParseResult<ParsnipDefinition> ParseRule_Definition(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_Definition != null) { return state.Mem_ParseRule_Definition; }

				var result = ParsePlus(state, factory, (s, f) => ParseRule_DefinitionItem(s, f));
				if (result == null) return null;
				return state.Mem_ParseRule_Definition = new ParseResult<ParsnipDefinition>() { Node = factory.Definition1(result.Node), State = result.State };
			}

			// Selection: rule | comment | (`<EOL>)
			private ParseResult<IParsnipDefinitionItem> Mem_ParseRule_DefinitionItem;
			private static ParseResult<IParsnipDefinitionItem> ParseRule_DefinitionItem(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_DefinitionItem != null) { return state.Mem_ParseRule_DefinitionItem; }

				var r1 = ParseRule_Rule(state, factory);
				if (r1 != null) return state.Mem_ParseRule_DefinitionItem = new ParseResult<IParsnipDefinitionItem>() { Node = factory.DefinitionItem1(r1.Node), State = r1.State };
				var r2 = ParseRule_Comment(state, factory);
				if (r2 != null) return state.Mem_ParseRule_DefinitionItem = new ParseResult<IParsnipDefinitionItem>() { Node = factory.DefinitionItem2(r2.Node), State = r2.State };
				var r3 = ParseRule_DefinitionItem_C3(state, factory);
				if (r3 != null) return state.Mem_ParseRule_DefinitionItem = new ParseResult<IParsnipDefinitionItem>() { Node = factory.DefinitionItem3(), State = r3.State };
				return null;
			}

			// Sequence: `<EOL>
			private static ParseResult<EmptyNode> ParseRule_DefinitionItem_C3(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseIntrinsic_EndOfLine(state, factory);
				if (r1 == null) return null;
				return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
			}

			// Sequence: rule-head rule-body
			private ParseResult<Rule> Mem_ParseRule_Rule;
			private static ParseResult<Rule> ParseRule_Rule(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_Rule != null) { return state.Mem_ParseRule_Rule; }

				var r1 = ParseRule_RuleHead(state, factory);
				if (r1 == null) return null;
				var r2 = ParseRule_RuleBody(r1.State, factory);
				if (r2 == null) return null;
				return state.Mem_ParseRule_Rule = new ParseResult<Rule>() { Node = factory.Rule1(r1.Node, r2.Node), State = r2.State };
			}

			// Selection: (rule-head-prefix `-- class-identifier `-- `<EOL>) | (rule-head-prefix `-- `<EOL>)
			private ParseResult<RuleHead> Mem_ParseRule_RuleHead;
			private static ParseResult<RuleHead> ParseRule_RuleHead(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_RuleHead != null) { return state.Mem_ParseRule_RuleHead; }

				var r1 = ParseRule_RuleHead_C1(state, factory);
				if (r1 != null) return state.Mem_ParseRule_RuleHead = new ParseResult<RuleHead>() { Node = factory.RuleHead1(r1.Node.Item1, r1.Node.Item2), State = r1.State };
				var r2 = ParseRule_RuleHead_C2(state, factory);
				if (r2 != null) return state.Mem_ParseRule_RuleHead = new ParseResult<RuleHead>() { Node = factory.RuleHead2(r2.Node), State = r2.State };
				return null;
			}

			// Sequence: rule-head-prefix `-- class-identifier `-- `<EOL>
			private static ParseResult<(RuleHeadPrefix, ClassIdentifier)> ParseRule_RuleHead_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseRule_RuleHeadPrefix(state, factory);
				if (r1 == null) return null;
				var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseRule_ClassIdentifier(r2.State, factory);
				if (r3 == null) return null;
				var r4 = ParseIntrinsic_OptionalHorizontalWhitespace(r3.State, factory);
				if (r4 == null) return null;
				var r5 = ParseIntrinsic_EndOfLine(r4.State, factory);
				if (r5 == null) return null;
				return new ParseResult<(RuleHeadPrefix, ClassIdentifier)>() { Node = (r1.Node, r3.Node), State = r5.State };
			}

			// Sequence: rule-head-prefix `-- `<EOL>
			private static ParseResult<RuleHeadPrefix> ParseRule_RuleHead_C2(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseRule_RuleHeadPrefix(state, factory);
				if (r1 == null) return null;
				var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseIntrinsic_EndOfLine(r2.State, factory);
				if (r3 == null) return null;
				return new ParseResult<RuleHeadPrefix>() { Node = r1.Node, State = r3.State };
			}

			// Sequence: rule-identifier `-- `":"
			private ParseResult<RuleHeadPrefix> Mem_ParseRule_RuleHeadPrefix;
			private static ParseResult<RuleHeadPrefix> ParseRule_RuleHeadPrefix(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_RuleHeadPrefix != null) { return state.Mem_ParseRule_RuleHeadPrefix; }

				var r1 = ParseRule_RuleIdentifier(state, factory);
				if (r1 == null) return null;
				var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseLexeme(r2.State, ":");
				if (r3 == null) return null;
				return state.Mem_ParseRule_RuleHeadPrefix = new ParseResult<RuleHeadPrefix>() { Node = factory.RuleHeadPrefix1(r1.Node), State = r3.State };
			}

			// Repetition: choice+
			private ParseResult<RuleBody> Mem_ParseRule_RuleBody;
			private static ParseResult<RuleBody> ParseRule_RuleBody(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_RuleBody != null) { return state.Mem_ParseRule_RuleBody; }

				var result = ParsePlus(state, factory, (s, f) => ParseRule_Choice(s, f));
				if (result == null) return null;
				return state.Mem_ParseRule_RuleBody = new ParseResult<RuleBody>() { Node = factory.RuleBody1(result.Node), State = result.State };
			}

			// Sequence: union `-- `<EOLOS>
			private ParseResult<Choice> Mem_ParseRule_Choice;
			private static ParseResult<Choice> ParseRule_Choice(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_Choice != null) { return state.Mem_ParseRule_Choice; }

				var r1 = ParseRule_Union(state, factory);
				if (r1 == null) return null;
				var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseIntrinsic_EndOfLineOrStream(r2.State, factory);
				if (r3 == null) return null;
				return state.Mem_ParseRule_Choice = new ParseResult<Choice>() { Node = factory.Choice1(r1.Node), State = r3.State };
			}

			// Series: sequence/(-- "|" --)
			private ParseResult<Union> Mem_ParseRule_Union;
			private static ParseResult<Union> ParseRule_Union(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_Union != null) { return state.Mem_ParseRule_Union; }

				var result = ParseSeries(state, factory, (s, f) => ParseRule_Sequence(s, f), (s, f) => ParseRule_Union_D(s, f));
				if (result == null) return null;
				return state.Mem_ParseRule_Union = new ParseResult<Union>() { Node = factory.Union1(result.Node), State = result.State };
			}

			// Sequence: -- "|" --
			private static ParseResult<(String, String, String)> ParseRule_Union_D(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseIntrinsic_OptionalHorizontalWhitespace(state, factory);
				if (r1 == null) return null;
				var r2 = ParseLexeme(r1.State, "|");
				if (r2 == null) return null;
				var r3 = ParseIntrinsic_OptionalHorizontalWhitespace(r2.State, factory);
				if (r3 == null) return null;
				return new ParseResult<(String, String, String)>() { Node = (r1.Node, r2.Node, r3.Node), State = r3.State };
			}

			// Series: special/--
			private ParseResult<Sequence> Mem_ParseRule_Sequence;
			private static ParseResult<Sequence> ParseRule_Sequence(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_Sequence != null) { return state.Mem_ParseRule_Sequence; }

				var result = ParseSeries(state, factory, (s, f) => ParseRule_Special(s, f), (s, f) => ParseIntrinsic_OptionalHorizontalWhitespace(s, f));
				if (result == null) return null;
				return state.Mem_ParseRule_Sequence = new ParseResult<Sequence>() { Node = factory.Sequence1(result.Node), State = result.State };
			}

			// Selection: (token `"/" token) | segment
			private ParseResult<Segment> Mem_ParseRule_Special;
			private static ParseResult<Segment> ParseRule_Special(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_Special != null) { return state.Mem_ParseRule_Special; }

				var r1 = ParseRule_Special_C1(state, factory);
				if (r1 != null) return state.Mem_ParseRule_Special = new ParseResult<Segment>() { Node = factory.Special1(r1.Node.Item1, r1.Node.Item2), State = r1.State };
				var r2 = ParseRule_Segment(state, factory);
				if (r2 != null) return state.Mem_ParseRule_Special = new ParseResult<Segment>() { Node = factory.Special2(r2.Node), State = r2.State };
				return null;
			}

			// Sequence: token `"/" token
			private static ParseResult<(IToken, IToken)> ParseRule_Special_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseRule_Token(state, factory);
				if (r1 == null) return null;
				var r2 = ParseLexeme(r1.State, "/");
				if (r2 == null) return null;
				var r3 = ParseRule_Token(r2.State, factory);
				if (r3 == null) return null;
				return new ParseResult<(IToken, IToken)>() { Node = (r1.Node, r3.Node), State = r3.State };
			}

			// Sequence: ("`" | "~" | "&")? cardinality
			private ParseResult<Segment> Mem_ParseRule_Segment;
			private static ParseResult<Segment> ParseRule_Segment(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_Segment != null) { return state.Mem_ParseRule_Segment; }

				var r1 = ParseRule_Segment_S1(state, factory);
				if (r1 == null) return null;
				var r2 = ParseRule_Cardinality(r1.State, factory);
				if (r2 == null) return null;
				return state.Mem_ParseRule_Segment = new ParseResult<Segment>() { Node = factory.Segment1(r1.Node, r2.Node), State = r2.State };
			}

			// Repetition: ("`" | "~" | "&")?
			private static ParseResult<String> ParseRule_Segment_S1(PackratState state, IParsnipRuleFactory factory)
			{
				var result = ParseMaybe(state, factory, (s, f) => ParseRule_Segment_S1_M(s, f));
				if (result == null) return null;
				return new ParseResult<String>() { Node = result.Node, State = result.State };
			}

			// Selection: "`" | "~" | "&"
			private static ParseResult<String> ParseRule_Segment_S1_M(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "`");
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				var r2 = ParseLexeme(state, "~");
				if (r2 != null) return new ParseResult<String>() { Node = r2.Node, State = r2.State };
				var r3 = ParseLexeme(state, "&");
				if (r3 != null) return new ParseResult<String>() { Node = r3.Node, State = r3.State };
				return null;
			}

			// Sequence: token ("+" | "?" | "*")?
			private ParseResult<TokenCardinality> Mem_ParseRule_Cardinality;
			private static ParseResult<TokenCardinality> ParseRule_Cardinality(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_Cardinality != null) { return state.Mem_ParseRule_Cardinality; }

				var r1 = ParseRule_Token(state, factory);
				if (r1 == null) return null;
				var r2 = ParseRule_Cardinality_S2(r1.State, factory);
				if (r2 == null) return null;
				return state.Mem_ParseRule_Cardinality = new ParseResult<TokenCardinality>() { Node = factory.Cardinality1(r1.Node, r2.Node), State = r2.State };
			}

			// Repetition: ("+" | "?" | "*")?
			private static ParseResult<String> ParseRule_Cardinality_S2(PackratState state, IParsnipRuleFactory factory)
			{
				var result = ParseMaybe(state, factory, (s, f) => ParseRule_Cardinality_S2_M(s, f));
				if (result == null) return null;
				return new ParseResult<String>() { Node = result.Node, State = result.State };
			}

			// Selection: "+" | "?" | "*"
			private static ParseResult<String> ParseRule_Cardinality_S2_M(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "+");
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				var r2 = ParseLexeme(state, "?");
				if (r2 != null) return new ParseResult<String>() { Node = r2.Node, State = r2.State };
				var r3 = ParseLexeme(state, "*");
				if (r3 != null) return new ParseResult<String>() { Node = r3.Node, State = r3.State };
				return null;
			}

			// Selection: (`".") | <CSTRING> | rule-identifier | ((`"<" intrinsic-identifier `">") | "--") | (`"(" union `")")
			private ParseResult<IToken> Mem_ParseRule_Token;
			private static ParseResult<IToken> ParseRule_Token(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_Token != null) { return state.Mem_ParseRule_Token; }

				var r1 = ParseRule_Token_C1(state, factory);
				if (r1 != null) return state.Mem_ParseRule_Token = new ParseResult<IToken>() { Node = factory.Token1(), State = r1.State };
				var r2 = ParseIntrinsic_CString(state, factory);
				if (r2 != null) return state.Mem_ParseRule_Token = new ParseResult<IToken>() { Node = factory.Token2(r2.Node), State = r2.State };
				var r3 = ParseRule_RuleIdentifier(state, factory);
				if (r3 != null) return state.Mem_ParseRule_Token = new ParseResult<IToken>() { Node = factory.Token3(r3.Node), State = r3.State };
				var r4 = ParseRule_Token_C4(state, factory);
				if (r4 != null) return state.Mem_ParseRule_Token = new ParseResult<IToken>() { Node = factory.Token4(r4.Node), State = r4.State };
				var r5 = ParseRule_Token_C5(state, factory);
				if (r5 != null) return state.Mem_ParseRule_Token = new ParseResult<IToken>() { Node = factory.Token5(r5.Node), State = r5.State };
				return null;
			}

			// Sequence: `"."
			private static ParseResult<EmptyNode> ParseRule_Token_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, ".");
				if (r1 == null) return null;
				return new ParseResult<EmptyNode>() { Node = EmptyNode.Instance, State = r1.State };
			}

			// Selection: (`"<" intrinsic-identifier `">") | "--"
			private static ParseResult<String> ParseRule_Token_C4(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseRule_Token_C4_C1(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				var r2 = ParseLexeme(state, "--");
				if (r2 != null) return new ParseResult<String>() { Node = r2.Node, State = r2.State };
				return null;
			}

			// Sequence: `"<" intrinsic-identifier `">"
			private static ParseResult<String> ParseRule_Token_C4_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "<");
				if (r1 == null) return null;
				var r2 = ParseRule_IntrinsicIdentifier(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseLexeme(r2.State, ">");
				if (r3 == null) return null;
				return new ParseResult<String>() { Node = r2.Node, State = r3.State };
			}

			// Sequence: `"(" union `")"
			private static ParseResult<Union> ParseRule_Token_C5(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseLexeme(state, "(");
				if (r1 == null) return null;
				var r2 = ParseRule_Union(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseLexeme(r2.State, ")");
				if (r3 == null) return null;
				return new ParseResult<Union>() { Node = r2.Node, State = r3.State };
			}

			// Sequence: <Aa> (<Aa> | "-")*
			private ParseResult<RuleIdentifier> Mem_ParseRule_RuleIdentifier;
			private static ParseResult<RuleIdentifier> ParseRule_RuleIdentifier(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_RuleIdentifier != null) { return state.Mem_ParseRule_RuleIdentifier; }

				var r1 = ParseIntrinsic_AnyLetter(state, factory);
				if (r1 == null) return null;
				var r2 = ParseRule_RuleIdentifier_S2(r1.State, factory);
				if (r2 == null) return null;
				return state.Mem_ParseRule_RuleIdentifier = new ParseResult<RuleIdentifier>() { Node = factory.RuleIdentifier1(r1.Node, r2.Node), State = r2.State };
			}

			// Repetition: (<Aa> | "-")*
			private static ParseResult<IReadOnlyList<String>> ParseRule_RuleIdentifier_S2(PackratState state, IParsnipRuleFactory factory)
			{
				var result = ParseStar(state, factory, (s, f) => ParseRule_RuleIdentifier_S2_M(s, f));
				if (result == null) return null;
				return new ParseResult<IReadOnlyList<String>>() { Node = result.Node, State = result.State };
			}

			// Selection: <Aa> | "-"
			private static ParseResult<String> ParseRule_RuleIdentifier_S2_M(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseIntrinsic_AnyLetter(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				var r2 = ParseLexeme(state, "-");
				if (r2 != null) return new ParseResult<String>() { Node = r2.Node, State = r2.State };
				return null;
			}

			// Series: csharp-identifier/"."
			private ParseResult<ClassIdentifier> Mem_ParseRule_ClassIdentifier;
			private static ParseResult<ClassIdentifier> ParseRule_ClassIdentifier(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_ClassIdentifier != null) { return state.Mem_ParseRule_ClassIdentifier; }

				var result = ParseSeries(state, factory, (s, f) => ParseRule_CsharpIdentifier(s, f), (s, f) => ParseLexeme(s, "."));
				if (result == null) return null;
				return state.Mem_ParseRule_ClassIdentifier = new ParseResult<ClassIdentifier>() { Node = factory.ClassIdentifier1(result.Node), State = result.State };
			}

			// Sequence: <Aa> (<Aa> | <#>)*
			private ParseResult<String> Mem_ParseRule_CsharpIdentifier;
			private static ParseResult<String> ParseRule_CsharpIdentifier(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_CsharpIdentifier != null) { return state.Mem_ParseRule_CsharpIdentifier; }

				var r1 = ParseIntrinsic_AnyLetter(state, factory);
				if (r1 == null) return null;
				var r2 = ParseRule_CsharpIdentifier_S2(r1.State, factory);
				if (r2 == null) return null;
				return state.Mem_ParseRule_CsharpIdentifier = new ParseResult<String>() { Node = factory.CsharpIdentifier1(r1.Node, r2.Node), State = r2.State };
			}

			// Repetition: (<Aa> | <#>)*
			private static ParseResult<IReadOnlyList<String>> ParseRule_CsharpIdentifier_S2(PackratState state, IParsnipRuleFactory factory)
			{
				var result = ParseStar(state, factory, (s, f) => ParseRule_CsharpIdentifier_S2_M(s, f));
				if (result == null) return null;
				return new ParseResult<IReadOnlyList<String>>() { Node = result.Node, State = result.State };
			}

			// Selection: <Aa> | <#>
			private static ParseResult<String> ParseRule_CsharpIdentifier_S2_M(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseIntrinsic_AnyLetter(state, factory);
				if (r1 != null) return new ParseResult<String>() { Node = r1.Node, State = r1.State };
				var r2 = ParseIntrinsic_AnyDigit(state, factory);
				if (r2 != null) return new ParseResult<String>() { Node = r2.Node, State = r2.State };
				return null;
			}

			// Selection: <Aa>+ | "#"
			private ParseResult<String> Mem_ParseRule_IntrinsicIdentifier;
			private static ParseResult<String> ParseRule_IntrinsicIdentifier(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_IntrinsicIdentifier != null) { return state.Mem_ParseRule_IntrinsicIdentifier; }

				var r1 = ParseRule_IntrinsicIdentifier_C1(state, factory);
				if (r1 != null) return state.Mem_ParseRule_IntrinsicIdentifier = new ParseResult<String>() { Node = factory.IntrinsicIdentifier1(r1.Node), State = r1.State };
				var r2 = ParseLexeme(state, "#");
				if (r2 != null) return state.Mem_ParseRule_IntrinsicIdentifier = new ParseResult<String>() { Node = factory.IntrinsicIdentifier2(r2.Node), State = r2.State };
				return null;
			}

			// Repetition: <Aa>+
			private static ParseResult<IReadOnlyList<String>> ParseRule_IntrinsicIdentifier_C1(PackratState state, IParsnipRuleFactory factory)
			{
				var result = ParsePlus(state, factory, (s, f) => ParseIntrinsic_AnyLetter(s, f));
				if (result == null) return null;
				return new ParseResult<IReadOnlyList<String>>() { Node = result.Node, State = result.State };
			}

			// Sequence: `"//" (~<EOLOS> .)* `<EOLOS>
			private ParseResult<String> Mem_ParseRule_Comment;
			private static ParseResult<String> ParseRule_Comment(PackratState state, IParsnipRuleFactory factory)
			{
				if (state.Mem_ParseRule_Comment != null) { return state.Mem_ParseRule_Comment; }

				var r1 = ParseLexeme(state, "//");
				if (r1 == null) return null;
				var r2 = ParseRule_Comment_S2(r1.State, factory);
				if (r2 == null) return null;
				var r3 = ParseIntrinsic_EndOfLineOrStream(r2.State, factory);
				if (r3 == null) return null;
				return state.Mem_ParseRule_Comment = new ParseResult<String>() { Node = factory.Comment1(r2.Node), State = r3.State };
			}

			// Repetition: (~<EOLOS> .)*
			private static ParseResult<IReadOnlyList<String>> ParseRule_Comment_S2(PackratState state, IParsnipRuleFactory factory)
			{
				var result = ParseStar(state, factory, (s, f) => ParseRule_Comment_S2_M(s, f));
				if (result == null) return null;
				return new ParseResult<IReadOnlyList<String>>() { Node = result.Node, State = result.State };
			}

			// Sequence: ~<EOLOS> .
			private static ParseResult<String> ParseRule_Comment_S2_M(PackratState state, IParsnipRuleFactory factory)
			{
				var r1 = ParseIntrinsic_EndOfLineOrStream(state, factory);
				if (r1 != null) return null;
				var r2 = ParseIntrinsic_AnyCharacter(state, factory);
				if (r2 == null) return null;
				return new ParseResult<String>() { Node = r2.Node, State = r2.State };
			}
		}
	}
}
