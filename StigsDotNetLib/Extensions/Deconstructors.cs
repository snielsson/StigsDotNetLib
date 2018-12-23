// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System.Collections.Generic;

namespace StigsDotNetLib.Extensions {
	public static class Deconstructors {
		public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> @this, out TKey key, out TValue val) => (key, val) = (@this.Key, @this.Value);
	}
}