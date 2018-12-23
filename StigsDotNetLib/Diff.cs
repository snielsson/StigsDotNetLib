// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

namespace StigsDotNetLib {
	public class Diff<T> {
		public Diff(T o, T n) {
			Old = o;
			New = n;
		}
		public T Old { get; }
		public T New { get; }
		public static Diff<T> Create(T o, T n) => !Equals(o, n) ? new Diff<T>(o, n) : null;
	}
}