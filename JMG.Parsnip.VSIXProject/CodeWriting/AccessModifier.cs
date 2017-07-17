using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public enum AccessModifier
	{
		Public,
		Private,
		Internal
	}

	public static class AccessModifierExtensions
	{
		public static String ToCSharpString(this AccessModifier access)
		{
			switch (access)
			{
				case AccessModifier.Public: return "public";
				case AccessModifier.Private: return "private";
				case AccessModifier.Internal: return "internal";
				default: throw new InvalidOperationException($"Unexpected access modifier: {access}");
			}
		}
	}
}
