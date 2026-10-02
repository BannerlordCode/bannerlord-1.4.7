using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x0200001B RID: 27
	public static class BannerHelper
	{
		// Token: 0x060000FC RID: 252 RVA: 0x0000D0C8 File Offset: 0x0000B2C8
		public static ItemObject GetRandomBannerItemForHero(Hero hero)
		{
			return Campaign.Current.Models.BannerItemModel.GetPossibleRewardBannerItemsForHero(hero).GetRandomElementInefficiently<ItemObject>();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000D0E4 File Offset: 0x0000B2E4
		public static void AddBannerBonusForBanner(BannerEffect bannerEffect, BannerComponent bannerComponent, ref ExplainedNumber bonuses)
		{
			if (bannerComponent != null && bannerComponent.BannerEffect == bannerEffect)
			{
				BannerHelper.AddBannerEffectToStat(ref bonuses, bannerEffect.IncrementType, bannerComponent.GetBannerEffectBonus(), bannerEffect.Name);
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000D10A File Offset: 0x0000B30A
		private static void AddBannerEffectToStat(ref ExplainedNumber stat, EffectIncrementType effectIncrementType, float number, TextObject effectName)
		{
			if (effectIncrementType == EffectIncrementType.Add)
			{
				stat.Add(number, effectName, null);
				return;
			}
			if (effectIncrementType == EffectIncrementType.AddFactor)
			{
				stat.AddFactor(number, effectName);
			}
		}
	}
}
