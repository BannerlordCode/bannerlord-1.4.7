using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000020 RID: 32
	public static class Kernel32
	{
		// Token: 0x06000119 RID: 281
		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr LoadLibrary(string lpFileName);

		// Token: 0x0600011A RID: 282
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x0600011B RID: 283
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int GetLastError();

		// Token: 0x0600011C RID: 284
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern IntPtr GetConsoleWindow();

		// Token: 0x0600011D RID: 285
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		public static extern int GetUserGeoID(Kernel32.GeoTypeId type);

		// Token: 0x0200004B RID: 75
		public enum GeoTypeId
		{
			// Token: 0x040002FE RID: 766
			Nation = 16,
			// Token: 0x040002FF RID: 767
			Region = 14
		}
	}
}
