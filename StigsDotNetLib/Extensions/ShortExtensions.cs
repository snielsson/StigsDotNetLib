// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System;

namespace StigsDotNetLib.Extensions {
	public static class ShortExtensions {
		public static unsafe byte[] GetBytes(this ushort value) {
			var numArray = new byte[sizeof(ushort)];
			fixed (byte* numPtr = numArray) *(ushort*) numPtr = value;
			return numArray;
		}

		public static unsafe void WriteBytes(this ushort value, byte[] buffer, int offset) {
			if (offset > buffer.Length + sizeof(ushort)) throw new IndexOutOfRangeException();
			fixed (byte* numPtr = &buffer[offset]) *(ushort*) numPtr = value;
		}

		public static unsafe byte[] GetBytes(this short value) {
			var numArray = new byte[sizeof(short)];
			fixed (byte* numPtr = numArray) *(short*) numPtr = value;
			return numArray;
		}

		public static unsafe void WriteBytes(this short value, byte[] buffer, int offset) {
			if (offset > buffer.Length + sizeof(short)) throw new IndexOutOfRangeException();
			fixed (byte* numPtr = &buffer[offset]) *(short*) numPtr = value;
		}
	}
}