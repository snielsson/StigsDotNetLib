// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System;
using System.Collections.Generic;

namespace StigsDotNetLib.Extensions {
	public static class ReadOnlyListExtensions {
		/// <summary>
		///     Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <typeparam name="TSearch">The type of the searched item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="comparer">The comparer that is used to compare the value with the list items.</param>
		/// <returns></returns>
		public static int BinarySearch<TItem, TSearch>(this IReadOnlyList<TItem> list, TSearch value, Func<TSearch, TItem, int> comparer) {
			if (list == null) throw new ArgumentNullException(nameof(list));
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));
			var lower = 0;
			var upper = list.Count - 1;
			while (lower <= upper) {
				var middle = lower + (upper - lower) / 2;
				var comparisonResult = comparer(value, list[middle]);
				if (comparisonResult < 0)
					upper = middle - 1;
				else if (comparisonResult > 0)
					lower = middle + 1;
				else
					return middle;
			}
			return ~lower;
		}

		/// <summary>
		///     Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <returns></returns>
		public static int BinarySearch<TItem>(this IReadOnlyList<TItem> list, TItem value) => BinarySearch(list, value, Comparer<TItem>.Default);

		/// <summary>
		///     Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="comparer">The comparer that is used to compare the value with the list items.</param>
		/// <returns></returns>
		public static int BinarySearch<TItem>(this IReadOnlyList<TItem> list, TItem value, IComparer<TItem> comparer) => list.BinarySearch(value, comparer.Compare);

		public static IEnumerable<T> Tail<T>(this IReadOnlyList<T> @this, int index) {
			while (index < @this.Count) yield return @this[index++];
		}

		public static IEnumerable<T> Slice<T>(this IReadOnlyList<T> @this, int index, int count) {
			for (var i = 0; i < count && i + index < @this.Count; i++) yield return @this[index + i];
		}

		/// <summary>
		///     Returns the inclusive interval of list defined as: [startIndex;endIndex].
		/// </summary>
		public static IEnumerable<T> Range<T>(this IReadOnlyList<T> @this, int startIndex, int endIndex) {
			if (startIndex < 0) throw new ArgumentException("startIndex is less than zero.", nameof(startIndex));
			if (endIndex > @this.Count - 1) throw new ArgumentException("endIndex is out of range.", nameof(endIndex));
			for (var i = startIndex; i <= endIndex; i++) yield return @this[i];
		}

		public static (IEnumerable<T>, IEnumerable<Diff<T>>) MergeWithDiffs<T>(this IReadOnlyList<T> oldItems, IReadOnlyList<T> newItems, Comparer<T> comparer = null) =>
			MergeWithDiffs(oldItems, newItems, 0, newItems.Count, comparer);

		public static (List<T>, IEnumerable<Diff<T>>) MergeWithDiffs<T>(this IReadOnlyList<T> @this, IReadOnlyList<T> items, int offset, int length, Comparer<T> comparer = null) {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			if (items == null) throw new ArgumentNullException(nameof(items));
			if (length < 1) throw new ArgumentNullException(nameof(length), $"length must be > 0, but was {length}.");
			if (offset + length > items.Count)
				throw new ArgumentNullException(nameof(offset) + " and " + nameof(length), $"offset ({offset}) + length ({length}) (= {offset + length}) must be <= items.Count ({items.Count}).");
			comparer = comparer ?? Comparer<T>.Default;
			var result = new List<T>(@this.Count + items.Count);
			var diffs = new List<Diff<T>>(items.Count);
			int itemsIndex = offset, thisIndex = 0;
			var newEndIndex = itemsIndex + length - 1;
			while (itemsIndex <= newEndIndex && thisIndex < @this.Count) {
				var item = items[itemsIndex];
				var thisItem = @this[thisIndex];
				var comparison = comparer.Compare(thisItem, item);
				if (comparison < 0) {
					result.Add(thisItem);
					thisIndex++;
					continue;
				}
				if (comparison > 0) {
					result.Add(item);
					itemsIndex++;
					continue;
				}
				Diff<T> diff = Diff<T>.Create(thisItem, item);
				if (diff != null) diffs.Add(diff);
				result.Add(item);
				thisIndex++;
				itemsIndex++;
			}
			if (thisIndex < @this.Count) result.AddRange(@this.Tail(thisIndex));
			else if (itemsIndex <= newEndIndex) result.AddRange(items.Slice(itemsIndex, newEndIndex - itemsIndex + 1));
			return (result, diffs.Count > 0 ? diffs : null);
		}
	}
}