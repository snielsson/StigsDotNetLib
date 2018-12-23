// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Runtime.Serialization;

namespace StigsDotNetLib.Exceptions {
	public class Exception<T> : Exception {
		public Exception(T thrower) => Thrower = thrower;
		protected Exception(T thrower, SerializationInfo info, StreamingContext context) : base(info, context) => Thrower = thrower;
		public Exception(T thrower, string message) : base(message) => Thrower = thrower;
		public Exception(T thrower, string message, Exception innerException) : base(message, innerException) => Thrower = thrower;
		public T Thrower { get; }
	}

	public class Exception<TThrower, TValue> : Exception<TThrower> {
		public Exception(TThrower thrower, TValue value) : base(thrower) => Value = value;
		protected Exception(TThrower thrower, TValue value, SerializationInfo info, StreamingContext context) : base(thrower, info, context) => Value = value;
		public Exception(TThrower thrower, TValue value, string message) : base(thrower, message) => Value = value;
		public Exception(TThrower thrower, TValue value, string message, Exception innerException) : base(thrower, message, innerException) => Value = value;
		public TValue Value { get; }
	}

}