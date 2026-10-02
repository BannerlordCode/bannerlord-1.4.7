using System;
using System.Threading;

namespace TaleWorlds.Library
{
	// Token: 0x0200008F RID: 143
	public static class SingleThreadedSynchronizationContextManager
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x00012620 File Offset: 0x00010820
		public static void Initialize()
		{
			if (SingleThreadedSynchronizationContextManager._synchronizationContext == null)
			{
				SingleThreadedSynchronizationContextManager._synchronizationContext = new SingleThreadedSynchronizationContext();
				SynchronizationContext.SetSynchronizationContext(SingleThreadedSynchronizationContextManager._synchronizationContext);
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001263D File Offset: 0x0001083D
		public static void Tick()
		{
			SingleThreadedSynchronizationContextManager._synchronizationContext.Tick();
		}

		// Token: 0x04000196 RID: 406
		private static SingleThreadedSynchronizationContext _synchronizationContext;
	}
}
