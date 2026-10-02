using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AC RID: 428
	public abstract class PartyImpairmentModel : MBGameModel<PartyImpairmentModel>
	{
		// Token: 0x06001D30 RID: 7472
		public abstract ExplainedNumber GetDisorganizedStateDuration(MobileParty party);

		// Token: 0x06001D31 RID: 7473
		public abstract float GetVulnerabilityStateDuration(PartyBase party);

		// Token: 0x06001D32 RID: 7474
		public abstract float GetSiegeExpectedVulnerabilityTime();

		// Token: 0x06001D33 RID: 7475
		public abstract bool CanGetDisorganized(PartyBase partyBase);
	}
}
