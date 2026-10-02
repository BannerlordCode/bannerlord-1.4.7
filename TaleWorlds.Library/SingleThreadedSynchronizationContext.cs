using System;
using System.Collections.Generic;
using System.Threading;

namespace TaleWorlds.Library
{
	// Token: 0x0200008E RID: 142
	public sealed class SingleThreadedSynchronizationContext : SynchronizationContext
	{
		// Token: 0x0600050F RID: 1295 RVA: 0x00012456 File Offset: 0x00010656
		public SingleThreadedSynchronizationContext()
		{
			this._worksLock = new object();
			this._futureWorks = new List<SingleThreadedSynchronizationContext.WorkRequest>(100);
			this._currentWorks = new List<SingleThreadedSynchronizationContext.WorkRequest>(100);
			this._mainThreadId = Thread.CurrentThread.ManagedThreadId;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00012494 File Offset: 0x00010694
		public override void Send(SendOrPostCallback callback, object state)
		{
			if (this._mainThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				callback.DynamicInvokeWithLog(new object[] { state });
				return;
			}
			using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
			{
				object worksLock = this._worksLock;
				lock (worksLock)
				{
					this._futureWorks.Add(new SingleThreadedSynchronizationContext.WorkRequest(callback, state, manualResetEvent));
				}
				manualResetEvent.WaitOne();
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001252C File Offset: 0x0001072C
		public override void Post(SendOrPostCallback callback, object state)
		{
			SingleThreadedSynchronizationContext.WorkRequest workRequest = new SingleThreadedSynchronizationContext.WorkRequest(callback, state, null);
			object worksLock = this._worksLock;
			lock (worksLock)
			{
				this._futureWorks.Add(workRequest);
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001257C File Offset: 0x0001077C
		public void Tick()
		{
			object worksLock = this._worksLock;
			lock (worksLock)
			{
				List<SingleThreadedSynchronizationContext.WorkRequest> currentWorks = this._currentWorks;
				this._currentWorks = this._futureWorks;
				this._futureWorks = currentWorks;
			}
			foreach (SingleThreadedSynchronizationContext.WorkRequest workRequest in this._currentWorks)
			{
				workRequest.Invoke();
			}
			this._currentWorks.Clear();
		}

		// Token: 0x04000192 RID: 402
		private List<SingleThreadedSynchronizationContext.WorkRequest> _futureWorks;

		// Token: 0x04000193 RID: 403
		private List<SingleThreadedSynchronizationContext.WorkRequest> _currentWorks;

		// Token: 0x04000194 RID: 404
		private readonly object _worksLock;

		// Token: 0x04000195 RID: 405
		private readonly int _mainThreadId;

		// Token: 0x020000E4 RID: 228
		private struct WorkRequest
		{
			// Token: 0x060007A7 RID: 1959 RVA: 0x00019373 File Offset: 0x00017573
			public WorkRequest(SendOrPostCallback callback, object state, ManualResetEvent waitHandle = null)
			{
				this._callback = callback;
				this._state = state;
				this._waitHandle = waitHandle;
			}

			// Token: 0x060007A8 RID: 1960 RVA: 0x0001938A File Offset: 0x0001758A
			public void Invoke()
			{
				this._callback.DynamicInvokeWithLog(new object[] { this._state });
				ManualResetEvent waitHandle = this._waitHandle;
				if (waitHandle == null)
				{
					return;
				}
				waitHandle.Set();
			}

			// Token: 0x040002EF RID: 751
			private readonly SendOrPostCallback _callback;

			// Token: 0x040002F0 RID: 752
			private readonly object _state;

			// Token: 0x040002F1 RID: 753
			private readonly ManualResetEvent _waitHandle;
		}
	}
}
