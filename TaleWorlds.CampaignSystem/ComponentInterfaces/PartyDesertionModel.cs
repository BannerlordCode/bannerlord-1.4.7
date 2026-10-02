using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BF RID: 447
	public abstract class PartyDesertionModel : MBGameModel<PartyDesertionModel>
	{
		// Token: 0x06001DDD RID: 7645
		public abstract TroopRoster GetTroopsToDesert(MobileParty mobileParty);

		// Token: 0x06001DDE RID: 7646
		public abstract float GetDesertionChanceForTroop(MobileParty mobileParty, in TroopRosterElement troopRosterElement);

		// Token: 0x06001DDF RID: 7647
		public abstract int GetMoraleThresholdForTroopDesertion();
	}
}
