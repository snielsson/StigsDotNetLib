// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System.Collections.Generic;

namespace StigsDotNetLib.Extensions {
	public static class CollectionExtensions {
		public static ICollection<T> Add<T>(this ICollection<T> @this, T item) {
			@this.Add(item);
			return @this;
		}
		public static bool IsEmpty<T>(this ICollection<T> @this) => @this.Count == 0;
		public static bool IsNullOrEmpty<T>(this ICollection<T> @this) => @this == null || @this.Count == 0;
	}
}