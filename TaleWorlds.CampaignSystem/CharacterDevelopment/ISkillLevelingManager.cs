using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003AC RID: 940
	public interface ISkillLevelingManager
	{
		// Token: 0x06003687 RID: 13959
		void OnCombatHit(CharacterObject affectorCharacter, CharacterObject affectedCharacter, CharacterObject captain, Hero commander, float speedBonusFromMovement, float shotDifficulty, WeaponComponentData affectorWeapon, float hitPointRatio, CombatXpModel.MissionTypeEnum missionType, bool isAffectorMounted, bool isTeamKill, bool isAffectorUnderCommand, float damageAmount, bool isFatal, bool isSiegeEngineHit, bool isHorseCharge, bool isSneakAttack);

		// Token: 0x06003688 RID: 13960
		void OnSiegeEngineDestroyed(MobileParty party, SiegeEngineType destroyedSiegeEngine);

		// Token: 0x06003689 RID: 13961
		void OnSimulationCombatKill(CharacterObject affectorCharacter, CharacterObject affectedCharacter, PartyBase affectorParty, PartyBase commanderParty);

		// Token: 0x0600368A RID: 13962
		void OnTradeProfitMade(PartyBase party, int tradeProfit);

		// Token: 0x0600368B RID: 13963
		void OnTradeProfitMade(Hero hero, int tradeProfit);

		// Token: 0x0600368C RID: 13964
		void OnSettlementProjectFinished(Settlement settlement);

		// Token: 0x0600368D RID: 13965
		void OnSettlementGoverned(Hero governor, Settlement settlement);

		// Token: 0x0600368E RID: 13966
		void OnInfluenceSpent(Hero hero, float amountSpent);

		// Token: 0x0600368F RID: 13967
		void OnGainRelation(Hero hero, Hero gainedRelationWith, float relationChange, ChangeRelationAction.ChangeRelationDetail detail = ChangeRelationAction.ChangeRelationDetail.Default);

		// Token: 0x06003690 RID: 13968
		void OnTroopRecruited(Hero hero, int amount, int tier);

		// Token: 0x06003691 RID: 13969
		void OnBribeGiven(int amount);

		// Token: 0x06003692 RID: 13970
		void OnWarehouseProduction(EquipmentElement production);

		// Token: 0x06003693 RID: 13971
		void OnAIPartyLootCasualties(int goldAmount, Hero winnerPartyLeader, PartyBase defeatedParty);

		// Token: 0x06003694 RID: 13972
		void OnBanditsRecruited(MobileParty mobileParty, CharacterObject bandit, int count);

		// Token: 0x06003695 RID: 13973
		void OnMainHeroReleasedFromCaptivity(float captivityTime);

		// Token: 0x06003696 RID: 13974
		void OnMainHeroTortured();

		// Token: 0x06003697 RID: 13975
		void OnMainHeroDisguised(bool isNotCaught);

		// Token: 0x06003698 RID: 13976
		void OnRaid(MobileParty attackerParty, ItemRoster lootedItems);

		// Token: 0x06003699 RID: 13977
		void OnLoot(MobileParty attackerParty, MobileParty forcedParty, ItemRoster lootedItems, bool attacked);

		// Token: 0x0600369A RID: 13978
		void OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster);

		// Token: 0x0600369B RID: 13979
		void OnSurgeryApplied(MobileParty party, bool surgerySuccess, int troopTier);

		// Token: 0x0600369C RID: 13980
		void OnTacticsUsed(MobileParty party, float xp);

		// Token: 0x0600369D RID: 13981
		void OnHideoutSpotted(MobileParty party, PartyBase spottedParty);

		// Token: 0x0600369E RID: 13982
		void OnTrackDetected(Track track);

		// Token: 0x0600369F RID: 13983
		void OnTravelOnFoot(Hero hero, float speed);

		// Token: 0x060036A0 RID: 13984
		void OnTravelOnHorse(Hero hero, float speed);

		// Token: 0x060036A1 RID: 13985
		void OnTravelOnWater(MobileParty party, float speed);

		// Token: 0x060036A2 RID: 13986
		void OnHeroHealedWhileWaiting(Hero hero, int healingAmount);

		// Token: 0x060036A3 RID: 13987
		void OnRegularTroopHealedWhileWaiting(MobileParty mobileParty, int healedTroopCount, float averageTier);

		// Token: 0x060036A4 RID: 13988
		void OnLeadingArmy(MobileParty mobileParty);

		// Token: 0x060036A5 RID: 13989
		void OnSieging(MobileParty mobileParty);

		// Token: 0x060036A6 RID: 13990
		void OnSiegeEngineBuilt(MobileParty mobileParty, SiegeEngineType siegeEngine);

		// Token: 0x060036A7 RID: 13991
		void OnUpgradeTroops(PartyBase party, CharacterObject troop, CharacterObject upgrade, int numberOfTroops);

		// Token: 0x060036A8 RID: 13992
		void OnPersuasionSucceeded(Hero targetHero, SkillObject skill, PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient);

		// Token: 0x060036A9 RID: 13993
		void OnPrisonBreakEnd(Hero prisonerHero, bool isSucceeded);

		// Token: 0x060036AA RID: 13994
		void OnWallBreached(MobileParty party);

		// Token: 0x060036AB RID: 13995
		void OnForceVolunteers(MobileParty attackerParty, PartyBase forcedParty);

		// Token: 0x060036AC RID: 13996
		void OnForceSupplies(MobileParty attackerParty, ItemRoster lootedItems, bool attacked);

		// Token: 0x060036AD RID: 13997
		void OnAIPartiesTravel(Hero hero, bool isCaravanParty, TerrainType currentTerrainType);

		// Token: 0x060036AE RID: 13998
		void OnTraverseTerrain(MobileParty mobileParty, TerrainType currentTerrainType);

		// Token: 0x060036AF RID: 13999
		void OnBattleEnded(PartyBase party, CharacterObject troop, int excessXp);

		// Token: 0x060036B0 RID: 14000
		void OnFoodConsumed(MobileParty mobileParty, bool wasStarving);

		// Token: 0x060036B1 RID: 14001
		void OnAlleyCleared(Alley alley);

		// Token: 0x060036B2 RID: 14002
		void OnDailyAlleyTick(Alley alley, Hero alleyLeader);

		// Token: 0x060036B3 RID: 14003
		void OnBoardGameWonAgainstLord(Hero lord, BoardGameHelper.AIDifficulty difficulty, bool extraXpGain);

		// Token: 0x060036B4 RID: 14004
		void OnShipDamaged(Ship ship, float rawDamage, float finalDamage);

		// Token: 0x060036B5 RID: 14005
		void OnShipRepaired(Ship ship, float repairedHitPoints);

		// Token: 0x060036B6 RID: 14006
		void OnHideoutMissionEnd(bool isSucceeded);

		// Token: 0x060036B7 RID: 14007
		void OnHideoutClearedAsGhost();
	}
}
