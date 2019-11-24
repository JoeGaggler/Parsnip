using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public class CodeWriter
	{
		private Int32 __INDENTATION_LEVEL = 0;
		private StringBuilder stringBuilder = new StringBuilder();

		private class _IndentationScope : IDisposable
		{
			private readonly CodeWriter writer;

			public _IndentationScope(CodeWriter gen)
			{
				this.writer = gen;
				this.writer.__INDENTATION_LEVEL++;
			}

			public void Dispose()
			{
				this.writer.__INDENTATION_LEVEL--;
			}
		}

		public IDisposable IndentedScope()
		{
			return new _IndentationScope(this);
		}

		public String GetText()
		{
			return this.stringBuilder.ToString();
		}

		public void RawCode(String value)
		{
			this.stringBuilder.Append(value);
		}

		public void LineOfCode(String line)
		{
			this.stringBuilder.Append(new String('\t', __INDENTATION_LEVEL));
			this.stringBuilder.AppendLine(line);
		}

		public void StartLineOfCode(String line)
		{
			this.stringBuilder.Append(new String('\t', __INDENTATION_LEVEL));
			this.stringBuilder.Append(line);
		}

		public void EndOfLine()
		{
			this.stringBuilder.AppendLine();
		}

		public void MemberVariable(String name, String type, Access access)
		{
			LineOfCode($"{access.ToAccessString()} {type} {name};");
		}

		public void UsingNamespace(String namespaceIdentifier)
		{
			LineOfCode($"using {namespaceIdentifier};");
		}

		public void Comment(String comment)
		{
			LineOfCode($"// {comment}");
		}

		public void SwitchBreak()
		{
			LineOfCode("break;");
		}

		public void Continue()
		{
			LineOfCode("continue;");
		}

		public void Throw<TException>() where TException : Exception
		{
			LineOfCode($"throw new {typeof(TException).Name}();");
		}

		public void Return(string returnValue)
		{
			LineOfCode($"return {returnValue};");
		}

		public IDisposable BraceScope()
		{
			return new BraceScope(this);
		}

		public IDisposable Namespace(String namespaceIdentifier)
		{
			return new NamespaceScope(this, namespaceIdentifier);
		}

		public IDisposable Class(String className, Access access)
		{
			LineOfCode($"{access.ToAccessString()} class {className}");
			return new BraceScope(this);
		}

		public IDisposable Interface(String interfaceName, Access access)
		{
			LineOfCode($"{access.ToAccessString()} interface {interfaceName}");
			return new BraceScope(this);
		}

		public IDisposable While(String whileCondition)
		{
			LineOfCode($"while ({whileCondition})");
			return new BraceScope(this);
		}

		public IDisposable DoWhile(String whileCondition)
		{
			LineOfCode("do");
			var whileString = String.Format(" while ({0});", whileCondition);
			return new BraceScope(this, withClosingBrace: whileString);
		}

		public IDisposable Switch(String switchCondition)
		{
			LineOfCode($"switch ({switchCondition})");
			return new BraceScope(this);
		}

		public IDisposable SwitchCase(string caseString)
		{
			LineOfCode($"case {caseString}:");
			return new BraceScope(this);
		}

		public IDisposable SwitchCase(string caseStringFormat, params String[] caseStringArgs)
		{
			var caseString = String.Format(caseStringFormat, caseStringArgs);
			LineOfCode($"case {caseString}:");
			return new BraceScope(this);
		}

		public IDisposable SwitchDefault()
		{
			LineOfCode("default:");
			return new BraceScope(this);
		}

		public IDisposable Constructor(Access access, String typeName, IReadOnlyList<LocalVarDecl> parameters)
		{
			LineOfCode($"{access.ToAccessString()} {typeName}({parameters.GetParameterListString()})");
			return new BraceScope(this);
		}

		public IDisposable Method(Access access, Boolean isStatic, String returnType, String methodName, IReadOnlyList<LocalVarDecl> parameters)
		{
			LineOfCode($"{access.ToAccessString()}{(isStatic ? " static":"")} {returnType} {methodName}({parameters.GetParameterListString()})");
			return new BraceScope(this);
		}

		public IDisposable ForEach(String enumerable)
		{
			LineOfCode($"foreach ({enumerable})");
			return new BraceScope(this);
		}

		public IDisposable If(String condition)
		{
			LineOfCode($"if ({condition})");
			return new BraceScope(this);
		}

		public IDisposable ElseIf(String condition)
		{
			LineOfCode($"else if ({condition})");
			return new BraceScope(this);
		}

		public IDisposable Else()
		{
			LineOfCode("else");
			return new BraceScope(this);
		}
	}
}
