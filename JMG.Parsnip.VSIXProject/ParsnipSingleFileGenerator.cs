using JMG.Parsnip.VSIXProject.CodeGeneration;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject
{
	internal class ParsnipSingleFileGenerator : IVsSingleFileGenerator
	{
		public static String GUID_STRING_BRACES = "{381F20E9-2A4C-4F5B-8355-58A76B23DDCA}";
		private CodeGenerator codeGen;

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
			var fileName = Path.GetFileNameWithoutExtension(wszInputFilePath);
			var className = NameGen.ClassName(fileName);

			return $"{className} - {wszInputFilePath}";

			var syntacticModel = SyntacticModel.Parser.Parse(wszInputFilePath);
		}
	}
}
