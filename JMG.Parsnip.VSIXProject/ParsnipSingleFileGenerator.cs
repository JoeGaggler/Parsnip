using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace JMG.Parsnip.VSIXProject
{
    [Guid("381F20E9-2A4C-4F5B-8355-58A76B23DDCA")]
    public sealed class ParsnipSingleFileGenerator : BaseCodeGeneratorWithSite
    {
        public override string GetDefaultExtension()
        {
            return ".cs";
        }

        private Boolean TryGetCommand(String inputFileName, out String command, out String baseName, out String extension)
        {
            extension = Path.GetExtension(inputFileName);

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFileName);
            if (String.IsNullOrEmpty(fileNameWithoutExtension))
            {
                command = null;
                baseName = null;
                return false;
            }

            baseName = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);
            var secondExtension = Path.GetExtension(fileNameWithoutExtension);
            if (secondExtension == null || secondExtension.Length < 2)
            {
                command = null;
                return false;
            }

            command = "parsnip";
            return true;
        }

        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            String result = Generate(inputFileContent, this.FileNamespace, inputFileName);
            var bytes = Encoding.UTF8.GetBytes(result);
            return bytes;
        }

        private string Generate(string fileContents, string defaultNamespace, string inputFilePath)
        {
            try
            {
                var folder = Path.GetDirectoryName(inputFilePath);

                if (!TryGetCommand(inputFilePath, out var command, out var baseName, out var extension))
                {
                    return GetErrorHeader() + $"#error Unable to detect intended generator from the file name. Provide the name of the dotnet tool using the secondary file extension like this: MyFile.toolname.txt";
                }

                var inputFileName = Path.GetFileName(inputFilePath);

                String args = $"{command} --namespace \"{defaultNamespace}\" --name \"{baseName}\" --extension \"{extension}\"";

                var processStartInfo = new ProcessStartInfo("dotnet", args);
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.ErrorDialog = false;
                processStartInfo.StandardOutputEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
                processStartInfo.UseShellExecute = false;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.WorkingDirectory = folder;

                var p = Process.Start(processStartInfo);

                using (var writer = p.StandardInput)
                {
                    p.StandardInput.Write(fileContents);
                }

                var outputString = p.StandardOutput.ReadToEnd();
                var errorString = p.StandardError.ReadToEnd();

                var didExit = p.WaitForExit((int)TimeSpan.FromSeconds(10).TotalMilliseconds);
                if (didExit)
                {
                    if (p.ExitCode == 0)
                    {
                        return outputString;
                    }
                    else
                    {
                        return GetErrorHeader() + $"#error The generator returned error code {p.ExitCode}{Environment.NewLine}/*{Environment.NewLine}{errorString}{Environment.NewLine}*/";
                    }
                }
                else
                {
                    p.Kill();
                    return GetErrorHeader() + $"#error The generator failed to complete within ten seconds.";
                }
            }
            catch (Exception exc)
            {
                return GetErrorHeader() + $"#error Failed to launch the generator{Environment.NewLine}/*{Environment.NewLine}{exc.ToString()}{Environment.NewLine}*/";
            }
        }

        public String GetErrorHeader()
        {
            var versionString = typeof(ParsnipSingleFileGenerator).Assembly
                    .GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), false)
                    .OfType<System.Reflection.AssemblyInformationalVersionAttribute>()
                    .FirstOrDefault()?.InformationalVersion
                    ?? "unknown";

            return $"// Parsnip C# Code Generator {versionString}{Environment.NewLine}";
        }
    }
}
