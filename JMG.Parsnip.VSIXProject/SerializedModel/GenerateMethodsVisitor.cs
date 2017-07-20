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

		private static String GetParseResultTypeString(INodeType returnType) => $"ParseResult<{NameGen.TypeString(returnType)}>";
		private static String GetParseResultTypeString(IParseFunction target) => $"ParseResult<{NameGen.TypeString(target.ReturnType)}>";

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

		private IDisposable MethodStuff(IParseFunction target, Access access, String name, String stateName, IReadOnlyList<LocalVarDecl> parameters, Boolean isMemoized)
		{
			var returnType = GetParseResultTypeString(target);
			var memName = $"Mem_{name}";

			if (isMemoized)
			{
				writer.LineOfCode($"private {returnType} {memName};");
			}

			var disposable = writer.Method(access, true, returnType, name, parameters);

			if (isMemoized)
			{
				writer.LineOfCode($"if ({stateName}.{memName} != null) {{ return {stateName}.{memName}; }}");
				writer.EndOfLine();
			}

			return disposable;
		}

		private String GetReturnStatement(INodeType returnType, IReadOnlyList<Decl> nodes, String stateReference, String factoryName, InterfaceMethod interfaceMethod)
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

			var returnTypeString = GetParseResultTypeString(returnType);
			return ($"return new {returnTypeString}() {{ Node = {nodeString}, State = {stateReference} }};");
		}

		public void Visit(Selection target, Signature input)
		{
			var returnType = GetParseResultTypeString(target);
			using (MethodStuff(target, input.Access, input.Name, "state", typicalParams, input.IsMemoized))
			{
				int stepIndex = 0;
				foreach (var step in target.Steps)
				{
					stepIndex++;
					var func = step.Function;
					var invoker = this.parsnipCode.Invokers[func];
					var resultName = $"r{stepIndex}";

					writer.VarAssign(resultName, invoker("state", "factory"));

					var factoryName = "factory";
					var interfaceMethod = step.InterfaceMethod;
					var nodeReference = $"{resultName}.Node";

					var decl = new Decl(func.ReturnType, nodeReference);

					writer.LineOfCode($"if ({resultName} != null) {GetReturnStatement(target.ReturnType, new[] { decl }, $"{resultName}.State", factoryName, interfaceMethod)}");
				}

				// No choices matched
				writer.Return("null");
			}
		}

		public void Visit(Sequence target, Signature input)
		{
			var returnType = GetParseResultTypeString(target);
			using (MethodStuff(target, input.Access, input.Name, "state", typicalParams, input.IsMemoized))
			{
				var returnedResults = new List<Decl>();

				int stepIndex = 0;
				var currentState = "state";
				foreach (var step in target.Steps)
				{
					stepIndex++;
					var func = step.Function;
					var type = func.ReturnType;
					var invoker = this.parsnipCode.Invokers[func];
					var resultName = $"r{stepIndex}";
					var nodeName = $"{resultName}.Node";
					var nextState = $"{resultName}.State";

					if (step.IsReturned)
					{
						returnedResults.Add(new Decl(type, nodeName));
					}

					writer.VarAssign(resultName, invoker(currentState, "factory"));

					switch (step.MatchAction)
					{
						case MatchAction.Consume: writer.IfNullReturnNull(resultName); currentState = nextState; break;
						case MatchAction.Fail: writer.IfNotNullReturnNull(resultName); currentState = nextState; break;
						case MatchAction.Rewind: writer.IfNullReturnNull(resultName); break;
						case MatchAction.Ignore: writer.IfNullReturnNull(resultName); currentState = nextState; break;
					}
				}

				writer.LineOfCode(GetReturnStatement(target.ReturnType, returnedResults, currentState, "factory", target.InterfaceMethod));
			}
		}

		public void Visit(Intrinsic target, Signature input)
		{
			var returnType = GetParseResultTypeString(target);
			using (MethodStuff(target, input.Access, input.Name, "state", typicalParams, input.IsMemoized))
			{
				writer.Comment("TODO: Intrinsic");
			}
		}

		public void Visit(LiteralString target, Signature input)
		{
			var returnType = GetParseResultTypeString(target);
			using (MethodStuff(target, input.Access, input.Name, "state", typicalParams, input.IsMemoized))
			{
				writer.Comment("TODO: LiteralString");
			}
		}

		public void Visit(ReferencedRule target, Signature input)
		{
			var returnType = GetParseResultTypeString(target);
			using (MethodStuff(target, input.Access, input.Name, "state", typicalParams, input.IsMemoized))
			{
				writer.Comment("TODO: ReferencedRule");
			}
		}

		public void Visit(CardinalityFunction target, Signature input)
		{
			var returnType = GetParseResultTypeString(target);
			using (MethodStuff(target, input.Access, input.Name, "state", typicalParams, input.IsMemoized))
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
				var innerInvocation = parsnipCode.Invokers[innerFunc]("s", "f");
				var invocation = $"{methodName}(state, factory, (s, f) => {innerInvocation})";

				var resultName = "result";
				writer.VarAssign(resultName, invocation);
				writer.IfNullReturnNull(resultName);

				var nodeReference = $"{resultName}.Node";
				var decl = new Decl(innerFunc.ReturnType, nodeReference);

				writer.LineOfCode(GetReturnStatement(target.ReturnType, new[] { decl }, $"{resultName}.State", "factory", target.InterfaceMethod));
			}
		}
	}
}
