using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BC RID: 444
	public abstract class PartyShipLimitModel : MBGameModel<PartyShipLimitModel>
	{
		// Token: 0x06001DC9 RID: 7625
		public abstract int GetIdealShipNumber(MobileParty mobileParty);

		// Token: 0x06001DCA RID: 7626
		public abstract int GetIdealShipNumber(Clan clan);

		// Token: 0x06001DCB RID: 7627
		public abstract float GetShipPriority(MobileParty mobileParty, Ship ship, bool isSelling);
	}
}
