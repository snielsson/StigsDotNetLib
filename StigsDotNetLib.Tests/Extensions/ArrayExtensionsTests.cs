// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using Shouldly;
using StigsDotNetLib.Extensions;
using Xunit;

namespace StigsDotNetLib.Tests.Extensions {
	public class ArrayExtensionsTests {
		[Fact]
		public void MergeSortedWorks() {
			var aArray = new[] {1, 2, 5};
			var bArray = new[] {1, 4, 6};
			var cArray = new[] {1, 1, 6};
			var dArray = new[] {2, 1};
			int[] result1 = aArray.MergeSorted(bArray, (x, y) => x.CompareTo(y), false, false);
			result1.ShouldBe(new[] {1, 1, 2, 4, 5, 6});
			int[] result2 = aArray.MergeSorted(bArray, (x, y) => x.CompareTo(y), false, true);
			result2.ShouldBe(new[] {1, 1, 2, 4, 5, 6});
			int[] result3 = aArray.MergeSorted(bArray, (x, y) => x.CompareTo(y), true, false);
			result3.ShouldBe(new[] {1, 2, 4, 5, 6});
			int[] result4 = aArray.MergeSorted(bArray, (x, y) => x.CompareTo(y), true, true);
			result4.ShouldBe(new[] {1, 2, 4, 5, 6});

			int[] result5 = aArray.MergeSorted(cArray, (x, y) => x.CompareTo(y), false, false);
			result5.ShouldBe(new[] {1, 1, 1, 2, 5, 6});
			Should.Throw<ArgumentException>(() => aArray.MergeSorted(cArray, (x, y) => x.CompareTo(y), false, true));
			int[] result6 = aArray.MergeSorted(cArray, (x, y) => x.CompareTo(y), true, false);
			result6.ShouldBe(new[] {1, 2, 5, 6});
			Should.Throw<ArgumentException>(() => aArray.MergeSorted(cArray, (x, y) => x.CompareTo(y), true, true));

			Should.Throw<ArgumentException>(() => aArray.MergeSorted(dArray, (x, y) => x.CompareTo(y), false, false), "dArray is not sorted so should throw");
			Should.Throw<ArgumentException>(() => aArray.MergeSorted(dArray, (x, y) => x.CompareTo(y), false, true), "dArray is not sorted so should throw");
			Should.Throw<ArgumentException>(() => aArray.MergeSorted(dArray, (x, y) => x.CompareTo(y), true, false), "dArray is not sorted so should throw");
			Should.Throw<ArgumentException>(() => aArray.MergeSorted(dArray, (x, y) => x.CompareTo(y), true, true), "dArray is not sorted so should throw");

			Should.Throw<ArgumentNullException>(() => ((int[]) null).MergeSorted(null, (x, y) => x.CompareTo(y)));
			Should.Throw<ArgumentNullException>(() => ((int[]) null).MergeSorted(bArray, (x, y) => x.CompareTo(y)));
			Should.Throw<ArgumentNullException>(() => aArray.MergeSorted(null, (x, y) => x.CompareTo(y)));
			Should.Throw<ArgumentNullException>(() => aArray.MergeSorted(bArray, null));

			int[] result7 = aArray.MergeSorted(Empty<int>.Array, (x, y) => x.CompareTo(y));
			result7.ShouldBe(aArray);
			ReferenceEquals(aArray, result7).ShouldBe(false, "should be a copy of aArray");

			var result8 = Empty<int>.Array.MergeSorted(bArray, (x, y) => x.CompareTo(y));
			result8.ShouldBe(bArray);
			ReferenceEquals(bArray, result8).ShouldBe(false, "should be a copy of bArray");
		}
	}
}