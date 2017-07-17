using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.Extensions
{
	public static class ListExtensions
	{
		public static IReadOnlyList<T> Empty<T>()
		{
			return new List<T>();
		}

		public static IReadOnlyList<T> Appending<T>(this IReadOnlyList<T> collection, T item)
		{
			return collection.Concat(new[] { item }).ToList();
		}

		public static IReadOnlyList<T> Appending<T>(this IReadOnlyList<T> collection, IReadOnlyList<T> list)
		{
			var newList = new List<T>(collection.Count + list.Count);
			newList.AddRange(collection);
			newList.AddRange(list);
			return newList;
		}

		public static IReadOnlyList<T> Prepending<T>(this IReadOnlyList<T> collection, T item)
		{
			return (new[] { item }).Concat(collection).ToList();
		}

		public static IReadOnlyList<T> Replacing<T>(this IReadOnlyList<T> collection, T oldItem, T newItem) where T : class
		{
			var hasIndex = collection.IndexOfObject(oldItem);
			if (hasIndex is Int32 index)
			{
				var newList = new List<T>(collection);
				newList[index] = newItem;
				return newList;
			}
			else
			{
				throw new InvalidOperationException("Replacing failed because target item was not in the list.");
			}
		}

		public static Int32? IndexOfObject<T>(this IReadOnlyList<T> collection, T item) where T : class
		{
			Int32 index = 0;
			Int32 count = collection.Count;
			while (index < count)
			{
				if (Object.ReferenceEquals(collection[index], item)) return index;
				index++;
			}
			return null;
		}

		public static Boolean Contains<T>(this IReadOnlyList<T> collection, T item) where T : class => null != collection.IndexOfObject(item);
	}
}
