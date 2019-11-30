// Code Generated via Parsnip Packrat Parser Producer
// Version: 1.23
// Date: 2019-11-29 22:06:56

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
			public readonly Int32 Advanced;

			public ParseResult(T node, Int32 advanced)
			{
				this.Node = node;
				this.Advanced = advanced;
			}
		}

		private class EmptyNode { private EmptyNode() { } public static EmptyNode Instance = new EmptyNode(); }

		public static ParsnipDefinition Parse(String input, IParsnipRuleFactory factory)
		{
			var states = new PackratState[input.Length + 1];
			Enumerable.Range(0, input.Length + 1).ToList().ForEach(i => states[i] = new PackratState());
			var result = ParseRule_Definition(input, 0, states, factory);
			return result?.Node;
		}


		private static ParseResult<T> ParseMaybe<T>(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory, Func<String, Int32, PackratState[], IParsnipRuleFactory, ParseResult<T>> parseAction)
		{
			var result = parseAction(input, inputPosition, states, factory);
			if (result != null)
			{
				return result;
			}
			return new ParseResult<T>(default(T), 0);
		}

		private static ParseResult<IReadOnlyList<T>> ParseStar<T>(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory, Func<String, Int32, PackratState[], IParsnipRuleFactory, ParseResult<T>> parseAction)
		{
			var list = new List<T>();
			var nextResultInputPosition = inputPosition;
			while (true)
			{
				var nextResult = parseAction(input, nextResultInputPosition, states, factory);
				if (nextResult == null)
				{
					break;
				}
				list.Add(nextResult.Node);
				nextResultInputPosition = nextResultInputPosition + nextResult.Advanced;
			}
			return new ParseResult<IReadOnlyList<T>>(list, nextResultInputPosition - inputPosition);
		}

		private static ParseResult<IReadOnlyList<T>> ParsePlus<T>(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory, Func<String, Int32, PackratState[], IParsnipRuleFactory, ParseResult<T>> parseAction)
		{
			var list = new List<T>();
			var nextResultInputPosition = inputPosition;
			var firstResult = parseAction(input, nextResultInputPosition, states, factory);
			if (firstResult == null)
			{
				return null;
			}
			list.Add(firstResult.Node);
			nextResultInputPosition = nextResultInputPosition + firstResult.Advanced;
			while (true)
			{
				var nextResult = parseAction(input, nextResultInputPosition, states, factory);
				if (nextResult == null)
				{
					break;
				}
				list.Add(nextResult.Node);
				nextResultInputPosition = nextResultInputPosition + nextResult.Advanced;
			}
			return new ParseResult<IReadOnlyList<T>>(list, nextResultInputPosition - inputPosition);
		}

		private static ParseResult<IReadOnlyList<T>> ParseSeries<T, D>(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory, Func<String, Int32, PackratState[], IParsnipRuleFactory, ParseResult<T>> parseAction, Func<String, Int32, PackratState[], IParsnipRuleFactory, ParseResult<D>> parseDelimiterAction)
		{
			var list = new List<T>();
			var nextResultInputPosition = inputPosition;
			var firstResult = parseAction(input, nextResultInputPosition, states, factory);
			if (firstResult == null)
			{
				return null;
			}
			nextResultInputPosition += firstResult.Advanced;
			list.Add(firstResult.Node);
			while (true)
			{
				var delimResult = parseDelimiterAction(input, nextResultInputPosition, states, factory);
				if (delimResult == null)
				{
					break;
				}
				var nextResult = parseAction(input, nextResultInputPosition + delimResult.Advanced, states, factory);
				if (nextResult == null)
				{
					break;
				}
				list.Add(nextResult.Node);
				nextResultInputPosition = nextResultInputPosition + delimResult.Advanced + nextResult.Advanced;
			}
			return new ParseResult<IReadOnlyList<T>>(list, nextResultInputPosition - inputPosition);
		}

		private static ParseResult<String> ParseIntrinsic_AnyCharacter(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (inputPosition >= input.Length)
			{
				return null;
			}
			return new ParseResult<String>(input.Substring(inputPosition, 1), 1);
		}

		private static ParseResult<String> ParseIntrinsic_AnyLetter(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (inputPosition >= input.Length)
			{
				return null;
			}
			else if (!Char.IsLetter(input[inputPosition]))
			{
				return null;
			}
			return new ParseResult<String>(input.Substring(inputPosition, 1), 1);
		}

		private static ParseResult<String> ParseIntrinsic_AnyDigit(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (inputPosition >= input.Length)
			{
				return null;
			}
			else if (!Char.IsDigit(input[inputPosition]))
			{
				return null;
			}
			return new ParseResult<String>(input.Substring(inputPosition, 1), 1);
		}

		private static ParseResult<String> ParseIntrinsic_EndOfLine(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var result1 = ParseLexeme(input, inputPosition, "\r\n");
			if (result1 != null)
			{
				return new ParseResult<String>(result1.Node, result1.Advanced);
			}
			var result2 = ParseLexeme(input, inputPosition, "\n");
			if (result2 != null)
			{
				return new ParseResult<String>(result2.Node, result2.Advanced);
			}
			return null;
		}

		private static ParseResult<EmptyNode> ParseIntrinsic_EndOfStream(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (inputPosition == input.Length)
			{
				return new ParseResult<EmptyNode>(EmptyNode.Instance, 0);
			}
			return null;
		}

		private static ParseResult<EmptyNode> ParseIntrinsic_EndOfLineOrStream(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (inputPosition == input.Length)
			{
				return new ParseResult<EmptyNode>(EmptyNode.Instance, 0);
			}
			var result1 = ParseLexeme(input, inputPosition, "\r\n");
			if (result1 != null)
			{
				return new ParseResult<EmptyNode>(EmptyNode.Instance, result1.Advanced);
			}
			var result2 = ParseLexeme(input, inputPosition, "\n");
			if (result2 != null)
			{
				return new ParseResult<EmptyNode>(EmptyNode.Instance, result2.Advanced);
			}
			return null;
		}

		private static ParseResult<String> ParseIntrinsic_CString(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var resultStart = ParseLexeme(input, inputPosition, "\"");
			if (resultStart == null) return null;
			var nextInputPosition = inputPosition + resultStart.Advanced;
			var sb = new System.Text.StringBuilder();
			while (true)
			{
				var inputPosition2 = nextInputPosition;
				var resultEscape = ParseLexeme(input, inputPosition2, "\\");
				if (resultEscape != null)
				{
					inputPosition2 = inputPosition2 + resultEscape.Advanced;
					var resultToken = ParseIntrinsic_AnyCharacter(input, inputPosition2, states, factory);
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
					inputPosition2 = inputPosition2 + resultToken.Advanced;
					continue;
				}
				var resultEnd = ParseLexeme(input, inputPosition2, "\"");
				if (resultEnd != null)
				{
					return new ParseResult<String>(sb.ToString(), inputPosition2 + resultEnd.Advanced - inputPosition);
				}
				var resultChar = ParseIntrinsic_AnyCharacter(input, inputPosition2, states, factory);
				if (resultChar == null) return null;
				sb.Append(resultChar.Node);
				nextInputPosition = inputPosition2 + resultChar.Advanced;
			}
		}

		private static ParseResult<String> ParseIntrinsic_OptionalHorizontalWhitespace(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (inputPosition >= input.Length)
			{
				return new ParseResult<String>(String.Empty, 0);
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
			return new ParseResult<String>(input.Substring(inputPosition, nextInputPosition - inputPosition), nextInputPosition - inputPosition);
		}

		private static ParseResult<String> ParseLexeme(String input, Int32 inputPosition, String lexeme)
		{
			var lexemeLength = lexeme.Length;
			if (inputPosition + lexemeLength > input.Length) return null;
			if (input.Substring(inputPosition, lexemeLength) != lexeme) return null;
			return new ParseResult<String>(lexeme, lexemeLength);
		}

		// Repetition: definition-item+
		private static ParseResult<ParsnipDefinition> ParseRule_Definition(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_Definition is var mem && mem != null) { return mem; }

			var result = ParsePlus(input, inputPosition, states, factory, (i, p, s, f) => ParseRule_DefinitionItem(i, p, s, f));
			if (result == null) return null;
			return states[inputPosition].Mem_ParseRule_Definition = new ParseResult<ParsnipDefinition>(factory.Definition1(result.Node), result.Advanced);
		}

		// Selection: rule | comment | (`<EOL>)
		private static ParseResult<IParsnipDefinitionItem> ParseRule_DefinitionItem(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_DefinitionItem is var mem && mem != null) { return mem; }

			var r1 = ParseRule_Rule(input, inputPosition, states, factory);
			if (r1 != null) return states[inputPosition].Mem_ParseRule_DefinitionItem = new ParseResult<IParsnipDefinitionItem>(factory.DefinitionItem1(r1.Node), r1.Advanced);
			var r2 = ParseRule_Comment(input, inputPosition, states, factory);
			if (r2 != null) return states[inputPosition].Mem_ParseRule_DefinitionItem = new ParseResult<IParsnipDefinitionItem>(factory.DefinitionItem2(r2.Node), r2.Advanced);
			var r3 = ParseRule_DefinitionItem_C3(input, inputPosition, states, factory);
			if (r3 != null) return states[inputPosition].Mem_ParseRule_DefinitionItem = new ParseResult<IParsnipDefinitionItem>(factory.DefinitionItem3(), r3.Advanced);
			return null;
		}

		// Sequence: `<EOL>
		private static ParseResult<EmptyNode> ParseRule_DefinitionItem_C3(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseIntrinsic_EndOfLine(input, inputPosition, states, factory);
			if (r1 == null) return null;
			return new ParseResult<EmptyNode>(EmptyNode.Instance, inputPosition + r1.Advanced - inputPosition);
		}

		// Sequence: rule-head rule-body
		private static ParseResult<Rule> ParseRule_Rule(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_Rule is var mem && mem != null) { return mem; }

			var r1 = ParseRule_RuleHead(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseRule_RuleBody(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			return states[inputPosition].Mem_ParseRule_Rule = new ParseResult<Rule>(factory.Rule1(r1.Node, r2.Node), inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		// Selection: (rule-head-prefix `-- class-identifier `-- `<EOL>) | (rule-head-prefix `-- `<EOL>)
		private static ParseResult<RuleHead> ParseRule_RuleHead(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_RuleHead is var mem && mem != null) { return mem; }

			var r1 = ParseRule_RuleHead_C1(input, inputPosition, states, factory);
			if (r1 != null) return states[inputPosition].Mem_ParseRule_RuleHead = new ParseResult<RuleHead>(factory.RuleHead1(r1.Node.Item1, r1.Node.Item2), r1.Advanced);
			var r2 = ParseRule_RuleHead_C2(input, inputPosition, states, factory);
			if (r2 != null) return states[inputPosition].Mem_ParseRule_RuleHead = new ParseResult<RuleHead>(factory.RuleHead2(r2.Node), r2.Advanced);
			return null;
		}

		// Sequence: rule-head-prefix `-- class-identifier `-- `<EOL>
		private static ParseResult<(RuleHeadPrefix, ClassIdentifier)> ParseRule_RuleHead_C1(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseRule_RuleHeadPrefix(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			var r3 = ParseRule_ClassIdentifier(input, inputPosition + r1.Advanced + r2.Advanced, states, factory);
			if (r3 == null) return null;
			var r4 = ParseIntrinsic_OptionalHorizontalWhitespace(input, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced, states, factory);
			if (r4 == null) return null;
			var r5 = ParseIntrinsic_EndOfLine(input, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced + r4.Advanced, states, factory);
			if (r5 == null) return null;
			return new ParseResult<(RuleHeadPrefix, ClassIdentifier)>((r1.Node, r3.Node), inputPosition + r1.Advanced + r2.Advanced + r3.Advanced + r4.Advanced + r5.Advanced - inputPosition);
		}

		// Sequence: rule-head-prefix `-- `<EOL>
		private static ParseResult<RuleHeadPrefix> ParseRule_RuleHead_C2(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseRule_RuleHeadPrefix(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			var r3 = ParseIntrinsic_EndOfLine(input, inputPosition + r1.Advanced + r2.Advanced, states, factory);
			if (r3 == null) return null;
			return new ParseResult<RuleHeadPrefix>(r1.Node, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Sequence: rule-identifier `-- `":"
		private static ParseResult<RuleHeadPrefix> ParseRule_RuleHeadPrefix(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_RuleHeadPrefix is var mem && mem != null) { return mem; }

			var r1 = ParseRule_RuleIdentifier(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			var r3 = ParseLexeme(input, inputPosition + r1.Advanced + r2.Advanced, ":");
			if (r3 == null) return null;
			return states[inputPosition].Mem_ParseRule_RuleHeadPrefix = new ParseResult<RuleHeadPrefix>(factory.RuleHeadPrefix1(r1.Node), inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Repetition: choice+
		private static ParseResult<RuleBody> ParseRule_RuleBody(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_RuleBody is var mem && mem != null) { return mem; }

			var result = ParsePlus(input, inputPosition, states, factory, (i, p, s, f) => ParseRule_Choice(i, p, s, f));
			if (result == null) return null;
			return states[inputPosition].Mem_ParseRule_RuleBody = new ParseResult<RuleBody>(factory.RuleBody1(result.Node), result.Advanced);
		}

		// Sequence: union `-- `<EOLOS>
		private static ParseResult<Choice> ParseRule_Choice(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_Choice is var mem && mem != null) { return mem; }

			var r1 = ParseRule_Union(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			var r3 = ParseIntrinsic_EndOfLineOrStream(input, inputPosition + r1.Advanced + r2.Advanced, states, factory);
			if (r3 == null) return null;
			return states[inputPosition].Mem_ParseRule_Choice = new ParseResult<Choice>(factory.Choice1(r1.Node), inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Series: sequence/(-- "|" --)
		private static ParseResult<Union> ParseRule_Union(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_Union is var mem && mem != null) { return mem; }

			var result = ParseSeries(input, inputPosition, states, factory, (i, p, s, f) => ParseRule_Sequence(i, p, s, f), (i, p, s, f) => ParseRule_Union_D(i, p, s, f));
			if (result == null) return null;
			return states[inputPosition].Mem_ParseRule_Union = new ParseResult<Union>(factory.Union1(result.Node), result.Advanced);
		}

		// Sequence: -- "|" --
		private static ParseResult<(String, String, String)> ParseRule_Union_D(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseIntrinsic_OptionalHorizontalWhitespace(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseLexeme(input, inputPosition + r1.Advanced, "|");
			if (r2 == null) return null;
			var r3 = ParseIntrinsic_OptionalHorizontalWhitespace(input, inputPosition + r1.Advanced + r2.Advanced, states, factory);
			if (r3 == null) return null;
			return new ParseResult<(String, String, String)>((r1.Node, r2.Node, r3.Node), inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Series: special/--
		private static ParseResult<Sequence> ParseRule_Sequence(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_Sequence is var mem && mem != null) { return mem; }

			var result = ParseSeries(input, inputPosition, states, factory, (i, p, s, f) => ParseRule_Special(i, p, s, f), (i, p, s, f) => ParseIntrinsic_OptionalHorizontalWhitespace(i, p, s, f));
			if (result == null) return null;
			return states[inputPosition].Mem_ParseRule_Sequence = new ParseResult<Sequence>(factory.Sequence1(result.Node), result.Advanced);
		}

		// Selection: (token `"/" token) | segment
		private static ParseResult<Segment> ParseRule_Special(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_Special is var mem && mem != null) { return mem; }

			var r1 = ParseRule_Special_C1(input, inputPosition, states, factory);
			if (r1 != null) return states[inputPosition].Mem_ParseRule_Special = new ParseResult<Segment>(factory.Special1(r1.Node.Item1, r1.Node.Item2), r1.Advanced);
			var r2 = ParseRule_Segment(input, inputPosition, states, factory);
			if (r2 != null) return states[inputPosition].Mem_ParseRule_Special = new ParseResult<Segment>(factory.Special2(r2.Node), r2.Advanced);
			return null;
		}

		// Sequence: token `"/" token
		private static ParseResult<(IToken, IToken)> ParseRule_Special_C1(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseRule_Token(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseLexeme(input, inputPosition + r1.Advanced, "/");
			if (r2 == null) return null;
			var r3 = ParseRule_Token(input, inputPosition + r1.Advanced + r2.Advanced, states, factory);
			if (r3 == null) return null;
			return new ParseResult<(IToken, IToken)>((r1.Node, r3.Node), inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Sequence: ("`" | "~" | "&")? cardinality
		private static ParseResult<Segment> ParseRule_Segment(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_Segment is var mem && mem != null) { return mem; }

			var r1 = ParseRule_Segment_S1(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseRule_Cardinality(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			return states[inputPosition].Mem_ParseRule_Segment = new ParseResult<Segment>(factory.Segment1(r1.Node, r2.Node), inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		// Repetition: ("`" | "~" | "&")?
		private static ParseResult<String> ParseRule_Segment_S1(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var result = ParseMaybe(input, inputPosition, states, factory, (i, p, s, f) => ParseRule_Segment_S1_M(i, p, s, f));
			if (result == null) return null;
			return new ParseResult<String>(result.Node, result.Advanced);
		}

		// Selection: "`" | "~" | "&"
		private static ParseResult<String> ParseRule_Segment_S1_M(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseLexeme(input, inputPosition, "`");
			if (r1 != null) return new ParseResult<String>(r1.Node, r1.Advanced);
			var r2 = ParseLexeme(input, inputPosition, "~");
			if (r2 != null) return new ParseResult<String>(r2.Node, r2.Advanced);
			var r3 = ParseLexeme(input, inputPosition, "&");
			if (r3 != null) return new ParseResult<String>(r3.Node, r3.Advanced);
			return null;
		}

		// Sequence: token ("+" | "?" | "*")?
		private static ParseResult<TokenCardinality> ParseRule_Cardinality(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_Cardinality is var mem && mem != null) { return mem; }

			var r1 = ParseRule_Token(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseRule_Cardinality_S2(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			return states[inputPosition].Mem_ParseRule_Cardinality = new ParseResult<TokenCardinality>(factory.Cardinality1(r1.Node, r2.Node), inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		// Repetition: ("+" | "?" | "*")?
		private static ParseResult<String> ParseRule_Cardinality_S2(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var result = ParseMaybe(input, inputPosition, states, factory, (i, p, s, f) => ParseRule_Cardinality_S2_M(i, p, s, f));
			if (result == null) return null;
			return new ParseResult<String>(result.Node, result.Advanced);
		}

		// Selection: "+" | "?" | "*"
		private static ParseResult<String> ParseRule_Cardinality_S2_M(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseLexeme(input, inputPosition, "+");
			if (r1 != null) return new ParseResult<String>(r1.Node, r1.Advanced);
			var r2 = ParseLexeme(input, inputPosition, "?");
			if (r2 != null) return new ParseResult<String>(r2.Node, r2.Advanced);
			var r3 = ParseLexeme(input, inputPosition, "*");
			if (r3 != null) return new ParseResult<String>(r3.Node, r3.Advanced);
			return null;
		}

		// Selection: (`".") | <CSTRING> | rule-identifier | ((`"<" intrinsic-identifier `">") | "--") | (`"(" union `")")
		private static ParseResult<IToken> ParseRule_Token(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_Token is var mem && mem != null) { return mem; }

			var r1 = ParseRule_Token_C1(input, inputPosition, states, factory);
			if (r1 != null) return states[inputPosition].Mem_ParseRule_Token = new ParseResult<IToken>(factory.Token1(), r1.Advanced);
			var r2 = ParseIntrinsic_CString(input, inputPosition, states, factory);
			if (r2 != null) return states[inputPosition].Mem_ParseRule_Token = new ParseResult<IToken>(factory.Token2(r2.Node), r2.Advanced);
			var r3 = ParseRule_RuleIdentifier(input, inputPosition, states, factory);
			if (r3 != null) return states[inputPosition].Mem_ParseRule_Token = new ParseResult<IToken>(factory.Token3(r3.Node), r3.Advanced);
			var r4 = ParseRule_Token_C4(input, inputPosition, states, factory);
			if (r4 != null) return states[inputPosition].Mem_ParseRule_Token = new ParseResult<IToken>(factory.Token4(r4.Node), r4.Advanced);
			var r5 = ParseRule_Token_C5(input, inputPosition, states, factory);
			if (r5 != null) return states[inputPosition].Mem_ParseRule_Token = new ParseResult<IToken>(factory.Token5(r5.Node), r5.Advanced);
			return null;
		}

		// Sequence: `"."
		private static ParseResult<EmptyNode> ParseRule_Token_C1(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseLexeme(input, inputPosition, ".");
			if (r1 == null) return null;
			return new ParseResult<EmptyNode>(EmptyNode.Instance, inputPosition + r1.Advanced - inputPosition);
		}

		// Selection: (`"<" intrinsic-identifier `">") | "--"
		private static ParseResult<String> ParseRule_Token_C4(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseRule_Token_C4_C1(input, inputPosition, states, factory);
			if (r1 != null) return new ParseResult<String>(r1.Node, r1.Advanced);
			var r2 = ParseLexeme(input, inputPosition, "--");
			if (r2 != null) return new ParseResult<String>(r2.Node, r2.Advanced);
			return null;
		}

		// Sequence: `"<" intrinsic-identifier `">"
		private static ParseResult<String> ParseRule_Token_C4_C1(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseLexeme(input, inputPosition, "<");
			if (r1 == null) return null;
			var r2 = ParseRule_IntrinsicIdentifier(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			var r3 = ParseLexeme(input, inputPosition + r1.Advanced + r2.Advanced, ">");
			if (r3 == null) return null;
			return new ParseResult<String>(r2.Node, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Sequence: `"(" union `")"
		private static ParseResult<Union> ParseRule_Token_C5(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseLexeme(input, inputPosition, "(");
			if (r1 == null) return null;
			var r2 = ParseRule_Union(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			var r3 = ParseLexeme(input, inputPosition + r1.Advanced + r2.Advanced, ")");
			if (r3 == null) return null;
			return new ParseResult<Union>(r2.Node, inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Sequence: <Aa> (<Aa> | "-")*
		private static ParseResult<RuleIdentifier> ParseRule_RuleIdentifier(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_RuleIdentifier is var mem && mem != null) { return mem; }

			var r1 = ParseIntrinsic_AnyLetter(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseRule_RuleIdentifier_S2(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			return states[inputPosition].Mem_ParseRule_RuleIdentifier = new ParseResult<RuleIdentifier>(factory.RuleIdentifier1(r1.Node, r2.Node), inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		// Repetition: (<Aa> | "-")*
		private static ParseResult<IReadOnlyList<String>> ParseRule_RuleIdentifier_S2(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var result = ParseStar(input, inputPosition, states, factory, (i, p, s, f) => ParseRule_RuleIdentifier_S2_M(i, p, s, f));
			if (result == null) return null;
			return new ParseResult<IReadOnlyList<String>>(result.Node, result.Advanced);
		}

		// Selection: <Aa> | "-"
		private static ParseResult<String> ParseRule_RuleIdentifier_S2_M(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseIntrinsic_AnyLetter(input, inputPosition, states, factory);
			if (r1 != null) return new ParseResult<String>(r1.Node, r1.Advanced);
			var r2 = ParseLexeme(input, inputPosition, "-");
			if (r2 != null) return new ParseResult<String>(r2.Node, r2.Advanced);
			return null;
		}

		// Series: csharp-identifier/"."
		private static ParseResult<ClassIdentifier> ParseRule_ClassIdentifier(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_ClassIdentifier is var mem && mem != null) { return mem; }

			var result = ParseSeries(input, inputPosition, states, factory, (i, p, s, f) => ParseRule_CsharpIdentifier(i, p, s, f), (i, p, s, f) => ParseLexeme(i, p, "."));
			if (result == null) return null;
			return states[inputPosition].Mem_ParseRule_ClassIdentifier = new ParseResult<ClassIdentifier>(factory.ClassIdentifier1(result.Node), result.Advanced);
		}

		// Sequence: <Aa> (<Aa> | <#>)*
		private static ParseResult<String> ParseRule_CsharpIdentifier(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_CsharpIdentifier is var mem && mem != null) { return mem; }

			var r1 = ParseIntrinsic_AnyLetter(input, inputPosition, states, factory);
			if (r1 == null) return null;
			var r2 = ParseRule_CsharpIdentifier_S2(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			return states[inputPosition].Mem_ParseRule_CsharpIdentifier = new ParseResult<String>(factory.CsharpIdentifier1(r1.Node, r2.Node), inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		// Repetition: (<Aa> | <#>)*
		private static ParseResult<IReadOnlyList<String>> ParseRule_CsharpIdentifier_S2(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var result = ParseStar(input, inputPosition, states, factory, (i, p, s, f) => ParseRule_CsharpIdentifier_S2_M(i, p, s, f));
			if (result == null) return null;
			return new ParseResult<IReadOnlyList<String>>(result.Node, result.Advanced);
		}

		// Selection: <Aa> | <#>
		private static ParseResult<String> ParseRule_CsharpIdentifier_S2_M(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseIntrinsic_AnyLetter(input, inputPosition, states, factory);
			if (r1 != null) return new ParseResult<String>(r1.Node, r1.Advanced);
			var r2 = ParseIntrinsic_AnyDigit(input, inputPosition, states, factory);
			if (r2 != null) return new ParseResult<String>(r2.Node, r2.Advanced);
			return null;
		}

		// Selection: <Aa>+ | "#"
		private static ParseResult<String> ParseRule_IntrinsicIdentifier(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_IntrinsicIdentifier is var mem && mem != null) { return mem; }

			var r1 = ParseRule_IntrinsicIdentifier_C1(input, inputPosition, states, factory);
			if (r1 != null) return states[inputPosition].Mem_ParseRule_IntrinsicIdentifier = new ParseResult<String>(factory.IntrinsicIdentifier1(r1.Node), r1.Advanced);
			var r2 = ParseLexeme(input, inputPosition, "#");
			if (r2 != null) return states[inputPosition].Mem_ParseRule_IntrinsicIdentifier = new ParseResult<String>(factory.IntrinsicIdentifier2(r2.Node), r2.Advanced);
			return null;
		}

		// Repetition: <Aa>+
		private static ParseResult<IReadOnlyList<String>> ParseRule_IntrinsicIdentifier_C1(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var result = ParsePlus(input, inputPosition, states, factory, (i, p, s, f) => ParseIntrinsic_AnyLetter(i, p, s, f));
			if (result == null) return null;
			return new ParseResult<IReadOnlyList<String>>(result.Node, result.Advanced);
		}

		// Sequence: `"//" (~<EOLOS> .)* `<EOLOS>
		private static ParseResult<String> ParseRule_Comment(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			if (states[inputPosition].Mem_ParseRule_Comment is var mem && mem != null) { return mem; }

			var r1 = ParseLexeme(input, inputPosition, "//");
			if (r1 == null) return null;
			var r2 = ParseRule_Comment_S2(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			var r3 = ParseIntrinsic_EndOfLineOrStream(input, inputPosition + r1.Advanced + r2.Advanced, states, factory);
			if (r3 == null) return null;
			return states[inputPosition].Mem_ParseRule_Comment = new ParseResult<String>(factory.Comment1(r2.Node), inputPosition + r1.Advanced + r2.Advanced + r3.Advanced - inputPosition);
		}

		// Repetition: (~<EOLOS> .)*
		private static ParseResult<IReadOnlyList<String>> ParseRule_Comment_S2(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var result = ParseStar(input, inputPosition, states, factory, (i, p, s, f) => ParseRule_Comment_S2_M(i, p, s, f));
			if (result == null) return null;
			return new ParseResult<IReadOnlyList<String>>(result.Node, result.Advanced);
		}

		// Sequence: ~<EOLOS> .
		private static ParseResult<String> ParseRule_Comment_S2_M(String input, Int32 inputPosition, PackratState[] states, IParsnipRuleFactory factory)
		{
			var r1 = ParseIntrinsic_EndOfLineOrStream(input, inputPosition, states, factory);
			if (r1 != null) return null;
			var r2 = ParseIntrinsic_AnyCharacter(input, inputPosition + r1.Advanced, states, factory);
			if (r2 == null) return null;
			return new ParseResult<String>(r2.Node, inputPosition + r1.Advanced + r2.Advanced - inputPosition);
		}

		private class PackratState
		{
			public PackratState()
			{
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

