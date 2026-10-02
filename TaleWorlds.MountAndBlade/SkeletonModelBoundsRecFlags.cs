using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D9 RID: 473
	[EngineStruct("Skeleton_model_bounds_rec_flags", true, "smbrf", false)]
	public enum SkeletonModelBoundsRecFlags : sbyte
	{
		// Token: 0x0400097F RID: 2431
		None,
		// Token: 0x04000980 RID: 2432
		UseSmallerRadiusMultWhileHoldingShield,
		// Token: 0x04000981 RID: 2433
		Sweep,
		// Token: 0x04000982 RID: 2434
		DoNotScaleAccordingToAgentScale = 4
	}
}
