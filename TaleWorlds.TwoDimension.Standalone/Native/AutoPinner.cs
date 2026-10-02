using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native
{
	// Token: 0x02000015 RID: 21
	internal class AutoPinner : IDisposable
	{
		// Token: 0x06000105 RID: 261 RVA: 0x00005C53 File Offset: 0x00003E53
		public AutoPinner(object obj)
		{
			if (obj != null)
			{
				this._pinnedObject = GCHandle.Alloc(obj, GCHandleType.Pinned);
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005C6B File Offset: 0x00003E6B
		public static implicit operator IntPtr(AutoPinner autoPinner)
		{
			if (autoPinner._pinnedObject.IsAllocated)
			{
				return autoPinner._pinnedObject.AddrOfPinnedObject();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005C8B File Offset: 0x00003E8B
		public void Dispose()
		{
			if (this._pinnedObject.IsAllocated)
			{
				this._pinnedObject.Free();
			}
		}

		// Token: 0x04000060 RID: 96
		private GCHandle _pinnedObject;
	}
}
