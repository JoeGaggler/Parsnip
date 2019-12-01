using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.Extensions
{
	/// <summary>
	/// Extensions for <see cref="List{T}"/>
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Creates an empty list
		/// </summary>
		/// <typeparam name="T">Type of list</typeparam>
		/// <returns>Empty list</returns>
		public static IReadOnlyList<T> Empty<T>() => new List<T>();

		/// <summary>
		/// Creates a new read-only list by mapping from a list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="list"></param>
		/// <param name="func"></param>
		/// <returns>New list</returns>
		public static IReadOnlyList<TOutput> SelectReadOnlyList<T, TOutput>(this IReadOnlyList<T> list, Func<T, TOutput> func)
		{
			var newList = new List<TOutput>(list.Count);
			foreach (var item in list)
			{
				newList.Add(func(item));
			}
			return newList;
		}

		/// <summary>
		/// Creates a new read-only list that appends a new item
		/// </summary>
		/// <typeparam name="T">Item type</typeparam>
		/// <param name="collection">Original list</param>
		/// <param name="item">Appended item</param>
		/// <returns>New list</returns>
		public static IReadOnlyList<T> Appending<T>(this IReadOnlyList<T> collection, T item) => collection.Concat(new[] { item }).ToList();

		/// <summary>
		/// Indicates if the collection only contains items that are all equatable
		/// </summary>
		/// <typeparam name="T">Item type</typeparam>
		/// <param name="collection">List</param>
		/// <returns>True if all of the items are equatable</returns>
		public static Boolean AllEqual<T>(this IReadOnlyList<T> collection) where T : IEquatable<T>
		{
			if (collection.Count < 2) return true;

			IEquatable<T> first = collection[0];
			return collection.All(i => first.Equals(i));
		}

		/// <summary>
		/// Creates a new read-only list that appends another list
		/// </summary>
		/// <typeparam name="T">Item type</typeparam>
		/// <param name="collection">Original list</param>
		/// <param name="list">Appended list</param>
		/// <returns>New list</returns>
		public static IReadOnlyList<T> Appending<T>(this IReadOnlyList<T> collection, IReadOnlyList<T> list)
		{
			var newList = new List<T>(collection.Count + list.Count);
			newList.AddRange(collection);
			newList.AddRange(list);
			return newList;
		}

		/// <summary>
		/// Creates a new read-only list that prepends a new item
		/// </summary>
		/// <typeparam name="T">Item type</typeparam>
		/// <param name="collection">Original list</param>
		/// <param name="item">Prepended item</param>
		/// <returns>New list</returns>
		public static IReadOnlyList<T> Prepending<T>(this IReadOnlyList<T> collection, T item) => (new[] { item }).Concat(collection).ToList();

		/// <summary>
		/// Creates a new readonly list by replacing an item from an existing list
		/// </summary>
		/// <typeparam name="T">Item type</typeparam>
		/// <param name="collection">Original list</param>
		/// <param name="oldItem">Item to be replaced</param>
		/// <param name="newItem">Item that is replacing</param>
		/// <returns>New list</returns>
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

		/// <summary>
		/// Gets the index of an object if it exists in the list
		/// </summary>
		/// <typeparam name="T">Item type</typeparam>
		/// <param name="collection">List</param>
		/// <param name="item">Item to locate</param>
		/// <returns>Index of the item, or null if not found</returns>
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

		/// <summary>
		/// Indicates of the item exists in the list
		/// </summary>
		/// <typeparam name="T">Item type</typeparam>
		/// <param name="collection">List</param>
		/// <param name="item">Item to locate</param>
		/// <returns>True if the item exists in the list</returns>
		public static Boolean Contains<T>(this IReadOnlyList<T> collection, T item) where T : class => null != collection.IndexOfObject(item);

		/// <summary>
		/// Concatenates an enumeration of strings
		/// </summary>
		/// <typeparam name="T">Enumeration type</typeparam>
		/// <param name="stringList">Strings</param>
		/// <returns>Concatenated string</returns>
        public static String Concat<T>(this T stringList) where T : IEnumerable<String> => String.Join(String.Empty, stringList);
	}
}
