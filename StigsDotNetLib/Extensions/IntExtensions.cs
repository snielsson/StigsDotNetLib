// Copyright � 2014-2018 TradingLens.com. All rights reserved.

using System;

namespace StigsDotNetLib.Extensions {
	public static class IntExtensions {
		public static unsafe byte[] GetBytes(this int value) {
			var numArray = new byte[4];
			fixed (byte* numPtr = numArray) *(int*) numPtr = value;
			return numArray;
		}

		public static unsafe void WriteBytes(this int value, byte[] buffer, int offset) {
			if (offset > buffer.Length + sizeof(int)) throw new IndexOutOfRangeException();
			fixed (byte* numPtr = &buffer[offset]) *(int*) numPtr = value;
		}

		public static int AsMultiplesOf(this int @this, int multipleSize) {
			@this.AssertIsMultipleOf(multipleSize);
			return @this / multipleSize;
		}

		public static bool IsMultipleOf(this int @this, int multipleSize) => @this % multipleSize == 0;
	}
}