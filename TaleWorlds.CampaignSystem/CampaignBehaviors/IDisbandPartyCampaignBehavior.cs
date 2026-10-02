using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003FD RID: 1021
	public interface IDisbandPartyCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x0600405E RID: 16478
		bool IsPartyWaitingForDisband(MobileParty party);
	}
}
