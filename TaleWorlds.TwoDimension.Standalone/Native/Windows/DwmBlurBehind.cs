using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200001B RID: 27
	internal struct DwmBlurBehind
	{
		// Token: 0x04000079 RID: 121
		public BlurBehindConstraints dwFlags;

		// Token: 0x0400007A RID: 122
		[MarshalAs(UnmanagedType.Bool)]
		public bool fEnable;

		// Token: 0x0400007B RID: 123
		public IntPtr hRgnBlur;

		// Token: 0x0400007C RID: 124
		[MarshalAs(UnmanagedType.Bool)]
		public bool fTransitionOnMaximized;
	}
}
