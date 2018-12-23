// Copyright � 2014-2018 TradingLens.com. All rights reserved.

using System;
using System.IO;
using System.Threading.Tasks;

namespace StigsDotNetLib.Extensions {
	public static class StreamExtensions {
		public static Stream Write(this Stream @this, byte[] bytes, int offset = 0, int count = 0) {
			@this.Write(bytes, offset, count > 0 ? count : bytes.Length);
			return @this;
		}

		public static void FlushCloseAndDispose(this Stream @this) {
			@this.Flush();
			@this.Close();
			@this.Dispose();
		}

		public static int ReadInt32(this Stream @this) {
			var bytes = new byte[sizeof(int)];
			var count = @this.Read(bytes, 0, bytes.Length);
			if (count != bytes.Length) throw new Exception($"Not enough bytes to read Int32, read {count} but expected {bytes.Length}.");
			return BitConverter.ToInt32(bytes, 0);
		}

		public static long ReadInt64(this Stream @this) {
			var bytes = new byte[sizeof(long)];
			var count = @this.Read(bytes, 0, bytes.Length);
			if (count != bytes.Length) throw new Exception($"Not enough bytes to read Int64, read {count} but expected {bytes.Length}.");
			return BitConverter.ToInt64(bytes, 0);
		}

		public static long RemainingBytes(this Stream @this) => @this.Length - @this.Position;

		public static Task WriteAsync(this Stream @this, byte[] bytes) => @this.WriteAsync(bytes, 0, bytes.Length);
	}
}