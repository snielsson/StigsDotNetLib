// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

namespace StigsDotNetLib {

	//public class JobQueue<T> {
	//	private readonly System.Threading.Tasks.Dataflow.ActionBlock<Func<Task>> _eventInvokerQueue;
	//	private readonly ActionBlock<Job<T>> _jobQueue = new ActionBlock<Job<T>>(job => job.Run());
	//	public JobQueue() {
	//		_eventInvokerQueue = new ActionBlock<Func<Task>>(async func => {
	//			try {
	//				await func().ConfigureAwait(false);
	//			}
	//			catch (Exception ex) {
	//				OnEventExceptionEvent(ex);
	//			}
	//		});
	//	}

	//	public event Action<Exception> EventExceptionEvent;
	//	protected void OnEventExceptionEvent(Exception ex) => EventExceptionEvent?.Invoke(ex);

	//	public Job<T> Post<T>(Action action, T data = default(T), Func<Job<T>, Job<T>> beforePost = null) {
	//		var job = new Job<T>(action, data);
	//		if (beforePost != null) job = beforePost.Invoke(job);
	//		if (job != null) _jobQueue.Post(job);

	//		job.EndEvent += async (s, e) => await Task.CompletedTask;
	//		job.EndEvent += (s, e) => { };

	//		return job;
	//	}

	//	public class Job<T> {
	//		private readonly TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
	//		public Job(JobQueue<Job<T>> jobQueue, Action jobAction, T jobData) {
	//			JobQueue = jobQueue;
	//			JobAction = jobAction;
	//			JobData = jobData;
	//		}
	//		public JobQueue<Job<T>> JobQueue { get; }
	//		public Task Completion => _tcs.Task;
	//		public Action JobAction { get; }
	//		public T JobData { get; }
	//		public event Action<Exception> ExceptionEvent;
	//		public event Action<Job<T>> StartEvent;
	//		public event EventHandler<Job<T>> EndEvent;
	//		public void Run() {
	//			try {
	//				StartEvent?.Invoke(this);
	//				JobAction();
	//				_tcs.SetResult(JobData);
	//			}
	//			catch (Exception ex) {
	//				OnExceptionEvent(ex);
	//			}
	//			finally {
	//				EndEvent?.Invoke(this, this);
	//			}
	//		}
	//		protected void OnExceptionEvent(Exception ex) => ExceptionEvent?.Invoke(ex);
	//	}
	//}
}