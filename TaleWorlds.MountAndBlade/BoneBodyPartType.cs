using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D8 RID: 472
	[EngineStruct("Bone_body_part_type", false, null)]
	public enum BoneBodyPartType : sbyte
	{
		// Token: 0x04000971 RID: 2417
		None = -1,
		// Token: 0x04000972 RID: 2418
		Head,
		// Token: 0x04000973 RID: 2419
		Neck,
		// Token: 0x04000974 RID: 2420
		Chest,
		// Token: 0x04000975 RID: 2421
		Abdomen,
		// Token: 0x04000976 RID: 2422
		ShoulderLeft,
		// Token: 0x04000977 RID: 2423
		ShoulderRight,
		// Token: 0x04000978 RID: 2424
		ArmLeft,
		// Token: 0x04000979 RID: 2425
		ArmRight,
		// Token: 0x0400097A RID: 2426
		Legs,
		// Token: 0x0400097B RID: 2427
		NumOfBodyPartTypes,
		// Token: 0x0400097C RID: 2428
		CriticalBodyPartsBegin = 0,
		// Token: 0x0400097D RID: 2429
		CriticalBodyPartsEnd = 6
	}
}
