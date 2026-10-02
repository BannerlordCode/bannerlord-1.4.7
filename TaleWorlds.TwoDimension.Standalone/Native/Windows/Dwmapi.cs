using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200001A RID: 26
	internal static class Dwmapi
	{
		// Token: 0x0600010A RID: 266
		[DllImport("Dwmapi.dll")]
		public static extern IntPtr DwmEnableBlurBehindWindow(IntPtr hwnd, [In] ref DwmBlurBehind ppfd);
	}
}
