using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E5 RID: 485
	public abstract class SiegeEventModel : MBGameModel<SiegeEventModel>
	{
		// Token: 0x06001EE4 RID: 7908
		public abstract int GetSiegeEngineDestructionCasualties(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType destroyedSiegeEngine);

		// Token: 0x06001EE5 RID: 7909
		public abstract float GetCasualtyChance(MobileParty siegeParty, SiegeEvent siegeEvent, BattleSideEnum side);

		// Token: 0x06001EE6 RID: 7910
		public abstract int GetColleteralDamageCasualties(SiegeEngineType attackerSiegeEngine, MobileParty attackerParty);

		// Token: 0x06001EE7 RID: 7911
		public abstract float GetSiegeEngineHitChance(SiegeEngineType siegeEngineType, BattleSideEnum battleSide, SiegeBombardTargets target, Town town);

		// Token: 0x06001EE8 RID: 7912
		public abstract string GetSiegeEngineMapPrefabName(SiegeEngineType siegeEngineType, int wallLevel, BattleSideEnum side);

		// Token: 0x06001EE9 RID: 7913
		public abstract string GetSiegeEngineMapProjectilePrefabName(SiegeEngineType siegeEngineType);

		// Token: 0x06001EEA RID: 7914
		public abstract string GetSiegeEngineMapReloadAnimationName(SiegeEngineType siegeEngineType, BattleSideEnum side);

		// Token: 0x06001EEB RID: 7915
		public abstract string GetSiegeEngineMapFireAnimationName(SiegeEngineType siegeEngineType, BattleSideEnum side);

		// Token: 0x06001EEC RID: 7916
		public abstract sbyte GetSiegeEngineMapProjectileBoneIndex(SiegeEngineType siegeEngineType, BattleSideEnum side);

		// Token: 0x06001EED RID: 7917
		public abstract float GetSiegeStrategyScore(SiegeEvent siege, BattleSideEnum side, SiegeStrategy strategy);

		// Token: 0x06001EEE RID: 7918
		public abstract float GetConstructionProgressPerHour(SiegeEngineType type, SiegeEvent siegeEvent, ISiegeEventSide side);

		// Token: 0x06001EEF RID: 7919
		public abstract MobileParty GetEffectiveSiegePartyForSide(SiegeEvent siegeEvent, BattleSideEnum side);

		// Token: 0x06001EF0 RID: 7920
		public abstract float GetAvailableManDayPower(ISiegeEventSide side);

		// Token: 0x06001EF1 RID: 7921
		public abstract IEnumerable<SiegeEngineType> GetAvailableAttackerRangedSiegeEngines(PartyBase party);

		// Token: 0x06001EF2 RID: 7922
		public abstract IEnumerable<SiegeEngineType> GetAvailableDefenderSiegeEngines(PartyBase party);

		// Token: 0x06001EF3 RID: 7923
		public abstract IEnumerable<SiegeEngineType> GetAvailableAttackerRamSiegeEngines(PartyBase party);

		// Token: 0x06001EF4 RID: 7924
		public abstract IEnumerable<SiegeEngineType> GetAvailableAttackerTowerSiegeEngines(PartyBase party);

		// Token: 0x06001EF5 RID: 7925
		public abstract IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSettlement(Settlement settlement);

		// Token: 0x06001EF6 RID: 7926
		public abstract IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSiegeCamp(BesiegerCamp camp);

		// Token: 0x06001EF7 RID: 7927
		public abstract float GetSiegeEngineHitPoints(SiegeEvent siegeEvent, SiegeEngineType siegeEngine, BattleSideEnum battleSide);

		// Token: 0x06001EF8 RID: 7928
		public abstract int GetRangedSiegeEngineReloadTime(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine);

		// Token: 0x06001EF9 RID: 7929
		public abstract float GetSiegeEngineDamage(SiegeEvent siegeEvent, BattleSideEnum battleSide, SiegeEngineType siegeEngine, SiegeBombardTargets target);

		// Token: 0x06001EFA RID: 7930
		public abstract FlattenedTroopRoster GetPriorityTroopsForSallyOutAmbush();
	}
}
