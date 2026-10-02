using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017A RID: 378
	public static class QueryLibrary
	{
		// Token: 0x060013F2 RID: 5106 RVA: 0x00049A8B File Offset: 0x00047C8B
		public static bool IsInfantry(Agent a)
		{
			return !a.HasMount && !a.IsRangedCached;
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00049AA0 File Offset: 0x00047CA0
		public static bool IsInfantryWithoutBanner(Agent a)
		{
			return a.Banner == null && !a.HasMount && !a.IsRangedCached;
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00049ABD File Offset: 0x00047CBD
		public static bool HasShield(Agent a)
		{
			return a.HasShieldCached;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00049AC5 File Offset: 0x00047CC5
		public static bool IsRanged(Agent a)
		{
			return !a.HasMount && a.IsRangedCached;
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00049AD7 File Offset: 0x00047CD7
		public static bool IsRangedWithoutBanner(Agent a)
		{
			return a.Banner == null && !a.HasMount && a.IsRangedCached;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00049AF1 File Offset: 0x00047CF1
		public static bool IsCavalry(Agent a)
		{
			return a.HasMount && !a.IsRangedCached;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x00049B06 File Offset: 0x00047D06
		public static bool IsCavalryWithoutBanner(Agent a)
		{
			return a.Banner == null && a.HasMount && !a.IsRangedCached;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00049B23 File Offset: 0x00047D23
		public static bool IsRangedCavalry(Agent a)
		{
			return a.HasMount && a.IsRangedCached;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00049B35 File Offset: 0x00047D35
		public static bool IsRangedCavalryWithoutBanner(Agent a)
		{
			return a.Banner == null && a.HasMount && a.IsRangedCached;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00049B4F File Offset: 0x00047D4F
		public static bool HasSpear(Agent a)
		{
			return a.HasSpearCached;
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00049B57 File Offset: 0x00047D57
		public static bool HasThrown(Agent a)
		{
			return a.HasThrownCached;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00049B5F File Offset: 0x00047D5F
		public static bool IsHeavy(Agent a)
		{
			return MissionGameModels.Current.AgentStatCalculateModel.HasHeavyArmor(a);
		}
	}
}
