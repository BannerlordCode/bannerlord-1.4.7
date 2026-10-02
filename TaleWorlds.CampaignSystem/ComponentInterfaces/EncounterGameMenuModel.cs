using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BA RID: 442
	public abstract class EncounterGameMenuModel : MBGameModel<EncounterGameMenuModel>
	{
		// Token: 0x06001DBF RID: 7615
		public abstract string GetEncounterMenu(PartyBase attackerParty, PartyBase defenderParty, out bool startBattle, out bool joinBattle);

		// Token: 0x06001DC0 RID: 7616
		public abstract string GetRaidCompleteMenu();

		// Token: 0x06001DC1 RID: 7617
		public abstract string GetNewPartyJoinMenu(MobileParty newParty);

		// Token: 0x06001DC2 RID: 7618
		public abstract string GetGenericStateMenu();

		// Token: 0x06001DC3 RID: 7619
		public abstract bool IsPlunderMenu(string menuId);
	}
}
