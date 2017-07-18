using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Extensions;
using JMG.Parsnip.VSIXProject.CodeWriting;
using JMG.Parsnip.VSIXProject.SemanticModel;

namespace JMG.Parsnip.VSIXProject
{
	internal class ParsnipSingleFileGenerator : IVsSingleFileGenerator
	{
		public static String GUID_STRING_BRACES = "{381F20E9-2A4C-4F5B-8355-58A76B23DDCA}";
		private CodeWriter codeGen;

		Int32 IVsSingleFileGenerator.DefaultExtension(out String pbstrDefaultExtension)
		{
			pbstrDefaultExtension = ".cs";
			return VSConstants.S_OK;
		}

		Int32 IVsSingleFileGenerator.Generate(String wszInputFilePath, String bstrInputFileContents, String wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out UInt32 pcbOutput, IVsGeneratorProgress pGenerateProgress)
		{
			String result = Generate(bstrInputFileContents, wszDefaultNamespace, wszInputFilePath);
			var bytes = Encoding.UTF8.GetBytes(result);

			int outputLength = bytes.Length;
			rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(outputLength);
			Marshal.Copy(bytes, 0, rgbOutputFileContents[0], outputLength);
			pcbOutput = (uint)outputLength;

			return VSConstants.S_OK;
		}

		private String Generate(String bstrInputFileContents, String wszDefaultNamespace, String wszInputFilePath)
		{
			var fileName = Path.GetFileName(wszInputFilePath);
			var fileBaseName = Path.GetFileNameWithoutExtension(wszInputFilePath);
			var className = NameGen.ClassName(fileBaseName);

			var syntacticModel = SyntacticModel.Parser.Parse(wszInputFilePath);
			var semanticModel = SemanticModel.Analyzer.Analyze(syntacticModel);

			var writer = new CodeWriter();

			var versionString = typeof(ParsnipSingleFileGenerator).Assembly
				.GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), false)
				.OfType<System.Reflection.AssemblyInformationalVersionAttribute>()
				.FirstOrDefault()?.InformationalVersion
				?? "unknown";

			writer.Comment("Code Generated via Parsnip Packrat Parser Producer");
			writer.Comment($"Version: {versionString}");
			writer.Comment($"File: {fileName}");
			writer.Comment($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
			writer.EndOfLine();
			writer.UsingNamespace("System");
			writer.UsingNamespace("System.Linq");
			writer.UsingNamespace("System.Diagnostics");
			writer.UsingNamespace("System.Threading.Tasks");
			writer.UsingNamespace("System.Collections.Generic");
			writer.EndOfLine();

			using (writer.Namespace(wszDefaultNamespace))
			{
				var interfaceName = $"I{className}RuleFactory";
				using (writer.Interface(interfaceName, Access.Public))
				{

				}
				writer.EndOfLine();

				using (writer.Class(className, Access.Public))
				{
					writer.LineOfCode("private class ParseResult<T> { public T Node; public PackratState State; }");
					writer.EndOfLine();
					writer.LineOfCode("private class EmptyNode { private EmptyNode() { } public static EmptyNode Instance = new EmptyNode(); }");
					writer.EndOfLine();
					var firstRule = semanticModel.Rules[0];
					var firstRuleReturnType = NameGen.TypeString(firstRule.ReturnType);
					var firstRuleParseMethodName = NameGen.ParseFunctionMethodName(firstRule.RuleIdentifier);
					using (writer.Method(Access.Public, true, firstRuleReturnType, "Parse", new[] {
						new LocalVarDecl("String", "input"),
						new LocalVarDecl("IParsnipRuleFactory", "factory"),
					}))
					{
						writer.Assign("var states", "new PackratState[input.Length + 1]");
						writer.LineOfCode("Enumerable.Range(0, input.Length + 1).ToList().ForEach(i => states[i] = new PackratState(input, i, states, factory));");
						writer.Assign("var state", "states[0]");
						writer.Assign("var result", $"PackratState.{firstRuleParseMethodName}(state, factory)");
						writer.LineOfCode("if (result == null) return null;");
						writer.Return("result.Node");
					}
					writer.EndOfLine();
					var packratStateClassName = "PackratState";
					using (writer.Class(packratStateClassName, Access.Private))
					{
						writer.LineOfCode("private readonly string input;");
						writer.LineOfCode("private readonly int inputPosition;");
						writer.LineOfCode("private readonly PackratState[] states;");
						writer.LineOfCode("private readonly IParsnipRuleFactory factory;");
						writer.EndOfLine();
						using (writer.Constructor(Access.Public, packratStateClassName, new[] {
							new LocalVarDecl("String", "input"),
							new LocalVarDecl("Int32", "inputPosition"),
							new LocalVarDecl("PackratState[]", "states"),
							new LocalVarDecl("IParsnipRuleFactory", "factory")
						}))
						{
							writer.Assign("this.input", "input");
							writer.Assign("this.inputPosition", "inputPosition");
							writer.Assign("this.states", "states");
							writer.Assign("this.factory", "factory");
						}

						var methodAccess = Access.Public;
						var parameters = new List<LocalVarDecl>
						{
							new LocalVarDecl("PackratState", "state"),
							new LocalVarDecl(interfaceName, "factory")
						};

						var things = new List<MethodItem>();
						foreach (var rule in semanticModel.Rules)
						{
							var methodName = NameGen.ParseFunctionMethodName(rule.RuleIdentifier);

							GenerateMethods(things, methodName, methodAccess, rule.ParseFunction);

							// Only the first method is public
							methodAccess = Access.Private;
						}

						int attempt = 0;
						while (things.Count > 0)
						{
							attempt++;
							var thing = things[0];
							things.RemoveAt(0);

							writer.EndOfLine();
							var returnType = $"ParseResult<{NameGen.TypeString(thing.Func.ReturnType)}>";
							using (writer.Method(thing.Access, false, returnType, thing.Name, parameters))
							{

							}
						}
					}
				}
				writer.EndOfLine();
			}

			return writer.GetText();
		}

		private class MethodItem
		{
			public MethodItem(String name, Access access, SemanticModel.IParseFunction func)
			{
				this.Name = name;
				this.Access = access;
				this.Func = func;
			}

			public String Name { get; }
			public Access Access { get; }
			public IParseFunction Func { get; }
		}

		private void GenerateMethods(List<MethodItem> things, String baseName, Access access, IParseFunction func)
		{
			var visitor = new ThingVisitor(things, baseName);
			if (func != null)
			{
				func.ApplyVisitor(visitor);
			}
			else
			{

			}
		}

		private class ThingVisitor : IParseFunctionActionVisitor
		{
			private readonly List<MethodItem> things;
			private readonly String baseName;

			public ThingVisitor(List<MethodItem> things, String baseName)
			{
				this.things = things;
				this.baseName = baseName;
			}

			public void Visit(Selection target)
			{
				things.Add(new MethodItem(baseName, Access.Private, target));

				int index = 0;
				foreach (var step in target.Steps)
				{
					index++;
					String stepBaseName = $"{baseName}_C{index}";
					IParseFunction func = step.Function;


					var visitor = new ThingVisitor(things, stepBaseName);
					func.ApplyVisitor(visitor);
				}
			}

			public void Visit(Sequence target)
			{
				things.Add(new MethodItem(baseName, Access.Private, target));

				int index = 0;
				foreach (var step in target.Steps)
				{
					index++;
					String stepBaseName = $"{baseName}_S{index}";
					IParseFunction func = step.Function;


					var visitor = new ThingVisitor(things, stepBaseName);
					func.ApplyVisitor(visitor);
				}
			}

			public void Visit(Intrinsic target)
			{
				// No need to generate method for Intrinsic
			}

			public void Visit(LiteralString target)
			{
				// No need to generate method for LiteralString
			}

			public void Visit(ReferencedRule target)
			{
				// No need to generate method for ReferencedRule
			}
		}
	}
}
