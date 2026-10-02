using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200001D RID: 29
	public static class D3D11
	{
		// Token: 0x0600010B RID: 267
		[DllImport("d3d11.dll")]
		public static extern int D3D11CreateDevice(DXGI.IDXGIAdapter adapter, D3D11.D3D_DRIVER_TYPE driverType, IntPtr software, uint flags, IntPtr featureLevels, int featureLevelCount, int sdkVersion, out IntPtr ppDevice, IntPtr pFeatureLevel, out IntPtr ppImmediateContext);

		// Token: 0x02000044 RID: 68
		public enum D3D_DRIVER_TYPE
		{
			// Token: 0x040002E5 RID: 741
			UNKNOWN,
			// Token: 0x040002E6 RID: 742
			HARDWARE,
			// Token: 0x040002E7 RID: 743
			REFERENCE,
			// Token: 0x040002E8 RID: 744
			NULL_DRIVER,
			// Token: 0x040002E9 RID: 745
			SOFTWARE,
			// Token: 0x040002EA RID: 746
			WARP
		}
	}
}
