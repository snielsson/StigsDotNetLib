// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace StigsDotNetLib {
	//TEST PropertyExpression

	/// <summary>
	///     Utility class to make it easy to extract an Action and a Func to set and get the property pinpointed by the
	///     getExpression parameter.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TProp"></typeparam>
	public sealed class PropertyExpression<T, TProp> {
		public PropertyExpression(Expression<Func<T, TProp>> getExpression) {
			var memberExpression = (MemberExpression) getExpression.Body;
			var prop = (PropertyInfo) memberExpression.Member;
			var setMethod = prop.GetSetMethod();
			var parameterT = Expression.Parameter(typeof(T), "x");
			var parameterTProperty = Expression.Parameter(typeof(TProp), "y");
			Expression<Action<T, TProp>> setExpression =
				Expression.Lambda<Action<T, TProp>>(
					Expression.Call(parameterT, setMethod, parameterTProperty),
					parameterT,
					parameterTProperty
				);
			Getter = getExpression.Compile();
			Setter = setExpression.Compile();
		}
		public Func<T, TProp> Getter { get; set; }
		public Action<T, TProp> Setter { get; set; }
	}
}