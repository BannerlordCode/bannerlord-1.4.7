using System;
using System.Linq;
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
	// Token: 0x0200014E RID: 334
	public class DefaultSettlementMilitiaModel : SettlementMilitiaModel
	{
		// Token: 0x06001A1D RID: 6685 RVA: 0x00083EC9 File Offset: 0x000820C9
		public override int MilitiaToSpawnAfterSiege(Town town)
		{
			return 2 * (45 + MBRandom.RandomInt(10));
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00083ED7 File Offset: 0x000820D7
		public override ExplainedNumber CalculateMilitiaChange(Settlement settlement, bool includeDescriptions = false)
		{
			return DefaultSettlementMilitiaModel.CalculateMilitiaChangeInternal(settlement, includeDescriptions);
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x00083EE0 File Offset: 0x000820E0
		public override ExplainedNumber CalculateVeteranMilitiaSpawnChance(Settlement settlement)
		{
			ExplainedNumber explainedNumber = default(ExplainedNumber);
			Hero hero = null;
			if (settlement.IsFortification && settlement.Town.Governor != null)
			{
				hero = settlement.Town.Governor;
			}
			else if (settlement.IsVillage)
			{
				Settlement tradeBound = settlement.Village.TradeBound;
				if (((tradeBound != null) ? tradeBound.Town.Governor : null) != null)
				{
					hero = settlement.Village.TradeBound.Town.Governor;
				}
			}
			if (hero != null)
			{
				if (hero.GetPerkValue(DefaultPerks.Leadership.CitizenMilitia))
				{
					explainedNumber.Add(DefaultPerks.Leadership.CitizenMilitia.PrimaryBonus, null, null);
				}
				if (hero.GetPerkValue(DefaultPerks.Polearm.Drills))
				{
					explainedNumber.Add(DefaultPerks.Polearm.Drills.PrimaryBonus, null, null);
				}
				if (hero.GetPerkValue(DefaultPerks.Steward.SevenVeterans))
				{
					explainedNumber.Add(DefaultPerks.Steward.SevenVeterans.PrimaryBonus, null, null);
				}
			}
			if (settlement.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.BattanianMilitiaFeat))
			{
				explainedNumber.Add(DefaultCulturalFeats.BattanianMilitiaFeat.EffectBonus, null, null);
			}
			if (settlement.IsFortification)
			{
				settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.MilitiaVeterancyChance, ref explainedNumber);
			}
			if (settlement.OwnerClan.Kingdom != null && settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.LandGrantsForVeteran))
			{
				explainedNumber.AddFactor(0.1f, null);
			}
			return explainedNumber;
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x00084031 File Offset: 0x00082231
		public override void CalculateMilitiaSpawnRate(Settlement settlement, out float meleeTroopRate, out float rangedTroopRate)
		{
			meleeTroopRate = 0.5f;
			rangedTroopRate = 1f - meleeTroopRate;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x00084044 File Offset: 0x00082244
		private static ExplainedNumber CalculateMilitiaChangeInternal(Settlement settlement, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			if (settlement.IsVillage && settlement.Village.VillageState != Village.VillageStates.Normal)
			{
				return explainedNumber;
			}
			float militia = settlement.Militia;
			if (settlement.IsFortification)
			{
				explainedNumber.Add(2f, DefaultSettlementMilitiaModel.BaseText, null);
			}
			else if (settlement.IsVillage)
			{
				explainedNumber.Add(0.5f, DefaultSettlementMilitiaModel.BaseText, null);
			}
			float num = -militia * 0.025f;
			explainedNumber.Add(num, DefaultSettlementMilitiaModel.RetiredText, null);
			if (settlement.IsVillage)
			{
				float num2 = settlement.Village.Hearth / 400f;
				explainedNumber.Add(num2, DefaultSettlementMilitiaModel.FromHearthsText, null);
			}
			else if (settlement.IsFortification)
			{
				float num3 = settlement.Town.Prosperity / 1000f;
				explainedNumber.Add(num3, DefaultSettlementMilitiaModel.FromProsperityText, null);
				if (settlement.Town.InRebelliousState)
				{
					float num4 = MBMath.Map(settlement.Town.Loyalty, 0f, (float)Campaign.Current.Models.SettlementLoyaltyModel.RebelliousStateStartLoyaltyThreshold, (float)Campaign.Current.Models.SettlementLoyaltyModel.MilitiaBoostPercentage, 0f);
					float num5 = MathF.Abs(num3 * (num4 * 0.01f));
					explainedNumber.Add(num5, DefaultSettlementMilitiaModel.LowLoyaltyText, null);
				}
			}
			if (settlement.IsTown)
			{
				int num6 = settlement.Town.SoldItems.Sum<Town.SellLog>(delegate(Town.SellLog x)
				{
					if (x.Category.Properties != ItemCategory.Property.BonusToMilitia)
					{
						return 0;
					}
					return x.Number;
				});
				if (num6 > 0)
				{
					explainedNumber.Add(0.2f * (float)num6, DefaultSettlementMilitiaModel.MilitiaFromMarketText, null);
				}
				if (settlement.OwnerClan.Kingdom != null)
				{
					if (settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Serfdom) && settlement.IsTown)
					{
						explainedNumber.Add(-1f, DefaultPolicies.Serfdom.Name, null);
					}
					if (settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Cantons))
					{
						explainedNumber.Add(1f, DefaultPolicies.Cantons.Name, null);
					}
				}
				if (settlement.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.BattanianMilitiaFeat))
				{
					explainedNumber.Add(DefaultCulturalFeats.BattanianMilitiaFeat.EffectBonus, DefaultSettlementMilitiaModel.CultureText, null);
				}
			}
			if (settlement.IsCastle || settlement.IsTown)
			{
				settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.Militia, ref explainedNumber);
				if (settlement.IsCastle && settlement.Town.InRebelliousState)
				{
					settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.MilitiaReduction, ref explainedNumber);
				}
				DefaultSettlementMilitiaModel.GetSettlementMilitiaChangeDueToPolicies(settlement, ref explainedNumber);
				DefaultSettlementMilitiaModel.GetSettlementMilitiaChangeDueToPerks(settlement, ref explainedNumber);
				DefaultSettlementMilitiaModel.GetSettlementMilitiaChangeDueToIssues(settlement, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x000842F0 File Offset: 0x000824F0
		private static void GetSettlementMilitiaChangeDueToPerks(Settlement settlement, ref ExplainedNumber result)
		{
			if (settlement.Town != null && settlement.Town.Governor != null)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.SwiftStrike, settlement.Town, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Polearm.KeepAtBay, settlement.Town, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.MerryMen, settlement.Town, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Crossbow.LongShots, settlement.Town, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Throwing.SlingingCompetitions, settlement.Town, ref result);
				if (settlement.IsUnderSiege)
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Roguery.ArmsDealer, settlement.Town, ref result);
				}
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.SevenVeterans, settlement.Town, ref result);
			}
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x00084394 File Offset: 0x00082594
		private static void GetSettlementMilitiaChangeDueToPolicies(Settlement settlement, ref ExplainedNumber result)
		{
			Kingdom kingdom = settlement.OwnerClan.Kingdom;
			if (kingdom != null && kingdom.ActivePolicies.Contains(DefaultPolicies.Citizenship))
			{
				result.Add(1f, DefaultPolicies.Citizenship.Name, null);
			}
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x000843D8 File Offset: 0x000825D8
		private static void GetSettlementMilitiaChangeDueToIssues(Settlement settlement, ref ExplainedNumber result)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementMilitia, settlement, ref result);
		}

		// Token: 0x040008B5 RID: 2229
		private static readonly TextObject BaseText = new TextObject("{=militarybase}Base", null);

		// Token: 0x040008B6 RID: 2230
		private static readonly TextObject FromHearthsText = new TextObject("{=ecdZglky}From Hearths", null);

		// Token: 0x040008B7 RID: 2231
		private static readonly TextObject FromProsperityText = new TextObject("{=cTmiNAlI}From Prosperity", null);

		// Token: 0x040008B8 RID: 2232
		private static readonly TextObject RetiredText = new TextObject("{=gHnfFi1s}Retired", null);

		// Token: 0x040008B9 RID: 2233
		private static readonly TextObject MilitiaFromMarketText = new TextObject("{=7ve3bQxg}Weapons From Market", null);

		// Token: 0x040008BA RID: 2234
		private static readonly TextObject LowLoyaltyText = new TextObject("{=SJ2qsRdF}Low Loyalty", null);

		// Token: 0x040008BB RID: 2235
		private static readonly TextObject CultureText = GameTexts.FindText("str_culture", null);

		// Token: 0x040008BC RID: 2236
		private const int AutoSpawnMilitiaDayMultiplierAfterSiege = 25;

		// Token: 0x040008BD RID: 2237
		private const int BaseFortificationMilitiaChange = 2;

		// Token: 0x040008BE RID: 2238
		private const float BaseVillageMilitiaChange = 0.5f;
	}
}
