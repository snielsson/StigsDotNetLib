// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace StigsDotNetLib.Tests.Misc {
	/// <summary>
	/// A collection of various tests written to test of how fundamental c# language features, BCL types and other types work.
	/// </summary>
	public class MiscTests
	{
		[Fact]
		public void EnumerableToArrayCopiesReferencesForList() {
			var target = new List<TestRefObject> {new TestRefObject("1"), new TestRefObject("2"), new TestRefObject("3")};
			var array = target.ToArray();
			array.Length.ShouldBe(3);
			array[2].Name.ShouldBe("3");
			target[1].Name += " changed";
			target[2] = new TestRefObject("4");
			array[1].Name.ShouldBe("2 changed", "Expected value still to be changed because referencing same object.");
			array[2].Name.ShouldBe("3", "Expected value still to be '3' because reference to object '3' should have been copied by ToArray.");
			var array2 = target.ToArray();
			array2[1].Name.ShouldBe("2 changed");
			array2[2].Name.ShouldBe("4");
		}
		[Fact]
		public void EnumerableToArrayCopiesReferencesForHashSet() {
			var target = new HashSet<TestRefObject>();
			target.Add(new TestRefObject("1"));
			target.Add(new TestRefObject("2"));
			target.Add(new TestRefObject("3"));
			var array = target.ToArray();
			array.Length.ShouldBe(3);
			target.Add(new TestRefObject("4"));
			array.Length.ShouldBe(3);
		}

		[Fact]
		public void SubtractingNullableTimeSpansReturnsNullWhenOneOrBothOperandsAreNull() {
			TimeSpan x = TimeSpan.Zero;
			TimeSpan? y = TimeSpan.Zero;
			TimeSpan? z = null;
			(x-x).ShouldBe(TimeSpan.Zero);
			(x-y).ShouldBe(TimeSpan.Zero);
			(y-x).ShouldBe(TimeSpan.Zero);
			(y-y).ShouldBe(TimeSpan.Zero);
			(x-z).ShouldBe(null);
			(z-x).ShouldBe(null);
			(y-z).ShouldBe(null);
			(z-y).ShouldBe(null);
		}
	}
}