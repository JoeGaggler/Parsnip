using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel.Transformations
{
	internal static class AssignRuleFactoryMethods
	{
		public static ParsnipModel Go(ParsnipModel model)
		{
			var oldRules = model.Rules;
			foreach (var oldRule in oldRules)
			{
				var interfaceMethodName = NameGen.ParseFunctionMethodName(oldRule.RuleIdentifier);
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
						count++;
						var name = $"{interfaceMethodName}{count}";

						if (funcReturnType == EmptyNodeType.Instance)
						{
							interfaceMethod = new InterfaceMethod(input, name, new INodeType[0]);
						}
						else
						{
							interfaceMethod = new InterfaceMethod(input, name, new[] { funcReturnType });
						}
						interfaceMethods.Add(interfaceMethod);
					}

					var newStep = new SelectionStep(func, interfaceMethod);
					newSteps.Add(newStep);
				}

				return (new Selection(target.IsMemoized, newSteps, input), interfaceMethods);
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(Sequence target, INodeType input)
			{
				return (target.WithFactoryReturnType(input), new InterfaceMethod[0]);
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(Intrinsic target, INodeType input)
			{
				throw new NotImplementedException();
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(LiteralString target, INodeType input)
			{
				throw new NotImplementedException();
			}

			public (IParseFunction, IReadOnlyList<InterfaceMethod>) Visit(ReferencedRule target, INodeType input)
			{
				throw new NotImplementedException();
			}
		}
	}
}
