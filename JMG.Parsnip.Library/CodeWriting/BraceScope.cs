using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	internal class BraceScope : IDisposable
	{
		private CodeWriter codeGenerator;
		private String closingBrace;
		private IDisposable scope;

		public BraceScope(CodeWriter codeGenerator, String withClosingBrace = null)
		{
			this.codeGenerator = codeGenerator;
			this.closingBrace = "}" + (withClosingBrace ?? String.Empty);

			this.codeGenerator.LineOfCode("{");
			this.scope = this.codeGenerator.IndentedScope();
		}

		public void Dispose()
		{
			this.scope.Dispose();
			this.codeGenerator.LineOfCode(this.closingBrace);
		}
	}
}
