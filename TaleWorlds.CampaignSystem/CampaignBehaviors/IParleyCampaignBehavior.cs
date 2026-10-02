using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000408 RID: 1032
	public interface IParleyCampaignBehavior
	{
		// Token: 0x060040F2 RID: 16626
		PartyBase GetParleyedParty();

		// Token: 0x060040F3 RID: 16627
		void StartParley(PartyBase partyBase);
	}
}
