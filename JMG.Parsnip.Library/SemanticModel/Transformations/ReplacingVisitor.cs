using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SemanticModel.Transformations
{
	internal class ReplacingVisitor : IParseFunctionFuncVisitor<IParseFunction>
	{
		private ParsnipModel model;

		public ReplacingVisitor(ParsnipModel model)
		{
			this.model = model;
		}

		public Func<ReferencedRule, ReferencedRule> ReferencedRule { get; set; }

		public IParseFunction Visit(Selection target)
		{
			var newSteps = target.Steps.Select(i => new SelectionStep(i.Function.ApplyVisitor(this), i.InterfaceMethod)).ToList();
			return target.ReplacingSteps(newSteps);
		}

		public IParseFunction Visit(Sequence target)
		{
			var newSteps = target.Steps.Select(i => new SequenceStep(i.Function.ApplyVisitor(this), i.MatchAction)).ToList();
			return target.ReplacingSteps(newSteps);
		}

		public IParseFunction Visit(Intrinsic target) => target;

		public IParseFunction Visit(LiteralString target) => target;

		public IParseFunction Visit(LexemeIdentifier target) => target;

		public IParseFunction Visit(ReferencedRule target) => (ReferencedRule == null) ? target : (ReferencedRule(target));

		public IParseFunction Visit(Repetition target)
		{
			var inner = target.InnerParseFunction.ApplyVisitor(this);
			return new Repetition(inner, target.Cardinality, target.InterfaceMethod);
		}

		public IParseFunction Visit(Series target)
		{
			var repeated = target.RepeatedToken.ApplyVisitor(this);
			var delimiter = target.DelimiterToken.ApplyVisitor(this);
			return new Series(repeated, delimiter, target.InterfaceMethod);
		}
	}
}
