using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000016 RID: 22
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BitmapInfo
	{
		// Token: 0x04000061 RID: 97
		public BitmapInfoHeader bmiHeader;

		// Token: 0x04000062 RID: 98
		public byte r;

		// Token: 0x04000063 RID: 99
		public byte g;

		// Token: 0x04000064 RID: 100
		public byte b;

		// Token: 0x04000065 RID: 101
		public byte a;
	}
}
