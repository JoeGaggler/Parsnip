using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.SemanticModel.Transformations
{
	/// <summary>
	/// Assigns rule factory methods to all of the grammar rules
	/// </summary>
	/// <remarks>
	/// This should be run after all other rule transformations, otherwise the signature of the factory method may no longer match the transformed rule.
	/// </remarks>
	internal static class AssignRuleFactoryMethods
	{
		public static ParsnipModel Go(ParsnipModel model)
		{
			var oldRules = model.Rules;
			foreach (var oldRule in oldRules)
			{
				var interfaceMethodName = NameGen.InterfaceMethodName(oldRule.RuleIdentifier);
				var vis = new Visitor(interfaceMethodName);
				(var newFunc, var newMethods) = oldRule.ParseFunction.ApplyVisitor(vis, oldRule.ReturnType);
				var newRule = oldRule.WithParseFunction(newFunc);
				model = model.ReplacingRule(oldRule, newRule);
				foreach (var method in newMethods)
				{
					model = model.AddingInterfaceMethod(method);
				}
			}
			return model;
		}

		private class Visitor : IParseFunctionFuncVisitor<INodeType, (IParseFunction, IReadOnlyList<InterfaceMethod>)>
		{
			private String interfaceMethodName;
			private Int32 count = 0;

			public Visitor(String interfaceMethodName)
			{
				this.interfaceMethodName = interfaceMethodName;
			}

			private IReadOnlyList<INodeType> GetParameterTypesFromReturnType(INodeType funcReturnType)
			{
				if (funcReturnType == EmptyNodeType.Instance)
				{
					return new INodeType[0];
				}
				else if (funcReturnType is TupleNodeType tupleNodeType)
				{
					return tupleNodeType.Types;
				}
				else
				{
					return new[] { funcReturnType };
				}
			}

			private InterfaceMethod GenerateInterfaceMethod(INodeType inputType, INodeType outputType)
			{
				var name = $"{interfaceMethodName}{++count}";
				var parameterTypes = GetParameterTypesFromReturnType(inputType);
				if (parameterTypes.Count == 0)
				{
					return null;
				}

				return new InterfaceMethod(outputType, name, parameterTypes);
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(Selection target, INodeType input)
			{
				var interfaceMethods = new List<InterfaceMethod>();
				var newSteps = new List<SelectionStep>();
				foreach (var step in target.Steps)
				{
					IParseFunction func = step.Function;
					INodeType funcReturnType = func.ReturnType;

					InterfaceMethod interfaceMethod = null;
					if (input != EmptyNodeType.Instance)
					{
						var name = $"{interfaceMethodName}{++count}";
						interfaceMethod = new InterfaceMethod(input, name, GetParameterTypesFromReturnType(funcReturnType));
						interfaceMethods.Add(interfaceMethod);
					}

					var newStep = new SelectionStep(func, interfaceMethod);
					newSteps.Add(newStep);
				}

				return (new Selection(newSteps), interfaceMethods);
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(Sequence target, INodeType input)
			{
				var interfaceMethods = new List<InterfaceMethod>();

				InterfaceMethod interfaceMethod = null;
				var types = target.Steps.Where(i => i.IsReturned).Select(i => i.Function.ReturnType).ToList();
				if (types.Count > 0)
				{
					var name = $"{interfaceMethodName}{++count}";
					interfaceMethod = new InterfaceMethod(input, name, types);
					interfaceMethods.Add(interfaceMethod);
				}

				return (new Sequence(target.Steps, interfaceMethod), interfaceMethods);
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(Intrinsic target, INodeType input)
			{
				var interfaceMethod = GenerateInterfaceMethod(target.ReturnType, input);
				if (interfaceMethod == null)
				{
					return (target, new InterfaceMethod[0]);
				}
				return (new Intrinsic(target.Type, interfaceMethod), new InterfaceMethod[] { interfaceMethod });
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(LiteralString target, INodeType input)
			{
				var interfaceMethod = GenerateInterfaceMethod(target.ReturnType, input);
				if (interfaceMethod == null)
				{
					return (target, new InterfaceMethod[0]);
				}
				return (new LiteralString(target.Text, interfaceMethod), new InterfaceMethod[] { interfaceMethod });
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(ReferencedRule target, INodeType input)
			{
				var interfaceMethod = GenerateInterfaceMethod(target.ReturnType, input);
				if (interfaceMethod == null)
				{
					return (target, new InterfaceMethod[0]);
				}
				return (new ReferencedRule(target.Identifier, target.ReturnType, interfaceMethod), new InterfaceMethod[] { interfaceMethod });
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(Repetition target, INodeType input)
			{
				var interfaceMethod = GenerateInterfaceMethod(target.ReturnType, input);
				if (interfaceMethod == null)
				{
					return (target, new InterfaceMethod[0]);
				}
				return (new Repetition(target.InnerParseFunction, target.Cardinality, interfaceMethod), new InterfaceMethod[] { interfaceMethod });
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(Series target, INodeType input)
			{
				var interfaceMethod = GenerateInterfaceMethod(target.ReturnType, input);
				if (interfaceMethod == null)
				{
					return (target, new InterfaceMethod[0]);
				}
				return (new Series(target.RepeatedToken, target.DelimiterToken, interfaceMethod), new InterfaceMethod[] { interfaceMethod });
			}
		}
	}
}
