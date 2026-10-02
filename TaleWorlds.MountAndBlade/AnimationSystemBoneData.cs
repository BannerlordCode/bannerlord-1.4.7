using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200018D RID: 397
	[EngineStruct("Animation_system_bone_data", false, null)]
	[Serializable]
	public struct AnimationSystemBoneData
	{
		// Token: 0x040005C8 RID: 1480
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
		public sbyte[] IndicesOfRagdollBonesToCheckForCorpses;

		// Token: 0x040005C9 RID: 1481
		public sbyte CountOfRagdollBonesToCheckForCorpses;

		// Token: 0x040005CA RID: 1482
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public sbyte[] RagdollFallSoundBoneIndices;

		// Token: 0x040005CB RID: 1483
		public sbyte RagdollFallSoundBoneIndexCount;

		// Token: 0x040005CC RID: 1484
		public sbyte HeadLookDirectionBoneIndex;

		// Token: 0x040005CD RID: 1485
		public sbyte SpineLowerBoneIndex;

		// Token: 0x040005CE RID: 1486
		public sbyte SpineUpperBoneIndex;

		// Token: 0x040005CF RID: 1487
		public sbyte ThoraxLookDirectionBoneIndex;

		// Token: 0x040005D0 RID: 1488
		public sbyte NeckRootBoneIndex;

		// Token: 0x040005D1 RID: 1489
		public sbyte PelvisBoneIndex;

		// Token: 0x040005D2 RID: 1490
		public sbyte RightUpperArmBoneIndex;

		// Token: 0x040005D3 RID: 1491
		public sbyte LeftUpperArmBoneIndex;

		// Token: 0x040005D4 RID: 1492
		public sbyte FallBlowDamageBoneIndex;

		// Token: 0x040005D5 RID: 1493
		[CustomEngineStructMemberData("terrain_decal_bone_0_index")]
		public sbyte TerrainDecalBone0Index;

		// Token: 0x040005D6 RID: 1494
		[CustomEngineStructMemberData("terrain_decal_bone_1_index")]
		public sbyte TerrainDecalBone1Index;
	}
}
