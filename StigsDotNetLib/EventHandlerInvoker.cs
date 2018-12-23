// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;

namespace StigsDotNetLib {
	public static class EventHandlerInvoker {

		//USE EXTENSION METHODS...

		public static void Invoke(Delegate handlers, params object[] parameters) {
			var tmp = handlers;
			if (tmp == null) return;
			foreach (var @delegate in tmp?.GetInvocationList()) {
				@delegate.Method.Invoke(@delegate.Target, parameters);
			}
		}
		public static void InvokeOnQueue(Delegate handlers, params object[] parameters) {
			//TODO...
		}
	}
}