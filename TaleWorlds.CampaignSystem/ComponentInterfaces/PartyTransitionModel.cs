using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A6 RID: 422
	public abstract class PartyTransitionModel : MBGameModel<PartyTransitionModel>
	{
		// Token: 0x06001CCD RID: 7373
		public abstract CampaignTime GetTransitionTimeForEmbarking(MobileParty mobileParty);

		// Token: 0x06001CCE RID: 7374
		public abstract CampaignTime GetTransitionTimeDisembarking(MobileParty mobileParty);

		// Token: 0x06001CCF RID: 7375
		public abstract CampaignTime GetFleetTravelTimeToSettlement(MobileParty mobileParty, Settlement targetSettlement);
	}
}
