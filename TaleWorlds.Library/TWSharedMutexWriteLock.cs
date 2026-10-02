using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200009E RID: 158
	public struct TWSharedMutexWriteLock : IDisposable
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x000138F0 File Offset: 0x00011AF0
		public TWSharedMutexWriteLock(TWSharedMutex mtx)
		{
			mtx.EnterWriteLock();
			this._mtx = mtx;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x000138FF File Offset: 0x00011AFF
		public void Dispose()
		{
			this._mtx.ExitWriteLock();
		}

		// Token: 0x040001B4 RID: 436
		private readonly TWSharedMutex _mtx;
	}
}
