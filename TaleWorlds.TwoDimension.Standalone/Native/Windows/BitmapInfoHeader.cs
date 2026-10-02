using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000017 RID: 23
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BitmapInfoHeader
	{
		// Token: 0x04000066 RID: 102
		public uint biSize;

		// Token: 0x04000067 RID: 103
		public int biWidth;

		// Token: 0x04000068 RID: 104
		public int biHeight;

		// Token: 0x04000069 RID: 105
		public ushort biPlanes;

		// Token: 0x0400006A RID: 106
		public ushort biBitCount;

		// Token: 0x0400006B RID: 107
		public uint biCompression;

		// Token: 0x0400006C RID: 108
		public uint biSizeImage;

		// Token: 0x0400006D RID: 109
		public int biXPelsPerMeter;

		// Token: 0x0400006E RID: 110
		public int biYPelsPerMeter;

		// Token: 0x0400006F RID: 111
		public uint biClrUsed;

		// Token: 0x04000070 RID: 112
		public uint biClrImportant;
	}
}
