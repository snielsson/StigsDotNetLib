// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System.IO;

namespace StigsDotNetLib.Extensions {
	public static class DirectoryInfoExtensions {
		public static DirectoryInfo Combine(this DirectoryInfo @this, params string[] args) {
			var result = new DirectoryInfo(@this.FullName.Combine(args));
			return result;
		}

		public static FileInfo Combine(this DirectoryInfo @this, FileInfo fileInfo) {
			var result = new FileInfo(@this.FullName.Combine(fileInfo.Name));
			return result;
		}
	}
}