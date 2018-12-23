// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using Newtonsoft.Json;
using StigsDotNetLib.Extensions;

namespace StigsDotNetLib.Time {
	[JsonConverter(typeof(JsonConverter))]
	public struct UtcDateTime : IEquatable<UtcDateTime>, IComparable<UtcDateTime> {
		public class JsonConverter : Newtonsoft.Json.JsonConverter {
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => writer.WriteRawValue(((UtcDateTime) value)._value.ToJson());
			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => new UtcDateTime((DateTime) reader.Value);
			public override bool CanConvert(Type objectType) => objectType == typeof(UtcDateTime);
		}

		private readonly DateTime _value;
		public UtcDateTime(DateTime dateTime) => _value = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
		public UtcDateTime(long ticks) => _value = new DateTime(ticks, DateTimeKind.Utc);
		public UtcDateTime(int year, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0) => _value = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
		public static implicit operator DateTime(UtcDateTime x) => x._value;
		public static implicit operator string(UtcDateTime x) => x.ToString();
		public static implicit operator UtcDateTime(string x) => DateTime.Parse(x);
		public static implicit operator UtcDateTime(DateTime x) => new UtcDateTime(x.Ticks);
		public long Ticks => _value.Ticks;

		public DateTimeKind Kind => _value.Kind;

		public UtcDateTime Date => _value.Date;

		public DayOfWeek DayOfWeek => _value.DayOfWeek;

		public TimeSpan TimeOfDay => _value.TimeOfDay;

		public int Year => _value.Year;

		public int Month => _value.Month;

		public int Day => _value.Day;

		public static UtcDateTime MaxValue => new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc);
		public static UtcDateTime MinValue => new DateTime(DateTime.MinValue.Ticks, DateTimeKind.Utc);
		public static int SizeInBytes { get; } = sizeof(long);

		public bool IsMultipleOf(TimeSpan ts) => _value.IsMultipleOf(ts);

		public DateTime ToLocalTime(TimeZoneInfo timeZoneInfo) {
			var dt = new DateTime(TimeZoneInfo.ConvertTimeFromUtc(_value, timeZoneInfo).Ticks, DateTimeKind.Local);
			return dt;
		}

		public DateTime ToLocalTime(string s) => new DateTime(TimeZoneInfo.ConvertTimeFromUtc(_value, s.ToTimeZoneInfo()).Ticks, DateTimeKind.Local);

		public bool Equals(UtcDateTime other) => _value.Equals(other._value);
		public int CompareTo(UtcDateTime other) => _value.CompareTo(other._value);

		public override bool Equals(object obj) {
			if (obj is null) return false;
			return obj is UtcDateTime time && Equals(time);
		}

		public override int GetHashCode() => _value.GetHashCode();

		public static bool operator ==(UtcDateTime left, UtcDateTime right) => left.Equals(right);

		public static bool operator !=(UtcDateTime left, UtcDateTime right) => !left.Equals(right);

		public static bool operator <(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) < 0;

		public static bool operator >(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) > 0;

		public static bool operator <=(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) <= 0;

		public static bool operator >=(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) >= 0;

		public static bool operator ==(UtcDateTime left, DateTime right) => left.Equals(right);

		public static bool operator !=(UtcDateTime left, DateTime right) => !left.Equals(right);

		public static bool operator <(UtcDateTime left, DateTime right) => left.CompareTo(right) < 0;

		public static bool operator >(UtcDateTime left, DateTime right) => left.CompareTo(right) > 0;

		public static bool operator <=(UtcDateTime left, DateTime right) => left.CompareTo(right) <= 0;

		public static bool operator >=(UtcDateTime left, DateTime right) => left.CompareTo(right) >= 0;

		public override string ToString() => ToString("yyyy-MM-dd HH:mm:ss UTC");

		public static UtcDateTime operator +(UtcDateTime left, TimeSpan right) => new UtcDateTime(left._value + right);
		public static UtcDateTime operator -(UtcDateTime left, TimeSpan right) => new UtcDateTime(left._value - right);
		public static TimeSpan operator -(UtcDateTime left, UtcDateTime right) => left._value - right._value;
		public string ToString(string format) => _value.ToString(format);
		public UtcDateTime Add(TimeSpan ts) => new UtcDateTime(_value.Add(ts).Ticks);
		public UtcDateTime AddTicks(long val) => new UtcDateTime(_value.AddTicks(val));
		public UtcDateTime AddMilliseconds(double val) => new UtcDateTime(_value.AddMilliseconds(val));
		public UtcDateTime AddSeconds(double val) => new UtcDateTime(_value.AddSeconds(val));
		public UtcDateTime AddMinutes(double val) => new UtcDateTime(_value.AddMinutes(val));
		public UtcDateTime AddHours(double val) => new UtcDateTime(_value.AddHours(val));
		public UtcDateTime AddDays(double val) => new UtcDateTime(_value.AddDays(val));
		public UtcDateTime AddMonths(int val) => new UtcDateTime(_value.AddMonths(val));
		public UtcDateTime AddYears(int val) => new UtcDateTime(_value.AddYears(val));
		public int ToUnixTime() => _value.ToUnixTime();
	}
}