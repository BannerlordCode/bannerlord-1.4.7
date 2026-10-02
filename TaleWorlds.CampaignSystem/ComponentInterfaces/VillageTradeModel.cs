using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FB RID: 507
	public abstract class VillageTradeModel : MBGameModel<VillageTradeModel>
	{
		// Token: 0x06001F8A RID: 8074
		public abstract float TradeBoundDistanceLimitAsDays(MobileParty.NavigationType navigationType);

		// Token: 0x06001F8B RID: 8075
		public abstract Settlement GetTradeBoundToAssignForVillage(Village village);
	}
}
