// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StigsDotNetLib.Exceptions;

namespace StigsDotNetLib.Extensions {
	public static class ObjectExtensions {
		public static T GetOrCreate<T>(this T @this) where T : new() => @this != null ? @this : new T();
		public static T GetOrCreate<T>(this T @this, Func<T> f) where T : new() => @this != null ? @this : f.Invoke();

		public static string ToJson(this object @this, JsonSerializerSettings settings = null, params object[] args) {
			foreach (var arg in args)
				if (arg is JsonConverter jsonConverter)
					settings.Converters.Add(jsonConverter);
			return JsonConvert.SerializeObject(@this, settings);
		}

		public static string ToJson(this object @this, bool pretty, JsonSerializerSettings settings = null) {
			if (pretty) return ToPrettyJson(@this, settings);
			if (settings == null) return JsonConvert.SerializeObject(@this, Formatting.None);
			return ToJson(@this, settings);
		}

		public static string ToPrettyJson(this object @this, JsonSerializerSettings settings = null) {
			if (settings == null) return JsonConvert.SerializeObject(@this, Formatting.Indented, settings);
			return JsonConvert.SerializeObject(@this, Formatting.Indented);
		}

		public static async Task<string> ToPrettyJsonFile(this object @this, string filepath, JsonSerializerSettings settings = null) {
			var json = @this.ToPrettyJson(settings);
			await File.WriteAllTextAsync(filepath, json);
			return json;
		}

		public static async Task<string> ToPrettyJsonFile(this object @this, DirectoryInfo directoryInfo, string fileName, JsonSerializerSettings settings = null, params object[] args) =>
			await ToPrettyJsonFile(@this, directoryInfo.FullName.Combine(fileName));

		public static string ToJsonFile(this object @this, string filepath, JsonSerializerSettings settings = null,
			params object[] args) {
			var json = @this.ToJson(settings, args);
			File.WriteAllText(filepath, json);
			return json;
		}

		public static T JsonClone<T>(this T @this) => @this.ToJson().FromJson<T>();

		public static int HashCombine<T, TU>(this T @this, params TU[] objs) {
			var result = @this.GetHashCode();
			for (var i = 0; i < objs.Length; ++i) {
				var obj = objs[i];
				if (obj != null) result = (result * 397) ^ obj.GetHashCode();
			}
			return result;
		}

		public static T JsonMerge<T>(this string @this, JsonMergeSettings mergeSettings,
			JsonSerializerSettings settings = null, params object[] objs) {
			var result = new JObject(@this.FromJson<T>(settings));
			foreach (var obj in objs) result.Merge(obj, mergeSettings);
			return result.ToObject<T>();
		}

		public static T JsonMerge<T>(this T @this, JsonMergeSettings mergeSettings,
			JsonSerializerSettings settings = null) {
			var result = new JObject(@this.ToJson(settings).FromJson<T>(settings));
			return result.ToObject<T>();
		}

		public static object JsonMerge(this object @this, JsonSerializerSettings settings = null,
			params object[] objs) => @this.JsonMerge(settings, objs);

		public static string GetCaller(this object @this, [CallerMemberName] string caller = null) => caller;

		public static T EnsureValid<T>(this T @this, string message = null, ValidationContext validationContext = null) {
			var validationResults = new List<ValidationResult>();
			validationContext = validationContext ?? new ValidationContext(@this);
			Validator.TryValidateObject(@this, validationContext, validationResults);
			var validatableObject = @this as IValidatableObject;
			validatableObject?.Validate(validationContext)?.AddTo(validationResults);
			if (validationResults.Count == 0) return @this;
			throw new ValidationException(validationResults.ToPrettyJson());
		}

		//public static Result<T> Validate<T>(this T @this, string message = null, ValidationContext validationContext = null)
		//{
		//	var validationResults = new List<ValidationResult>();
		//	validationContext = validationContext ?? new ValidationContext(@this);
		//	Validator.TryValidateObject(@this, validationContext, validationResults);
		//	var validatableObject = @this as IValidatableObject;
		//	validatableObject?.Validate(validationContext)?.AddTo(validationResults);
		//	return (@this, validationResults);
		//}

		public static T Lock<T>(this T @this, Action action) {
			lock (@this) {
				action();
				return @this;
			}
		}
		public static T Lock<T>(this T @this, object lockObject, Action action) {
			lock (lockObject.NotNull()) {
				action();
				return @this;
			}
		}
		public static void Throw<T>(this T @this) => throw new Exception<T>(@this);
		public static void Throw<T>(this T @this, string message) => throw new Exception<T>(@this, message);
		public static void Throw<T>(this T @this, string message, Exception innerException) => throw new Exception<T>(@this, message, innerException);
		public static void Throw<T, TValue>(this T @this, TValue u) => throw new Exception<T, TValue>(@this, u);
		public static void Throw<T, TValue>(this T @this, TValue u, string message) => throw new Exception<T, TValue>(@this, u, message);
		public static void Throw<T, TValue>(this T @this, string message, TValue u) => throw new Exception<T, TValue>(@this, u, message);
		public static void Throw<T, TValue>(this T @this, TValue u, string message, Exception innerException) => throw new Exception<T, TValue>(@this, u, message, innerException);

		//TEST
		public static T Assert<T>(this T @this, bool predicate, Func<T, string> errorMsg = null) {
			if (!predicate) throw new AssertException<T>(@this, errorMsg?.Invoke(@this) ?? $"Assert failed on {@this}.");
			return @this;
		}
		//TEST
		public static T Assert<T>(this T @this, Func<T, bool> predicate, Func<T, string> errorMsg = null) {
			if (!predicate(@this)) throw new AssertException<T>(@this, errorMsg?.Invoke(@this) ?? $"Assert failed on {@this}.");
			return @this;
		}

		public static T NotNull<T>(this T @this, string msg = null) {
			if (@this != null) AssertException<T>.Throw(@this, msg ?? $"{@this} of type {typeof(T)} is not null.");
			return @this;
		}

		public class AssertException<T> : Exception<T> {
			public AssertException(T thrower, string msg) : base(thrower, msg) { }
			public static void Throw(T thrower, string s) => throw new AssertException<T>(thrower, s);
		}
		public class AssertException<T, TU> : Exception<T, TU> {
			public AssertException(T thrower, TU u, string msg) : base(thrower, u, msg) { }
		}
	}

}