using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200018F RID: 399
	[EngineStruct("Animation_system_bone_data_quadruped", false, null)]
	[Serializable]
	public struct AnimationSystemBoneDataQuadruped
	{
		// Token: 0x040005EE RID: 1518
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
		public sbyte[] BoneIndicesToModifyOnSlopingGround;

		// Token: 0x040005EF RID: 1519
		public sbyte BoneIndicesToModifyOnSlopingGroundCount;

		// Token: 0x040005F0 RID: 1520
		public sbyte BodyRotationReferenceBoneIndex;

		// Token: 0x040005F1 RID: 1521
		public sbyte RiderSitBoneIndex;

		// Token: 0x040005F2 RID: 1522
		public sbyte ReinHandleBoneIndex;

		// Token: 0x040005F3 RID: 1523
		[CustomEngineStructMemberData("rein_collision_1_bone_index")]
		public sbyte ReinCollision1BoneIndex;

		// Token: 0x040005F4 RID: 1524
		[CustomEngineStructMemberData("rein_collision_2_bone_index")]
		public sbyte ReinCollision2BoneIndex;

		// Token: 0x040005F5 RID: 1525
		public sbyte ReinHeadBoneIndex;

		// Token: 0x040005F6 RID: 1526
		public sbyte ReinHeadRightAttachmentBoneIndex;

		// Token: 0x040005F7 RID: 1527
		public sbyte ReinHeadLeftAttachmentBoneIndex;

		// Token: 0x040005F8 RID: 1528
		public sbyte ReinRightHandBoneIndex;

		// Token: 0x040005F9 RID: 1529
		public sbyte ReinLeftHandBoneIndex;
	}
}
