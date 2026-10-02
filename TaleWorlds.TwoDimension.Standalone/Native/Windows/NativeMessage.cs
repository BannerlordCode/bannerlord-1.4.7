using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000021 RID: 33
	public struct NativeMessage
	{
		// Token: 0x04000083 RID: 131
		public IntPtr handle;

		// Token: 0x04000084 RID: 132
		public WindowMessage msg;

		// Token: 0x04000085 RID: 133
		public IntPtr wParam;

		// Token: 0x04000086 RID: 134
		public IntPtr lParam;

		// Token: 0x04000087 RID: 135
		public uint time;

		// Token: 0x04000088 RID: 136
		public Point p;
	}
}
