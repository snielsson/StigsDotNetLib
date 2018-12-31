// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using Newtonsoft.Json;
using StigsDotNetLib.Extensions;

namespace StigsDotNetLib.Time {
	[JsonConverter(typeof(JsonConverter))]
	public struct UtcDateTime : IEquatable<UtcDateTime>, IComparable<UtcDateTime> {
		public class JsonConverter : Newtonsoft.Json.JsonConverter {
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => writer.WriteRawValue(((UtcDateTime) value).Value.ToJson());
			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => new UtcDateTime((DateTime) reader.Value);
			public override bool CanConvert(Type objectType) => objectType == typeof(UtcDateTime);
		}

		public DateTime Value { get; }
		public UtcDateTime(DateTime dateTime) => Value = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
		public UtcDateTime(long ticks) => Value = new DateTime(ticks, DateTimeKind.Utc);
		public UtcDateTime(int year, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0) => Value = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
		public static implicit operator DateTime(UtcDateTime x) => x.Value;
		public static implicit operator string(UtcDateTime x) => x.ToString();
		public static implicit operator UtcDateTime(string x) => DateTime.Parse(x);
		public static implicit operator UtcDateTime(DateTime x) => new UtcDateTime(x.Ticks);



		public long Ticks => Value.Ticks;

		public DateTimeKind Kind => Value.Kind;

		public UtcDateTime Date => Value.Date;

		public DayOfWeek DayOfWeek => Value.DayOfWeek;

		public TimeSpan TimeOfDay => Value.TimeOfDay;

		public int Year => Value.Year;

		public int Month => Value.Month;

		public int Day => Value.Day;

		public static UtcDateTime MaxValue => new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc);
		public static UtcDateTime MinValue => new DateTime(DateTime.MinValue.Ticks, DateTimeKind.Utc);
		public static int SizeInBytes { get; } = sizeof(long);
		private static UtcDateTime? _now;
		public static UtcDateTime Now { get => _now ?? DateTime.UtcNow; }
		public static UtcDateTime Set(UtcDateTime? val) {
			_now = val;
			return Now;
		}

		public bool IsMultipleOf(TimeSpan ts) => Value.IsMultipleOf(ts);

		public DateTime ToLocalTime(TimeZoneInfo timeZoneInfo) {
			var dt = new DateTime(TimeZoneInfo.ConvertTimeFromUtc(Value, timeZoneInfo).Ticks, DateTimeKind.Local);
			return dt;
		}

		public DateTime ToLocalTime(string s) => new DateTime(TimeZoneInfo.ConvertTimeFromUtc(Value, s.ToTimeZoneInfo()).Ticks, DateTimeKind.Local);

		public bool Equals(UtcDateTime other) => Value.Equals(other.Value);
		public int CompareTo(UtcDateTime other) => Value.CompareTo(other.Value);

		public override bool Equals(object obj) {
			if (obj is null) return false;
			return obj is UtcDateTime time && Equals(time);
		}

		public override int GetHashCode() => Value.GetHashCode();

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

		public static UtcDateTime operator +(UtcDateTime left, TimeSpan right) => new UtcDateTime(left.Value + right);
		public static UtcDateTime operator -(UtcDateTime left, TimeSpan right) => new UtcDateTime(left.Value - right);
		public static TimeSpan operator -(UtcDateTime left, UtcDateTime right) => left.Value - right.Value;
		public string ToString(string format) => Value.ToString(format);
		public UtcDateTime Add(TimeSpan ts) => new UtcDateTime(Value.Add(ts).Ticks);
		public UtcDateTime AddTicks(long val) => new UtcDateTime(Value.AddTicks(val));
		public UtcDateTime AddMilliseconds(double val) => new UtcDateTime(Value.AddMilliseconds(val));
		public UtcDateTime AddSeconds(double val) => new UtcDateTime(Value.AddSeconds(val));
		public UtcDateTime AddMinutes(double val) => new UtcDateTime(Value.AddMinutes(val));
		public UtcDateTime AddHours(double val) => new UtcDateTime(Value.AddHours(val));
		public UtcDateTime AddDays(double val) => new UtcDateTime(Value.AddDays(val));
		public UtcDateTime AddMonths(int val) => new UtcDateTime(Value.AddMonths(val));
		public UtcDateTime AddYears(int val) => new UtcDateTime(Value.AddYears(val));
		public int ToUnixTime() => Value.ToUnixTime();
	}
}