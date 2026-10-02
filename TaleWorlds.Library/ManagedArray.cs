using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000061 RID: 97
	[Serializable]
	public struct ManagedArray
	{
		// Token: 0x060002C0 RID: 704 RVA: 0x000086C8 File Offset: 0x000068C8
		public ManagedArray(IntPtr array, int length)
		{
			this.Array = array;
			this.Length = length;
		}

		// Token: 0x04000120 RID: 288
		internal IntPtr Array;

		// Token: 0x04000121 RID: 289
		internal int Length;
	}
}
