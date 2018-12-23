// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System.Collections.Generic;

namespace StigsDotNetLib.Extensions {
	public static class IComparerExtensions {
		//EASY: IComparerExtensions.Reverse not tested
		public static IComparer<T> Reverse<T>(this IComparer<T> @this) => new ReverseComparer<T>(@this);
	}
}