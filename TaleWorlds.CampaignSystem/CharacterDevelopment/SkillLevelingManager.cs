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
	// Token: 0x020003AD RID: 941
	public static class SkillLevelingManager
	{
		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x060036B8 RID: 14008 RVA: 0x000E4721 File Offset: 0x000E2921
		private static ISkillLevelingManager Instance
		{
			get
			{
				return Campaign.Current.SkillLevelingManager;
			}
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x000E4730 File Offset: 0x000E2930
		public static void OnCombatHit(CharacterObject affectorCharacter, CharacterObject affectedCharacter, CharacterObject captain, Hero commander, float speedBonusFromMovement, float shotDifficulty, WeaponComponentData affectorWeapon, float hitPointRatio, CombatXpModel.MissionTypeEnum missionType, bool isAffectorMounted, bool isTeamKill, bool isAffectorUnderCommand, float damageAmount, bool isFatal, bool isSiegeEngineHit, bool isHorseCharge, bool isSneakAttack)
		{
			SkillLevelingManager.Instance.OnCombatHit(affectorCharacter, affectedCharacter, captain, commander, speedBonusFromMovement, shotDifficulty, affectorWeapon, hitPointRatio, missionType, isAffectorMounted, isTeamKill, isAffectorUnderCommand, damageAmount, isFatal, isSiegeEngineHit, isHorseCharge, isSneakAttack);
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x000E4765 File Offset: 0x000E2965
		public static void OnSiegeEngineDestroyed(MobileParty party, SiegeEngineType destroyedSiegeEngine)
		{
			SkillLevelingManager.Instance.OnSiegeEngineDestroyed(party, destroyedSiegeEngine);
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x000E4773 File Offset: 0x000E2973
		public static void OnWallBreached(MobileParty party)
		{
			SkillLevelingManager.Instance.OnWallBreached(party);
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x000E4780 File Offset: 0x000E2980
		public static void OnSimulationCombatKill(CharacterObject affectorCharacter, CharacterObject affectedCharacter, PartyBase affectorParty, PartyBase commanderParty)
		{
			SkillLevelingManager.Instance.OnSimulationCombatKill(affectorCharacter, affectedCharacter, affectorParty, commanderParty);
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x000E4790 File Offset: 0x000E2990
		public static void OnTradeProfitMade(PartyBase party, int tradeProfit)
		{
			SkillLevelingManager.Instance.OnTradeProfitMade(party, tradeProfit);
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x000E479E File Offset: 0x000E299E
		public static void OnTradeProfitMade(Hero hero, int tradeProfit)
		{
			SkillLevelingManager.Instance.OnTradeProfitMade(hero, tradeProfit);
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x000E47AC File Offset: 0x000E29AC
		public static void OnSettlementProjectFinished(Settlement settlement)
		{
			SkillLevelingManager.Instance.OnSettlementProjectFinished(settlement);
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x000E47B9 File Offset: 0x000E29B9
		public static void OnSettlementGoverned(Hero governor, Settlement settlement)
		{
			SkillLevelingManager.Instance.OnSettlementGoverned(governor, settlement);
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x000E47C7 File Offset: 0x000E29C7
		public static void OnInfluenceSpent(Hero hero, float amountSpent)
		{
			SkillLevelingManager.Instance.OnInfluenceSpent(hero, amountSpent);
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x000E47D5 File Offset: 0x000E29D5
		public static void OnGainRelation(Hero hero, Hero gainedRelationWith, float relationChange, ChangeRelationAction.ChangeRelationDetail detail = ChangeRelationAction.ChangeRelationDetail.Default)
		{
			SkillLevelingManager.Instance.OnGainRelation(hero, gainedRelationWith, relationChange, detail);
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x000E47E5 File Offset: 0x000E29E5
		public static void OnTroopRecruited(Hero hero, int amount, int tier)
		{
			SkillLevelingManager.Instance.OnTroopRecruited(hero, amount, tier);
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000E47F4 File Offset: 0x000E29F4
		public static void OnBribeGiven(int amount)
		{
			SkillLevelingManager.Instance.OnBribeGiven(amount);
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x000E4801 File Offset: 0x000E2A01
		public static void OnBanditsRecruited(MobileParty mobileParty, CharacterObject bandit, int count)
		{
			SkillLevelingManager.Instance.OnBanditsRecruited(mobileParty, bandit, count);
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x000E4810 File Offset: 0x000E2A10
		public static void OnMainHeroReleasedFromCaptivity(float captivityTime)
		{
			SkillLevelingManager.Instance.OnMainHeroReleasedFromCaptivity(captivityTime);
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x000E481D File Offset: 0x000E2A1D
		public static void OnMainHeroTortured()
		{
			SkillLevelingManager.Instance.OnMainHeroTortured();
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x000E4829 File Offset: 0x000E2A29
		public static void OnMainHeroDisguised(bool isNotCaught)
		{
			SkillLevelingManager.Instance.OnMainHeroDisguised(isNotCaught);
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x000E4836 File Offset: 0x000E2A36
		public static void OnRaid(MobileParty attackerParty, ItemRoster lootedItems)
		{
			SkillLevelingManager.Instance.OnRaid(attackerParty, lootedItems);
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x000E4844 File Offset: 0x000E2A44
		public static void OnLoot(MobileParty attackerParty, MobileParty forcedParty, ItemRoster lootedItems, bool attacked)
		{
			SkillLevelingManager.Instance.OnLoot(attackerParty, forcedParty, lootedItems, attacked);
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x000E4854 File Offset: 0x000E2A54
		public static void OnForceVolunteers(MobileParty attackerParty, PartyBase forcedParty)
		{
			SkillLevelingManager.Instance.OnForceVolunteers(attackerParty, forcedParty);
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x000E4862 File Offset: 0x000E2A62
		public static void OnForceSupplies(MobileParty attackerParty, ItemRoster lootedItems, bool attacked)
		{
			SkillLevelingManager.Instance.OnForceSupplies(attackerParty, lootedItems, attacked);
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x000E4871 File Offset: 0x000E2A71
		public static void OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster)
		{
			SkillLevelingManager.Instance.OnPrisonerSell(mobileParty, in prisonerRoster);
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x000E487F File Offset: 0x000E2A7F
		public static void OnSurgeryApplied(MobileParty party, bool surgerySuccess, int troopTier)
		{
			SkillLevelingManager.Instance.OnSurgeryApplied(party, surgerySuccess, troopTier);
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x000E488E File Offset: 0x000E2A8E
		public static void OnTacticsUsed(MobileParty party, float xp)
		{
			SkillLevelingManager.Instance.OnTacticsUsed(party, xp);
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x000E489C File Offset: 0x000E2A9C
		public static void OnHideoutSpotted(MobileParty party, PartyBase spottedParty)
		{
			SkillLevelingManager.Instance.OnHideoutSpotted(party, spottedParty);
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x000E48AA File Offset: 0x000E2AAA
		public static void OnTrackDetected(Track track)
		{
			SkillLevelingManager.Instance.OnTrackDetected(track);
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x000E48B7 File Offset: 0x000E2AB7
		public static void OnTravelOnFoot(Hero hero, float speed)
		{
			SkillLevelingManager.Instance.OnTravelOnFoot(hero, speed);
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x000E48C5 File Offset: 0x000E2AC5
		public static void OnTravelOnHorse(Hero hero, float speed)
		{
			SkillLevelingManager.Instance.OnTravelOnHorse(hero, speed);
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x000E48D3 File Offset: 0x000E2AD3
		public static void OnTravelOnWater(MobileParty party, float speed)
		{
			SkillLevelingManager.Instance.OnTravelOnWater(party, speed);
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x000E48E1 File Offset: 0x000E2AE1
		public static void OnAIPartiesTravel(Hero hero, bool isCaravanParty, TerrainType currentTerrainType)
		{
			SkillLevelingManager.Instance.OnAIPartiesTravel(hero, isCaravanParty, currentTerrainType);
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x000E48F0 File Offset: 0x000E2AF0
		public static void OnTraverseTerrain(MobileParty mobileParty, TerrainType currentTerrainType)
		{
			SkillLevelingManager.Instance.OnTraverseTerrain(mobileParty, currentTerrainType);
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x000E48FE File Offset: 0x000E2AFE
		public static void OnBattleEnded(PartyBase party, CharacterObject troop, int excessXp)
		{
			SkillLevelingManager.Instance.OnBattleEnded(party, troop, excessXp);
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x000E490D File Offset: 0x000E2B0D
		public static void OnHeroHealedWhileWaiting(Hero hero, int healingAmount)
		{
			SkillLevelingManager.Instance.OnHeroHealedWhileWaiting(hero, healingAmount);
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x000E491B File Offset: 0x000E2B1B
		public static void OnRegularTroopHealedWhileWaiting(MobileParty mobileParty, int healedTroopCount, float averageTier)
		{
			SkillLevelingManager.Instance.OnRegularTroopHealedWhileWaiting(mobileParty, healedTroopCount, averageTier);
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x000E492A File Offset: 0x000E2B2A
		public static void OnLeadingArmy(MobileParty mobileParty)
		{
			SkillLevelingManager.Instance.OnLeadingArmy(mobileParty);
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x000E4937 File Offset: 0x000E2B37
		public static void OnSieging(MobileParty mobileParty)
		{
			SkillLevelingManager.Instance.OnSieging(mobileParty);
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x000E4944 File Offset: 0x000E2B44
		public static void OnSiegeEngineBuilt(MobileParty mobileParty, SiegeEngineType siegeEngine)
		{
			SkillLevelingManager.Instance.OnSiegeEngineBuilt(mobileParty, siegeEngine);
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x000E4952 File Offset: 0x000E2B52
		public static void OnUpgradeTroops(PartyBase party, CharacterObject troop, CharacterObject upgrade, int numberOfTroops)
		{
			SkillLevelingManager.Instance.OnUpgradeTroops(party, troop, upgrade, numberOfTroops);
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x000E4962 File Offset: 0x000E2B62
		public static void OnPersuasionSucceeded(Hero targetHero, SkillObject skill, PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient)
		{
			SkillLevelingManager.Instance.OnPersuasionSucceeded(targetHero, skill, difficulty, argumentDifficultyBonusCoefficient);
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x000E4972 File Offset: 0x000E2B72
		public static void OnPrisonBreakEnd(Hero prisonerHero, bool isSucceeded)
		{
			SkillLevelingManager.Instance.OnPrisonBreakEnd(prisonerHero, isSucceeded);
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x000E4980 File Offset: 0x000E2B80
		public static void OnFoodConsumed(MobileParty mobileParty, bool wasStarving)
		{
			SkillLevelingManager.Instance.OnFoodConsumed(mobileParty, wasStarving);
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x000E498E File Offset: 0x000E2B8E
		public static void OnAlleyCleared(Alley alley)
		{
			SkillLevelingManager.Instance.OnAlleyCleared(alley);
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x000E499B File Offset: 0x000E2B9B
		public static void OnDailyAlleyTick(Alley alley, Hero alleyLeader)
		{
			SkillLevelingManager.Instance.OnDailyAlleyTick(alley, alleyLeader);
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x000E49A9 File Offset: 0x000E2BA9
		public static void OnBoardGameWonAgainstLord(Hero lord, BoardGameHelper.AIDifficulty difficulty, bool extraXpGain)
		{
			SkillLevelingManager.Instance.OnBoardGameWonAgainstLord(lord, difficulty, extraXpGain);
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x000E49B8 File Offset: 0x000E2BB8
		public static void OnProductionProducedToWarehouse(EquipmentElement production)
		{
			SkillLevelingManager.Instance.OnWarehouseProduction(production);
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x000E49C5 File Offset: 0x000E2BC5
		public static void OnAIPartyLootCasualties(int goldAmount, Hero winnerPartyLeader, PartyBase defeatedParty)
		{
			SkillLevelingManager.Instance.OnAIPartyLootCasualties(goldAmount, winnerPartyLeader, defeatedParty);
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x000E49D4 File Offset: 0x000E2BD4
		public static void OnShipDamaged(Ship ship, float rawDamage, float finalDamage)
		{
			SkillLevelingManager.Instance.OnShipDamaged(ship, rawDamage, finalDamage);
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x000E49E3 File Offset: 0x000E2BE3
		public static void OnShipRepaired(Ship ship, float repairedHitPoints)
		{
			SkillLevelingManager.Instance.OnShipRepaired(ship, repairedHitPoints);
		}
	}
}
