using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Extensions;

namespace JMG.Parsnip.VSIXProject.SyntacticModel
{
	/// <summary>
	/// Mediator between the IVsSingleFileGenerator.Generate method, and the 
	/// </summary>
	internal static class Parser
	{
		public static ParsnipDefinition Parse(string wszInputFilePath)
		{
			var inputString = System.IO.File.ReadAllText(wszInputFilePath);
			var definition = Generated.Parsnip.Parse(inputString, new Generated.ParsnipRuleFactory());
			return definition;
		}
	}
}
