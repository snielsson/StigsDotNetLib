// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System.Collections.Generic;

namespace StigsDotNetLib {
	public static class Empty<T> {
		public static IEnumerable<T> Enumeration { get; } = new T[0];
		public static T[] Array { get; } = new T[0];
	}

}