// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StigsDotNetLib.Extensions {
	public static class ListExtensions {
		//EASY: IListExtensions.BinarySearch not tested
		/// <summary>
		///     Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <typeparam name="TSearch">The type of the searched item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="comparer">The comparer that is used to compare the value with the list items.</param>
		/// <returns></returns>
		public static int BinarySearch<TItem, TSearch>(this IList<TItem> list, TSearch value, Func<TSearch, TItem, int> comparer) {
			if (list == null) throw new ArgumentNullException(nameof(list));
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));
			var lower = 0;
			var upper = list.Count - 1;
			while (lower <= upper) {
				var middle = lower + (upper - lower) / 2;
				var comparisonResult = comparer(value, list[middle]);
				if (comparisonResult < 0) upper = middle - 1;
				else if (comparisonResult > 0) lower = middle + 1;
				else return middle;
			}
			return ~lower;
		}

		//EASY: IListExtensions.BinarySearch not tested
		/// <summary>
		///     Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <returns></returns>
		public static int BinarySearch<TItem>(this IList<TItem> list, TItem value) => BinarySearch(list, value, Comparer<TItem>.Default);

		//EASY: IListExtensions.BinarySearch not tested
		/// <summary>
		///     Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="comparer">The comparer that is used to compare the value with the list items.</param>
		/// <returns></returns>
		public static int BinarySearch<TItem>(this IList<TItem> list, TItem value, IComparer<TItem> comparer) => list.BinarySearch(value, comparer.Compare);

		//EASY: IListExtensions.InsertSorted not tested
		public static IList<T> InsertSorted<T>(this IList<T> @this, T value, IComparer<T> comparer = null, bool strictSorting = false) {
			comparer = comparer ?? Comparer<T>.Default;
			Debug.Assert(@this.IsSorted(comparer, strictSorting));
			var index = @this.BinarySearch(value);
			if (strictSorting && index >= 0) throw new ArgumentException($"Value is already at index {index}.");
			if (index < 0) index = ~index;
			@this.Insert(index < 0 ? ~index : index, value);
			return @this;
		}

		//EASY: IListExtensions.Tail not tested
		public static IEnumerable<T> Tail<T>(this IList<T> @this, int index) {
			while (index < @this.Count) yield return @this[index++];
		}

		//EASY: IListExtensions.Slice not tested
		public static IEnumerable<T> Slice<T>(this IList<T> @this, int index, int count) {
			for (var i = 0; i < count && i + index < @this.Count; i++) yield return @this[index + i];
		}

		/// <summary>
		///     Returns the inclusive interval of list defined as: [startIndex;endIndex].
		/// </summary>
		public static IEnumerable<T> Range<T>(this IList<T> @this, int startIndex, int endIndex) {
			if (startIndex < 0) throw new ArgumentException("startIndex is less than zero.", nameof(startIndex));
			if (endIndex > @this.Count - 1) throw new ArgumentException("endIndex is out of range.", nameof(endIndex));
			for (var i = startIndex; i <= endIndex; i++) yield return @this[i];
		}

		public static T PickRandom<T>(this IList<T> @this, Random random) {
			var index = random.Next(0, @this.Count);
			return @this[index];
		}

		public static T PickRandom<T>(this IList<T> @this, int seed = 0) => PickRandom(@this, new Random(seed));
	}
}