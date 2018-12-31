// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;

namespace StigsDotNetLib.Extensions {
	public static class UriExtensions
	{
		public static Uri AssertIsBaseUrl(this Uri @this) => @this.Assert(x => string.IsNullOrEmpty(x.Query), x => $"{x} is not a base url because it has a query part.");
	}
}