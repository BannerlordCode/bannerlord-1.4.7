using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000193 RID: 403
	[Flags]
	[EngineStruct("Combat_hit_result_flags", true, "chrf", false)]
	public enum CombatHitResultFlags : byte
	{
		// Token: 0x0400061C RID: 1564
		NormalHit = 0,
		// Token: 0x0400061D RID: 1565
		HitWithStartOfTheAnimation = 1,
		// Token: 0x0400061E RID: 1566
		HitWithArm = 2,
		// Token: 0x0400061F RID: 1567
		HitWithBackOfTheWeapon = 4
	}
}
