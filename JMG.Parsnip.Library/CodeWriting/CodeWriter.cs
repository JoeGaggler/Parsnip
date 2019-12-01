using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	/// <summary>
	/// Quick-and-dirty way to produce C# code
	/// </summary>
	internal class CodeWriter
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

		internal IDisposable IndentedScope()
		{
			return new _IndentationScope(this);
		}

		/// <summary>
		/// Gets the text that has been produced by this <see cref="CodeWriter"/>
		/// </summary>
		/// <returns></returns>
		public String GetText()
		{
			return this.stringBuilder.ToString();
		}

		/// <summary>
		/// Appends raw code without any processing
		/// </summary>
		/// <param name="value">Code</param>
		public void RawCode(String value)
		{
			this.stringBuilder.Append(value);
		}

		/// <summary>
		/// Writes a line of code with the correct indentation and line ending
		/// </summary>
		/// <param name="line">Code</param>
		public void LineOfCode(String line)
		{
			this.stringBuilder.Append(new String('\t', __INDENTATION_LEVEL));
			this.stringBuilder.AppendLine(line);
		}

		/// <summary>
		/// Write a line of code with the correct indentation, but without the line ending
		/// </summary>
		/// <param name="line">Code</param>
		public void StartLineOfCode(String line)
		{
			this.stringBuilder.Append(new String('\t', __INDENTATION_LEVEL));
			this.stringBuilder.Append(line);
		}

		/// <summary>
		/// Writes a line ending
		/// </summary>
		public void EndOfLine()
		{
			this.stringBuilder.AppendLine();
		}

		/// <summary>
		/// Writes a member variable
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="type">Type</param>
		/// <param name="access">Access modifier</param>
		public void MemberVariable(String name, String type, Access access)
		{
			LineOfCode($"{access.ToAccessString()} {type} {name};");
		}

		/// <summary>
		/// Writes a using statement
		/// </summary>
		/// <param name="namespaceIdentifier">Namespace</param>
		public void UsingNamespace(String namespaceIdentifier)
		{
			LineOfCode($"using {namespaceIdentifier};");
		}

		/// <summary>
		/// Writes a comment
		/// </summary>
		/// <param name="comment">Comment</param>
		public void Comment(String comment)
		{
			LineOfCode($"// {comment}");
		}

		/// <summary>
		/// Writes a break statement
		/// </summary>
		public void SwitchBreak()
		{
			LineOfCode("break;");
		}

		/// <summary>
		/// Writes a continue statement
		/// </summary>
		public void Continue()
		{
			LineOfCode("continue;");
		}

		/// <summary>
		/// Writes a return statement
		/// </summary>
		/// <param name="returnValue"></param>
		public void Return(string returnValue)
		{
			LineOfCode($"return {returnValue};");
		}

		/// <summary>
		/// Write a brace scope
		/// </summary>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable BraceScope()
		{
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes a namespace block
		/// </summary>
		/// <param name="namespaceIdentifier">Namespace</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable Namespace(String namespaceIdentifier)
		{
			return new NamespaceScope(this, namespaceIdentifier);
		}

		/// <summary>
		/// Writes a class
		/// </summary>
		/// <param name="className">Name</param>
		/// <param name="access">Access modifier</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable Class(String className, Access access)
		{
			LineOfCode($"{access.ToAccessString()} class {className}");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes an interface
		/// </summary>
		/// <param name="interfaceName">Name</param>
		/// <param name="access">Access modifier</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable Interface(String interfaceName, Access access)
		{
			LineOfCode($"{access.ToAccessString()} interface {interfaceName}");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes a while block
		/// </summary>
		/// <param name="whileCondition">Condition</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable While(String whileCondition)
		{
			LineOfCode($"while ({whileCondition})");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes a do-while block
		/// </summary>
		/// <param name="whileCondition">Condition</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable DoWhile(String whileCondition)
		{
			LineOfCode("do");
			var whileString = String.Format(" while ({0});", whileCondition);
			return new BraceScope(this, withClosingBrace: whileString);
		}

		/// <summary>
		/// Writes a switch block
		/// </summary>
		/// <param name="switchCondition">Condition</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable Switch(String switchCondition)
		{
			LineOfCode($"switch ({switchCondition})");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes a case block
		/// </summary>
		/// <param name="caseString">Case</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable SwitchCase(string caseString)
		{
			LineOfCode($"case {caseString}:");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes a default block
		/// </summary>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable SwitchDefault()
		{
			LineOfCode("default:");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes a constructor
		/// </summary>
		/// <param name="access">Access modifier</param>
		/// <param name="typeName">Name</param>
		/// <param name="parameters">Parameters</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable Constructor(Access access, String typeName, IReadOnlyList<LocalVarDecl> parameters)
		{
			LineOfCode($"{access.ToAccessString()} {typeName}({parameters.GetParameterListString()})");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes a method
		/// </summary>
		/// <param name="access">Access modifier</param>
		/// <param name="isStatic">Static</param>
		/// <param name="returnType">Return type</param>
		/// <param name="methodName">Name</param>
		/// <param name="parameters">Parameters</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable Method(Access access, Boolean isStatic, String returnType, String methodName, IReadOnlyList<LocalVarDecl> parameters)
		{
			LineOfCode($"{access.ToAccessString()}{(isStatic ? " static":"")} {returnType} {methodName}({parameters.GetParameterListString()})");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes a for-each block
		/// </summary>
		/// <param name="enumerable">Enumerable</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable ForEach(String enumerable)
		{
			LineOfCode($"foreach ({enumerable})");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes an if block
		/// </summary>
		/// <param name="condition">Condition</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable If(String condition)
		{
			LineOfCode($"if ({condition})");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes an else-if block
		/// </summary>
		/// <param name="condition">Condition</param>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable ElseIf(String condition)
		{
			LineOfCode($"else if ({condition})");
			return new BraceScope(this);
		}

		/// <summary>
		/// Writes an else block
		/// </summary>
		/// <returns>Disposable that writes the closing brace when disposed</returns>
		public IDisposable Else()
		{
			LineOfCode("else");
			return new BraceScope(this);
		}
	}
}
