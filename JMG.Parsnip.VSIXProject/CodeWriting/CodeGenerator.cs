using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMG.Parsnip.VSIXProject.Visiting;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	/// <summary>
	/// Generates C# code
	/// </summary>
	internal class CodeWriter
	{
		public String WriteFileScope(FileScope fileScope)
		{
			var sb = new StringBuilder();
			using (TextWriter textWriter = new StringWriter(sb))
			{
				IFileScopeItemVisitor visitor = new FileScopeItemVisitor(textWriter);
				fileScope.Items.ApplyVisitor(visitor);
			}
			return sb.ToString();
		}

		private class FileScopeItemVisitor : IFileScopeItemVisitor
		{
			private TextWriter writer;

			public FileScopeItemVisitor(TextWriter writer)
			{
				this.writer = writer;
			}

			public void Visit(UsingNamespace target) => writer.WriteLine($"using {target.Identifier};");

			public void Visit(Comment target) => writer.WriteLine($"// {target.Text}");

			public void Visit(EmptyLine target) => writer.WriteLine();

			public void Visit(Namespace target)
			{
				writer.WriteLine($"namespace {target.Identifier}");

				using (var scope = new BraceScope(writer, 0))
				{
					var visitor = new NamespaceScopeItemVisitor(writer, 1);
					target.Items.ApplyVisitor(visitor);
				}
			}
		}

		private class NamespaceScopeItemVisitor : INamespaceItemVisitor
		{
			public readonly TextWriter writer;
			public readonly Int32 depth;

			public NamespaceScopeItemVisitor(TextWriter writer, Int32 depth)
			{
				this.writer = writer;
				this.depth = depth;
			}

			public void Visit(Class target)
			{
				writer.WriteIndentation(depth);
				writer.WriteLine($"{target.Access.ToCSharpString()} class {target.Name}");
				using (var scope = new BraceScope(writer, depth))
				{
					var visitor = new ClassScopeItemVisitor(writer, scope.Depth);
					target.Items.ApplyVisitor(visitor);
				}
			}
		}

		private class ClassScopeItemVisitor : IClassScopeItemVisitor
		{
			private TextWriter writer;
			private Int32 depth;

			public ClassScopeItemVisitor(TextWriter writer, Int32 depth)
			{
				this.writer = writer;
				this.depth = depth;
			}
		}
	}
}
