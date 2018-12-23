// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;

namespace StigsDotNetLib {
	public class ReverseComparer<T> : IComparer<T> {
		public static readonly ReverseComparer<T> Default = new ReverseComparer<T>();
		private readonly Func<T, T, int> _compareFunc;
		public ReverseComparer() { }
		public ReverseComparer(IComparer<T> comparer) {
			_compareFunc = (x, y) => comparer.Compare(y, x);
		}
		public int Compare(T x, T y) => _compareFunc?.Invoke(x, y) ?? Comparer<T>.Default.Compare(y, x);
	}

}