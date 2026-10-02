using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000026 RID: 38
	public struct Point
	{
		// Token: 0x0600011E RID: 286 RVA: 0x00005CF8 File Offset: 0x00003EF8
		public Point(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x040000BB RID: 187
		public int X;

		// Token: 0x040000BC RID: 188
		public int Y;
	}
}
