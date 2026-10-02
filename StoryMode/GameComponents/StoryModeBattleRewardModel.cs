using System;
using System.Collections.Generic;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace StoryMode.GameComponents
{
	// Token: 0x0200003E RID: 62
	public class StoryModeBattleRewardModel : BattleRewardModel
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x00018CDA File Offset: 0x00016EDA
		public override int CalculateGoldLossAfterDefeat(Hero partyLeaderHero)
		{
			return base.BaseModel.CalculateGoldLossAfterDefeat(partyLeaderHero);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00018CE8 File Offset: 0x00016EE8
		public override ExplainedNumber CalculateInfluenceGain(PartyBase winnerParty, float influenceValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, float influenceMultiplierForWinnerSide, bool includeDescriptions)
		{
			return base.BaseModel.CalculateInfluenceGain(winnerParty, influenceValueOfBattleForWinnerSide, contributionShareOfWinnerParty, influenceMultiplierForWinnerSide, includeDescriptions);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00018CFC File Offset: 0x00016EFC
		public override float CalculateMoraleChangeOnRoundVictory(PartyBase party, MapEventSide partySide, BattleSideEnum roundWinner)
		{
			return base.BaseModel.CalculateMoraleChangeOnRoundVictory(party, partySide, roundWinner);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00018D0C File Offset: 0x00016F0C
		public override ExplainedNumber CalculateMoraleGainVictory(PartyBase winnerParty, float renownValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, bool includeDescriptions)
		{
			return base.BaseModel.CalculateMoraleGainVictory(winnerParty, renownValueOfBattleForWinnerSide, contributionShareOfWinnerParty, includeDescriptions);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00018D1E File Offset: 0x00016F1E
		public override int CalculatePlunderedGoldAmountFromDefeatedParty(PartyBase defeatedParty)
		{
			return base.BaseModel.CalculatePlunderedGoldAmountFromDefeatedParty(defeatedParty);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00018D2C File Offset: 0x00016F2C
		public override ExplainedNumber CalculateRenownGain(PartyBase winnerParty, float renownValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, float renownMultiplierForWinnerSide, bool includeDescriptions)
		{
			if (TutorialPhase.Instance != null && !TutorialPhase.Instance.IsCompleted && winnerParty == PartyBase.MainParty)
			{
				return default(ExplainedNumber);
			}
			return base.BaseModel.CalculateRenownGain(winnerParty, renownValueOfBattleForWinnerSide, contributionShareOfWinnerParty, renownMultiplierForWinnerSide, includeDescriptions);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00018D70 File Offset: 0x00016F70
		public override float CalculateShipDamageAfterDefeat(Ship ship)
		{
			return 0f;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00018D77 File Offset: 0x00016F77
		public override MBReadOnlyList<KeyValuePair<Ship, MapEventParty>> DistributeDefeatedPartyShipsAmongWinners(MapEvent mapEvent, MBReadOnlyList<Ship> shipsToLoot, MBReadOnlyList<MapEventParty> winnerParties)
		{
			return new MBReadOnlyList<KeyValuePair<Ship, MapEventParty>>();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00018D7E File Offset: 0x00016F7E
		public override float GetAITradePenalty()
		{
			return base.BaseModel.GetAITradePenalty();
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00018D8B File Offset: 0x00016F8B
		public override float GetBannerLootChanceFromDefeatedHero(Hero defeatedHero)
		{
			return base.BaseModel.GetBannerLootChanceFromDefeatedHero(defeatedHero);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00018D99 File Offset: 0x00016F99
		public override ItemObject GetBannerRewardForWinningMapEvent(MapEvent mapEvent)
		{
			return base.BaseModel.GetBannerRewardForWinningMapEvent(mapEvent);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00018DA7 File Offset: 0x00016FA7
		public override float GetExpectedLootedItemValueFromCasualty(Hero winnerPartyLeaderHero, CharacterObject casualtyCharacter)
		{
			return base.BaseModel.GetExpectedLootedItemValueFromCasualty(winnerPartyLeaderHero, casualtyCharacter);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00018DB6 File Offset: 0x00016FB6
		public override Figurehead GetFigureheadLoot(MBReadOnlyList<MapEventParty> defeatedParties, PartyBase defeatedSideLeaderParty)
		{
			return base.BaseModel.GetFigureheadLoot(defeatedParties, defeatedSideLeaderParty);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00018DC5 File Offset: 0x00016FC5
		public override MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootCasualtyChances(MBReadOnlyList<MapEventParty> winnerParties, PartyBase defeatedParty)
		{
			return base.BaseModel.GetLootCasualtyChances(winnerParties, defeatedParty);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00018DD4 File Offset: 0x00016FD4
		public override EquipmentElement GetLootedItemFromTroop(CharacterObject character, float targetValue)
		{
			return base.BaseModel.GetLootedItemFromTroop(character, targetValue);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00018DE3 File Offset: 0x00016FE3
		public override MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootGoldChances(MBReadOnlyList<MapEventParty> winnerParties)
		{
			return base.BaseModel.GetLootGoldChances(winnerParties);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00018DF1 File Offset: 0x00016FF1
		public override MBList<KeyValuePair<MapEventParty, float>> GetLootItemChancesForWinnerParties(MBReadOnlyList<MapEventParty> winnerParties, PartyBase defeatedParty)
		{
			return base.BaseModel.GetLootItemChancesForWinnerParties(winnerParties, defeatedParty);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00018E00 File Offset: 0x00017000
		public override void GetCaptureMemberChancesForWinnerParties(MapEvent endedMapEvent, MBReadOnlyList<MapEventParty> winnerParties, out MBList<KeyValuePair<MapEventParty, float>> woundedMemberChances, out MBList<KeyValuePair<MapEventParty, float>> healthyMemberChances)
		{
			base.BaseModel.GetCaptureMemberChancesForWinnerParties(endedMapEvent, winnerParties, out woundedMemberChances, out healthyMemberChances);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00018E14 File Offset: 0x00017014
		public override MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootPrisonerChances(MBReadOnlyList<MapEventParty> winnerParties, TroopRosterElement prisonerElement)
		{
			if (StoryModeData.IsConspiracyTroop(prisonerElement.Character))
			{
				MBList<KeyValuePair<MapEventParty, float>> mblist = new MBList<KeyValuePair<MapEventParty, float>>();
				foreach (MapEventParty mapEventParty in winnerParties)
				{
					mblist.Add(new KeyValuePair<MapEventParty, float>(mapEventParty, 0f));
				}
				return mblist;
			}
			return base.BaseModel.GetLootPrisonerChances(winnerParties, prisonerElement);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00018E90 File Offset: 0x00017090
		public override float GetMainPartyMemberScatterChance()
		{
			return base.BaseModel.GetMainPartyMemberScatterChance();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00018E9D File Offset: 0x0001709D
		public override int GetPlayerGainedRelationAmount(MapEvent mapEvent, Hero hero)
		{
			return base.BaseModel.GetPlayerGainedRelationAmount(mapEvent, hero);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00018EAC File Offset: 0x000170AC
		public override float GetShipSiegeEngineHitMoraleEffect(Ship ship, SiegeEngineType siegeEngineType)
		{
			return base.BaseModel.GetShipSiegeEngineHitMoraleEffect(ship, siegeEngineType);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00018EBB File Offset: 0x000170BB
		public override float GetSunkenShipMoraleEffect(PartyBase shipOwner, Ship ship)
		{
			return base.BaseModel.GetSunkenShipMoraleEffect(shipOwner, ship);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00018ECA File Offset: 0x000170CA
		public override MBReadOnlyList<MapEventParty> GetWinnerPartiesThatCanPlunderGoldFromShips(MBReadOnlyList<MapEventParty> winnerParties)
		{
			return base.BaseModel.GetWinnerPartiesThatCanPlunderGoldFromShips(winnerParties);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00018ED8 File Offset: 0x000170D8
		public override bool CanTroopBeTakenPrisoner(CharacterObject troop)
		{
			return !StoryModeData.IsConspiracyTroop(troop) && base.BaseModel.CanTroopBeTakenPrisoner(troop);
		}
	}
}
