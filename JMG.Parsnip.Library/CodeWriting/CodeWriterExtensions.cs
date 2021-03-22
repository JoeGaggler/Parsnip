using System;

namespace JMG.Parsnip.CodeWriting
{
	internal static class CodeWriterExtensions
	{
		public static void Assign(this CodeWriter writer, String left, String right) => writer.LineOfCode($"{left} = {right};");

		public static void VarAssign(this CodeWriter writer, String left, String right) => writer.LineOfCode($"var {left} = {right};");

		public static void IfNotNullReturnNull(this CodeWriter writer, String operand) => writer.LineOfCode($"if ({operand} != null) {{ return null; }}");

		public static void IfNullReturnNull(this CodeWriter writer, String operand) => writer.LineOfCode($"if ({operand} == null) {{ return null; }}");

		public static void IfTrueReturnNull(this CodeWriter writer, String condition) => writer.LineOfCode($"if ({condition}) {{ return null; }}");
	}
}
