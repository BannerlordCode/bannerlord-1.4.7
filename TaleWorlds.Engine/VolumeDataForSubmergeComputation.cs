using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200004D RID: 77
	[EngineStruct("rglWater_renderer::Volume_data_for_submerge_computation", false, null)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct VolumeDataForSubmergeComputation
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00005D45 File Offset: 0x00003F45
		public float Height
		{
			get
			{
				return this.LocalScale[(int)this.DynamicUpAxis];
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x00005D58 File Offset: 0x00003F58
		public float Width
		{
			get
			{
				return this.LocalScale[(int)((this.DynamicUpAxis + 1) % (FloaterVolumeDynamicUpAxis)3)];
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00005D6F File Offset: 0x00003F6F
		public float Depth
		{
			get
			{
				return this.LocalScale[(int)((this.DynamicUpAxis + 2) % (FloaterVolumeDynamicUpAxis)3)];
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00005D86 File Offset: 0x00003F86
		public Vec3 Up
		{
			get
			{
				return this.LocalFrame.rotation[(int)this.DynamicUpAxis];
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00005D9E File Offset: 0x00003F9E
		public Vec3 Side
		{
			get
			{
				return this.LocalFrame.rotation[(int)((this.DynamicUpAxis + 1) % (FloaterVolumeDynamicUpAxis)3)];
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x00005DBA File Offset: 0x00003FBA
		public Vec3 Forward
		{
			get
			{
				return this.LocalFrame.rotation[(int)((this.DynamicUpAxis + 2) % (FloaterVolumeDynamicUpAxis)3)];
			}
		}

		// Token: 0x040000AC RID: 172
		public Vec3 DynamicLocalBottomPos;

		// Token: 0x040000AD RID: 173
		public MatrixFrame LocalFrame;

		// Token: 0x040000AE RID: 174
		public Vec3 LocalScale;

		// Token: 0x040000AF RID: 175
		public FloaterVolumeDynamicUpAxis DynamicUpAxis;

		// Token: 0x040000B0 RID: 176
		public Vec3 OutGlobalWaterSurfaceNormal;

		// Token: 0x040000B1 RID: 177
		public float InOutWaterHeightWrtVolume;
	}
}
