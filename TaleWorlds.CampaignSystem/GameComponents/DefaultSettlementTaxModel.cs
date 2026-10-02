using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000152 RID: 338
	public class DefaultSettlementTaxModel : SettlementTaxModel
	{
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001A58 RID: 6744 RVA: 0x000856C0 File Offset: 0x000838C0
		public override float SettlementCommissionRateTown
		{
			get
			{
				return 0.7f;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001A59 RID: 6745 RVA: 0x000856C7 File Offset: 0x000838C7
		public override float SettlementCommissionRateVillage
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x000856CE File Offset: 0x000838CE
		public override int SettlementCommissionDecreaseSecurityThreshold
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x000856D2 File Offset: 0x000838D2
		public override int MaximumDecreaseBasedOnSecuritySecurity
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x000856D8 File Offset: 0x000838D8
		public override float GetTownTaxRatio(Town town)
		{
			float num = 1f;
			if (town.Settlement.OwnerClan.Kingdom != null && town.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.CrownDuty))
			{
				num += 0.05f;
			}
			return this.SettlementCommissionRateTown * num;
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x00085730 File Offset: 0x00083930
		public override float GetVillageTaxRatio(Village village)
		{
			float num = this.SettlementCommissionRateVillage;
			if (village.Settlement.OwnerClan.Kingdom != null && village.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.LandGrantsForVeteran))
			{
				num -= num * 0.05f;
			}
			return num;
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x00085784 File Offset: 0x00083984
		public override float GetTownCommissionChangeBasedOnSecurity(Town town, float commission)
		{
			if (town.Security < (float)this.SettlementCommissionDecreaseSecurityThreshold)
			{
				float num = MBMath.Map((float)this.SettlementCommissionDecreaseSecurityThreshold - town.Security, 0f, (float)this.SettlementCommissionDecreaseSecurityThreshold, (float)this.MaximumDecreaseBasedOnSecuritySecurity, 0f);
				commission -= commission * (num * 0.01f);
				return commission;
			}
			return commission;
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x000857DC File Offset: 0x000839DC
		public override ExplainedNumber CalculateTownTax(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateDailyTaxInternal(town, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x00085804 File Offset: 0x00083A04
		private float CalculateDailyTax(Town town, ref ExplainedNumber explainedNumber)
		{
			float prosperity = town.Prosperity;
			float num = 1f;
			if (town.Settlement.OwnerClan.Kingdom != null && town.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.CouncilOfTheCommons))
			{
				num -= 0.05f;
			}
			float num2 = 0.35f;
			float num3 = prosperity * num2 * num;
			explainedNumber.Add(num3, this.ProsperityText, null);
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00085878 File Offset: 0x00083A78
		private void CalculateDailyTaxInternal(Town town, ref ExplainedNumber result)
		{
			float num = this.CalculateDailyTax(town, ref result);
			this.CalculatePolicyGoldCut(town, num, ref result);
			if (PerkHelper.GetPerkValueForTown(DefaultPerks.Bow.QuickDraw, town))
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.QuickDraw, town, ref result);
			}
			if (town.Governor != null)
			{
				if (town.Governor.GetPerkValue(DefaultPerks.Steward.Logistician))
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.Logistician, town, ref result);
				}
				if (town.Governor.GetPerkValue(DefaultPerks.Steward.PriceOfLoyalty))
				{
					int num2 = town.Governor.GetSkillValue(DefaultSkills.Steward) - Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus;
					result.AddFactor(DefaultPerks.Steward.PriceOfLoyalty.SecondaryBonus * (float)num2, DefaultPerks.Steward.PriceOfLoyalty.Name);
				}
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Scouting.DesertBorn, town))
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Scouting.DesertBorn, town, ref result);
				}
			}
			if (town.IsTown && town.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.KhuzaitDecreasedTaxFeat))
			{
				result.AddFactor(DefaultCulturalFeats.KhuzaitDecreasedTaxFeat.EffectBonus, GameTexts.FindText("str_culture", null));
			}
			this.GetSettlementTaxChangeDueToIssues(town, ref result);
			this.CalculateSettlementTaxDueToSecurity(town, ref result);
			this.CalculateSettlementTaxDueToLoyalty(town, ref result);
			this.CalculateSettlementTaxDueToBuildings(town, ref result);
			result.Clamp(0f, float.MaxValue);
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x000859B4 File Offset: 0x00083BB4
		private void CalculateSettlementTaxDueToSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			SettlementSecurityModel settlementSecurityModel = Campaign.Current.Models.SettlementSecurityModel;
			if (town.Security >= (float)settlementSecurityModel.ThresholdForTaxBoost)
			{
				settlementSecurityModel.CalculateGoldGainDueToHighSecurity(town, ref explainedNumber);
				return;
			}
			if (town.Security >= (float)settlementSecurityModel.ThresholdForHigherTaxCorruption && town.Security < (float)settlementSecurityModel.ThresholdForTaxCorruption)
			{
				settlementSecurityModel.CalculateGoldCutDueToLowSecurity(town, ref explainedNumber);
			}
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00085A10 File Offset: 0x00083C10
		private void CalculateSettlementTaxDueToLoyalty(Town town, ref ExplainedNumber explainedNumber)
		{
			SettlementLoyaltyModel settlementLoyaltyModel = Campaign.Current.Models.SettlementLoyaltyModel;
			if (town.Loyalty >= (float)settlementLoyaltyModel.ThresholdForTaxBoost)
			{
				settlementLoyaltyModel.CalculateGoldGainDueToHighLoyalty(town, ref explainedNumber);
				return;
			}
			if (town.Loyalty >= (float)settlementLoyaltyModel.ThresholdForHigherTaxCorruption && town.Loyalty <= (float)settlementLoyaltyModel.ThresholdForTaxCorruption)
			{
				settlementLoyaltyModel.CalculateGoldCutDueToLowLoyalty(town, ref explainedNumber);
				return;
			}
			if (town.Loyalty < (float)settlementLoyaltyModel.ThresholdForHigherTaxCorruption)
			{
				explainedNumber.AddFactor(-1f, DefaultSettlementTaxModel.VeryLowLoyalty);
			}
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00085A8B File Offset: 0x00083C8B
		private void CalculateSettlementTaxDueToBuildings(Town town, ref ExplainedNumber result)
		{
			town.AddEffectOfBuildings(BuildingEffectEnum.TaxPerDay, ref result);
			town.AddEffectOfBuildings(BuildingEffectEnum.DenarByBoundVillageHeartPerDay, ref result);
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x00085AA0 File Offset: 0x00083CA0
		private void CalculatePolicyGoldCut(Town town, float rawTax, ref ExplainedNumber explainedNumber)
		{
			if (town.MapFaction.IsKingdomFaction)
			{
				Kingdom kingdom = (Kingdom)town.MapFaction;
				if (town.IsTown)
				{
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.Magistrates))
					{
						explainedNumber.Add(-0.05f * rawTax, DefaultPolicies.Magistrates.Name, null);
					}
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.Bailiffs))
					{
						explainedNumber.Add(-0.05f * rawTax, DefaultPolicies.Bailiffs.Name, null);
					}
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.TribunesOfThePeople))
					{
						explainedNumber.Add(-0.05f * rawTax, DefaultPolicies.TribunesOfThePeople.Name, null);
					}
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.Cantons))
				{
					explainedNumber.Add(-0.1f * rawTax, DefaultPolicies.Cantons.Name, null);
				}
			}
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x00085B79 File Offset: 0x00083D79
		private void GetSettlementTaxChangeDueToIssues(Town center, ref ExplainedNumber result)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementTax, center.Owner.Settlement, ref result);
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x00085BA0 File Offset: 0x00083DA0
		public override int CalculateVillageTaxFromIncome(Village village, int marketIncome)
		{
			if (marketIncome == 0)
			{
				return 0;
			}
			return (int)((float)marketIncome * Campaign.Current.Models.SettlementTaxModel.GetVillageTaxRatio(village));
		}

		// Token: 0x040008D5 RID: 2261
		private readonly TextObject ProsperityText = GameTexts.FindText("str_prosperity", null);

		// Token: 0x040008D6 RID: 2262
		private static readonly TextObject VeryLowLoyalty = new TextObject("{=CcQzFnpN}Very Low Loyalty", null);
	}
}
