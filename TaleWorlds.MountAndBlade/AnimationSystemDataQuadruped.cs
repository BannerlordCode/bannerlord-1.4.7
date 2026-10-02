using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000190 RID: 400
	[EngineStruct("Animation_system_data_quadruped", false, null)]
	[Serializable]
	public struct AnimationSystemDataQuadruped
	{
		// Token: 0x040005FA RID: 1530
		public Vec3 ReinHandleLeftLocalPosition;

		// Token: 0x040005FB RID: 1531
		public Vec3 ReinHandleRightLocalPosition;

		// Token: 0x040005FC RID: 1532
		public string ReinSkeleton;

		// Token: 0x040005FD RID: 1533
		public string ReinCollisionBody;

		// Token: 0x040005FE RID: 1534
		public sbyte IndexOfBoneToDetectGroundSlopeFront;

		// Token: 0x040005FF RID: 1535
		public sbyte IndexOfBoneToDetectGroundSlopeBack;

		// Token: 0x04000600 RID: 1536
		public AnimationSystemBoneDataQuadruped Bones;
	}
}
