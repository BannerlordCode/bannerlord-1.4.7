using System;
using System.Threading;

namespace TaleWorlds.Library
{
	// Token: 0x0200009C RID: 156
	public class TWSharedMutex
	{
		// Token: 0x06000590 RID: 1424 RVA: 0x000137DC File Offset: 0x000119DC
		public void EnterReadLock()
		{
			for (;;)
			{
				if (Volatile.Read(ref this._writerFlag) != 1 && Volatile.Read(ref this._writeRequests) <= 0)
				{
					Interlocked.Increment(ref this._readerCount);
					if (Volatile.Read(ref this._writerFlag) == 0 && Volatile.Read(ref this._writeRequests) == 0)
					{
						break;
					}
					Interlocked.Decrement(ref this._readerCount);
				}
				else
				{
					Thread.SpinWait(4);
				}
			}
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00013844 File Offset: 0x00011A44
		public void EnterWriteLock()
		{
			Interlocked.Increment(ref this._writeRequests);
			while (Interlocked.CompareExchange(ref this._writerFlag, 1, 0) != 0)
			{
				Thread.SpinWait(4);
			}
			while (Volatile.Read(ref this._readerCount) > 0)
			{
				Thread.SpinWait(4);
			}
			Interlocked.Decrement(ref this._writeRequests);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00013898 File Offset: 0x00011A98
		public void ExitReadLock()
		{
			Interlocked.Decrement(ref this._readerCount);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x000138A6 File Offset: 0x00011AA6
		public void ExitWriteLock()
		{
			Volatile.Write(ref this._writerFlag, 0);
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x000138B4 File Offset: 0x00011AB4
		public bool IsReadLockHeld
		{
			get
			{
				return Volatile.Read(ref this._readerCount) > 0;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x000138C4 File Offset: 0x00011AC4
		public bool IsWriteLockHeld
		{
			get
			{
				return Volatile.Read(ref this._writerFlag) > 0;
			}
		}

		// Token: 0x040001B0 RID: 432
		private int _readerCount;

		// Token: 0x040001B1 RID: 433
		private int _writerFlag;

		// Token: 0x040001B2 RID: 434
		private int _writeRequests;
	}
}
