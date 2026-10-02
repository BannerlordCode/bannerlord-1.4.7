using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000141 RID: 321
	public class DefaultPregnancyModel : PregnancyModel
	{
		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060019A1 RID: 6561 RVA: 0x00080F9C File Offset: 0x0007F19C
		public override float PregnancyDurationInDays
		{
			get
			{
				return (float)((Campaign.Current.Options.AccelerationMode == GameAccelerationMode.Fast) ? 18 : 36);
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060019A2 RID: 6562 RVA: 0x00080FB7 File Offset: 0x0007F1B7
		public override float MaternalMortalityProbabilityInLabor
		{
			get
			{
				return 0.015f;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060019A3 RID: 6563 RVA: 0x00080FBE File Offset: 0x0007F1BE
		public override float StillbirthProbability
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060019A4 RID: 6564 RVA: 0x00080FC5 File Offset: 0x0007F1C5
		public override float DeliveringFemaleOffspringProbability
		{
			get
			{
				return 0.51f;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060019A5 RID: 6565 RVA: 0x00080FCC File Offset: 0x0007F1CC
		public override float DeliveringTwinsProbability
		{
			get
			{
				return 0.03f;
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x00080FD3 File Offset: 0x0007F1D3
		private bool IsHeroAgeSuitableForPregnancy(Hero hero)
		{
			return hero.Age >= 18f && hero.Age <= 45f;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x00080FF4 File Offset: 0x0007F1F4
		public override float GetDailyChanceOfPregnancyForHero(Hero hero)
		{
			int num = hero.Children.Count + 1;
			float num2 = (float)(4 + 4 * hero.Clan.Tier);
			int count = hero.Clan.AliveLords.Count;
			float num3 = ((hero != Hero.MainHero && hero.Spouse != Hero.MainHero) ? Math.Min(1f, (2f * num2 - (float)count) / num2) : 1f);
			float num4 = (1.2f - (hero.Age - 18f) * 0.04f) / (float)(num * num) * 0.12f * num3;
			float num5 = ((hero.Spouse != null && this.IsHeroAgeSuitableForPregnancy(hero)) ? num4 : 0f);
			ExplainedNumber explainedNumber = new ExplainedNumber(num5, false, null);
			if (hero.GetPerkValue(DefaultPerks.Charm.Virile) || hero.Spouse.GetPerkValue(DefaultPerks.Charm.Virile))
			{
				explainedNumber.AddFactor(DefaultPerks.Charm.Virile.PrimaryBonus, DefaultPerks.Charm.Virile.Name);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x04000888 RID: 2184
		private const int MinPregnancyAge = 18;

		// Token: 0x04000889 RID: 2185
		private const int MaxPregnancyAge = 45;
	}
}
