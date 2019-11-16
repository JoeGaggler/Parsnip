using ParsnipCLI.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsnipCLI
{
    public interface IMyParserFactory
    {
        ParsnipDefinition Definition1(List<ParsnipDefinitionItem> t0);
        ParsnipDefinitionItem DefinitionItem1(ParsnipRule t0);
        ParsnipDefinitionItem DefinitionItem2(String node);
        ParsnipDefinitionItem DefinitionItem3();
        ParsnipRule Rule1(ParsnipRuleHead node1, ParsnipRuleBody node2);
        ParsnipRuleHead RuleHead1((ParsnipRuleHeadPrefix, ParsnipClassIdentifier) node);
        ParsnipRuleHead RuleHead2(ParsnipRuleHeadPrefix node);
        ParsnipRuleHeadPrefix RuleHeadPrefix(ParsnipRuleIdentifier node);
        ParsnipRuleBody RuleBody1(List<ParsnipChoice> node);
        ParsnipChoice Choice1(ParsnipUnion node);
        ParsnipUnion Union1(List<ParsnipSequence> node);
        ParsnipSequence Sequence1(List<ParsnipSegment> node);
        ParsnipSegment Special1((IToken, IToken) node);
        ParsnipSegment Special2(ParsnipSegment node);
        ParsnipSegment Segment1(String node1, TokenCardinality node2);
        TokenCardinality Cardinality1(IToken node1, String node2);
        IToken Token1();
        IToken Token2(String node);
        IToken Token3(ParsnipRuleIdentifier node);
        IToken Token4(String node);
        IToken Token5(ParsnipUnion node);
        ParsnipRuleIdentifier RuleIdentifier1(String node1, IReadOnlyList<String> node2);
        ParsnipClassIdentifier ClassIdentifier1(List<String> node);
        String CsharpIdentifier1(String node1, IReadOnlyList<String> node2);
        String IntrinsicIdentifier1(IReadOnlyList<String> node);
        String IntrinsicIdentifier2(String node);
        String Comment1(IReadOnlyList<String> node);
    }

    public class MyParser
    {
        private class EmptyNode { private EmptyNode() { } public static EmptyNode Instance = new EmptyNode(); }

        private struct ParseResult<T>
        {
            public readonly static ParseResult<T> Fail = new ParseResult<T>(default, -1);

            public T Node;
            public int Length;

            public ParseResult(T node, int length)
            {
                this.Node = node;
                this.Length = length;
            }

            public Boolean DidFail => this.Length == -1;
        }

        public ParsnipDefinition Parse(ReadOnlySpan<Char> input, IMyParserFactory factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var r1 = ParseRule_Definition(input, 0, factory);
            if (r1.DidFail) { return default; }
            return r1.Node;
        }

        private delegate ParseResult<T> MyFunc<T>(ReadOnlySpan<Char> input, int start, IMyParserFactory factory);

        private static ParseResult<List<T>> ParsePlus<T>(ReadOnlySpan<Char> input, int start, IMyParserFactory factory, MyFunc<T> parseFunc)
        {
            var list = new List<T>();
            var firstResult = parseFunc(input, start, factory);
            if (firstResult.DidFail)
            {
                return ParseResult<List<T>>.Fail;
            }
            list.Add(firstResult.Node);
            var s = start + firstResult.Length;
            while (true)
            {
                var nextResult = parseFunc(input, s, factory);
                if (nextResult.DidFail)
                {
                    break;
                }
                list.Add(nextResult.Node);
                s = s + nextResult.Length;
            }
            return new ParseResult<List<T>>(list, s);
        }

        private static ParseResult<List<T>> ParseStar<T>(ReadOnlySpan<Char> input, int start, IMyParserFactory factory, MyFunc<T> parseFunc)
        {
            var list = new List<T>();
            var s = start;
            while (true)
            {
                var nextResult = parseFunc(input, s, factory);
                if (nextResult.DidFail)
                {
                    break;
                }
                list.Add(nextResult.Node);
                s = s + nextResult.Length;
            }
            return new ParseResult<List<T>>(list, s);
        }

        private static ParseResult<T> ParseMaybe<T>(ReadOnlySpan<Char> input, int start, IMyParserFactory factory, MyFunc<T> parseFunc)
        {
            var nextResult = parseFunc(input, start, factory);
            if (!nextResult.DidFail)
            {
                return nextResult;
            }
            return new ParseResult<T>(default, 0);
        }

        private static ParseResult<List<T>> ParseSeries<T, D>(ReadOnlySpan<Char> input, int start, IMyParserFactory factory, MyFunc<T> parseAction, MyFunc<D> parseDelimiterAction)
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

        private static ParseResult<String> ParseIntrinsic_AnyCharacter(ReadOnlySpan<Char> input, int start)
			{
				var input = state.input;
				var inputPosition = state.inputPosition;
				if (inputPosition >= input.Length)
				{
					return null;
				}
				return new ParseResult<String>() { Node = state.input.Substring(inputPosition, 1), State = state.states[inputPosition + 1] };
			}

			private static ParseResult<String> ParseIntrinsic_AnyLetter(ReadOnlySpan<Char> input, int start)
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

			private static ParseResult<String> ParseIntrinsic_AnyDigit(ReadOnlySpan<Char> input, int start)
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

        private static ParseResult<String> ParseIntrinsic_EndOfLine(ReadOnlySpan<Char> input, int start)
        {
            var result1 = ParseLexeme(input, start, "\r\n");
            if (!result1.DidFail)
            {
                return new ParseResult<String>(result1.Node, result1.Length);
            }
            var result2 = ParseLexeme(input, start, "\n");
            if (!result2.DidFail)
            {
                return new ParseResult<String>(result2.Node, result2.Length);
            }
            return ParseResult<String>.Fail;
        }

        private static ParseResult<EmptyNode> ParseIntrinsic_EndOfLineOrStream(ReadOnlySpan<Char> input, int start)
        {
            if (input.IsEmpty || input.Length == start)
            {
                return new ParseResult<EmptyNode>(EmptyNode.Instance, 0);
            }
            var result1 = ParseLexeme(input, start, "\r\n");
            if (!result1.DidFail)
            {
                return new ParseResult<EmptyNode>(EmptyNode.Instance, result1.Length);
            }
            var result2 = ParseLexeme(input, start, "\n");
            if (!result2.DidFail)
            {
                return new ParseResult<EmptyNode>(EmptyNode.Instance, result2.Length);
            }
            return ParseResult<EmptyNode>.Fail;
        }

        private static ParseResult<String> ParseIntrinsic_CString(ReadOnlySpan<Char> input, int start)
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

        private static ParseResult<String> ParseIntrinsic_OptionalHorizontalWhitespace(ReadOnlySpan<Char> input, int start)
        {
            var inputPosition = start;
            var inputLength = input.Length;
            if (inputPosition >= inputLength)
            {
                return new ParseResult<String>(String.Empty, 0);
            }
            var sb = new System.Text.StringBuilder();
            var nextInputPosition = inputPosition;
            while (nextInputPosition < inputLength)
            {
                var ch = input[nextInputPosition];
                if (ch != ' ' && ch != '\t')
                {
                    break;
                }
                nextInputPosition++;
            }
            return new ParseResult<String>(input.Slice(inputPosition, nextInputPosition - inputPosition).ToString(), nextInputPosition - start);
        }

        private static ParseResult<String> ParseLexeme(ReadOnlySpan<Char> input, int start, String lexeme)
        {
            var lexemeLength = lexeme.Length;
            if (start + lexemeLength > input.Length) { return ParseResult<String>.Fail; }
            if (input.Slice(start, lexemeLength) != lexeme) { return ParseResult<String>.Fail; }
            return new ParseResult<String>(lexeme, lexemeLength);
        }

        // Repetition: definition-item+
        private ParseResult<ParsnipDefinition> ParseRule_Definition(ReadOnlySpan<Char> input, int start, IMyParserFactory factory)
        {
            var r1 = ParsePlus(input, start, factory, ParseRule_DefinitionItem);
            if (r1.DidFail) { return ParseResult<ParsnipDefinition>.Fail; }
            return new ParseResult<ParsnipDefinition>(factory.Definition1(r1.Node), start + r1.Length);
        }

        // Selection: rule | comment | (`<EOL>)
        private ParseResult<ParsnipDefinitionItem> ParseRule_DefinitionItem(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_Rule(input, start, factory);
            if (!r1.DidFail) { return new ParseResult<ParsnipDefinitionItem>(factory.DefinitionItem1(r1.Node), r1.Length); }
            var r2 = ParseRule_Comment(input, start, factory);
            if (!r2.DidFail) { return new ParseResult<ParsnipDefinitionItem>(factory.DefinitionItem2(r2.Node), r2.Length); }
            var r3 = DefinitionItem_C3(input, start, factory);
            if (!r3.DidFail) { return new ParseResult<ParsnipDefinitionItem>(factory.DefinitionItem3(), r3.Length); }
            return ParseResult<ParsnipDefinitionItem>.Fail;
        }

        // Sequence: `<EOL>
        private ParseResult<EmptyNode> DefinitionItem_C3(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseIntrinsic_EndOfLine(input, start);
            if (r1.DidFail) { return ParseResult<EmptyNode>.Fail; }
            return new ParseResult<EmptyNode>(EmptyNode.Instance, start);
        }

        // Sequence: rule-head rule-body
        private ParseResult<ParsnipRule> ParseRule_Rule(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_RuleHead(input, start, factory);
            if (r1.DidFail) { return ParseResult<ParsnipRule>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseRule_RuleBody(input, s2, factory);
            if (r2.DidFail) { return ParseResult<ParsnipRule>.Fail; }
            return new ParseResult<ParsnipRule>(factory.Rule1(r1.Node, r2.Node), s2 + r2.Length);
        }

        // Selection: (rule-head-prefix `-- class-identifier `-- `<EOL>) | (rule-head-prefix `-- `<EOL>)
        private static ParseResult<ParsnipRuleHead> ParseRule_RuleHead(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_RuleHead_C1(input, start, factory);
            if (!r1.DidFail) { return new ParseResult<ParsnipRuleHead>(factory.RuleHead1(r1.Node), r1.Length); }
            var r2 = ParseRule_RuleHead_C2(input, start, factory);
            if (!r2.DidFail) { return new ParseResult<ParsnipRuleHead>(factory.RuleHead2(r2.Node), r2.Length); }
            return ParseResult<ParsnipRuleHead>.Fail;
        }

        // Sequence: rule-head-prefix `-- class-identifier `-- `<EOL>
        private static ParseResult<(ParsnipRuleHeadPrefix, ParsnipClassIdentifier)> ParseRule_RuleHead_C1(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_RuleHeadPrefix(input, start, factory);
            if (r1.DidFail) { return ParseResult<(ParsnipRuleHeadPrefix, ParsnipClassIdentifier)>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(input, s2);
            if (r2.DidFail) { return ParseResult<(ParsnipRuleHeadPrefix, ParsnipClassIdentifier)>.Fail; }
            var s3 = s2 + r2.Length;
            var r3 = ParseRule_ClassIdentifier(input, s3, factory);
            if (r3.DidFail) { return ParseResult<(ParsnipRuleHeadPrefix, ParsnipClassIdentifier)>.Fail; }
            var s4 = s3 + r3.Length;
            var r4 = ParseIntrinsic_OptionalHorizontalWhitespace(input, s4);
            if (r4.DidFail) { return ParseResult<(ParsnipRuleHeadPrefix, ParsnipClassIdentifier)>.Fail; }
            var s5 = s4 + r4.Length;
            var r5 = ParseIntrinsic_EndOfLine(input, s5);
            if (r5.DidFail) { return ParseResult<(ParsnipRuleHeadPrefix, ParsnipClassIdentifier)>.Fail; }
            return new ParseResult<(ParsnipRuleHeadPrefix, ParsnipClassIdentifier)>((r1.Node, r3.Node), s5 + r5.Length);
        }

        // Sequence: rule-head-prefix `-- `<EOL>
        private static ParseResult<ParsnipRuleHeadPrefix> ParseRule_RuleHead_C2(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_RuleHeadPrefix(input, start, factory);
            if (r1.DidFail) { return ParseResult<ParsnipRuleHeadPrefix>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(input, s2);
            if (r2.DidFail) { return ParseResult<ParsnipRuleHeadPrefix>.Fail; }
            var s3 = s2 + r2.Length;
            var r3 = ParseIntrinsic_EndOfLine(input, s3);
            if (r3.DidFail) { return ParseResult<ParsnipRuleHeadPrefix>.Fail; }
            return new ParseResult<ParsnipRuleHeadPrefix>(r1.Node, s3 + r3.Length);
        }

        // Sequence: rule-identifier `-- `":"
        private static ParseResult<ParsnipRuleHeadPrefix> ParseRule_RuleHeadPrefix(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_RuleIdentifier(input, start, factory);
            if (r1.DidFail) { return ParseResult<ParsnipRuleHeadPrefix>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(input, s2);
            if (r2.DidFail) { return ParseResult<ParsnipRuleHeadPrefix>.Fail; }
            var s3 = s2 + r2.Length;
            var r3 = ParseLexeme(input, s3, ":");
            if (r3.DidFail) { return ParseResult<ParsnipRuleHeadPrefix>.Fail; }
            return new ParseResult<ParsnipRuleHeadPrefix>(factory.RuleHeadPrefix(r1.Node), s3 + r3.Length);
        }

        // Repetition: choice+
        private static ParseResult<ParsnipRuleBody> ParseRule_RuleBody(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParsePlus(input, start, factory, ParseRule_Choice);
            if (r1.DidFail) { return ParseResult<ParsnipRuleBody>.Fail; }
            return new ParseResult<ParsnipRuleBody>(factory.RuleBody1(r1.Node), start + r1.Length);
        }

        // Sequence: union `-- `<EOLOS>
        private ParseResult<ParsnipChoice> Mem_ParseRule_Choice;
        private static ParseResult<ParsnipChoice> ParseRule_Choice(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_Union(input, start, factory);
            if (r1.DidFail) { return ParseResult<ParsnipChoice>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseIntrinsic_OptionalHorizontalWhitespace(input, s2);
            if (r2.DidFail) { return ParseResult<ParsnipChoice>.Fail; }
            var s3 = s2 + r2.Length;
            var r3 = ParseIntrinsic_EndOfLineOrStream(input, s3);
            if (r3.DidFail) { return ParseResult<ParsnipChoice>.Fail; }
            return new ParseResult<ParsnipChoice>(factory.Choice1(r1.Node), s3 + r3.Length);
        }

        // Series: sequence/(-- "|" --)
        private static ParseResult<ParsnipUnion> ParseRule_Union(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseSeries(input, start, factory, ParseRule_Sequence, ParseRule_Union_D);
            if (r1.DidFail) { return ParseResult<ParsnipUnion>.Fail; }
            return new ParseResult<ParsnipUnion>(factory.Union1(r1.Node), start + r1.Length);
        }

        // Sequence: -- "|" --
        private static ParseResult<EmptyNode> ParseRule_Union_D(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseIntrinsic_OptionalHorizontalWhitespace(input, start);
            if (r1.DidFail) { return ParseResult<EmptyNode>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseLexeme(input, s2, "|");
            if (r2.DidFail) { return ParseResult<EmptyNode>.Fail; }
            var s3 = s2 + r2.Length;
            var r3 = ParseIntrinsic_OptionalHorizontalWhitespace(input, s3);
            if (r3.DidFail) { return ParseResult<EmptyNode>.Fail; }
            return new ParseResult<EmptyNode>(EmptyNode.Instance, s3 + r3.Length);
        }

        // Series: special/--
        private static ParseResult<ParsnipSequence> ParseRule_Sequence(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseSeries(input, start, factory, ParseRule_Special, (i, s, f) => ParseIntrinsic_OptionalHorizontalWhitespace(i, s));
            if (r1.DidFail) { return ParseResult<ParsnipSequence>.Fail; }
            return new ParseResult<ParsnipSequence>(factory.Sequence1(r1.Node), start + r1.Length);
        }

        // Selection: (token `"/" token) | segment
        private static ParseResult<ParsnipSegment> ParseRule_Special(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_Special_C1(input, start, factory);
            if (!r1.DidFail) { return new ParseResult<ParsnipSegment>(factory.Special1(r1.Node), r1.Length); }
            var r2 = ParseRule_Segment(input, start, factory);
            if (!r2.DidFail) { return new ParseResult<ParsnipSegment>(factory.Special2(r2.Node), r2.Length); }
            return ParseResult<ParsnipSegment>.Fail;
        }

        // Sequence: token `"/" token
        private static ParseResult<(IToken, IToken)> ParseRule_Special_C1(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_Token(input, start, factory);
            if (r1.DidFail) { return ParseResult<(IToken, IToken)>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseLexeme(input, s2, "|");
            if (r2.DidFail) { return ParseResult<(IToken, IToken)>.Fail; }
            var s3 = s2 + r2.Length;
            var r3 = ParseRule_Token(input, s3, factory);
            if (r3.DidFail) { return ParseResult<(IToken, IToken)>.Fail; }
            return new ParseResult<(IToken, IToken)>((r1.Node, r3.Node), s3 + r3.Length);
        }

        // Sequence: ("`" | "~" | "&")? cardinality
        private static ParseResult<ParsnipSegment> ParseRule_Segment(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_Segment_S1(input, start, factory);
            if (r1.DidFail) { return ParseResult<ParsnipSegment>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseRule_Cardinality(input, s2, factory);
            if (r2.DidFail) { return ParseResult<ParsnipSegment>.Fail; }
            return new ParseResult<ParsnipSegment>(factory.Segment1(r1.Node, r2.Node), s2 + r2.Length);
        }

        // Repetition: ("`" | "~" | "&")?
        private static ParseResult<String> ParseRule_Segment_S1(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseMaybe(input, start, factory, ParseRule_Segment_S1_M);
            if (r1.DidFail) { return ParseResult<String>.Fail; }
            return new ParseResult<String>(r1.Node, start + r1.Length);
        }

        // Selection: "`" | "~" | "&"
        private static ParseResult<String> ParseRule_Segment_S1_M(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseLexeme(input, start, "`");
            if (!r1.DidFail) { return new ParseResult<String>(r1.Node, r1.Length); }
            var r2 = ParseLexeme(input, start, "~");
            if (!r2.DidFail) { return new ParseResult<String>(r2.Node, r2.Length); }
            var r3 = ParseLexeme(input, start, "&");
            if (!r3.DidFail) { return new ParseResult<String>(r3.Node, r3.Length); }
            return ParseResult<String>.Fail;
        }

        // Sequence: token ("+" | "?" | "*")?
        private static ParseResult<TokenCardinality> ParseRule_Cardinality(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_Token(input, start, factory);
            if (r1.DidFail) { return ParseResult<TokenCardinality>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseRule_Cardinality_S2(input, s2, factory);
            if (r2.DidFail) { return ParseResult<TokenCardinality>.Fail; }
            return new ParseResult<TokenCardinality>(factory.Cardinality1(r1.Node, r2.Node), s2 + r2.Length);
        }

        // Repetition: ("+" | "?" | "*")?
        private static ParseResult<String> ParseRule_Cardinality_S2(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseMaybe(input, start, factory, ParseRule_Cardinality_S2_M);
            if (r1.DidFail) { return ParseResult<String>.Fail; }
            return new ParseResult<String>(r1.Node, start + r1.Length);
        }

        // Selection: "+" | "?" | "*"
        private static ParseResult<String> ParseRule_Cardinality_S2_M(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseLexeme(input, start, "+");
            if (!r1.DidFail) { return new ParseResult<String>(r1.Node, r1.Length); }
            var r2 = ParseLexeme(input, start, "?");
            if (!r2.DidFail) { return new ParseResult<String>(r2.Node, r2.Length); }
            var r3 = ParseLexeme(input, start, "*");
            if (!r3.DidFail) { return new ParseResult<String>(r3.Node, r3.Length); }
            return ParseResult<String>.Fail;
        }

        // Selection: (`".") | <CSTRING> | rule-identifier | ((`"<" intrinsic-identifier `">") | "--") | (`"(" union `")")
        private ParseResult<IToken> Mem_ParseRule_Token;
        private static ParseResult<IToken> ParseRule_Token(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_Token_C1(input, start, factory);
            if (!r1.DidFail) { return new ParseResult<IToken>(factory.Token1(), r1.Length); }
            var r2 = ParseIntrinsic_CString(input, start);
            if (!r2.DidFail) { return new ParseResult<IToken>(factory.Token2(r2.Node), r2.Length); }
            var r3 = ParseRule_RuleIdentifier(input, start, factory);
            if (!r3.DidFail) { return new ParseResult<IToken>(factory.Token3(r3.Node), r3.Length); }
            var r4 = ParseRule_Token_C4(input, start, factory);
            if (!r4.DidFail) { return new ParseResult<IToken>(factory.Token4(r4.Node), r4.Length); }
            var r5 = ParseRule_Token_C5(input, start, factory);
            if (!r5.DidFail) { return new ParseResult<IToken>(factory.Token5(r5.Node), r5.Length); }
            return ParseResult<IToken>.Fail;
        }

        // Sequence: `"."
        private static ParseResult<EmptyNode> ParseRule_Token_C1(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseLexeme(input, start, ".");
            if (r1.DidFail) { return ParseResult<EmptyNode>.Fail; }
            return new ParseResult<EmptyNode>(EmptyNode.Instance, r1.Length);
        }

        // Selection: (`"<" intrinsic-identifier `">") | "--"
        private static ParseResult<String> ParseRule_Token_C4(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_Token_C4_C1(input, start, factory);
            if (!r1.DidFail) { return new ParseResult<String>(r1.Node, r1.Length); }
            var r2 = ParseLexeme(input, start, "--");
            if (!r2.DidFail) { return new ParseResult<String>(r2.Node, r2.Length); }
            return ParseResult<String>.Fail;
        }

        // Sequence: `"<" intrinsic-identifier `">"
        private static ParseResult<String> ParseRule_Token_C4_C1(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseLexeme(input, start, "<");
            if (r1.DidFail) { return ParseResult<String>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseRule_IntrinsicIdentifier(input, s2, factory);
            if (r2.DidFail) { return ParseResult<String>.Fail; }
            var s3 = s2 + r2.Length;
            var r3 = ParseLexeme(input, s3, ">");
            if (r3.DidFail) { return ParseResult<String>.Fail; }
            return new ParseResult<String>(r2.Node, s3 + r3.Length);
        }

        // Sequence: `"(" union `")"
        private static ParseResult<ParsnipUnion> ParseRule_Token_C5(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseLexeme(input, start, "(");
            if (r1.DidFail) { return ParseResult<ParsnipUnion>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseRule_Union(input, s2, factory);
            if (r2.DidFail) { return ParseResult<ParsnipUnion>.Fail; }
            var s3 = s2 + r2.Length;
            var r3 = ParseLexeme(input, s3, ")");
            if (r3.DidFail) { return ParseResult<ParsnipUnion>.Fail; }
            return new ParseResult<ParsnipUnion>(r2.Node, s3 + r3.Length);
        }

        // Sequence: <Aa> (<Aa> | "-")*
        private static ParseResult<ParsnipRuleIdentifier> ParseRule_RuleIdentifier(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseIntrinsic_AnyLetter(input, start);
            if (r1.DidFail) { return ParseResult<ParsnipRuleIdentifier>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseRule_RuleIdentifier_S2(input, s2, factory);
            if (r2.DidFail) { return ParseResult<ParsnipRuleIdentifier>.Fail; }
            return new ParseResult<ParsnipRuleIdentifier>(factory.RuleIdentifier1(r1.Node, r2.Node), s2 + r2.Length);
        }

        // Repetition: (<Aa> | "-")*
        private static ParseResult<List<String>> ParseRule_RuleIdentifier_S2(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseStar(input, start, factory, ParseRule_RuleIdentifier_S2_M);
            if (r1.DidFail) { return ParseResult<List<String>>.Fail; }
            return new ParseResult<List<String>>(r1.Node, start + r1.Length);
        }

        // Selection: <Aa> | "-"
        private static ParseResult<String> ParseRule_RuleIdentifier_S2_M(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseIntrinsic_AnyLetter(input, start);
            if (!r1.DidFail) { return new ParseResult<String>(r1.Node, r1.Length); }
            var r2 = ParseLexeme(input, start, "-");
            if (!r2.DidFail) { return new ParseResult<String>(r2.Node, r2.Length); }
            return ParseResult<String>.Fail;
        }

        // Series: csharp-identifier/"."
        private static ParseResult<ParsnipClassIdentifier> ParseRule_ClassIdentifier(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseSeries(input, start, factory, ParseRule_CsharpIdentifier, (i, s, f) => ParseLexeme(i, s, "."));
            if (r1.DidFail) { return ParseResult<ParsnipClassIdentifier>.Fail; }
            return new ParseResult<ParsnipClassIdentifier>(factory.ClassIdentifier1(r1.Node), start + r1.Length);
        }

        // Sequence: <Aa> (<Aa> | <#>)*
        private ParseResult<String> Mem_ParseRule_CsharpIdentifier;
        private static ParseResult<String> ParseRule_CsharpIdentifier(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseIntrinsic_AnyLetter(input, start);
            if (r1.DidFail) { return ParseResult<String>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseRule_CsharpIdentifier_S2(input, s2, factory);
            if (r2.DidFail) { return ParseResult<String>.Fail; }
            return new ParseResult<String>(factory.CsharpIdentifier1(r1.Node, r2.Node), s2 + r2.Length);
        }

        // Repetition: (<Aa> | <#>)*
        private static ParseResult<List<String>> ParseRule_CsharpIdentifier_S2(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseStar(input, start, factory, ParseRule_CsharpIdentifier_S2_M);
            if (r1.DidFail) { return ParseResult<List<String>>.Fail; }
            return new ParseResult<List<String>>(r1.Node, start + r1.Length);
        }

        // Selection: <Aa> | <#>
        private static ParseResult<String> ParseRule_CsharpIdentifier_S2_M(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseIntrinsic_AnyLetter(input, start);
            if (!r1.DidFail) { return new ParseResult<String>(r1.Node, r1.Length); }
            var r2 = ParseIntrinsic_AnyDigit(input, start);
            if (!r2.DidFail) { return new ParseResult<String>(r2.Node, r2.Length); }
            return ParseResult<String>.Fail;
        }

        // Selection: <Aa>+ | "#"
        private static ParseResult<String> ParseRule_IntrinsicIdentifier(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseRule_IntrinsicIdentifier_C1(input, start, factory);
            if (!r1.DidFail) { return new ParseResult<String>(factory.IntrinsicIdentifier1(r1.Node), r1.Length); }
            var r2 = ParseLexeme(input, start, "#");
            if (!r2.DidFail) { return new ParseResult<String>(factory.IntrinsicIdentifier2(r2.Node), r2.Length); }
            return ParseResult<String>.Fail;
        }

        // Repetition: <Aa>+
        private static ParseResult<List<String>> ParseRule_IntrinsicIdentifier_C1(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParsePlus(input, start, factory, (i, s, f) => ParseIntrinsic_AnyLetter(i, s));
            if (r1.DidFail) { return ParseResult<List<String>>.Fail; }
            return new ParseResult<List<String>>(r1.Node, start + r1.Length);
        }

        // Sequence: `"//" (~<EOLOS> .)* `<EOLOS>
        private static ParseResult<String> ParseRule_Comment(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseLexeme(input, start, "//");
            if (r1.DidFail) { return ParseResult<String>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseRule_Comment_S2(input, s2, factory);
            if (r2.DidFail) { return ParseResult<String>.Fail; }
            var s3 = s2 + r2.Length;
            var r3 = ParseIntrinsic_EndOfLineOrStream(input, s3);
            if (r3.DidFail) { return ParseResult<String>.Fail; }
            return new ParseResult<String>(factory.Comment1(r2.Node), s3 + r3.Length);
        }

        // Repetition: (~<EOLOS> .)*
        private static ParseResult<IReadOnlyList<String>> ParseRule_Comment_S2(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseStar(input, start, factory, ParseRule_Comment_S2_M);
            if (r1.DidFail) { return ParseResult<IReadOnlyList<String>>.Fail; }
            return new ParseResult<IReadOnlyList<String>>(r1.Node, start + r1.Length);
        }

        // Sequence: ~<EOLOS> .
        private static ParseResult<String> ParseRule_Comment_S2_M(ReadOnlySpan<Char> input, Int32 start, IMyParserFactory factory)
        {
            var r1 = ParseIntrinsic_EndOfLineOrStream(input, start);
            if (!r1.DidFail) { return ParseResult<String>.Fail; }
            var s2 = start + r1.Length;
            var r2 = ParseIntrinsic_AnyCharacter(input, s2);
            if (r2.DidFail) { return ParseResult<String>.Fail; }
            return new ParseResult<String>(r2.Node, s2 + r2.Length);
        }
    }
}
