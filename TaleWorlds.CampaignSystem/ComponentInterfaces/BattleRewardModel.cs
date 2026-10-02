using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B7 RID: 439
	public abstract class BattleRewardModel : MBGameModel<BattleRewardModel>
	{
		// Token: 0x06001D8D RID: 7565
		public abstract float GetBannerLootChanceFromDefeatedHero(Hero defeatedHero);

		// Token: 0x06001D8E RID: 7566
		public abstract ItemObject GetBannerRewardForWinningMapEvent(MapEvent mapEvent);

		// Token: 0x06001D8F RID: 7567
		public abstract int GetPlayerGainedRelationAmount(MapEvent mapEvent, Hero hero);

		// Token: 0x06001D90 RID: 7568
		public abstract ExplainedNumber CalculateRenownGain(PartyBase winnerParty, float renownValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, float renownMultiplierForWinnerSide, bool includeDescriptions);

		// Token: 0x06001D91 RID: 7569
		public abstract ExplainedNumber CalculateInfluenceGain(PartyBase winnerParty, float influenceValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, float influenceMultiplierForWinnerSide, bool includeDescriptions);

		// Token: 0x06001D92 RID: 7570
		public abstract ExplainedNumber CalculateMoraleGainVictory(PartyBase winnerParty, float renownValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, bool includeDescriptions);

		// Token: 0x06001D93 RID: 7571
		public abstract float CalculateMoraleChangeOnRoundVictory(PartyBase party, MapEventSide partySide, BattleSideEnum roundWinner);

		// Token: 0x06001D94 RID: 7572
		public abstract int CalculateGoldLossAfterDefeat(Hero partyLeaderHero);

		// Token: 0x06001D95 RID: 7573
		public abstract EquipmentElement GetLootedItemFromTroop(CharacterObject character, float targetValue);

		// Token: 0x06001D96 RID: 7574
		public abstract float GetExpectedLootedItemValueFromCasualty(Hero winnerPartyLeaderHero, CharacterObject casualtyCharacter);

		// Token: 0x06001D97 RID: 7575
		public abstract int CalculatePlunderedGoldAmountFromDefeatedParty(PartyBase defeatedParty);

		// Token: 0x06001D98 RID: 7576
		public abstract MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootGoldChances(MBReadOnlyList<MapEventParty> winnerParties);

		// Token: 0x06001D99 RID: 7577
		public abstract float GetMainPartyMemberScatterChance();

		// Token: 0x06001D9A RID: 7578
		public abstract float GetAITradePenalty();

		// Token: 0x06001D9B RID: 7579
		public abstract void GetCaptureMemberChancesForWinnerParties(MapEvent endedMapEvent, MBReadOnlyList<MapEventParty> winnerParties, out MBList<KeyValuePair<MapEventParty, float>> woundedMemberChances, out MBList<KeyValuePair<MapEventParty, float>> healthyMemberChances);

		// Token: 0x06001D9C RID: 7580
		public abstract MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootPrisonerChances(MBReadOnlyList<MapEventParty> winnerParties, TroopRosterElement prisonerElement);

		// Token: 0x06001D9D RID: 7581
		public abstract MBList<KeyValuePair<MapEventParty, float>> GetLootItemChancesForWinnerParties(MBReadOnlyList<MapEventParty> winnerParties, PartyBase defeatedParty);

		// Token: 0x06001D9E RID: 7582
		public abstract MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootCasualtyChances(MBReadOnlyList<MapEventParty> winnerParties, PartyBase defeatedParty);

		// Token: 0x06001D9F RID: 7583
		public abstract float CalculateShipDamageAfterDefeat(Ship ship);

		// Token: 0x06001DA0 RID: 7584
		public abstract MBReadOnlyList<KeyValuePair<Ship, MapEventParty>> DistributeDefeatedPartyShipsAmongWinners(MapEvent mapEvent, MBReadOnlyList<Ship> shipsToLoot, MBReadOnlyList<MapEventParty> winnerParties);

		// Token: 0x06001DA1 RID: 7585
		public abstract float GetSunkenShipMoraleEffect(PartyBase shipOwner, Ship ship);

		// Token: 0x06001DA2 RID: 7586
		public abstract float GetShipSiegeEngineHitMoraleEffect(Ship ship, SiegeEngineType siegeEngineType);

		// Token: 0x06001DA3 RID: 7587
		public abstract Figurehead GetFigureheadLoot(MBReadOnlyList<MapEventParty> defeatedParties, PartyBase defeatedSideLeaderParty);

		// Token: 0x06001DA4 RID: 7588
		public abstract MBReadOnlyList<MapEventParty> GetWinnerPartiesThatCanPlunderGoldFromShips(MBReadOnlyList<MapEventParty> winnerParties);

		// Token: 0x06001DA5 RID: 7589
		public abstract bool CanTroopBeTakenPrisoner(CharacterObject troop);
	}
}
