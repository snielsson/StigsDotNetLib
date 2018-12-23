// Copyright © 2014-2018 TradingLens.com. All rights reserved.

using System;
using System.Text.RegularExpressions;
using StigsDotNetLib.Caching;

namespace StigsDotNetLib.Extensions {

	public static class RegexExtensions {
		private static readonly LruCache<int, Regex> Cache = new LruCache<int, Regex>(1000);

		public static Regex AsCachedRegex(this string @this, RegexOptions options = RegexOptions.None) {
			options = options | RegexOptions.Compiled;
			var cacheKey = @this.HashCombine(options);
			return Cache.TryGet(cacheKey, out Regex val)? val : Cache.Set(cacheKey, new Regex(@this, options));
		}

		public static Regex AsRegex(this string @this, RegexOptions options = RegexOptions.None) {
			options = options | RegexOptions.Compiled;
			return new Regex(@this, options);
		}

		public static string Extract(this Regex @this, string input, int group) {
			var match = @this.Match(input);
			if (!match.Success) return null;
			if (match.Groups.Count <= group) throw new ArgumentException($"Number of matching groups lower than the provided values for group ({group}).");
			return match.Groups[group].Value;
		}

		public static string Extract(this string @this, string pattern, int group = 1, RegexOptions options = RegexOptions.None) {
			var regex = pattern.AsCachedRegex(options);
			return Extract(@this, regex, group);
		}

		public static string Extract(this string @this, Regex regex, int group = 1) {
			var match = regex.Match(@this);
			if (match.Success) return match.Groups[group].Value;
			return null;
		}

		public static (string, string) Extract(this Regex @this, string input, int group1, int group2) {
			var match = @this.Match(input);
			if (!match.Success) return (null, null);
			if (match.Groups.Count <= Math.Max(group1, group2)) throw new ArgumentException($"Number of matching groups lower than the provided values for group1 ({group1}) and group2 ({group2}).");
			return (match.Groups[group1].Value, match.Groups[group2].Value);
		}

		public static (string, string) Extract(this string @this, string pattern, int group1, int group2, RegexOptions options = RegexOptions.None) {
			var regex = pattern.AsCachedRegex(options);
			return regex.Extract(@this, group1, group2);
		}

		public static T Extract<T>(this string @this, string pattern, Func<string, T> parseFunc, int group = 1, RegexOptions options = RegexOptions.None) {
			var regex = pattern.AsCachedRegex(options);
			var match = regex.Match(@this);
			if (!match.Success) return default(T);
			if (match.Groups.Count <= group) return default(T);
			return parseFunc(match.Groups[group].Value);
		}

		public static bool Matches(this string @this, string pattern, int startAt = 0, RegexOptions options = RegexOptions.None, bool skipCaching = false) {
			var regex = skipCaching ? new Regex(pattern, options) : pattern.AsCachedRegex(options);
			return regex.IsMatch(pattern, startAt);
		}
	}
}