using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.CodeWriting;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject.SerializedModel
{
	internal class ParsnipCode
	{
		public IReadOnlyDictionary<IParseFunction, Invoker> Invokers => invokers;
		private Dictionary<IParseFunction, Invoker> invokers = new Dictionary<IParseFunction, Invoker>();

		public IReadOnlyDictionary<String, String> RuleMethodNames => ruleMethodNames;
		private Dictionary<String, String> ruleMethodNames = new Dictionary<String, String>();

		public IReadOnlyDictionary<IntrinsicType, Signature> Intrinsics => intrinsics;
		private Dictionary<IntrinsicType, Signature> intrinsics = new Dictionary<IntrinsicType, Signature>();

		private List<Signature> methodItems = new List<Signature>();

		public ParsnipCode()
		{
			AddIntrinsic(IntrinsicType.AnyCharacter, "Parse_Intrinsic_AnyCharacter");
			AddIntrinsic(IntrinsicType.AnyLetter, "Parse_Intrinsic_AnyLetter");
			AddIntrinsic(IntrinsicType.EndOfLine, "Parse_Intrinsic_EndOfLine");
			AddIntrinsic(IntrinsicType.EndOfStream, "Parse_Intrinsic_EndOfStream");
			AddIntrinsic(IntrinsicType.CString, "Parse_Intrinsic_CString");
		}

		private void AddIntrinsic(IntrinsicType type, String name)
		{
			var sig = new Signature(name, Access.Private, new Intrinsic(IntrinsicType.AnyCharacter), (s, f) => $"{name}({s}, {f})");
			this.methodItems.Add(sig);
			this.intrinsics[type] = sig;
		}

		public void AddSignature(Signature signature) => this.methodItems.Add(signature);
		public void MapFunctionInvocation(IParseFunction function, Invoker invoker) => this.invokers[function] = invoker;

		public void WriteMethods(CodeWriter writer, String interfaceName, ParsnipModel semanticModel)
		{
			// Populate dictionary of rule identifiers to method names
			foreach (var rule in semanticModel.Rules)
			{
				var methodName = NameGen.ParseFunctionMethodName(rule.RuleIdentifier);
				this.ruleMethodNames[rule.RuleIdentifier] = methodName;
			}

			//
			var methodAccess = Access.Public;
			foreach (var rule in semanticModel.Rules)
			{
				var methodName = this.RuleMethodNames[rule.RuleIdentifier];

				//this.GenerateMethods(methodName, methodAccess, rule.ParseFunction);
				rule.ParseFunction.ApplyVisitor(new GenerateSignaturesVisitor(this, methodName));
				foreach (var i in methodItems)
				{
					invokers[i.Func] = i.Invoker;
				}

				// Only the first method is public
				methodAccess = Access.Private;
			}

			//
			var vis = new GenerateMethodsVisitor(this, writer, interfaceName);
			foreach (var methodItem in this.methodItems)
			{
				writer.EndOfLine();

				var func = methodItem.Func;
				func.ApplyVisitor(vis, methodItem);
			}
		}
	}
}
