using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200009D RID: 157
	public struct TWSharedMutexReadLock : IDisposable
	{
		// Token: 0x06000596 RID: 1430 RVA: 0x000138D4 File Offset: 0x00011AD4
		public TWSharedMutexReadLock(TWSharedMutex mtx)
		{
			mtx.EnterReadLock();
			this._mtx = mtx;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000138E3 File Offset: 0x00011AE3
		public void Dispose()
		{
			this._mtx.ExitReadLock();
		}

		// Token: 0x040001B3 RID: 435
		private readonly TWSharedMutex _mtx;
	}
}
