using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.CodeWriting
{
	/// <summary>
	/// C# Access Modifiers
	/// </summary>
	public enum Access
	{
		/// <summary>
		/// public access modifier
		/// </summary>
		Public,

		/// <summary>
		/// private access modifier
		/// </summary>
		Private,

		/// <summary>
		/// internal access modifier
		/// </summary>
		Internal
	}

	/// <summary>
	/// Extensions for <see cref="Access"/>
	/// </summary>
	public static class AccessExtensions
	{
		/// <summary>
		/// Gets the string for an access modifier
		/// </summary>
		/// <param name="access">Access modifier</param>
		/// <returns>String representation of the access modifier</returns>
		public static String ToAccessString(this Access access)
		{
			switch (access)
			{
				case Access.Public: return "public";
				case Access.Private: return "private";
				case Access.Internal: return "internal";
				default: throw new InvalidOperationException("Invalid Access");
			}
		}
	}
}
