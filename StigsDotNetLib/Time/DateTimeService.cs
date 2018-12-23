// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using StigsDotNetLib.Exceptions;
using StigsDotNetLib.Extensions;

namespace StigsDotNetLib.Time {
	public class DateTimeService {
		private DateTime? _utcNow;
		/// <summary>
		///     Set UtcNow to freeze time.
		///     Set time t.
		/// </summary>
		public DateTime? UtcNow {
			get => _utcNow ?? DateTime.UtcNow;
			set => _utcNow = value.Assert(value == null || value.Value.Kind == DateTimeKind.Utc);
		}
	}


	//NOT DONE
	public class Calendar<T> {
		//(private readonly List<CalendarEvent> _calendarEvents;
		//public IEnumerable<CalendarEvent<T>> GetEvents(UtcDateTime timeStamp) {
		//	var first  =_calendarEvents.BinarySearch()
		//}

		public class CalendarEvent : IComparable<CalendarEvent> {
			private readonly List<(TimeSpan TimeSpan, T Value)> _intervals;
			private readonly TimeSpan _intervalsDuration;
			public UtcDateTime FirstStartTime { get;}
			public UtcDateTime LastEndTime { get; }
			public CalendarEvent(UtcDateTime firstStartTime, UtcDateTime lastEndTime, List<(TimeSpan, T)> intervals) {
				if(LastEndTime < FirstStartTime) throw new InvalidOperationException($"LastEndTime ({LastEndTime}) < FirstStartTime ({FirstStartTime}).");
				FirstStartTime = firstStartTime;
				LastEndTime = lastEndTime;
				_intervals = intervals;
				_intervalsDuration = TimeSpan.FromTicks(_intervals.Sum(x => x.Item1.Ticks));
				if((LastEndTime - FirstStartTime) < _intervalsDuration) throw new InvalidOperationException($"CalendarEvent duration ({LastEndTime-FirstStartTime}) is less than the duration of the intervals ({_intervalsDuration}).");
			}

			public T GetEvent(UtcDateTime timeStamp) {
				if (timeStamp < FirstStartTime || timeStamp > LastEndTime) return default(T);
				var current = FirstStartTime.AddTicks(_intervalsDuration.Ticks * ((timeStamp.Ticks - FirstStartTime.Ticks) / _intervalsDuration.Ticks));
				foreach (var interval in _intervals) {
					Debug.Assert(current <= timeStamp);
					var next = current + interval.TimeSpan;
					if (timeStamp < next) return interval.Value;
					current = next;
				}
				return default(T);
			}


			public int CompareTo(CalendarEvent other) {
				if (ReferenceEquals(this, other)) return 0;
				if (ReferenceEquals(null, other)) return 1;
				var firstStartTimeComparison = FirstStartTime.CompareTo(other.FirstStartTime);
				if (firstStartTimeComparison != 0) return firstStartTimeComparison;
				return LastEndTime.CompareTo(other.LastEndTime);
			}
			public static bool operator <(CalendarEvent left, CalendarEvent right) => Comparer<CalendarEvent>.Default.Compare(left, right) < 0;
			public static bool operator >(CalendarEvent left, CalendarEvent right) => Comparer<CalendarEvent>.Default.Compare(left, right) > 0;
			public static bool operator <=(CalendarEvent left, CalendarEvent right) => Comparer<CalendarEvent>.Default.Compare(left, right) <= 0;
			public static bool operator >=(CalendarEvent left, CalendarEvent right) => Comparer<CalendarEvent>.Default.Compare(left, right) >= 0;
		}
	}
}