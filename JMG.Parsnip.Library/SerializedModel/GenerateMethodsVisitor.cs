using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.CodeWriting;
using JMG.Parsnip.SemanticModel;

namespace JMG.Parsnip.SerializedModel
{
	internal class GenerateMethodsVisitor : IParseFunctionActionVisitor<Signature>
	{
		private readonly CodeWriter writer;
		private readonly String interfaceName;
		private readonly IReadOnlyDictionary<IParseFunction, Invoker> invokers;

		public GenerateMethodsVisitor(CodeWriter writer, String interfaceName, IReadOnlyDictionary<SemanticModel.IParseFunction, Invoker> invokers)
        {
			this.writer = writer;
			this.interfaceName = interfaceName;
			this.invokers = invokers;
		}

		private class Decl
		{
			public Decl(INodeType type, String name)
			{
				this.Type = type;
				this.Name = name;
			}

			public INodeType Type { get; }
			public String Name { get; }
		}

		private String GetReturnExpression(INodeType returnType, IReadOnlyList<Decl> nodes, String inputPositionReference, String factoryName, InterfaceMethod interfaceMethod)
        {
			String nodeString;
			if (returnType == EmptyNodeType.Instance)
			{
				nodeString = "EmptyNode.Instance";
			}
			else if (interfaceMethod != null)
			{
				String param;
				if (nodes.Count == 1 && nodes[0].Type == EmptyNodeType.Instance)
				{
					param = String.Empty;
				}
				else
				{
					param = String.Join(", ", nodes.Select(i => i.Name));
				}

				nodeString = $"{factoryName}.{interfaceMethod.Name}({param})";
			}
			else if (nodes.Count == 1)
			{
				nodeString = nodes[0].Name;
			}
			else
			{
				// Make a tuple
				nodeString = $"({String.Join(", ", nodes.Select(i => i.Name))})";
			}

			var returnTypeString = returnType.GetParseResultTypeString();
			return ($"new {returnTypeString}({nodeString}, {inputPositionReference})");
		}

		private void BasicTargetWithInterfaceMethod(IParseFunction target, InterfaceMethod interfaceMethod, Signature signature)
		{
			var resultName = "r";
			var nodeName = $"{resultName}.Node";
			var invoker = this.invokers[target];
			writer.VarAssign(resultName, invoker("input", "inputPosition", "states", "factory")); // Invocation
			writer.IfNullReturnNull(resultName);

			var decl = new Decl(target.ReturnType, nodeName);

			var returnExpression = GetReturnExpression(target.ReturnType, new[] { decl }, "inputPosition", "factory", interfaceMethod);
			if (signature.IsMemoized)
			{
				var memField = NameGen.MemoizedFieldName(signature.Name);
				returnExpression = $"states[inputPosition].{memField} = {returnExpression}";
			}
			writer.Return(returnExpression);
		}

		public void Visit(Selection target, Signature input)
		{
			int stepIndex = 0;
			foreach (var step in target.Steps)
			{
				stepIndex++;
				var func = step.Function;
				var invoker = this.invokers[func];
				var resultName = $"r{stepIndex}";

				writer.VarAssign(resultName, invoker("input", "inputPosition", "states", "factory")); // Invocation

				var factoryName = "factory";
				var interfaceMethod = step.InterfaceMethod;
				var nodeReference = $"{resultName}.Node";

				IReadOnlyList<Decl> nodes;
				if (func.ReturnType is TupleNodeType tupleNodeType)
				{
					nodes = tupleNodeType.Types.Select((i, j) => new Decl(i, $"{nodeReference}.Item{j + 1}")).ToList();
				}
				else
				{
					nodes = new[] { new Decl(func.ReturnType, nodeReference) };
				}

				var returnExpression = GetReturnExpression(target.ReturnType, nodes, $"{resultName}.Advanced", factoryName, interfaceMethod);
				if (input.IsMemoized)
				{
					var memField = NameGen.MemoizedFieldName(input.Name);
					returnExpression = $"states[inputPosition].{memField} = {returnExpression}";
				}
				writer.LineOfCode($"if ({resultName} != null) return {returnExpression};");
			}

			// No choices matched
			writer.Return("null");
		}

		public void Visit(Sequence target, Signature input)
		{
			var returnedResults = new List<Decl>();

			int stepIndex = 0;
            var currentInputPosition = "inputPosition";
			foreach (var step in target.Steps)
			{
				stepIndex++;
				var func = step.Function;
				var type = func.ReturnType;
				var invoker = this.invokers[func];
				var resultName = $"r{stepIndex}";
				var nodeName = $"{resultName}.Node";

				if (step.IsReturned)
				{
					returnedResults.Add(new Decl(type, nodeName));
				}

				writer.VarAssign(resultName, invoker("input", currentInputPosition, "states", "factory")); // Invocation

				switch (step.MatchAction)
				{
					case MatchAction.Consume: writer.IfNullReturnNull(resultName); break;
					case MatchAction.Fail: writer.IfNotNullReturnNull(resultName); break;
					case MatchAction.Rewind: writer.IfNullReturnNull(resultName); break;
					case MatchAction.Ignore: writer.IfNullReturnNull(resultName); break;
				}
                currentInputPosition = $"{currentInputPosition} + {resultName}.Advanced";
            }

			var returnExpression = GetReturnExpression(target.ReturnType, returnedResults, $"{currentInputPosition} - inputPosition", "factory", target.InterfaceMethod);
			if (input.IsMemoized)
			{
				var memField = NameGen.MemoizedFieldName(input.Name);
				returnExpression = $"states[inputPosition].{memField} = {returnExpression}";
			}
			writer.Return(returnExpression);
		}

		public void Visit(Intrinsic target, Signature input)
		{
			BasicTargetWithInterfaceMethod(target, target.InterfaceMethod, input);
		}

		public void Visit(LiteralString target, Signature input)
		{
			BasicTargetWithInterfaceMethod(target, target.InterfaceMethod, input);
		}

		public void Visit(ReferencedRule target, Signature input)
		{
			BasicTargetWithInterfaceMethod(target, target.InterfaceMethod, input);
		}

		public void Visit(Repetition target, Signature input)
		{
			String methodName;
			switch (target.Cardinality)
			{
				case Cardinality.Maybe: methodName = "ParseMaybe"; break;
				case Cardinality.Plus: methodName = "ParsePlus"; break;
				case Cardinality.Star: methodName = "ParseStar"; break;
				default: throw new InvalidOperationException();
			}

			var innerFunc = target.InnerParseFunction;
			var innerInvocation = this.invokers[innerFunc]("i", "p", "s", "f"); // Invocation
			var invocation = $"{methodName}(input, inputPosition, states, factory, (i, p, s, f) => {innerInvocation})"; // Invocation

			var resultName = "result";
			writer.VarAssign(resultName, invocation);
			writer.IfNullReturnNull(resultName);

			var nodeReference = $"{resultName}.Node";
			var decl = new Decl(innerFunc.ReturnType, nodeReference);

			var returnExpression = GetReturnExpression(target.ReturnType, new[] { decl }, $"{resultName}.Advanced", "factory", target.InterfaceMethod);
			if (input.IsMemoized)
			{
				var memField = NameGen.MemoizedFieldName(input.Name);
				returnExpression = $"states[inputPosition].{memField} = {returnExpression}";
			}
			writer.Return(returnExpression);
		}

		public void Visit(Series target, Signature input)
		{
			var methodName = "ParseSeries";

			var repeatedFunc = target.RepeatedToken;
			var repeatedInvocation = this.invokers[repeatedFunc]("i", "p", "s", "f"); // Invocation

			var delimFunc = target.DelimiterToken;
			var delimInvocation = this.invokers[delimFunc]("i", "p", "s", "f"); // Invocation

			var invocation = $"{methodName}(input, inputPosition, states, factory, (i, p, s, f) => {repeatedInvocation}, (i, p, s, f) => {delimInvocation})"; // Invocation

			var resultName = "result";
			writer.VarAssign(resultName, invocation);
			writer.IfNullReturnNull(resultName);

			var nodeReference = $"{resultName}.Node";
			var decl = new Decl(target.ReturnType, nodeReference);

			var returnExpression = GetReturnExpression(target.ReturnType, new[] { decl }, $"{resultName}.Advanced", "factory", target.InterfaceMethod);
			if (input.IsMemoized)
			{
				var memField = NameGen.MemoizedFieldName(input.Name);
				returnExpression = $"states[inputPosition].{memField} = {returnExpression}";
			}
			writer.Return(returnExpression);
		}
	}
}
