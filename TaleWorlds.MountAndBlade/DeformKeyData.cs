using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BF RID: 447
	[EngineStruct("Deform_Key_Data", false, null)]
	public struct DeformKeyData
	{
		// Token: 0x04000874 RID: 2164
		public int GroupId;

		// Token: 0x04000875 RID: 2165
		public int KeyTimePoint;

		// Token: 0x04000876 RID: 2166
		public float KeyMin;

		// Token: 0x04000877 RID: 2167
		public float KeyMax;

		// Token: 0x04000878 RID: 2168
		public float Value;

		// Token: 0x04000879 RID: 2169
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string Id;
	}
}
