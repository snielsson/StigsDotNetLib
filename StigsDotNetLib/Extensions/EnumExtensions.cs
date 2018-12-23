// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace StigsDotNetLib.Extensions {

	public static class EnumExtensions {
		private static readonly ConcurrentDictionary<Type, Dictionary<string, object>> EnumValues =
			new ConcurrentDictionary<Type, Dictionary<string, object>>();
		private static readonly ConcurrentDictionary<Type, Dictionary<object, string>> EnumNames =
			new ConcurrentDictionary<Type, Dictionary<object, string>>();

		public static T GetEnumValue<T>(this Type @this, string name) {
			//TEST
			Dictionary<string, object> keyValueMap = EnumValues.GetOrAdd(@this, enumType => {
				string[] keys = Enum.GetNames(enumType);
				var values = Enum.GetValues(enumType);
				var map = new Dictionary<string, object>();
				for (var i = 0; i < keys.Length; i++) map[keys[i]] = values.GetValue(i);
				return map;
			});
			return (T) keyValueMap[name];
		}

		public static string GetEnumName(this Type @this, object val) {
			//TEST
			Dictionary<object, string> valueKeyMap = EnumNames.GetOrAdd(@this, enumType => {
				string[] keys = Enum.GetNames(enumType);
				var values = Enum.GetValues(enumType);
				var map = new Dictionary<object, string>();
				for (var i = 0; i < keys.Length; i++) map[values.GetValue(i)] = keys[i];
				return map;
			});
			return valueKeyMap[val];
		}
	}

}