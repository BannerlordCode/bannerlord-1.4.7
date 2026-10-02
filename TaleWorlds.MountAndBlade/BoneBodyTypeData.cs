using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001DA RID: 474
	[EngineStruct("Bone_body_type_data", false, null)]
	public struct BoneBodyTypeData
	{
		// Token: 0x04000983 RID: 2435
		[CustomEngineStructMemberData(true)]
		public readonly BoneBodyPartType BodyPartType;

		// Token: 0x04000984 RID: 2436
		[CustomEngineStructMemberData(true)]
		public readonly sbyte Priority;

		// Token: 0x04000985 RID: 2437
		[CustomEngineStructMemberData(true)]
		public readonly SkeletonModelBoundsRecFlags DataFlags;
	}
}
