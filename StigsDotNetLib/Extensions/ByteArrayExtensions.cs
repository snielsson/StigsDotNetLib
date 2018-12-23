// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace StigsDotNetLib.Extensions {
	public static class ByteArrayExtensions {
		//TEST: test this and blog about it.
		public static byte[] BlitToByteArray<T>(this T[] @this, int offset = 0, int length = 0) {
			var marshalledSize = Marshal.SizeOf<T>();
			var bytes = new byte[(length == 0 ? @this.Length : length) * marshalledSize];
			Debug.Assert(bytes.Length.IsMultipleOf(marshalledSize), $"Expected number of bytes ({bytes.Length}) to be a multiple of the marshalled size ({marshalledSize}) of {typeof(T)}.");
			GCHandle pinnedHandle = default(GCHandle);
			try {
				pinnedHandle = GCHandle.Alloc(@this, GCHandleType.Pinned);
				Marshal.Copy(pinnedHandle.AddrOfPinnedObject() + offset * marshalledSize, bytes, 0, bytes.Length);
				return bytes;
			}
			finally {
				if (pinnedHandle.IsAllocated) pinnedHandle.Free();
			}
		}

		//TEST: test this and blog about it.
		public static T[] BlitFromByteArray<T>(this byte[] @this, int byteSizeOfElements) {
			GCHandle pinnedHandle = default(GCHandle);
			try {
				var count = @this.Length.AsMultiplesOf(byteSizeOfElements);
				var result = new T[count];
				pinnedHandle = GCHandle.Alloc(result, GCHandleType.Pinned);
				Marshal.Copy(@this, 0, pinnedHandle.AddrOfPinnedObject(), @this.Length);
				return result;
			}
			finally {
				if (pinnedHandle.IsAllocated) pinnedHandle.Free();
			}
		}
	}
}