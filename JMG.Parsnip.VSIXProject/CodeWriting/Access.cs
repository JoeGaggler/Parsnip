using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.CodeWriting
{
	public enum Access { Public, Private }

	public static class AccessExtensions
	{
		public static String ToAccessString(this Access access)
		{
			switch (access)
			{
				case Access.Public: return "public";
				case Access.Private: return "private";
				default: throw new InvalidOperationException("Invalid Access");
			}
		}
	}
}
