// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace StigsDotNetLib.Extensions {
	public static class FileSystemExtensions {
		public static void CopyFilesAsParallel(string srcRootDir, string targetRootDir, Regex srcFilterRegex,
			bool overwrite = true) {
			CopyFiles(srcRootDir, targetRootDir, srcFilterRegex, true, overwrite);
		}

		public static void CopyFiles(string srcRootDir, string trgRootDir, Regex srcRegex, bool asParallel = false,
			bool overwrite = true) {
			srcRootDir = srcRootDir.AsFullDirectoryPath();
			trgRootDir = trgRootDir.AsFullDirectoryPath();
			var files = srcRootDir.EnumerateFiles(srcRegex);
			if (asParallel) files = files.AsParallel();
			foreach (var file in files) {
				var relativePath = Path.GetRelativePath(srcRootDir, file);
				var outputFile = Path.Combine(trgRootDir, relativePath);
				EnsureDir(Path.GetDirectoryName(outputFile));
				File.Copy(file, outputFile, overwrite);
			}
		}

		public static IEnumerable<string> EnumerateFiles(this string @this, string pattern) => @this.EnumerateFiles(new Regex(pattern));

		public static IEnumerable<string> EnumerateFiles(this string @this, Regex regex) {
			return Directory.EnumerateFiles(@this, "*.*", SearchOption.AllDirectories).Where(x => regex.IsMatch(x));
		}

		public static string EnsureDir(this string @this, bool deleteIfExists = false) {
			var directoryInfo = new DirectoryInfo(@this);
			if (directoryInfo.Exists) {
				if (!deleteIfExists) return @this;
				Directory.Delete(@this, true);
			}

			directoryInfo.Create();
			return Path.GetFullPath(@this);
		}

		public static string GetFilePath(this string @this, string dir = ".") => GetFilePath(@this, new DirectoryInfo(dir));

		public static string GetFilePath(this string @this, DirectoryInfo dir) {
			while (dir != null) {
				var path = Path.Combine(dir.FullName, @this);
				if (File.Exists(path)) return path;
				dir = dir.Parent;
			}

			return null;
		}
		public static string AsFullDirectoryPath(this string @this) {
			try {
				var directoryInfo = new DirectoryInfo(@this);
				return directoryInfo.FullName;
			}
			catch (Exception) {
				return null;
			}
		}
		public static string AssertExistingDir(this string @this) {
			try {
				var directoryInfo = new DirectoryInfo(@this);
				if (!directoryInfo.Exists) throw new ArgumentException($"{@this} is not an existing directory.");
			}
			catch (Exception ex) {
				throw new ArgumentException($"{@this} is not an existing directory.", ex);
			}

			return @this;
		}

		public static string SetFileExtension(this string @this, string ext) => Path.Combine(Path.GetDirectoryName(@this),
			Path.GetFileNameWithoutExtension(@this) + "." + ext.TrimStart('.'));

	}
}