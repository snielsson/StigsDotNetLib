// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using Shouldly;
using Xunit;

namespace StigsDotNetLib.Tests {

	public class TimeGuidTests {
		[Fact]
		public void TimeGuidWorks() {
			var nextSeq = TimeGuid.NextSeq;
			var target = TimeGuid.New();
			var target2 = TimeGuid.New();
			var target3 = TimeGuid.New();

			Guid g1 = target;
			//target.Seq.ShouldBe();
		}

	}
}