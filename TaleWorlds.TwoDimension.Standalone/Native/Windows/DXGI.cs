using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200001E RID: 30
	public static class DXGI
	{
		// Token: 0x0600010C RID: 268
		[DllImport("dxgi.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int CreateDXGIFactory(ref Guid riid, out IntPtr factory);

		// Token: 0x04000081 RID: 129
		public static Guid IID_IDXGIAdapter = new Guid("2411E7E1-12AC-4CCF-BD14-9798E8534DC0");

		// Token: 0x04000082 RID: 130
		public static Guid IID_IDXGIFactory = new Guid("7B7166EC-21C7-44AE-B21A-C9AE321AE369");

		// Token: 0x02000045 RID: 69
		[Guid("7B7166EC-21C7-44AE-B21A-C9AE321AE369")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDXGIFactory
		{
			// Token: 0x0600018C RID: 396
			int SetPrivateData();

			// Token: 0x0600018D RID: 397
			int SetPrivateDataInterface();

			// Token: 0x0600018E RID: 398
			int GetPrivateData();

			// Token: 0x0600018F RID: 399
			int GetParent();

			// Token: 0x06000190 RID: 400
			[PreserveSig]
			int EnumAdapters(uint index, out DXGI.IDXGIAdapter adapter);
		}

		// Token: 0x02000046 RID: 70
		[Guid("2411E7E1-12AC-4CCF-BD14-9798E8534DC0")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDXGIAdapter
		{
			// Token: 0x06000191 RID: 401
			[PreserveSig]
			int SetPrivateData();

			// Token: 0x06000192 RID: 402
			[PreserveSig]
			int SetPrivateDataInterface();

			// Token: 0x06000193 RID: 403
			[PreserveSig]
			int GetPrivateData();

			// Token: 0x06000194 RID: 404
			[PreserveSig]
			int GetParent();

			// Token: 0x06000195 RID: 405
			[PreserveSig]
			int EnumOutputs(uint Output, [MarshalAs(UnmanagedType.Interface)] out DXGI.IDXGIOutput ppOutput);

			// Token: 0x06000196 RID: 406
			[PreserveSig]
			int GetDesc(out DXGI.DXGI_ADAPTER_DESC desc);
		}

		// Token: 0x02000047 RID: 71
		[Guid("AE02EEDB-C735-4690-8D52-5A8DC20213AA")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDXGIOutput
		{
			// Token: 0x06000197 RID: 407
			int SetPrivateData();

			// Token: 0x06000198 RID: 408
			int SetPrivateDataInterface();

			// Token: 0x06000199 RID: 409
			int GetPrivateData();

			// Token: 0x0600019A RID: 410
			int GetParent();

			// Token: 0x0600019B RID: 411
			int GetDesc(out DXGI.DXGI_OUTPUT_DESC desc);

			// Token: 0x0600019C RID: 412
			int GetDisplayModeList();

			// Token: 0x0600019D RID: 413
			int FindClosestMatchingMode();

			// Token: 0x0600019E RID: 414
			int WaitForVBlank();

			// Token: 0x0600019F RID: 415
			int TakeOwnership();

			// Token: 0x060001A0 RID: 416
			int ReleaseOwnership();

			// Token: 0x060001A1 RID: 417
			int GetGammaControlCapabilities();

			// Token: 0x060001A2 RID: 418
			int SetGammaControl();

			// Token: 0x060001A3 RID: 419
			int GetGammaControl();

			// Token: 0x060001A4 RID: 420
			int SetDisplaySurface();

			// Token: 0x060001A5 RID: 421
			int GetDisplaySurfaceData();

			// Token: 0x060001A6 RID: 422
			int GetFrameStatistics();
		}

		// Token: 0x02000048 RID: 72
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct DXGI_ADAPTER_DESC
		{
			// Token: 0x040002EB RID: 747
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string Description;

			// Token: 0x040002EC RID: 748
			public uint VendorId;

			// Token: 0x040002ED RID: 749
			public uint DeviceId;

			// Token: 0x040002EE RID: 750
			public uint SubSysId;

			// Token: 0x040002EF RID: 751
			public uint Revision;

			// Token: 0x040002F0 RID: 752
			public UIntPtr DedicatedVideoMemory;

			// Token: 0x040002F1 RID: 753
			public UIntPtr DedicatedSystemMemory;

			// Token: 0x040002F2 RID: 754
			public UIntPtr SharedSystemMemory;

			// Token: 0x040002F3 RID: 755
			public UIntPtr AdapterLuid;
		}

		// Token: 0x02000049 RID: 73
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct DXGI_OUTPUT_DESC
		{
			// Token: 0x040002F4 RID: 756
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string DeviceName;

			// Token: 0x040002F5 RID: 757
			public DXGI.RECT DesktopCoordinates;

			// Token: 0x040002F6 RID: 758
			public bool AttachedToDesktop;

			// Token: 0x040002F7 RID: 759
			public uint Rotation;

			// Token: 0x040002F8 RID: 760
			public IntPtr Monitor;
		}

		// Token: 0x0200004A RID: 74
		public struct RECT
		{
			// Token: 0x060001A7 RID: 423 RVA: 0x000061D2 File Offset: 0x000043D2
			public override bool Equals(object o)
			{
				return o != null && o is DXGI.RECT && this == (DXGI.RECT)o;
			}

			// Token: 0x060001A8 RID: 424 RVA: 0x000061F4 File Offset: 0x000043F4
			public override int GetHashCode()
			{
				return 0;
			}

			// Token: 0x060001A9 RID: 425 RVA: 0x000061F7 File Offset: 0x000043F7
			public static bool operator ==(DXGI.RECT r1, DXGI.RECT r2)
			{
				return r1.bottom == r2.bottom && r1.right == r2.right && r1.top == r2.top && r1.left == r2.left;
			}

			// Token: 0x060001AA RID: 426 RVA: 0x00006233 File Offset: 0x00004433
			public static bool operator !=(DXGI.RECT r1, DXGI.RECT r2)
			{
				return r1.bottom != r2.bottom || r1.right != r2.right || r1.top != r2.top || r1.left != r2.left;
			}

			// Token: 0x040002F9 RID: 761
			public int left;

			// Token: 0x040002FA RID: 762
			public int top;

			// Token: 0x040002FB RID: 763
			public int right;

			// Token: 0x040002FC RID: 764
			public int bottom;
		}
	}
}
