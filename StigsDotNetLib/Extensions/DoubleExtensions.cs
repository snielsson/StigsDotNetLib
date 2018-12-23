// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace StigsDotNetLib.Extensions {
	public static class DoubleExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsZero(this double @this, double? tolerance = null) => Math.Abs(@this) < (tolerance ?? double.Epsilon);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEqualTo(this double @this, double comparand, double? tolerance = null) => (@this - comparand).IsZero(tolerance);

		public static bool HasFraction(this double @this, double? tolerance = null) => Math.Abs(@this % 1) > tolerance;
	}
}