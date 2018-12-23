// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;
using System.Linq;

namespace StigsDotNetLib.Extensions {
	public static class EnumerableExtensions {
		public static IEnumerable<(T, int)> Occurences<T>(this IEnumerable<T> @this, int minValue = int.MinValue, int maxValue = int.MaxValue) =>
			@this.GroupBy(x => x).Select(x => (x.Key, x.Count())).Where(x => x.Item2 >= minValue && x.Item2 <= maxValue);
		public static IEnumerable<TU> ValidateDistinct<T, TU>(this IEnumerable<T> @this, Func<(T, int), TU> onValidationError) => @this.Occurences(2).Select(onValidationError);
		public static IEnumerable<TU> ValidateWhiteList<T, TU>(this IEnumerable<T> @this, IEnumerable<T> whitelist, Func<T, TU> onValidationError) => @this.Except(whitelist).Distinct().Select(onValidationError);
		public static IEnumerable<TU> ValidateBlackList<T, TU>(this IEnumerable<T> @this, IEnumerable<T> blackList, Func<T, TU> onValidationError) => @this.Intersect(blackList).Distinct().Select(onValidationError);
	}
}