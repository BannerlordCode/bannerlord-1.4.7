using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F9 RID: 249
	public class DefaultBuildingConstructionModel : BuildingConstructionModel
	{
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001695 RID: 5781 RVA: 0x0006881F File Offset: 0x00066A1F
		public override int TownBoostCost
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001696 RID: 5782 RVA: 0x00068826 File Offset: 0x00066A26
		public override int TownBoostBonus
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001697 RID: 5783 RVA: 0x0006882A File Offset: 0x00066A2A
		public override int CastleBoostCost
		{
			get
			{
				return 250;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001698 RID: 5784 RVA: 0x00068831 File Offset: 0x00066A31
		public override int CastleBoostBonus
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x00068838 File Offset: 0x00066A38
		public override ExplainedNumber CalculateDailyConstructionPower(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateDailyConstructionPowerInternal(town, ref explainedNumber, false);
			return explainedNumber;
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00068860 File Offset: 0x00066A60
		public override int CalculateDailyConstructionPowerWithoutBoost(Town town)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			return this.CalculateDailyConstructionPowerInternal(town, ref explainedNumber, true);
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x00068888 File Offset: 0x00066A88
		public override int GetBoostAmount(Town town)
		{
			object obj = (town.IsCastle ? this.CastleBoostBonus : this.TownBoostBonus);
			float num = 0f;
			if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Steward.Relocation))
			{
				num += DefaultPerks.Steward.Relocation.SecondaryBonus;
			}
			if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Trade.SpringOfGold))
			{
				num += DefaultPerks.Trade.SpringOfGold.SecondaryBonus;
			}
			object obj2 = obj;
			return obj2 + (int)(obj2 * num);
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00068905 File Offset: 0x00066B05
		public override int GetBoostCost(Town town)
		{
			if (!town.IsCastle)
			{
				return this.TownBoostCost;
			}
			return this.CastleBoostCost;
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x0006891C File Offset: 0x00066B1C
		private int CalculateDailyConstructionPowerInternal(Town town, ref ExplainedNumber result, bool omitBoost = false)
		{
			float num = town.Prosperity * 0.01f;
			result.Add(num, GameTexts.FindText("str_prosperity", null), null);
			if (!omitBoost && town.BoostBuildingProcess > 0)
			{
				int num2 = (town.IsCastle ? this.CastleBoostCost : this.TownBoostCost);
				int num3 = this.GetBoostAmount(town);
				float num4 = MathF.Min(1f, (float)town.BoostBuildingProcess / (float)num2);
				float num5 = 0f;
				if (town.IsTown && town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.Clockwork))
				{
					num5 += DefaultPerks.Engineering.Clockwork.SecondaryBonus;
				}
				num3 += MathF.Round((float)num3 * num5);
				result.Add((float)num3 * num4, DefaultBuildingConstructionModel.BoostText, null);
			}
			if (town.Governor != null)
			{
				Settlement currentSettlement = town.Governor.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) == town)
				{
					SkillHelper.AddSkillBonusForTown(DefaultSkillEffects.TownProjectBuildingBonus, town, ref result);
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.ForcedLabor, town, ref result);
				}
			}
			if (town.Governor != null)
			{
				Settlement currentSettlement2 = town.Governor.CurrentSettlement;
				if (((currentSettlement2 != null) ? currentSettlement2.Town : null) == town && !town.BuildingsInProgress.IsEmpty<Building>())
				{
					if (town.Governor.GetPerkValue(DefaultPerks.Steward.ForcedLabor) && town.Settlement.Party.PrisonRoster.TotalManCount > 0)
					{
						float num6 = MathF.Min(0.3f, (float)town.Settlement.Party.PrisonRoster.TotalManCount / 3f * DefaultPerks.Steward.ForcedLabor.SecondaryBonus);
						result.AddFactor(num6, DefaultPerks.Steward.ForcedLabor.Name);
					}
					if (town.IsCastle && town.Governor.GetPerkValue(DefaultPerks.Engineering.MilitaryPlanner))
					{
						result.AddFactor(DefaultPerks.Engineering.MilitaryPlanner.SecondaryBonus, DefaultPerks.Engineering.MilitaryPlanner.Name);
					}
					else if (town.IsTown && town.Governor.GetPerkValue(DefaultPerks.Engineering.Carpenters))
					{
						result.AddFactor(DefaultPerks.Engineering.Carpenters.SecondaryBonus, DefaultPerks.Engineering.Carpenters.Name);
					}
					Building building = town.BuildingsInProgress.Peek();
					if ((building.BuildingType == DefaultBuildingTypes.SettlementFortifications || building.BuildingType == DefaultBuildingTypes.CastleBarracks || building.BuildingType == DefaultBuildingTypes.SettlementBarracks) && town.Governor.GetPerkValue(DefaultPerks.Engineering.Stonecutters))
					{
						result.AddFactor(DefaultPerks.Engineering.Stonecutters.PrimaryBonus, DefaultPerks.Engineering.Stonecutters.Name);
					}
				}
			}
			int num7 = town.SoldItems.Sum<Town.SellLog>(delegate(Town.SellLog x)
			{
				if (x.Category.Properties != ItemCategory.Property.BonusToProduction)
				{
					return 0;
				}
				return x.Number;
			});
			if (num7 > 0)
			{
				result.Add(0.25f * (float)num7, DefaultBuildingConstructionModel.ProductionFromMarketText, null);
			}
			BuildingType buildingType = (town.BuildingsInProgress.IsEmpty<Building>() ? null : town.BuildingsInProgress.Peek().BuildingType);
			if (buildingType != null && buildingType.IsMilitaryProject)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.Confidence, town, ref result);
			}
			if (buildingType == DefaultBuildingTypes.SettlementMarketplace)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.SelfMadeMan, town, ref result);
			}
			town.AddEffectOfBuildings(BuildingEffectEnum.ConstructionPerDay, ref result);
			if (town.Loyalty >= 75f)
			{
				float num8 = MBMath.Map(town.Loyalty, 75f, 100f, 0f, 0.2f);
				result.AddFactor(num8, DefaultBuildingConstructionModel.HighLoyaltyBonusText);
			}
			else if (town.Loyalty > 25f && town.Loyalty <= 50f)
			{
				float num9 = MBMath.Map(town.Loyalty, 25f, 50f, 0.5f, 0f);
				result.AddFactor(-num9, DefaultBuildingConstructionModel.LowLoyaltyPenaltyText);
			}
			else if (town.Loyalty <= 25f)
			{
				result.LimitMax(0f, DefaultBuildingConstructionModel.VeryLowLoyaltyPenaltyText);
			}
			if (town.Loyalty > 25f && town.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.BattanianConstructionFeat))
			{
				result.AddFactor(DefaultCulturalFeats.BattanianConstructionFeat.EffectBonus, this.CultureText);
			}
			result.LimitMin(0f);
			return (int)result.ResultNumber;
		}

		// Token: 0x0400078B RID: 1931
		private const float HammerMultiplier = 0.01f;

		// Token: 0x0400078C RID: 1932
		private const int VeryLowLoyaltyValue = 25;

		// Token: 0x0400078D RID: 1933
		private const float MediumLoyaltyValue = 50f;

		// Token: 0x0400078E RID: 1934
		private const float HighLoyaltyValue = 75f;

		// Token: 0x0400078F RID: 1935
		private const float HighestLoyaltyValue = 100f;

		// Token: 0x04000790 RID: 1936
		private static readonly TextObject ProductionFromMarketText = new TextObject("{=vaZDJGMx}Construction from Market", null);

		// Token: 0x04000791 RID: 1937
		private static readonly TextObject BoostText = new TextObject("{=yX1RycON}Boost from Reserve", null);

		// Token: 0x04000792 RID: 1938
		private static readonly TextObject HighLoyaltyBonusText = new TextObject("{=aSniKUJv}High Loyalty", null);

		// Token: 0x04000793 RID: 1939
		private static readonly TextObject LowLoyaltyPenaltyText = new TextObject("{=SJ2qsRdF}Low Loyalty", null);

		// Token: 0x04000794 RID: 1940
		private static readonly TextObject VeryLowLoyaltyPenaltyText = new TextObject("{=CcQzFnpN}Very Low Loyalty", null);

		// Token: 0x04000795 RID: 1941
		private readonly TextObject CultureText = GameTexts.FindText("str_culture", null);
	}
}
