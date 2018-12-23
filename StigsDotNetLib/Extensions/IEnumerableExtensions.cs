// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace StigsDotNetLib.Extensions {

	public static class IEnumerableExtensions {
		private const long TrieThreshold = 10000;

		//EASY: IEnumerableExtensions not tested
		public static string ToCsv<T>(this IEnumerable<T> @this, string separator = ",") {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			return string.Join(separator, @this);
		}

		public static IReadOnlyDictionary<K, V> ToReadOnlyDictionary<K, V>(this IEnumerable<V> @this, Func<V, K> keySelector, long expectedSize) {
			if (typeof(K) == typeof(string) && expectedSize > TrieThreshold) {
				//TODO: use a trie for optimization
				return @this.ToDictionary(keySelector);
			}
			return @this.ToDictionary(keySelector);
		}

		//EASY: IEnumerableExtensions not tested
		public static string ToLines<T>(this IEnumerable<T> @this) {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			return string.Join("\n", @this);
		}

		//EASY: IEnumerableExtensions not tested
		public static ICollection<T> AddTo<T>(this IEnumerable<T> @this, ICollection<T> target) {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			foreach (var item in @this) target.Add(item);
			return target;
		}

		//EASY: IEnumerableExtensions not tested
		public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action) {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			foreach (var item in @this) action(item);
		}

		public static bool IsSorted<T>(this IEnumerable<T> @this, IComparer<T> comparer = null, bool strictSorting = false) {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			if (strictSorting) return @this.IsSortedStrict(comparer);
			comparer = comparer ?? Comparer<T>.Default;
			using (IEnumerator<T> enumerator = @this.GetEnumerator()) {
				if (!enumerator.MoveNext()) return true;
				var prev = enumerator.Current;
				while (enumerator.MoveNext()) {
					if (comparer.Compare(prev, enumerator.Current) > 0) return false;
					prev = enumerator.Current;
				}
				return true;
			}
		}

		public static bool IsSortedStrict<T>(this IEnumerable<T> @this, IComparer<T> comparer = null) {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			comparer = comparer ?? Comparer<T>.Default;
			using (IEnumerator<T> enumerator = @this.GetEnumerator()) {
				if (!enumerator.MoveNext()) return true;
				var prev = enumerator.Current;
				while (enumerator.MoveNext()) {
					if (comparer.Compare(prev, enumerator.Current) >= 0) return false;
					prev = enumerator.Current;
				}
				return true;
			}
		}
	}
}