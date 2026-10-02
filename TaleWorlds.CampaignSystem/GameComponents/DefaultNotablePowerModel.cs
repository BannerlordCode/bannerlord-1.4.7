using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200012F RID: 303
	public class DefaultNotablePowerModel : NotablePowerModel
	{
		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001902 RID: 6402 RVA: 0x0007BAD1 File Offset: 0x00079CD1
		public override int NotableDisappearPowerLimit
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0007BAD8 File Offset: 0x00079CD8
		public override ExplainedNumber CalculateDailyPowerChangeForHero(Hero hero, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			if (!hero.IsActive)
			{
				return explainedNumber;
			}
			if (hero.Power > (float)this.RegularNotableMaxPowerLevel)
			{
				this.CalculateDailyPowerChangeForInfluentialNotables(hero, ref explainedNumber);
			}
			this.CalculateDailyPowerChangePerPropertyOwned(hero, ref explainedNumber);
			if (hero.Issue != null)
			{
				this.CalculatePowerChangeFromIssues(hero, ref explainedNumber);
			}
			if (hero.IsArtisan)
			{
				explainedNumber.Add(-0.1f, this._propertyEffect, null);
			}
			if (hero.IsGangLeader)
			{
				explainedNumber.Add(-0.4f, this._propertyEffect, null);
			}
			if (hero.IsRuralNotable)
			{
				explainedNumber.Add(0.1f, this._propertyEffect, null);
			}
			if (hero.IsHeadman)
			{
				explainedNumber.Add(0.1f, this._propertyEffect, null);
			}
			if (hero.IsMerchant)
			{
				explainedNumber.Add(0.2f, this._propertyEffect, null);
			}
			if (hero.CurrentSettlement != null)
			{
				if (hero.CurrentSettlement.IsVillage && hero.CurrentSettlement.Village.Bound.IsCastle)
				{
					explainedNumber.Add(0.1f, this._propertyEffect, null);
				}
				if (hero.SupporterOf == hero.CurrentSettlement.OwnerClan)
				{
					this.CalculateDailyPowerChangeForAffiliationWithRulerClan(ref explainedNumber);
				}
			}
			return explainedNumber;
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x0007BC11 File Offset: 0x00079E11
		public override int RegularNotableMaxPowerLevel
		{
			get
			{
				return this.NotablePowerRanks[1].MinPowerValue;
			}
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x0007BC24 File Offset: 0x00079E24
		private void CalculateDailyPowerChangePerPropertyOwned(Hero hero, ref ExplainedNumber explainedNumber)
		{
			int count = hero.OwnedAlleys.Count;
			explainedNumber.Add(0.1f * (float)count, this._propertyEffect, null);
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x0007BC52 File Offset: 0x00079E52
		private void CalculateDailyPowerChangeForAffiliationWithRulerClan(ref ExplainedNumber explainedNumber)
		{
			explainedNumber.Add(0.2f, this._rulerClanEffect, null);
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x0007BC68 File Offset: 0x00079E68
		private void CalculateDailyPowerChangeForInfluentialNotables(Hero hero, ref ExplainedNumber explainedNumber)
		{
			float num = -1f * ((hero.Power - (float)this.RegularNotableMaxPowerLevel) / 500f);
			explainedNumber.Add(num, this._currentRankEffect, null);
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x0007BC9E File Offset: 0x00079E9E
		private void CalculatePowerChangeFromIssues(Hero hero, ref ExplainedNumber explainedNumber)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectOfHero(DefaultIssueEffects.IssueOwnerPower, hero, ref explainedNumber);
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x0007BCBB File Offset: 0x00079EBB
		public override TextObject GetPowerRankName(Hero hero)
		{
			return this.GetPowerRank(hero).Name;
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0007BCC9 File Offset: 0x00079EC9
		public override float GetInfluenceBonusToClan(Hero hero)
		{
			return this.GetPowerRank(hero).InfluenceBonus;
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0007BCD8 File Offset: 0x00079ED8
		private DefaultNotablePowerModel.NotablePowerRank GetPowerRank(Hero hero)
		{
			int num = 0;
			for (int i = 0; i < this.NotablePowerRanks.Length; i++)
			{
				if (hero.Power > (float)this.NotablePowerRanks[i].MinPowerValue)
				{
					num = i;
				}
			}
			return this.NotablePowerRanks[num];
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x0007BD24 File Offset: 0x00079F24
		public override int GetInitialPower(Hero hero)
		{
			int num = 0;
			float randomFloat = MBRandom.RandomFloat;
			num += ((randomFloat < 0.2f) ? MBRandom.RandomInt((int)((float)(this.NotablePowerRanks[0].MinPowerValue + this.NotablePowerRanks[1].MinPowerValue) * 0.5f), this.NotablePowerRanks[1].MinPowerValue) : ((randomFloat < 0.8f) ? MBRandom.RandomInt(this.NotablePowerRanks[1].MinPowerValue, this.NotablePowerRanks[2].MinPowerValue) : MBRandom.RandomInt(this.NotablePowerRanks[2].MinPowerValue, (int)((float)this.NotablePowerRanks[2].MinPowerValue * 2f))));
			if ((hero.Occupation == Occupation.GangLeader || hero.Occupation == Occupation.Artisan || hero.Occupation == Occupation.RuralNotable || hero.Occupation == Occupation.Merchant || hero.Occupation == Occupation.Headman) && hero.HomeSettlement.IsVillage && hero.HomeSettlement.Village.Bound != null && hero.HomeSettlement.Village.Bound.IsCastle)
			{
				num += (int)(MBRandom.RandomFloat * 20f);
			}
			return num;
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0007BE5F File Offset: 0x0007A05F
		public override int GetInitialNotableSupporterCost(Hero hero)
		{
			return 20000 + 10000 * Clan.PlayerClan.SupporterNotables.Count;
		}

		// Token: 0x04000823 RID: 2083
		private DefaultNotablePowerModel.NotablePowerRank[] NotablePowerRanks = new DefaultNotablePowerModel.NotablePowerRank[]
		{
			new DefaultNotablePowerModel.NotablePowerRank(new TextObject("{=aTeuX4L0}Regular", null), 0, 0.05f),
			new DefaultNotablePowerModel.NotablePowerRank(new TextObject("{=nTETQEmy}Influential", null), 100, 0.1f),
			new DefaultNotablePowerModel.NotablePowerRank(new TextObject("{=UCpyo9hw}Powerful", null), 200, 0.15f)
		};

		// Token: 0x04000824 RID: 2084
		private TextObject _currentRankEffect = new TextObject("{=7j9uHxLM}Current Rank Effect", null);

		// Token: 0x04000825 RID: 2085
		private TextObject _militiaEffect = new TextObject("{=R1MaIgOb}Militia Effect", null);

		// Token: 0x04000826 RID: 2086
		private TextObject _rulerClanEffect = new TextObject("{=JE3RTqx5}Ruler Clan Effect", null);

		// Token: 0x04000827 RID: 2087
		private TextObject _propertyEffect = new TextObject("{=yDomN9L2}Property Effect", null);

		// Token: 0x02000598 RID: 1432
		private struct NotablePowerRank
		{
			// Token: 0x06004E6A RID: 20074 RVA: 0x00181B81 File Offset: 0x0017FD81
			public NotablePowerRank(TextObject name, int minPowerValue, float influenceBonus)
			{
				this.Name = name;
				this.MinPowerValue = minPowerValue;
				this.InfluenceBonus = influenceBonus;
			}

			// Token: 0x040017DA RID: 6106
			public readonly TextObject Name;

			// Token: 0x040017DB RID: 6107
			public readonly int MinPowerValue;

			// Token: 0x040017DC RID: 6108
			public readonly float InfluenceBonus;
		}
	}
}
