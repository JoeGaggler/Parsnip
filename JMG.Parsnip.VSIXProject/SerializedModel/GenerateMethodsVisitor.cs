using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.CodeWriting;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject.SerializedModel
{
	internal class GenerateMethodsVisitor : IParseFunctionActionVisitor<Signature>
	{
		private readonly CodeWriter writer;
		private readonly String interfaceName;
		private readonly ParsnipCode parsnipCode;

		private readonly IReadOnlyList<LocalVarDecl> typicalParams;

		public GenerateMethodsVisitor(ParsnipCode parsnipCode, CodeWriter writer, String interfaceName)
		{
			this.writer = writer;
			this.interfaceName = interfaceName;
			this.parsnipCode = parsnipCode;
			this.typicalParams = new List<LocalVarDecl>
			{
				new LocalVarDecl("PackratState", "state"),
				new LocalVarDecl(interfaceName, "factory")
			};
		}

		public void Visit(Selection target, Signature input)
		{
			var returnType = $"ParseResult<{NameGen.TypeString(target.ReturnType)}>";
			using (writer.Method(input.Access, false, returnType, input.Name, typicalParams))
			{
				writer.Comment("TODO: Selection");
			}
		}

		public void Visit(Sequence target, Signature input)
		{
			var returnType = $"ParseResult<{NameGen.TypeString(target.ReturnType)}>";
			using (writer.Method(input.Access, false, returnType, input.Name, typicalParams))
			{
				int stepIndex = 0;
				var currentState = "state";
				foreach (var step in target.Steps)
				{
					stepIndex++;
					var func = step.Function;
					var invoker = this.parsnipCode.Invokers[func];
					var resultName = $"r{stepIndex}";
					var nextState = $"{resultName}.State";

					// TODO: CALL DEPENDS ON FUNC!
					writer.VarAssign(resultName, invoker(currentState, "factory"));

					switch (step.MatchAction)
					{
						case MatchAction.Consume: writer.IfNullReturnNull(resultName); currentState = nextState; break;
						case MatchAction.Fail: writer.IfNotNullReturnNull(resultName); currentState = nextState; break;
						case MatchAction.Rewind: writer.IfNullReturnNull(resultName); break;
						case MatchAction.Ignore: writer.IfNullReturnNull(resultName); currentState = nextState; break;
					}
				}

				writer.Comment("TODO: Sequence combiner");
			}
		}

		public void Visit(Intrinsic target, Signature input)
		{
			var returnType = $"ParseResult<{NameGen.TypeString(target.ReturnType)}>";
			using (writer.Method(input.Access, false, returnType, input.Name, typicalParams))
			{
				writer.Comment("TODO: Intrinsic");
			}
		}

		public void Visit(LiteralString target, Signature input)
		{
			writer.Comment("TODO: LiteralString");
		}

		public void Visit(ReferencedRule target, Signature input)
		{
			writer.Comment("TODO: ReferencedRule");
		}
	}
}
