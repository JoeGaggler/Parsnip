using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.Visiting
{
	public static class AggregateExtensions
	{
		public static void ApplyVisitor<TVisitor>(this IEnumerable<IActionVisitable<TVisitor>> items, TVisitor visitor)
		{
			foreach (var item in items)
			{
				item.ApplyVisitor(visitor);
			}
		}
	}
}
