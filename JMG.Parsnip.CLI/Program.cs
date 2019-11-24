using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.CLI
{
    class Program
    {
        static async Task<Int32> Main(string[] args)
        {
            var versionString = typeof(Program).Assembly
                    .GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), false)
                    .OfType<System.Reflection.AssemblyInformationalVersionAttribute>()
                    .FirstOrDefault()?.InformationalVersion
                    ?? "unknown";

            // Bootstrap command, run from "Generated" directory: 
            // ..\..\..\JMG.Parsnip.CLI\bin\Debug\netcoreapp3.0\JMG.Parsnip.exe --namespace "JMG.Parsnip.SyntacticModel.Generated" --name Parsnip --extension ".txt" < Parsnip.parsnip.txt > Parsnip.parsnip.cs
            if (args.Length < 6)
            {
                Console.Error.WriteLine("#error Expected arguments: namespace, name, extension");
                return 1;
            }
            String defaultNamespace = null;
            String nameArg = null;
            String extensionArg = null;

            String currentArg = null;
            foreach (var arg in args)
            {
                switch (currentArg)
                {
                    case "--namespace": defaultNamespace = arg; currentArg = null; break;
                    case "--name": nameArg = arg; currentArg = null; break;
                    case "--extension": extensionArg = arg; currentArg = null; break;
                    case null: currentArg = arg; break;
                }
            }
            if (String.IsNullOrEmpty(defaultNamespace))
            {
                Console.Error.WriteLine("#error Expected namespace argument");
                return 1;
            }
            if (String.IsNullOrEmpty(nameArg))
            {
                Console.Error.WriteLine("#error Expected name argument");
                return 1;
            }
            if (String.IsNullOrEmpty(extensionArg))
            {
                Console.Error.WriteLine("#error Expected extension argument");
                return 1;
            }

            String fileContents;
            using (var inputStream = Console.OpenStandardInput())
            using (var reader = new StreamReader(inputStream))
            {
                fileContents = await reader.ReadToEndAsync();
            }

            var output = VSIXProject.ParsnipGenerator.Generate(fileContents, defaultNamespace, $"{nameArg}{extensionArg}", versionString);
            Console.Out.WriteLine(output);

            return 0;
        }
    }
}
