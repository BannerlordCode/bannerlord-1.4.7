using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000167 RID: 359
	public class DefaultVolunteerModel : VolunteerModel
	{
		// Token: 0x06001B19 RID: 6937 RVA: 0x0008CC8C File Offset: 0x0008AE8C
		public override int MaximumIndexHeroCanRecruitFromHero(Hero buyerHero, Hero sellerHero, int useValueAsRelation = -101)
		{
			int num = this.MaximumIndexCanPartyRecruitFromHeroInternal(buyerHero, sellerHero);
			int num2 = ((useValueAsRelation < -100) ? buyerHero.GetRelation(sellerHero) : useValueAsRelation);
			int num3 = ((num2 >= 100) ? 7 : ((num2 >= 80) ? 6 : ((num2 >= 60) ? 5 : ((num2 >= 40) ? 4 : ((num2 >= 20) ? 3 : ((num2 >= 10) ? 2 : ((num2 >= 5) ? 1 : ((num2 >= 0) ? 0 : (-1)))))))));
			int num4 = ((sellerHero.CurrentSettlement != null && buyerHero.MapFaction == sellerHero.CurrentSettlement.MapFaction) ? 1 : 0);
			int num5 = ((buyerHero != Hero.MainHero) ? 1 : 0);
			int num6 = ((sellerHero.CurrentSettlement != null && buyerHero.MapFaction.IsAtWarWith(sellerHero.CurrentSettlement.MapFaction)) ? (-(1 + num5)) : 0);
			if (buyerHero.IsMinorFactionHero && sellerHero.CurrentSettlement != null && sellerHero.CurrentSettlement.IsVillage)
			{
				num6 = 0;
			}
			int num7 = 0;
			if (sellerHero.IsMerchant && buyerHero.GetPerkValue(DefaultPerks.Trade.ArtisanCommunity))
			{
				num7 += (int)DefaultPerks.Trade.ArtisanCommunity.SecondaryBonus;
			}
			if (sellerHero.Culture == buyerHero.Culture && buyerHero.GetPerkValue(DefaultPerks.Leadership.CombatTips))
			{
				num7 += (int)DefaultPerks.Leadership.CombatTips.SecondaryBonus;
			}
			if (sellerHero.IsRuralNotable && buyerHero.GetPerkValue(DefaultPerks.Charm.Firebrand))
			{
				num7 += (int)DefaultPerks.Charm.Firebrand.SecondaryBonus;
			}
			if (sellerHero.IsUrbanNotable && buyerHero.GetPerkValue(DefaultPerks.Charm.FlexibleEthics))
			{
				num7 += (int)DefaultPerks.Charm.FlexibleEthics.SecondaryBonus;
			}
			if (sellerHero.IsArtisan && buyerHero.PartyBelongedTo != null && buyerHero.PartyBelongedTo.EffectiveEngineer != null && buyerHero.PartyBelongedTo.EffectiveEngineer.GetPerkValue(DefaultPerks.Engineering.EngineeringGuilds))
			{
				num7 += (int)DefaultPerks.Engineering.EngineeringGuilds.PrimaryBonus;
			}
			return MathF.Min(6, num + num3 + num4 + num5 + num6 + num7);
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x0008CE5F File Offset: 0x0008B05F
		public override int MaximumIndexGarrisonCanRecruitFromHero(Settlement settlement, Hero sellerHero)
		{
			return this.MaximumIndexCanPartyRecruitFromHeroInternal(settlement.Owner, sellerHero);
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0008CE70 File Offset: 0x0008B070
		private int MaximumIndexCanPartyRecruitFromHeroInternal(Hero buyerHero, Hero sellerHero)
		{
			Settlement currentSettlement = sellerHero.CurrentSettlement;
			int num = 1;
			int num2 = ((buyerHero == Hero.MainHero) ? Campaign.Current.Models.DifficultyModel.GetPlayerRecruitSlotBonus() : 0);
			int num3 = 0;
			if (sellerHero.IsGangLeader && currentSettlement != null && currentSettlement.OwnerClan == buyerHero.Clan)
			{
				if (currentSettlement.IsTown)
				{
					Hero governor = currentSettlement.Town.Governor;
					if (governor != null && governor.GetPerkValue(DefaultPerks.Roguery.OneOfTheFamily))
					{
						goto IL_009A;
					}
				}
				if (!currentSettlement.IsVillage)
				{
					goto IL_00A8;
				}
				Hero governor2 = currentSettlement.Village.Bound.Town.Governor;
				if (governor2 == null || !governor2.GetPerkValue(DefaultPerks.Roguery.OneOfTheFamily))
				{
					goto IL_00A8;
				}
				IL_009A:
				num3 += (int)DefaultPerks.Roguery.OneOfTheFamily.SecondaryBonus;
			}
			IL_00A8:
			return MathF.Min(6, MathF.Max(0, num + num2 + num3));
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0008CF38 File Offset: 0x0008B138
		public override float GetDailyVolunteerProductionProbability(Hero hero, int index, Settlement settlement)
		{
			float num = 0.7f;
			int num2 = 0;
			foreach (Town town in hero.CurrentSettlement.MapFaction.Fiefs)
			{
				num2 += (town.IsTown ? (((town.Prosperity < 3000f) ? 1 : ((town.Prosperity < 6000f) ? 2 : 3)) + town.Villages.Count) : town.Villages.Count);
			}
			float num3 = ((num2 < 46) ? ((float)num2 / 46f * ((float)num2 / 46f)) : 1f);
			num += ((hero.CurrentSettlement != null && num3 < 1f) ? ((1f - num3) * 0.2f) : 0f);
			float num4 = 0.75f * MathF.Clamp(MathF.Pow(num, (float)(index + 1)), 0f, 1f);
			ExplainedNumber explainedNumber = new ExplainedNumber(num4, false, null);
			Clan clan = hero.Clan;
			if (((clan != null) ? clan.Kingdom : null) != null && hero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Cantons))
			{
				explainedNumber.AddFactor(0.2f, null);
			}
			Town town2;
			if (!settlement.IsTown)
			{
				Settlement tradeBound = settlement.Village.TradeBound;
				town2 = ((tradeBound != null) ? tradeBound.Town : null);
			}
			else
			{
				town2 = settlement.Town;
			}
			Town town3 = town2;
			if (town3 != null && hero.IsAlive && hero.VolunteerTypes[index] != null && hero.VolunteerTypes[index].IsMounted && PerkHelper.GetPerkValueForTown(DefaultPerks.Riding.CavalryTactics, town3))
			{
				explainedNumber.AddFactor(DefaultPerks.Riding.CavalryTactics.PrimaryBonus, null);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0008D104 File Offset: 0x0008B304
		public override CharacterObject GetBasicVolunteer(Hero sellerHero)
		{
			if (sellerHero.IsRuralNotable && sellerHero.CurrentSettlement.Village.Bound.IsCastle)
			{
				return sellerHero.Culture.EliteBasicTroop;
			}
			return sellerHero.Culture.BasicTroop;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0008D13C File Offset: 0x0008B33C
		public override bool CanHaveRecruits(Hero hero)
		{
			Occupation occupation = hero.Occupation;
			return occupation == Occupation.Mercenary || occupation - Occupation.Artisan <= 5;
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x0008D15E File Offset: 0x0008B35E
		public override int MaxVolunteerTier
		{
			get
			{
				return 4;
			}
		}
	}
}
