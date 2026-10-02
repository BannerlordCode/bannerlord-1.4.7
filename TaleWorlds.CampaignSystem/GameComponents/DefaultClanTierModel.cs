using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000106 RID: 262
	public class DefaultClanTierModel : ClanTierModel
	{
		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001729 RID: 5929 RVA: 0x0006CA29 File Offset: 0x0006AC29
		public override int MinClanTier
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x0600172A RID: 5930 RVA: 0x0006CA2C File Offset: 0x0006AC2C
		public override int MaxClanTier
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x0600172B RID: 5931 RVA: 0x0006CA2F File Offset: 0x0006AC2F
		public override int MercenaryEligibleTier
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x0600172C RID: 5932 RVA: 0x0006CA32 File Offset: 0x0006AC32
		public override int VassalEligibleTier
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x0600172D RID: 5933 RVA: 0x0006CA35 File Offset: 0x0006AC35
		public override int BannerEligibleTier
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x0600172E RID: 5934 RVA: 0x0006CA38 File Offset: 0x0006AC38
		public override int RebelClanStartingTier
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x0006CA3B File Offset: 0x0006AC3B
		public override int CompanionToLordClanStartingTier
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001730 RID: 5936 RVA: 0x0006CA3E File Offset: 0x0006AC3E
		private int KingdomEligibleTier
		{
			get
			{
				return Campaign.Current.Models.KingdomCreationModel.MinimumClanTierToCreateKingdom;
			}
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x0006CA54 File Offset: 0x0006AC54
		public override int CalculateInitialRenown(Clan clan)
		{
			int num = DefaultClanTierModel.TierLowerRenownLimits[clan.Tier];
			int num2 = ((clan.Tier >= this.MaxClanTier) ? (DefaultClanTierModel.TierLowerRenownLimits[this.MaxClanTier] + 1500) : DefaultClanTierModel.TierLowerRenownLimits[clan.Tier + 1]);
			int num3 = (int)((float)num2 - (float)(num2 - num) * 0.4f);
			return MBRandom.RandomInt(num, num3);
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x0006CAB5 File Offset: 0x0006ACB5
		public override int CalculateInitialInfluence(Clan clan)
		{
			return (int)(150f + (float)MBRandom.RandomInt((int)((float)this.CalculateInitialRenown(clan) / 15f)) + (float)MBRandom.RandomInt(MBRandom.RandomInt(MBRandom.RandomInt(400))));
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x0006CAEC File Offset: 0x0006ACEC
		public override int CalculateTier(Clan clan)
		{
			int num = this.MinClanTier;
			for (int i = this.MinClanTier + 1; i <= this.MaxClanTier; i++)
			{
				if (clan.Renown >= (float)DefaultClanTierModel.TierLowerRenownLimits[i])
				{
					num = i;
				}
			}
			return num;
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x0006CB2C File Offset: 0x0006AD2C
		public override ValueTuple<ExplainedNumber, bool> HasUpcomingTier(Clan clan, out TextObject extraExplanation, bool includeDescriptions = false)
		{
			bool flag = clan.Tier < this.MaxClanTier;
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			extraExplanation = null;
			if (flag)
			{
				int num = this.GetPartyLimitForTier(clan, clan.Tier + 1) - this.GetPartyLimitForTier(clan, clan.Tier);
				if (num != 0)
				{
					explainedNumber.Add((float)num, this._partyLimitBonusText, null);
				}
				int num2 = this.GetCompanionLimitFromTier(clan.Tier + 1) - this.GetCompanionLimitFromTier(clan.Tier);
				if (num2 != 0)
				{
					explainedNumber.Add((float)num2, this._companionLimitBonusText, null);
				}
				int nextClanTierPartySizeEffectChangeForHero = Campaign.Current.Models.PartySizeLimitModel.GetNextClanTierPartySizeEffectChangeForHero(clan.Leader);
				if (nextClanTierPartySizeEffectChangeForHero > 0)
				{
					explainedNumber.Add((float)nextClanTierPartySizeEffectChangeForHero, this._additionalCurrentPartySizeBonus, null);
				}
				int num3 = Campaign.Current.Models.WorkshopModel.GetMaxWorkshopCountForClanTier(clan.Tier + 1) - Campaign.Current.Models.WorkshopModel.GetMaxWorkshopCountForClanTier(clan.Tier);
				if (num3 > 0)
				{
					explainedNumber.Add((float)num3, this._additionalWorkshopCountBonus, null);
				}
				if (clan.Tier + 1 == this.MercenaryEligibleTier)
				{
					extraExplanation = this._mercenaryEligibleText;
				}
				else if (clan.Tier + 1 == this.VassalEligibleTier)
				{
					extraExplanation = this._vassalEligibleText;
				}
				else if (clan.Tier + 1 == this.KingdomEligibleTier)
				{
					extraExplanation = this._kingdomEligibleText;
				}
			}
			return new ValueTuple<ExplainedNumber, bool>(explainedNumber, flag);
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x0006CC93 File Offset: 0x0006AE93
		public override int GetRequiredRenownForTier(int tier)
		{
			return DefaultClanTierModel.TierLowerRenownLimits[tier];
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x0006CC9C File Offset: 0x0006AE9C
		public override int GetPartyLimitForTier(Clan clan, int clanTierToCheck)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			if (!clan.IsMinorFaction)
			{
				if (clanTierToCheck < 3)
				{
					explainedNumber.Add(1f, null, null);
				}
				else if (clanTierToCheck < 5)
				{
					explainedNumber.Add(2f, null, null);
				}
				else
				{
					explainedNumber.Add(3f, null, null);
				}
			}
			else
			{
				explainedNumber.Add(MathF.Clamp((float)clanTierToCheck, 1f, 4f), null, null);
			}
			this.AddPartyLimitPerkEffects(clan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x0006CD26 File Offset: 0x0006AF26
		private void AddPartyLimitPerkEffects(Clan clan, ref ExplainedNumber result)
		{
			if (clan.Leader != null && clan.Leader.GetPerkValue(DefaultPerks.Leadership.TalentMagnet))
			{
				result.Add(DefaultPerks.Leadership.TalentMagnet.SecondaryBonus, DefaultPerks.Leadership.TalentMagnet.Name, null);
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x0006CD60 File Offset: 0x0006AF60
		public override int GetCompanionLimit(Clan clan)
		{
			int num = this.GetCompanionLimitFromTier(clan.Tier);
			if (clan.Leader.GetPerkValue(DefaultPerks.Leadership.WePledgeOurSwords))
			{
				num += (int)DefaultPerks.Leadership.WePledgeOurSwords.PrimaryBonus;
			}
			if (clan.Leader.GetPerkValue(DefaultPerks.Charm.Camaraderie))
			{
				num += (int)DefaultPerks.Charm.Camaraderie.SecondaryBonus;
			}
			return num;
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x0006CDBB File Offset: 0x0006AFBB
		private int GetCompanionLimitFromTier(int clanTier)
		{
			return clanTier + 3;
		}

		// Token: 0x040007D1 RID: 2001
		private static readonly int[] TierLowerRenownLimits = new int[] { 0, 50, 150, 350, 900, 2350, 6150 };

		// Token: 0x040007D2 RID: 2002
		private readonly TextObject _partyLimitBonusText = GameTexts.FindText("str_clan_tier_party_limit_bonus", null);

		// Token: 0x040007D3 RID: 2003
		private readonly TextObject _companionLimitBonusText = GameTexts.FindText("str_clan_tier_companion_limit_bonus", null);

		// Token: 0x040007D4 RID: 2004
		private readonly TextObject _mercenaryEligibleText = GameTexts.FindText("str_clan_tier_mercenary_eligible", null);

		// Token: 0x040007D5 RID: 2005
		private readonly TextObject _vassalEligibleText = GameTexts.FindText("str_clan_tier_vassal_eligible", null);

		// Token: 0x040007D6 RID: 2006
		private readonly TextObject _additionalCurrentPartySizeBonus = GameTexts.FindText("str_clan_tier_party_size_bonus", null);

		// Token: 0x040007D7 RID: 2007
		private readonly TextObject _additionalWorkshopCountBonus = GameTexts.FindText("str_clan_tier_workshop_count_bonus", null);

		// Token: 0x040007D8 RID: 2008
		private readonly TextObject _kingdomEligibleText = GameTexts.FindText("str_clan_tier_kingdom_eligible", null);
	}
}
