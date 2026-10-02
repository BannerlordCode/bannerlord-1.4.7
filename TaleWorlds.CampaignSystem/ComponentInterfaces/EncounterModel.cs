using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000188 RID: 392
	public abstract class EncounterModel : MBGameModel<EncounterModel>
	{
		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06001BEB RID: 7147
		public abstract float NeededMaximumLandDistanceForEncounteringMobileParty { get; }

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001BEC RID: 7148
		public abstract float NeededMaximumNavalDistanceForEncounteringMobileParty { get; }

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06001BED RID: 7149
		public abstract float MaximumAllowedLandDistanceForEncounteringMobilePartyInArmy { get; }

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06001BEE RID: 7150
		public abstract float MaximumAllowedNavalDistanceForEncounteringMobilePartyInArmy { get; }

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06001BEF RID: 7151
		public abstract float NeededMaximumDistanceForEncounteringTown { get; }

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06001BF0 RID: 7152
		public abstract float NeededMaximumDistanceForEncounteringBlockade { get; }

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001BF1 RID: 7153
		public abstract float NeededMaximumDistanceForEncounteringVillage { get; }

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001BF2 RID: 7154
		public abstract float GetEncounterJoiningRadius { get; }

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06001BF3 RID: 7155
		public abstract float GetSettlementBeingNearFieldBattleRadius { get; }

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001BF4 RID: 7156
		public abstract float PlayerParleyDistance { get; }

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001BF5 RID: 7157
		public abstract int MinimumNumberOfMenForAttackingVillageViaScene { get; }

		// Token: 0x06001BF6 RID: 7158
		public abstract bool IsEncounterExemptFromHostileActions(PartyBase side1, PartyBase side2);

		// Token: 0x06001BF7 RID: 7159
		public abstract bool CanMainHeroDoParleyWithParty(PartyBase partyBase, out TextObject explanation);

		// Token: 0x06001BF8 RID: 7160
		public abstract Hero GetLeaderOfSiegeEvent(SiegeEvent siegeEvent, BattleSideEnum side);

		// Token: 0x06001BF9 RID: 7161
		public abstract Hero GetLeaderOfMapEvent(MapEvent mapEvent, BattleSideEnum side);

		// Token: 0x06001BFA RID: 7162
		public abstract int GetCharacterSergeantScore(Hero hero);

		// Token: 0x06001BFB RID: 7163
		public abstract IEnumerable<PartyBase> GetDefenderPartiesOfSettlement(Settlement settlement, MapEvent.BattleTypes mapEventType);

		// Token: 0x06001BFC RID: 7164
		public abstract PartyBase GetNextDefenderPartyOfSettlement(Settlement settlement, ref int partyIndex, MapEvent.BattleTypes mapEventType);

		// Token: 0x06001BFD RID: 7165
		public abstract MapEventComponent CreateMapEventComponentForEncounter(PartyBase attackerParty, PartyBase defenderParty, MapEvent.BattleTypes battleType);

		// Token: 0x06001BFE RID: 7166
		public abstract ExplainedNumber GetBribeChance(MobileParty defenderParty, MobileParty attackerParty);

		// Token: 0x06001BFF RID: 7167
		public abstract float GetSurrenderChance(MobileParty defenderParty, MobileParty attackerParty);

		// Token: 0x06001C00 RID: 7168
		public abstract float GetMapEventSideRunAwayChance(MapEventSide mapEventside);

		// Token: 0x06001C01 RID: 7169
		public abstract void FindNonAttachedNpcPartiesWhoWillJoinPlayerEncounter(List<MobileParty> partiesToJoinPlayerSide, List<MobileParty> partiesToJoinEnemySide);

		// Token: 0x06001C02 RID: 7170
		public abstract bool CanPlayerForceBanditsToJoin(out TextObject explanation);

		// Token: 0x06001C03 RID: 7171
		public abstract bool IsPartyUnderPlayerCommand(PartyBase party);

		// Token: 0x06001C04 RID: 7172
		public abstract MBReadOnlyList<MobileParty> GetPartiesToTeleportOnMapEventFinalize(MapEvent mapEvent);
	}
}
