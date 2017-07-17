using JMG.Parsnip.VSIXProject.CodeWriting;
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

			var codeGen = new CodeWriter();

			var versionString = typeof(ParsnipSingleFileGenerator).Assembly
				.GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), false)
				.OfType<System.Reflection.AssemblyInformationalVersionAttribute>()
				.FirstOrDefault()?.InformationalVersion
				?? "unknown";

			var items = new List<IFileScopeItem>
			{
				new Comment("Code Generated via Parsnip Packrat Parser Producer"),
				new Comment($"Version: {versionString}"),
				new Comment($"File: {fileName}"),
				new Comment($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"),
				new EmptyLine(),
				new UsingNamespace("System"),
				new UsingNamespace("System.Linq"),
				new UsingNamespace("System.Diagnostics"),
				new UsingNamespace("System.Threading.Tasks"),
				new UsingNamespace("System.Collections.Generic"),
				new EmptyLine(),
			};

			var items2 = CodeGenerator.FileScopeItemsForSemanticModel(semanticModel, wszDefaultNamespace, className);
			var items3 = items.Appending(items2);
			var fileScope = new FileScope(items3);

			return codeGen.WriteFileScope(fileScope);
		}
	}
}
