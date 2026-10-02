using System;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019E RID: 414
	public abstract class CombatSimulationModel : MBGameModel<CombatSimulationModel>
	{
		// Token: 0x06001C8D RID: 7309
		public abstract ExplainedNumber SimulateHit(CharacterObject strikerTroop, CharacterObject struckTroop, PartyBase strikerParty, PartyBase struckParty, float strikerAdvantage, MapEvent battle, float strikerSideMorale, float struckSideMorale);

		// Token: 0x06001C8E RID: 7310
		public abstract ExplainedNumber SimulateHit(Ship strikerShip, Ship struckShip, PartyBase strikerParty, PartyBase struckParty, SiegeEngineType siegeEngine, float strikerAdvantage, MapEvent battle, out int troopCasualties);

		// Token: 0x06001C8F RID: 7311
		[return: TupleElementNames(new string[] { "defenderRounds", "attackerRounds" })]
		public abstract ValueTuple<int, int> GetSimulationTicksForBattleRound(MapEvent mapEvent);

		// Token: 0x06001C90 RID: 7312
		public abstract int GetNumberOfEquipmentsBuilt(Settlement settlement);

		// Token: 0x06001C91 RID: 7313
		public abstract float GetMaximumSiegeEquipmentProgress(Settlement settlement);

		// Token: 0x06001C92 RID: 7314
		public abstract float GetSettlementAdvantage(Settlement settlement);

		// Token: 0x06001C93 RID: 7315
		public abstract void GetBattleAdvantage(MapEvent mapEvent, out ExplainedNumber defenderAdvantage, out ExplainedNumber attackerAdvantage);

		// Token: 0x06001C94 RID: 7316
		public abstract float GetShipSiegeEngineHitChance(Ship ship, SiegeEngineType siegeEngineType, BattleSideEnum battleSide);

		// Token: 0x06001C95 RID: 7317
		public abstract int GetPursuitRoundCount(MapEvent mapEvent);

		// Token: 0x06001C96 RID: 7318
		public abstract float GetBluntDamageChance(CharacterObject strikerTroop, CharacterObject strikedTroop, PartyBase strikerParty, PartyBase strikedParty, MapEvent battle);

		// Token: 0x06001C97 RID: 7319
		public abstract CampaignTime GetSimulationTickInterval(MapEvent mapEvent);

		// Token: 0x06001C98 RID: 7320
		public abstract MBList<ValueTuple<Ship, MapEventParty>> GetSimulationShips(MapEvent mapEvent, MBList<MapEventParty> battleParties);

		// Token: 0x06001C99 RID: 7321
		public abstract int GetParticipatingTroopCount(MapEventSide side);
	}
}
