// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using StigsDotNetLib.Extensions;

namespace StigsDotNetLib.Time {
	public struct UnixTime64 {
		private readonly DateTime _dateTime;

		public UnixTime64(DateTime dateTime) => _dateTime = dateTime;

		public UnixTime64(long l) => _dateTime = UnixTime32.UnixEpoch.AddMilliseconds(l);

		public int Value => _dateTime.ToUnixTime();

		public UnixTime64 AddDays(double x) => new UnixTime64(_dateTime.AddDays(x));

		public static implicit operator int(UnixTime64 x) => x.Value;

		public static implicit operator DateTime(UnixTime64 x) => x._dateTime;

		public static explicit operator UnixTime32(UnixTime64 x) => new UnixTime32(x._dateTime);
	}
}