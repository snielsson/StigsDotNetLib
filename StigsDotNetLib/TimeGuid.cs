// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Threading;

namespace StigsDotNetLib {
	public struct TimeGuid : IEquatable<TimeGuid>, IComparable<TimeGuid> {
		public DateTime TimeStamp { get; }
		private readonly int _seq;
		public uint Seq => (uint)_seq-int.MaxValue;
		public int Tag { get; }
		public static Func<DateTime> Clock = null;
		public static DateTime Now = Clock?.Invoke() ?? DateTime.UtcNow;
		private static int _nextSeq = int.MaxValue;
		public static uint NextSeq => (uint)_nextSeq-int.MaxValue;

		public TimeGuid(DateTime timeStamp, int seq, int tag) {
			TimeStamp = timeStamp;
			_seq = seq;
			Tag = tag;
		}
		public static TimeGuid New(DateTime? now = null, int tag = 0) => new TimeGuid(now ?? Now, Interlocked.Increment(ref _nextSeq), tag);
		public static implicit operator Guid(TimeGuid x) {
			var bytes = new byte[16];
			Array.Copy(BitConverter.GetBytes(x.TimeStamp.Ticks), bytes, 8);
			Array.Copy(BitConverter.GetBytes(x.Seq), 0, bytes, 8, 4);
			Array.Copy(BitConverter.GetBytes(x.Tag), 0, bytes, 12, 4);
			return new Guid(bytes);
		}
		public static implicit operator TimeGuid(Guid x) {
			byte[] bytes = x.ToByteArray();
			return new TimeGuid(new DateTime(BitConverter.ToInt64(bytes, 0)), BitConverter.ToInt32(bytes, 8), BitConverter.ToInt32(bytes, 12));
		}
		public bool Equals(TimeGuid other) => TimeStamp == other.TimeStamp && Seq == other.Seq && Tag == other.Tag;
		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			return obj is TimeGuid other && Equals(other);
		}
		public override int GetHashCode() {
			unchecked {
				var hashCode = TimeStamp.GetHashCode();
				hashCode = (hashCode * 397) ^ (int)Seq;
				hashCode = (hashCode * 397) ^ Tag;
				return hashCode;
			}
		}
		public static bool operator ==(TimeGuid left, TimeGuid right) => left.Equals(right);
		public static bool operator !=(TimeGuid left, TimeGuid right) => !left.Equals(right);

		public int CompareTo(TimeGuid other) {
			var timeStampTicksComparison = TimeStamp.CompareTo(other.TimeStamp);
			if (timeStampTicksComparison != 0) return timeStampTicksComparison;
			var seqComparison = Seq.CompareTo(other.Seq);
			if (seqComparison != 0) return seqComparison;
			return Tag.CompareTo(other.Tag);
		}
		public static bool operator <(TimeGuid left, TimeGuid right) => left.CompareTo(right) < 0;
		public static bool operator >(TimeGuid left, TimeGuid right) => left.CompareTo(right) > 0;
		public static bool operator <=(TimeGuid left, TimeGuid right) => left.CompareTo(right) <= 0;
		public static bool operator >=(TimeGuid left, TimeGuid right) => left.CompareTo(right) >= 0;
	}
}