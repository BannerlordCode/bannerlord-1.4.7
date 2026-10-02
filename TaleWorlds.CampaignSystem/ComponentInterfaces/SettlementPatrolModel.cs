using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000187 RID: 391
	public abstract class SettlementPatrolModel : MBGameModel<SettlementPatrolModel>
	{
		// Token: 0x06001BE7 RID: 7143
		public abstract CampaignTime GetPatrolPartySpawnDuration(Settlement settlement, bool naval);

		// Token: 0x06001BE8 RID: 7144
		public abstract bool CanSettlementHavePatrolParties(Settlement settlement, bool naval);

		// Token: 0x06001BE9 RID: 7145
		public abstract PartyTemplateObject GetPartyTemplateForPatrolParty(Settlement settlement, bool naval);
	}
}
