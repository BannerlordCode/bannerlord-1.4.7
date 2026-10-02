using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000192 RID: 402
	[EngineStruct("Melee_collision_reaction", true, "mcr", false)]
	public enum MeleeCollisionReaction
	{
		// Token: 0x04000615 RID: 1557
		Invalid = -1,
		// Token: 0x04000616 RID: 1558
		SlicedThrough,
		// Token: 0x04000617 RID: 1559
		ContinueChecking,
		// Token: 0x04000618 RID: 1560
		Stuck,
		// Token: 0x04000619 RID: 1561
		Bounced,
		// Token: 0x0400061A RID: 1562
		Staggered
	}
}
