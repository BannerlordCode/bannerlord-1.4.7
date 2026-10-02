using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F3 RID: 1011
	public class GarrisonTroopsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003F7F RID: 16255 RVA: 0x0011F658 File Offset: 0x0011D858
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUpEvent));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x0011F6AA File Offset: 0x0011D8AA
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0011F6AC File Offset: 0x0011D8AC
		private void OnNewGameCreatedPartialFollowUpEvent(CampaignGameStarter starter, int i)
		{
			List<Settlement> list = Campaign.Current.Settlements.WhereQ<Settlement>((Settlement x) => x.IsFortification).ToList<Settlement>();
			int count = list.Count;
			int num = count / 100 + ((count % 100 > i) ? 1 : 0);
			int num2 = count / 100 * i;
			for (int j = 0; j < i; j++)
			{
				num2 += ((count % 100 > j) ? 1 : 0);
			}
			for (int k = 0; k < num; k++)
			{
				Settlement settlement = list[num2 + k];
				settlement.AddGarrisonParty();
				this.FillGarrisonPartyOnNewGame(settlement.Town);
			}
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x0011F75C File Offset: 0x0011D95C
		private void FillGarrisonPartyOnNewGame(Town fortification)
		{
			PartyTemplateObject defaultPartyTemplate = fortification.Culture.DefaultPartyTemplate;
			float num = (float)70;
			float num2 = 1f + fortification.Prosperity / 1300f;
			int num3 = MathF.Round(num * num2);
			for (int i = 0; i < num3; i++)
			{
				int num4 = 0;
				float num5 = 0f;
				for (int j = 0; j < defaultPartyTemplate.Stacks.Count; j++)
				{
					num5 += (defaultPartyTemplate.Stacks[j].Character.IsRanged ? 6f : ((!defaultPartyTemplate.Stacks[j].Character.IsMounted) ? 2f : 1f)) * ((float)(defaultPartyTemplate.Stacks[j].MaxValue + defaultPartyTemplate.Stacks[j].MinValue) / 2f);
				}
				float num6 = MBRandom.RandomFloat * num5;
				for (int k = 0; k < defaultPartyTemplate.Stacks.Count; k++)
				{
					num6 -= (defaultPartyTemplate.Stacks[k].Character.IsRanged ? 6f : ((!defaultPartyTemplate.Stacks[k].Character.IsMounted) ? 2f : 1f)) * ((float)(defaultPartyTemplate.Stacks[k].MaxValue + defaultPartyTemplate.Stacks[k].MinValue) / 2f);
					if (num6 < 0f)
					{
						num4 = k;
						break;
					}
				}
				CharacterObject character = defaultPartyTemplate.Stacks[num4].Character;
				fortification.GarrisonParty.AddElementToMemberRoster(character, 1, false);
			}
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x0011F917 File Offset: 0x0011DB17
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (openToClaim && detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege && settlement != null)
			{
				this._newlyConqueredFortification = settlement;
			}
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x0011F92C File Offset: 0x0011DB2C
		private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (!Campaign.Current.GameStarted)
			{
				return;
			}
			if (mobileParty != null && mobileParty.IsLordParty && !mobileParty.IsDisbanding && mobileParty.LeaderHero != null && settlement.IsFortification && DiplomacyHelper.IsSameFactionAndNotEliminated(mobileParty.MapFaction, settlement.MapFaction) && (settlement.OwnerClan != Clan.PlayerClan || settlement == this._newlyConqueredFortification))
			{
				if (mobileParty.Army != null)
				{
					if (mobileParty.Army.LeaderParty == mobileParty)
					{
						this.ManageGarrisonForArmy(mobileParty, settlement);
						return;
					}
				}
				else if (!mobileParty.IsMainParty)
				{
					this.ManageGarrisonForParty(mobileParty, settlement);
				}
			}
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x0011F9C4 File Offset: 0x0011DBC4
		private void ManageGarrisonForArmy(MobileParty armyLeaderParty, Settlement settlement)
		{
			GarrisonTroopsCampaignBehavior.ArmyGarrisonTransferDataArgs armyGarrisonTransferDataArgs;
			this.CollectArmyGarrisonTransferDataArgs(armyLeaderParty, settlement, out armyGarrisonTransferDataArgs);
			if (armyGarrisonTransferDataArgs.IsLeavingTroopsToGarrison)
			{
				this.TryToLeaveTroopsToGarrisonForArmy(in armyGarrisonTransferDataArgs);
			}
			else
			{
				this.TryToTakeTroopsFromGarrisonForArmy(in armyGarrisonTransferDataArgs);
			}
			this._newlyConqueredFortification = null;
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x0011F9FC File Offset: 0x0011DBFC
		private void CollectArmyGarrisonTransferDataArgs(MobileParty armyLeaderParty, Settlement settlement, out GarrisonTroopsCampaignBehavior.ArmyGarrisonTransferDataArgs armyGarrionTransferDataArgs)
		{
			armyGarrionTransferDataArgs = default(GarrisonTroopsCampaignBehavior.ArmyGarrisonTransferDataArgs);
			int num = this.CalculateSettlementGarrisonPartySizeLimitWithFoodAndWage(settlement);
			int num2 = this.CalculateSettlementIdealPartySizeWithEffects(settlement);
			List<ValueTuple<MobileParty, int>> list = this.CalculateMobilePartiesIdealPartySizes(armyLeaderParty);
			MobileParty garrisonParty = settlement.Town.GarrisonParty;
			int num3 = ((garrisonParty != null) ? garrisonParty.Party.NumberOfRegularMembers : 0);
			int num4 = num3;
			int num5 = num2;
			foreach (ValueTuple<MobileParty, int> valueTuple in list)
			{
				num4 += valueTuple.Item1.Party.NumberOfRegularMembers;
				num5 += valueTuple.Item2;
			}
			float num6 = (float)num2 / (float)num5;
			int num7 = MBRandom.RoundRandomized((float)num4 * num6);
			int num8 = (settlement.IsTown ? 125 : 75);
			int num9 = (settlement.IsTown ? 750 : 500);
			num7 = Math.Min(num7, num);
			num7 = MBMath.ClampInt(num7, num8, num9);
			armyGarrionTransferDataArgs.Settlement = settlement;
			armyGarrionTransferDataArgs.ArmyPartiesIdealPartySizes = list;
			armyGarrionTransferDataArgs.TotalIdealPartySize = num5;
			armyGarrionTransferDataArgs.TotalMenCount = num4;
			armyGarrionTransferDataArgs.SettlementCurrentMenCount = num3;
			armyGarrionTransferDataArgs.SettlementFinalMenCount = num7;
			armyGarrionTransferDataArgs.IsLeavingTroopsToGarrison = num7 > num3;
			if (settlement.Town.GarrisonParty != null && settlement.Town.GarrisonParty.IsWageLimitExceeded())
			{
				armyGarrionTransferDataArgs.IsLeavingTroopsToGarrison = false;
			}
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x0011FB5C File Offset: 0x0011DD5C
		private void TryToLeaveTroopsToGarrisonForArmy(in GarrisonTroopsCampaignBehavior.ArmyGarrisonTransferDataArgs armyGarrisonTransferDataArgs)
		{
			GarrisonTroopsCampaignBehavior.ArmyGarrisonTransferDataArgs armyGarrisonTransferDataArgs2 = armyGarrisonTransferDataArgs;
			foreach (ValueTuple<MobileParty, int> valueTuple in armyGarrisonTransferDataArgs2.GetTroopsToLeaveDataForArmy())
			{
				MobileParty item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				this.LeaveTroopsToGarrison(item, armyGarrisonTransferDataArgs.Settlement, item2, true);
			}
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x0011FBCC File Offset: 0x0011DDCC
		private void TryToTakeTroopsFromGarrisonForArmy(in GarrisonTroopsCampaignBehavior.ArmyGarrisonTransferDataArgs armyGarrisonTransferDataArgs)
		{
			GarrisonTroopsCampaignBehavior.ArmyGarrisonTransferDataArgs armyGarrisonTransferDataArgs2 = armyGarrisonTransferDataArgs;
			foreach (ValueTuple<MobileParty, int> valueTuple in armyGarrisonTransferDataArgs2.GetTroopsToTakeDataForArmy())
			{
				MobileParty item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				this.TakeTroopsFromGarrison(item, armyGarrisonTransferDataArgs.Settlement, item2, false);
			}
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x0011FC3C File Offset: 0x0011DE3C
		private int CalculateSettlementIdealPartySizeWithEffects(Settlement settlement)
		{
			float num = (float)this.CalculateSettlementGarrisonPartySizeLimitWithFoodAndWage(settlement);
			float num2 = (settlement.IsTown ? this.GetProsperityEffectForTown(settlement.Town) : 1f);
			float num3 = 1f;
			if (this._newlyConqueredFortification != null)
			{
				num3 = (settlement.IsTown ? 1.75f : 1.33f);
			}
			float num4 = num2 * num3;
			return MBRandom.RoundRandomized(num * num4);
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x0011FC9C File Offset: 0x0011DE9C
		private void ManageGarrisonForParty(MobileParty mobileParty, Settlement settlement)
		{
			GarrisonTroopsCampaignBehavior.PartyGarrisonTransferDataArgs partyGarrisonTransferDataArgs;
			this.CollectPartyGarrisonTransferData(mobileParty, settlement, out partyGarrisonTransferDataArgs);
			if (partyGarrisonTransferDataArgs.IsLeavingTroopsToGarrison)
			{
				this.TryToLeaveTroopsToGarrisonForParty(in partyGarrisonTransferDataArgs);
			}
			else
			{
				this.TryToTakeTroopsFromGarrisonForParty(in partyGarrisonTransferDataArgs);
			}
			this._newlyConqueredFortification = null;
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x0011FCD4 File Offset: 0x0011DED4
		private void CollectPartyGarrisonTransferData(MobileParty mobileParty, Settlement settlement, out GarrisonTroopsCampaignBehavior.PartyGarrisonTransferDataArgs partyGarrisonTransferDataArgs)
		{
			partyGarrisonTransferDataArgs = default(GarrisonTroopsCampaignBehavior.PartyGarrisonTransferDataArgs);
			int num = this.CalculateSettlementGarrisonPartySizeLimitWithFoodAndWage(settlement);
			int num2 = this.CalculateSettlementIdealPartySizeWithEffects(settlement);
			int num3 = this.CalculateMobilePartySizeLimitWithFoodAndWage(mobileParty);
			MobileParty garrisonParty = settlement.Town.GarrisonParty;
			int num4 = ((garrisonParty != null) ? garrisonParty.Party.NumberOfRegularMembers : 0);
			int num5 = mobileParty.Party.NumberOfRegularMembers + num4;
			int num6 = num2 + num3;
			float num7 = (float)num2 / (float)num6;
			int num8 = MBRandom.RoundRandomized((float)num5 * num7);
			int num9 = (settlement.IsTown ? 125 : 75);
			int num10 = (settlement.IsTown ? 750 : 500);
			num8 = Math.Min(num8, num);
			num8 = MBMath.ClampInt(num8, num9, num10);
			partyGarrisonTransferDataArgs.Settlement = settlement;
			partyGarrisonTransferDataArgs.MobileParty = mobileParty;
			partyGarrisonTransferDataArgs.PartyIdealPartySize = num3;
			partyGarrisonTransferDataArgs.SettlementIdealPartySize = num;
			partyGarrisonTransferDataArgs.TotalIdealPartySize = num6;
			partyGarrisonTransferDataArgs.PartyCurrentMenCount = mobileParty.Party.NumberOfRegularMembers;
			partyGarrisonTransferDataArgs.SettlementCurrentMenCount = num4;
			partyGarrisonTransferDataArgs.TotalMenCount = num5;
			partyGarrisonTransferDataArgs.SettlementFinalMenCount = num8;
			partyGarrisonTransferDataArgs.IsLeavingTroopsToGarrison = num8 > num4;
			if ((settlement.Town.GarrisonParty != null && settlement.Town.GarrisonParty.IsWageLimitExceeded()) || (mobileParty.LeaderHero.Clan == Clan.PlayerClan && this._newlyConqueredFortification == null))
			{
				partyGarrisonTransferDataArgs.IsLeavingTroopsToGarrison = false;
			}
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x0011FE18 File Offset: 0x0011E018
		private void TryToLeaveTroopsToGarrisonForParty(in GarrisonTroopsCampaignBehavior.PartyGarrisonTransferDataArgs partyGarrisonTransferDataArgs)
		{
			GarrisonTroopsCampaignBehavior.PartyGarrisonTransferDataArgs partyGarrisonTransferDataArgs2 = partyGarrisonTransferDataArgs;
			int numberOfTroopsToLeaveForParty = partyGarrisonTransferDataArgs2.GetNumberOfTroopsToLeaveForParty();
			if (numberOfTroopsToLeaveForParty > 0)
			{
				this.LeaveTroopsToGarrison(partyGarrisonTransferDataArgs.MobileParty, partyGarrisonTransferDataArgs.Settlement, numberOfTroopsToLeaveForParty, true);
			}
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x0011FE4C File Offset: 0x0011E04C
		private void TryToTakeTroopsFromGarrisonForParty(in GarrisonTroopsCampaignBehavior.PartyGarrisonTransferDataArgs partyGarrisonTransferDataArgs)
		{
			GarrisonTroopsCampaignBehavior.PartyGarrisonTransferDataArgs partyGarrisonTransferDataArgs2 = partyGarrisonTransferDataArgs;
			int numberOfTroopsToTakeForParty = partyGarrisonTransferDataArgs2.GetNumberOfTroopsToTakeForParty();
			if (numberOfTroopsToTakeForParty > 0)
			{
				this.TakeTroopsFromGarrison(partyGarrisonTransferDataArgs.MobileParty, partyGarrisonTransferDataArgs.Settlement, numberOfTroopsToTakeForParty, false);
			}
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x0011FE80 File Offset: 0x0011E080
		private List<ValueTuple<MobileParty, int>> CalculateMobilePartiesIdealPartySizes(MobileParty armyLeaderParty)
		{
			List<ValueTuple<MobileParty, int>> list = new List<ValueTuple<MobileParty, int>>();
			List<MobileParty> list2 = new List<MobileParty>();
			if (armyLeaderParty != MobileParty.MainParty && (armyLeaderParty.LeaderHero.Clan != Clan.PlayerClan || this._newlyConqueredFortification != null))
			{
				list2.Add(armyLeaderParty);
			}
			foreach (MobileParty mobileParty in armyLeaderParty.AttachedParties)
			{
				if (mobileParty != MobileParty.MainParty && mobileParty.LeaderHero != null && (mobileParty.LeaderHero.Clan != Clan.PlayerClan || this._newlyConqueredFortification != null))
				{
					list2.Add(mobileParty);
				}
			}
			foreach (MobileParty mobileParty2 in list2)
			{
				int num = this.CalculateMobilePartySizeLimitWithFoodAndWage(mobileParty2);
				list.Add(new ValueTuple<MobileParty, int>(mobileParty2, num));
			}
			return list;
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x0011FF84 File Offset: 0x0011E184
		private int CalculateMobilePartySizeLimitWithFoodAndWage(MobileParty mobileParty)
		{
			int partySizeLimit = mobileParty.Party.PartySizeLimit;
			int num = MathF.Round((float)mobileParty.PaymentLimit / Campaign.Current.AverageWage);
			int num2 = 2;
			float numberOfMenOnMapToEatOneFood = (float)Campaign.Current.Models.MobilePartyFoodConsumptionModel.NumberOfMenOnMapToEatOneFood;
			float num3;
			if (mobileParty.Army == null)
			{
				num3 = mobileParty.Food;
			}
			else
			{
				num3 = mobileParty.Army.Parties.Sum<MobileParty>((MobileParty s) => s.Food);
			}
			float num4 = num3;
			int num5 = MathF.Round(numberOfMenOnMapToEatOneFood * num4 / (float)num2);
			num5 = MathF.Max(num5, 30);
			return MathF.Min(MathF.Min(num, partySizeLimit), num5);
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x0012002C File Offset: 0x0011E22C
		private int CalculateMaxGarrisonSizeTownCanFeed(Town town, bool includeMarketStocks = true)
		{
			SettlementFoodModel settlementFoodModel = Campaign.Current.Models.SettlementFoodModel;
			if (settlementFoodModel == null)
			{
				return 0;
			}
			float resultNumber = settlementFoodModel.CalculateTownFoodStocksChange(town, includeMarketStocks, false).ResultNumber;
			MobileParty garrisonParty = town.GarrisonParty;
			int num = ((garrisonParty != null) ? garrisonParty.Party.NumberOfRegularMembers : 0);
			float num2 = 0f;
			float num3 = 0f;
			if (town.Governor != null)
			{
				if (town.IsUnderSiege)
				{
					if (town.Governor.GetPerkValue(DefaultPerks.Steward.Gourmet))
					{
						num3 += DefaultPerks.Steward.Gourmet.SecondaryBonus;
					}
					if (town.Governor.GetPerkValue(DefaultPerks.Medicine.TriageTent))
					{
						num2 += DefaultPerks.Medicine.TriageTent.SecondaryBonus;
					}
				}
				if (town.Governor.GetPerkValue(DefaultPerks.Steward.MasterOfWarcraft))
				{
					num2 += DefaultPerks.Steward.MasterOfWarcraft.SecondaryBonus;
				}
			}
			float num4 = -town.Prosperity / (float)settlementFoodModel.NumberOfProsperityToEatOneFood;
			float num5 = 1f;
			float num6 = num4 * num5;
			int num7;
			if (resultNumber < num6)
			{
				if (this._newlyConqueredFortification != null)
				{
					num7 = (int)MBMath.Map(town.Prosperity, 0f, 8000f, 150f, 300f);
				}
				else
				{
					int num8 = MathF.Round(MathF.Abs(resultNumber - num6) * (float)settlementFoodModel.NumberOfMenOnGarrisonToEatOneFood / (1f + num2 + num3));
					num7 = Math.Max(num - num8, 0);
				}
			}
			else
			{
				int num9 = MathF.Round((MathF.Abs(num6) + resultNumber) * (float)settlementFoodModel.NumberOfMenOnGarrisonToEatOneFood / (1f + num2 + num3));
				num7 = num + num9;
			}
			return num7;
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x001201A4 File Offset: 0x0011E3A4
		private int CalculateSettlementGarrisonPartySizeLimitWithFoodAndWage(Settlement settlement)
		{
			int num = (int)Campaign.Current.Models.PartySizeLimitModel.CalculateGarrisonPartySizeLimit(settlement, false).ResultNumber;
			float num2 = FactionHelper.FindIdealGarrisonStrengthPerWalledCenter(settlement.OwnerClan.MapFaction as Kingdom, settlement.OwnerClan);
			List<float> list = new List<float>();
			float num3;
			if (settlement.OwnerClan.Kingdom != null)
			{
				foreach (Clan clan in settlement.OwnerClan.Kingdom.Clans)
				{
					list.Add(FactionHelper.OwnerClanEconomyEffectOnGarrisonSizeConstant(clan));
				}
				num3 = list.Average();
			}
			else
			{
				num3 = FactionHelper.OwnerClanEconomyEffectOnGarrisonSizeConstant(settlement.OwnerClan);
			}
			float num4 = FactionHelper.SettlementProsperityEffectOnGarrisonSizeConstant(settlement.Town);
			float num5 = FactionHelper.SettlementFoodPotentialEffectOnGarrisonSizeConstant(settlement);
			float num6 = num3 * num4 * num5;
			float num7 = 1.5f;
			int num8 = MathF.Round(num6 * num2 * num7);
			int num9 = this.CalculateMaxGarrisonSizeTownCanFeed(settlement.Town, true);
			return MathF.Min(MathF.Min(num, num9), num8);
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x001202BC File Offset: 0x0011E4BC
		private float GetProsperityEffectForTown(Town town)
		{
			return MBMath.Map(town.Prosperity, 0f, 8000f, 1f, 1.35f);
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x001202E0 File Offset: 0x0011E4E0
		private CharacterObject GetASuitableCharacterFromPartyRosterByWeight(TroopRoster troopRoster, bool archersAreHighPriority)
		{
			List<ValueTuple<CharacterObject, float>> list = new List<ValueTuple<CharacterObject, float>>();
			for (int i = 0; i < troopRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = troopRoster.GetElementCopyAtIndex(i);
				if (!elementCopyAtIndex.Character.IsHero)
				{
					if (archersAreHighPriority && elementCopyAtIndex.Character.IsRanged)
					{
						list.Add(new ValueTuple<CharacterObject, float>(elementCopyAtIndex.Character, (float)(elementCopyAtIndex.Number * 4)));
					}
					else
					{
						list.Add(new ValueTuple<CharacterObject, float>(elementCopyAtIndex.Character, (float)elementCopyAtIndex.Number));
					}
				}
			}
			if (!list.IsEmpty<ValueTuple<CharacterObject, float>>())
			{
				return MBRandom.ChooseWeighted<CharacterObject>(list);
			}
			return null;
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x00120370 File Offset: 0x0011E570
		private void LeaveTroopsToGarrison(MobileParty mobileParty, Settlement settlement, int numberOfTroopsToLeave, bool archersAreHighPriority)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			for (int i = 0; i < numberOfTroopsToLeave; i++)
			{
				CharacterObject asuitableCharacterFromPartyRosterByWeight = this.GetASuitableCharacterFromPartyRosterByWeight(mobileParty.MemberRoster, archersAreHighPriority);
				if (asuitableCharacterFromPartyRosterByWeight == null)
				{
					break;
				}
				foreach (TroopRosterElement troopRosterElement in mobileParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character == asuitableCharacterFromPartyRosterByWeight)
					{
						if (settlement.Town.GarrisonParty == null)
						{
							settlement.AddGarrisonParty();
						}
						if (troopRosterElement.WoundedNumber > 0)
						{
							settlement.Town.GarrisonParty.MemberRoster.AddToCounts(asuitableCharacterFromPartyRosterByWeight, 1, false, 1, 0, true, -1);
							troopRoster.AddToCounts(asuitableCharacterFromPartyRosterByWeight, 1, false, 1, 0, true, -1);
							mobileParty.MemberRoster.AddToCounts(asuitableCharacterFromPartyRosterByWeight, -1, false, -1, 0, true, -1);
							break;
						}
						settlement.Town.GarrisonParty.MemberRoster.AddToCounts(asuitableCharacterFromPartyRosterByWeight, 1, false, 0, 0, true, -1);
						troopRoster.AddToCounts(asuitableCharacterFromPartyRosterByWeight, 1, false, 0, 0, true, -1);
						mobileParty.MemberRoster.AddToCounts(asuitableCharacterFromPartyRosterByWeight, -1, false, 0, 0, true, -1);
						break;
					}
				}
			}
			if (troopRoster.Count > 0)
			{
				CampaignEventDispatcher.Instance.OnTroopGivenToSettlement(mobileParty.LeaderHero, settlement, troopRoster);
				this.ApplyKingdomInfluenceBonusForLeavingTroopToGarrison(mobileParty, settlement, troopRoster);
			}
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x001204C4 File Offset: 0x0011E6C4
		private void TakeTroopsFromGarrison(MobileParty mobileParty, Settlement settlement, int numberOfTroopsToTake, bool archersAreHighPriority)
		{
			for (int i = 0; i < numberOfTroopsToTake; i++)
			{
				CharacterObject asuitableCharacterFromPartyRosterByWeight = this.GetASuitableCharacterFromPartyRosterByWeight(settlement.Town.GarrisonParty.MemberRoster, archersAreHighPriority);
				if (asuitableCharacterFromPartyRosterByWeight == null)
				{
					break;
				}
				foreach (TroopRosterElement troopRosterElement in settlement.Town.GarrisonParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character == asuitableCharacterFromPartyRosterByWeight)
					{
						if (troopRosterElement.Number - troopRosterElement.WoundedNumber > 0)
						{
							mobileParty.MemberRoster.AddToCounts(asuitableCharacterFromPartyRosterByWeight, 1, false, 0, 0, true, -1);
							settlement.Town.GarrisonParty.MemberRoster.AddToCounts(asuitableCharacterFromPartyRosterByWeight, -1, false, 0, 0, true, -1);
							break;
						}
						mobileParty.MemberRoster.AddToCounts(asuitableCharacterFromPartyRosterByWeight, 1, false, 1, 0, true, -1);
						settlement.Town.GarrisonParty.MemberRoster.AddToCounts(asuitableCharacterFromPartyRosterByWeight, -1, false, -1, 0, true, -1);
						break;
					}
				}
			}
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x001205D8 File Offset: 0x0011E7D8
		private void ApplyKingdomInfluenceBonusForLeavingTroopToGarrison(MobileParty mobileParty, Settlement settlement, TroopRoster troopsToBeTransferred)
		{
			if (mobileParty.LeaderHero != null && settlement.OwnerClan != mobileParty.LeaderHero.Clan)
			{
				float num = 0f;
				foreach (TroopRosterElement troopRosterElement in troopsToBeTransferred.GetTroopRoster())
				{
					float troopPower = Campaign.Current.Models.MilitaryPowerModel.GetTroopPower(troopRosterElement.Character, BattleSideEnum.Defender, MapEvent.PowerCalculationContext.Siege, 0f);
					num += troopPower * (float)troopRosterElement.Number;
				}
				GainKingdomInfluenceAction.ApplyForLeavingTroopToGarrison(mobileParty.LeaderHero, num / 3f);
			}
		}

		// Token: 0x040012F1 RID: 4849
		private const int PartyMinMenNumberAfterDonation = 30;

		// Token: 0x040012F2 RID: 4850
		private const int MinGarrisonNumberForTown = 125;

		// Token: 0x040012F3 RID: 4851
		private const int MinGarrisonNumberForCastle = 75;

		// Token: 0x040012F4 RID: 4852
		private const int MaxGarrisonNumberForTown = 750;

		// Token: 0x040012F5 RID: 4853
		private const int MaxGarrisonNumberForCastle = 500;

		// Token: 0x040012F6 RID: 4854
		private Settlement _newlyConqueredFortification;

		// Token: 0x02000802 RID: 2050
		private struct ArmyGarrisonTransferDataArgs
		{
			// Token: 0x0600647A RID: 25722 RVA: 0x001C40FC File Offset: 0x001C22FC
			public List<ValueTuple<MobileParty, int>> GetTroopsToLeaveDataForArmy()
			{
				List<ValueTuple<MobileParty, int>> list = new List<ValueTuple<MobileParty, int>>();
				for (int i = 0; i < this.ArmyPartiesIdealPartySizes.Count; i++)
				{
					ValueTuple<MobileParty, int> valueTuple = this.ArmyPartiesIdealPartySizes[i];
					MobileParty item = valueTuple.Item1;
					int item2 = valueTuple.Item2;
					float num = (float)item2 / (float)this.TotalIdealPartySize;
					int num2 = MBMath.ClampInt(MBRandom.RoundRandomized((float)this.TotalMenCount * num), 30, item2);
					int numberOfRegularMembers = item.Party.NumberOfRegularMembers;
					num2 = Math.Min(num2, numberOfRegularMembers);
					int num3 = numberOfRegularMembers - num2;
					if (num3 > 0)
					{
						list.Add(new ValueTuple<MobileParty, int>(item, num3));
					}
				}
				int num4 = list.Sum<ValueTuple<MobileParty, int>>((ValueTuple<MobileParty, int> s) => s.Item2);
				int num5 = Math.Max(this.SettlementFinalMenCount - this.SettlementCurrentMenCount, 0);
				if (num4 > num5)
				{
					float num6 = (float)num5 / (float)num4;
					for (int j = list.Count - 1; j >= 0; j--)
					{
						list[j] = new ValueTuple<MobileParty, int>(list[j].Item1, MBRandom.RoundRandomized((float)list[j].Item2 * num6));
						if (list[j].Item2 == 0)
						{
							list.RemoveAt(j);
						}
					}
				}
				return list;
			}

			// Token: 0x0600647B RID: 25723 RVA: 0x001C4244 File Offset: 0x001C2444
			public List<ValueTuple<MobileParty, int>> GetTroopsToTakeDataForArmy()
			{
				List<ValueTuple<MobileParty, int>> list = new List<ValueTuple<MobileParty, int>>();
				if (this.SettlementFinalMenCount < this.SettlementCurrentMenCount)
				{
					for (int i = 0; i < this.ArmyPartiesIdealPartySizes.Count; i++)
					{
						ValueTuple<MobileParty, int> valueTuple = this.ArmyPartiesIdealPartySizes[i];
						MobileParty item = valueTuple.Item1;
						if (item.LeaderHero.Clan == this.Settlement.OwnerClan && !item.IsWageLimitExceeded())
						{
							int item2 = valueTuple.Item2;
							float num = (float)item2 / (float)this.TotalIdealPartySize;
							int num2 = MBMath.ClampInt(MBRandom.RoundRandomized((float)this.TotalMenCount * num), 30, item2);
							int numberOfRegularMembers = item.Party.NumberOfRegularMembers;
							int num3 = Math.Max(num2, numberOfRegularMembers) - numberOfRegularMembers;
							int num4 = Math.Max(item2 - numberOfRegularMembers, 0);
							num3 = Math.Min(num3, num4);
							if (num3 > 0)
							{
								list.Add(new ValueTuple<MobileParty, int>(item, num3));
							}
						}
					}
					int num5 = list.Sum<ValueTuple<MobileParty, int>>((ValueTuple<MobileParty, int> s) => s.Item2);
					int num6 = Math.Max(this.SettlementCurrentMenCount - this.SettlementFinalMenCount, 0);
					if (num5 > num6)
					{
						float num7 = (float)num6 / (float)num5;
						for (int j = list.Count - 1; j >= 0; j--)
						{
							list[j] = new ValueTuple<MobileParty, int>(list[j].Item1, MBRandom.RoundRandomized((float)list[j].Item2 * num7));
							if (list[j].Item2 == 0)
							{
								list.RemoveAt(j);
							}
						}
					}
				}
				return list;
			}

			// Token: 0x04002054 RID: 8276
			public Settlement Settlement;

			// Token: 0x04002055 RID: 8277
			public List<ValueTuple<MobileParty, int>> ArmyPartiesIdealPartySizes;

			// Token: 0x04002056 RID: 8278
			public int TotalIdealPartySize;

			// Token: 0x04002057 RID: 8279
			public int TotalMenCount;

			// Token: 0x04002058 RID: 8280
			public int SettlementFinalMenCount;

			// Token: 0x04002059 RID: 8281
			public int SettlementCurrentMenCount;

			// Token: 0x0400205A RID: 8282
			public bool IsLeavingTroopsToGarrison;
		}

		// Token: 0x02000803 RID: 2051
		private struct PartyGarrisonTransferDataArgs
		{
			// Token: 0x0600647C RID: 25724 RVA: 0x001C43D8 File Offset: 0x001C25D8
			public int GetNumberOfTroopsToLeaveForParty()
			{
				int num = 0;
				if (this.SettlementFinalMenCount > this.SettlementCurrentMenCount)
				{
					float num2 = (float)this.PartyIdealPartySize / (float)this.TotalIdealPartySize;
					int num3 = MBMath.ClampInt(MBRandom.RoundRandomized((float)this.TotalMenCount * num2), 30, this.PartyIdealPartySize);
					int partyCurrentMenCount = this.PartyCurrentMenCount;
					num3 = Math.Min(num3, partyCurrentMenCount);
					num = partyCurrentMenCount - num3;
					int num4 = Math.Max(this.SettlementIdealPartySize - this.SettlementCurrentMenCount, 0);
					num = Math.Min(num, num4);
				}
				return num;
			}

			// Token: 0x0600647D RID: 25725 RVA: 0x001C4454 File Offset: 0x001C2654
			public int GetNumberOfTroopsToTakeForParty()
			{
				int num = 0;
				if (this.MobileParty.LeaderHero.Clan == this.Settlement.OwnerClan && !this.MobileParty.IsWageLimitExceeded() && this.SettlementFinalMenCount < this.SettlementCurrentMenCount)
				{
					float num2 = (float)this.PartyIdealPartySize / (float)this.TotalIdealPartySize;
					int num3 = MBMath.ClampInt(MBRandom.RoundRandomized((float)this.TotalMenCount * num2), 30, this.PartyIdealPartySize);
					int partyCurrentMenCount = this.PartyCurrentMenCount;
					num = Math.Max(num3, partyCurrentMenCount) - partyCurrentMenCount;
					int num4 = Math.Max(this.PartyIdealPartySize - partyCurrentMenCount, 0);
					num = Math.Min(num, num4);
					int num5 = this.SettlementCurrentMenCount - this.SettlementFinalMenCount;
					num = Math.Min(num, num5);
				}
				return num;
			}

			// Token: 0x0400205B RID: 8283
			public Settlement Settlement;

			// Token: 0x0400205C RID: 8284
			public MobileParty MobileParty;

			// Token: 0x0400205D RID: 8285
			public int PartyIdealPartySize;

			// Token: 0x0400205E RID: 8286
			public int SettlementIdealPartySize;

			// Token: 0x0400205F RID: 8287
			public int TotalIdealPartySize;

			// Token: 0x04002060 RID: 8288
			public int TotalMenCount;

			// Token: 0x04002061 RID: 8289
			public int PartyCurrentMenCount;

			// Token: 0x04002062 RID: 8290
			public int SettlementFinalMenCount;

			// Token: 0x04002063 RID: 8291
			public int SettlementCurrentMenCount;

			// Token: 0x04002064 RID: 8292
			public bool IsLeavingTroopsToGarrison;
		}
	}
}
