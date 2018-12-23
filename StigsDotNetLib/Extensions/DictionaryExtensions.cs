// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace StigsDotNetLib.Extensions {
	public static class DictionaryExtensions {
		public static TValue Value<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, TKey key) => @this.TryGetValue(key, out var val) ? val : default(TValue);
		public static TValue Value<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key) => @this.TryGetValue(key, out var val) ? val : default(TValue);
		public static IDictionary<TKey, TValue> Add<TKey, TValue>(this IDictionary<TKey, TValue> @this, params IDictionary<TKey, TValue>[] dictionaries)
		{
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			if (dictionaries == null) return @this;
			foreach ((TKey k, TValue v) in dictionaries.SelectMany(x=>x)) @this[k] = v;
			return @this;
		}
		public static IDictionary<TKey, TValue> Add<TKey, TValue>(this IDictionary<TKey, TValue> @this, params IReadOnlyDictionary<TKey, TValue>[] dictionaries)
		{
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			if (dictionaries == null) return @this;
			foreach ((TKey k, TValue v) in dictionaries.SelectMany(x=>x)) @this[k] = v;
			return @this;
		}
	}
}