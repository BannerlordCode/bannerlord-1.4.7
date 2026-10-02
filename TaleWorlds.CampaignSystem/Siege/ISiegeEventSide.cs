using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x020002DD RID: 733
	public interface ISiegeEventSide
	{
		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06002833 RID: 10291
		SiegeEvent SiegeEvent { get; }

		// Token: 0x06002834 RID: 10292
		IEnumerable<PartyBase> GetInvolvedPartiesForEventType(MapEvent.BattleTypes mapEventType = MapEvent.BattleTypes.Siege);

		// Token: 0x06002835 RID: 10293
		PartyBase GetNextInvolvedPartyForEventType(ref int partyIndex, MapEvent.BattleTypes mapEventType = MapEvent.BattleTypes.Siege);

		// Token: 0x06002836 RID: 10294
		bool HasInvolvedPartyForEventType(PartyBase party, MapEvent.BattleTypes mapEventType = MapEvent.BattleTypes.Siege);

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06002837 RID: 10295
		SiegeStrategy SiegeStrategy { get; }

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06002838 RID: 10296
		BattleSideEnum BattleSide { get; }

		// Token: 0x06002839 RID: 10297
		void OnTroopsKilledOnSide(int killCount);

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x0600283A RID: 10298
		int NumberOfTroopsKilledOnSide { get; }

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x0600283B RID: 10299
		SiegeEvent.SiegeEnginesContainer SiegeEngines { get; }

		// Token: 0x0600283C RID: 10300
		void AddSiegeEngineMissile(SiegeEvent.SiegeEngineMissile missile);

		// Token: 0x0600283D RID: 10301
		void RemoveDeprecatedMissiles();

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x0600283E RID: 10302
		MBReadOnlyList<SiegeEvent.SiegeEngineMissile> SiegeEngineMissiles { get; }

		// Token: 0x0600283F RID: 10303
		void SetSiegeStrategy(SiegeStrategy strategy);

		// Token: 0x06002840 RID: 10304
		void InitializeSiegeEventSide();

		// Token: 0x06002841 RID: 10305
		void GetAttackTarget(ISiegeEventSide siegeEventSide, SiegeEngineType siegeEngine, int siegeEngineSlot, out SiegeBombardTargets targetType, out int targetIndex);

		// Token: 0x06002842 RID: 10306
		void FinalizeSiegeEvent();
	}
}
