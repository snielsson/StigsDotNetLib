// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Threading.Tasks;

namespace StigsDotNetLib {
	/// <summary>
	///     Wrapper of TaskCompletion source to ensure best practices when used in async scenarios.
	///     Inspired by https://blogs.msdn.microsoft.com/seteplia/2018/10/01/the-danger-of-taskcompletionsourcet-class/
	///     Conclusions in this blog post were:
	///     - TaskCompletionSource class was introduced in .NET 4.0 in a pre async-era for controlling a task's lifetime
	///     manually.
	///     - By default all the task's continuations are executed synchronously unless
	///     TaskCreationOptions.RunContinuationsAsynchronously option is specified.
	///     - All the "async" continuations(blocks between await statements) always run in a thread of an awaited task.
	///     - TaskCompletionSource instanced created with default constructor may cause deadlocks and other threading issues by
	///     running all "async" continuations in the thread that sets the result of a task.
	///     - If you use.NET 4.6.1+ you should always provide TaskCreationOptions.RunContinuationsAsynchronously when creating
	///     TaskCompletionSource instances.
	/// </summary>
	//TEST
	public class AsyncCompletion {
		private readonly TaskCompletionSource<object> _taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
		public Task Task => _taskCompletionSource.Task;
		public Exception Exception { get; private set; }
		public void SetComplete() => _taskCompletionSource.SetResult(null);
		public TException SetException<TException>(TException ex) where TException : Exception {
			Exception = ex;
			_taskCompletionSource.SetException(ex);
			return ex;
		}
		public void SetCancelled() => _taskCompletionSource.SetCanceled();
	}
	//TEST
	public class AsyncCompletion<T> {
		private readonly TaskCompletionSource<T> _taskCompletionSource = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
		public Task<T> Task => _taskCompletionSource.Task;
		public Exception Exception { get; private set; }
		public T SetResult(T result) {
			_taskCompletionSource.SetResult(result);
			return result;
		}
		public TException SetException<TException>(TException ex) where TException : Exception {
			Exception = ex;
			_taskCompletionSource.SetException(ex);
			return ex;
		}
		public void SetCancelled() => _taskCompletionSource.SetCanceled();
	}
}