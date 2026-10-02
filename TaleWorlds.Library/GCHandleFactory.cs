using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TaleWorlds.Library
{
	// Token: 0x02000034 RID: 52
	internal static class GCHandleFactory
	{
		// Token: 0x060001B8 RID: 440 RVA: 0x00006F60 File Offset: 0x00005160
		static GCHandleFactory()
		{
			for (int i = 0; i < 512; i++)
			{
				GCHandleFactory._handles.Add(GCHandle.Alloc(null, GCHandleType.Pinned));
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00006FA4 File Offset: 0x000051A4
		public static GCHandle GetHandle()
		{
			object locker = GCHandleFactory._locker;
			lock (locker)
			{
				if (GCHandleFactory._handles.Count > 0)
				{
					GCHandle gchandle = GCHandleFactory._handles[GCHandleFactory._handles.Count - 1];
					GCHandleFactory._handles.RemoveAt(GCHandleFactory._handles.Count - 1);
					return gchandle;
				}
			}
			return GCHandle.Alloc(null, GCHandleType.Pinned);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00007024 File Offset: 0x00005224
		public static void ReturnHandle(GCHandle handle)
		{
			object locker = GCHandleFactory._locker;
			lock (locker)
			{
				GCHandleFactory._handles.Add(handle);
			}
		}

		// Token: 0x040000AA RID: 170
		private static List<GCHandle> _handles = new List<GCHandle>();

		// Token: 0x040000AB RID: 171
		private static object _locker = new object();
	}
}
