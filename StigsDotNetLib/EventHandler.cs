// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

namespace StigsDotNetLib {
	public delegate void EventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e);

}