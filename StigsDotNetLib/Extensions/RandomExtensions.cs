// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System;
using System.Collections.Generic;

namespace StigsDotNetLib.Extensions {
	public static class RandomExtensions {
		public static T Pick<T>(this Random @this, IList<T> list) => list[@this.Next(list.Count)];
		public static char Pick(this Random @this, string str) => str[@this.Next(str.Length)];
	}
}