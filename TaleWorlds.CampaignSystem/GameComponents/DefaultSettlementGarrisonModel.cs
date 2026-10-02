using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200014C RID: 332
	public class DefaultSettlementGarrisonModel : SettlementGarrisonModel
	{
		// Token: 0x060019EF RID: 6639 RVA: 0x0008319E File Offset: 0x0008139E
		public override int GetMaximumDailyAutoRecruitmentCount(Town town)
		{
			return 1;
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x000831A4 File Offset: 0x000813A4
		public override ExplainedNumber CalculateBaseGarrisonChange(Settlement settlement, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			if ((settlement.IsTown || settlement.IsCastle) && settlement.OwnerClan.IsRebelClan && (settlement.OwnerClan.MapFaction == null || !settlement.OwnerClan.MapFaction.IsKingdomFaction))
			{
				explainedNumber.Add(2f, this.RebellionText, null);
			}
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementGarrison, settlement, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x0008322C File Offset: 0x0008142C
		public override int FindNumberOfTroopsToTakeFromGarrison(MobileParty mobileParty, Settlement settlement, float defaultIdealGarrisonStrengthPerWalledCenter = 0f)
		{
			MobileParty garrisonParty = settlement.Town.GarrisonParty;
			if (garrisonParty != null)
			{
				float num = garrisonParty.Party.CalculateCurrentStrength();
				float num2;
				if (garrisonParty.HasLimitedWage())
				{
					num2 = (float)garrisonParty.PaymentLimit / Campaign.Current.AverageWage;
					num2 /= 1.5f;
				}
				else
				{
					num2 = ((defaultIdealGarrisonStrengthPerWalledCenter > 0.1f) ? defaultIdealGarrisonStrengthPerWalledCenter : FactionHelper.FindIdealGarrisonStrengthPerWalledCenter(mobileParty.MapFaction as Kingdom, settlement.OwnerClan));
					float num3 = FactionHelper.OwnerClanEconomyEffectOnGarrisonSizeConstant(settlement.OwnerClan);
					num2 *= num3;
					num2 *= (settlement.IsTown ? 2f : 1f);
				}
				float partySizeLimit = (float)mobileParty.Party.PartySizeLimit;
				int numberOfAllMembers = mobileParty.Party.NumberOfAllMembers;
				float num4 = partySizeLimit / (float)numberOfAllMembers;
				float num5 = MathF.Min(11f, num4 * MathF.Sqrt(num4)) - 1f;
				float num6 = MathF.Pow(num / num2, 1.5f);
				float num7 = ((mobileParty.LeaderHero.Clan.Leader == mobileParty.LeaderHero) ? 2f : 1f);
				int num8 = 0;
				if (num5 * num6 * num7 > 1f)
				{
					num8 = MBRandom.RoundRandomized(num5 * num6 * num7);
				}
				int num9 = 25;
				num9 *= (settlement.IsTown ? 2 : 1);
				if (num8 > garrisonParty.Party.MemberRoster.TotalRegulars - num9)
				{
					num8 = garrisonParty.Party.MemberRoster.TotalRegulars - num9;
				}
				return num8;
			}
			return 0;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x000833A8 File Offset: 0x000815A8
		public override int FindNumberOfTroopsToLeaveToGarrison(MobileParty mobileParty, Settlement settlement)
		{
			MobileParty garrisonParty = settlement.Town.GarrisonParty;
			float num = 0f;
			if (garrisonParty != null)
			{
				num = garrisonParty.Party.CalculateCurrentStrength();
			}
			float num2;
			if (garrisonParty != null && garrisonParty.HasLimitedWage())
			{
				num2 = (float)garrisonParty.PaymentLimit / Campaign.Current.AverageWage;
			}
			else
			{
				num2 = FactionHelper.FindIdealGarrisonStrengthPerWalledCenter(mobileParty.MapFaction as Kingdom, settlement.OwnerClan);
				float num3 = FactionHelper.OwnerClanEconomyEffectOnGarrisonSizeConstant(settlement.OwnerClan);
				float num4 = FactionHelper.SettlementProsperityEffectOnGarrisonSizeConstant(settlement.Town);
				float num5 = FactionHelper.SettlementFoodPotentialEffectOnGarrisonSizeConstant(settlement);
				num2 *= num3;
				num2 *= num4;
				num2 *= num5;
			}
			if (num < num2)
			{
				int numberOfRegularMembers = mobileParty.Party.NumberOfRegularMembers;
				float num6 = 1f + (float)mobileParty.Party.MemberRoster.TotalWoundedRegulars / (float)mobileParty.Party.NumberOfRegularMembers;
				int partySizeLimit = mobileParty.Party.PartySizeLimit;
				float num7 = MathF.Pow(MathF.Min(2f, (float)numberOfRegularMembers / (float)partySizeLimit), 1.2f) * 0.75f;
				float num8 = (1f - num / num2) * (1f - num / num2);
				float num9 = 1f;
				if (mobileParty.Army != null)
				{
					num8 = MathF.Min(num8, 0.7f);
					num9 = 0.3f + mobileParty.Army.CalculateCurrentStrength() / mobileParty.Party.CalculateCurrentStrength() * 0.025f;
				}
				float num10 = (settlement.Town.IsOwnerUnassigned ? 0.75f : 0.5f);
				if (settlement.OwnerClan == mobileParty.LeaderHero.Clan || settlement.OwnerClan == mobileParty.Party.Owner.MapFaction.Leader.Clan)
				{
					num10 = 1f;
				}
				float num11 = MathF.Min(0.7f, num7 * num8 * num10 * num6 * num9);
				if ((float)numberOfRegularMembers * num11 > 1f)
				{
					return MBRandom.RoundRandomized((float)numberOfRegularMembers * num11);
				}
			}
			return 0;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00083590 File Offset: 0x00081790
		public override float GetMaximumDailyRepairAmount(Settlement settlement)
		{
			if (!settlement.IsUnderSiege)
			{
				if (!settlement.SettlementWallSectionHitPointsRatioList.All<float>((float ratio) => ratio >= 1f))
				{
					ExplainedNumber explainedNumber = new ExplainedNumber(settlement.MaxHitPointsOfOneWallSection * (float)settlement.WallSectionCount * 0.04f, false, null);
					if (settlement.IsFortification)
					{
						settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.WallRepairSpeed, ref explainedNumber);
					}
					return explainedNumber.ResultNumber;
				}
			}
			return 0f;
		}

		// Token: 0x0400089A RID: 2202
		private static readonly TextObject TownWallsText = new TextObject("{=SlmhqqH8}Town Walls", null);

		// Token: 0x0400089B RID: 2203
		private static readonly TextObject MoraleText = new TextObject("{=UjL7jVYF}Morale", null);

		// Token: 0x0400089C RID: 2204
		private static readonly TextObject FoodShortageText = new TextObject("{=qTFKvGSg}Food Shortage", null);

		// Token: 0x0400089D RID: 2205
		private readonly TextObject SurplusFoodText = GameTexts.FindText("str_surplus_food", null);

		// Token: 0x0400089E RID: 2206
		private readonly TextObject VillageBeingRaided = GameTexts.FindText("str_village_being_raided", null);

		// Token: 0x0400089F RID: 2207
		private readonly TextObject VillageLooted = GameTexts.FindText("str_village_looted", null);

		// Token: 0x040008A0 RID: 2208
		private readonly TextObject TownIsUnderSiege = GameTexts.FindText("str_villages_under_siege", null);

		// Token: 0x040008A1 RID: 2209
		private readonly TextObject RetiredText = GameTexts.FindText("str_retired", null);

		// Token: 0x040008A2 RID: 2210
		private readonly TextObject PaymentIsLessText = GameTexts.FindText("str_payment_is_less", null);

		// Token: 0x040008A3 RID: 2211
		private readonly TextObject UnpaidWagesText = GameTexts.FindText("str_unpaid_wages", null);

		// Token: 0x040008A4 RID: 2212
		private readonly TextObject RebellionText = GameTexts.FindText("str_rebel_settlement", null);

		// Token: 0x040008A5 RID: 2213
		private const int MaximumDailyAutoRecruitmentCount = 1;
	}
}
