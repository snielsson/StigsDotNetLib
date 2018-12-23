// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;

namespace StigsDotNetLib {
	public static class Consts {
		public const long BytesInKiloByte = 1024;
		public const long BytesInMegaByte = 1048576;
		public const long BytesInGigaByte = 1073741824;
		public const long BytesInTeraByte = 1099511627776;
		public const int MinutesPerDay = 1440;
		public static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);
		public static readonly TimeSpan OneMinute = TimeSpan.FromMinutes(1);
		public static readonly TimeSpan OneHour = TimeSpan.FromHours(1);
		public static readonly TimeSpan OneDay = TimeSpan.FromDays(1);
		public static readonly TimeSpan T0800 = TimeSpan.FromHours(8);
		public static readonly TimeSpan T0900 = TimeSpan.FromHours(8);
		public static readonly TimeSpan T0830 = TimeSpan.Parse("0830");
		public static readonly TimeSpan T1600 = TimeSpan.FromHours(16);
	}

}