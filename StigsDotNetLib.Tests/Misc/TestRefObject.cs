// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System.Threading;

namespace StigsDotNetLib.Tests.Misc {
	public class TestRefObject
	{
		public string Name { get; set; }
		private static int _instanceCount;
		public int InstanceNumber { get; }
		public TestRefObject(string name = null) {
			Name = name;
			InstanceNumber = Interlocked.Increment(ref _instanceCount);
		}
	}
}