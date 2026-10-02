using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000029 RID: 41
	public struct WindowClass
	{
		// Token: 0x040000BD RID: 189
		public uint style;

		// Token: 0x040000BE RID: 190
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public WndProc lpfnWndProc;

		// Token: 0x040000BF RID: 191
		public int cbClsExtra;

		// Token: 0x040000C0 RID: 192
		public int cbWndExtra;

		// Token: 0x040000C1 RID: 193
		public IntPtr hInstance;

		// Token: 0x040000C2 RID: 194
		public IntPtr hIcon;

		// Token: 0x040000C3 RID: 195
		public IntPtr hCursor;

		// Token: 0x040000C4 RID: 196
		public IntPtr hbrBackground;

		// Token: 0x040000C5 RID: 197
		[MarshalAs(UnmanagedType.LPTStr)]
		public string lpszMenuName;

		// Token: 0x040000C6 RID: 198
		[MarshalAs(UnmanagedType.LPTStr)]
		public string lpszClassName;
	}
}
