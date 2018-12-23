// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;
using System.Threading;
using Shouldly;
using Xunit;

namespace StigsDotNetLib.Tests {

	public class EventHandlerInvokerTests {

		public class SomeClassWithEvents {
			public event Action SimpleActionEvent;
			public event EventHandler<object, object> EventWithArgs;
			public void FireSimpleActionEventWithStandardInvocatorPattern() {
				SimpleActionEvent?.Invoke();
			}
			public void FireSimpleActionEventWithEventHandlerInvoker()
			{
				EventHandlerInvoker.Invoke(SimpleActionEvent);
			}
			public void FireEventWithArgsWithStandardInvocatorPattern(object sender, object args) {
				EventWithArgs?.Invoke(sender, args);
			}
			public void FireEventWithArgsWithEventHandlerInvoker(object sender, object args)
			{
				EventHandlerInvoker.Invoke(EventWithArgs, sender, args);
			}
		}

		[Fact]
		public void CanInvokeSimpleActionEvent() {
			var target = new SomeClassWithEvents();
			int simpleActionEventCount = 0;
			target.SimpleActionEvent += () => Interlocked.Increment(ref simpleActionEventCount);
			target.FireSimpleActionEventWithStandardInvocatorPattern();
			simpleActionEventCount.ShouldBe(1);
			target.FireSimpleActionEventWithEventHandlerInvoker();
			simpleActionEventCount.ShouldBe(2);
		}
		[Fact]
		public void CanInvokeEventWithArgs() {
			var target = new SomeClassWithEvents();
			var invocationHistory = new List<(object, object)>();
			target.EventWithArgs += (sender,args) => invocationHistory.Add((sender,args));
			var arg1 = "arg1";
			var arg2 = "arg2";
			var arg3 = "arg3";
			var arg4 = "arg4";
			target.FireEventWithArgsWithStandardInvocatorPattern(arg1,arg2);
			target.FireEventWithArgsWithEventHandlerInvoker(arg3, arg4);
			invocationHistory.Count.ShouldBe(2);
			invocationHistory[0].Item1.ShouldBe(arg1);
			invocationHistory[0].Item2.ShouldBe(arg2);
			invocationHistory[1].Item1.ShouldBe(arg3);
			invocationHistory[1].Item2.ShouldBe(arg4);
		}

	}
}