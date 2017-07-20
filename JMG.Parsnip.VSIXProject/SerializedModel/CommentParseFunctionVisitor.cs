using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject.SerializedModel
{
	internal class CommentParseFunctionVisitor : IParseFunctionFuncVisitor<String>
	{
		private readonly Boolean ShowHeader;
		private readonly Boolean ShowWrapped;
		private static CommentParseFunctionVisitor WithoutHeader = new CommentParseFunctionVisitor(showHeader: false, showWrapped: true);

		public CommentParseFunctionVisitor(Boolean showHeader = false, Boolean showWrapped = false)
		{
			this.ShowHeader = showHeader;
			this.ShowWrapped = showWrapped;
		}

		public String Visit(Selection target)
		{
			var steps = new List<String>();
			foreach (var step in target.Steps)
			{
				var value = step.Function.ApplyVisitor(WithoutHeader);
				steps.Add(value);
			}
			var all = String.Join(" | ", steps);
			if (this.ShowHeader)
			{
				all = $"Selection: {all}";
			}
			else if (this.ShowWrapped)
			{
				all = $"({all})";
			}
			return all;
		}

		public String Visit(Sequence target)
		{
			var steps = new List<String>();
			foreach (var step in target.Steps)
			{
				var value = step.Function.ApplyVisitor(WithoutHeader);
				switch (step.MatchAction)
				{
					case MatchAction.Consume: break;
					case MatchAction.Fail: value = "~" + value; break;
					case MatchAction.Ignore: value = "`" + value; break;
					case MatchAction.Rewind: value = "&" + value; break;
				}
				steps.Add(value);
			}
			var all = String.Join(" ", steps);
			if (this.ShowHeader)
			{
				all = $"Sequence: {all}";
			}
			else if (this.ShowWrapped)
			{
				all = $"({all})";
			}
			return all;
		}

		public String Visit(Intrinsic target)
		{
			String value;
			switch (target.Type)
			{
				case IntrinsicType.AnyCharacter: value = "."; break;
				case IntrinsicType.AnyLetter: value = "<Aa>"; break;
				case IntrinsicType.CString: value = "<CSTRING>"; break;
				case IntrinsicType.EndOfLine: value = "<EOL>"; break;
				case IntrinsicType.EndOfStream: value = "<EOS>"; break;
				default: value = "<UNKNOWN_INTRINSIC>"; break;
			}
			if (this.ShowHeader)
			{
				value = $"Intrinsic: {value}";
			}
			return value;
		}

		public String Visit(LiteralString target)
		{
			var value = $"\"{target.Text}\"";
			if (this.ShowHeader)
			{
				value = $"Intrinsic: {value}";
			}
			return value;
		}

		public String Visit(ReferencedRule target)
		{
			var value = target.Identifier;
			if (this.ShowHeader)
			{
				value = $"Rule: {value}";
			}
			return value;
		}

		public String Visit(CardinalityFunction target)
		{
			var baseString = target.InnerParseFunction.ApplyVisitor(WithoutHeader);
			switch (target.Cardinality)
			{
				case Cardinality.One: return baseString;
				case Cardinality.Maybe: return $"{baseString}?";
				case Cardinality.Plus: return $"{baseString}+";
				case Cardinality.Star: return $"{baseString}*";
				default: throw new NotImplementedException();
			}
		}
	}
}
