using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MBHelpers
{
	// Token: 0x020000DC RID: 220
	public static class BannerHelper
	{
		// Token: 0x060008F9 RID: 2297 RVA: 0x0000F546 File Offset: 0x0000D746
		public static void AddBannerBonusForBanner(BannerEffect bannerEffect, BannerComponent bannerComponent, ref FactoredNumber bonuses)
		{
			if (bannerComponent != null && bannerComponent.BannerEffect == bannerEffect)
			{
				BannerHelper.AddBannerEffectToStat(ref bonuses, bannerEffect.IncrementType, bannerComponent.GetBannerEffectBonus());
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0000F566 File Offset: 0x0000D766
		private static void AddBannerEffectToStat(ref FactoredNumber stat, EffectIncrementType effectIncrementType, float number)
		{
			if (effectIncrementType == EffectIncrementType.Add)
			{
				stat.Add(number);
				return;
			}
			if (effectIncrementType == EffectIncrementType.AddFactor)
			{
				stat.AddFactor(number);
			}
		}
	}
}
