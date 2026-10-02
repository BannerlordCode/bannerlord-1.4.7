using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000195 RID: 405
	public abstract class PartyTradeModel : MBGameModel<PartyTradeModel>
	{
		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001C5B RID: 7259
		public abstract int CaravanTransactionHighestValueItemCount { get; }

		// Token: 0x06001C5C RID: 7260
		public abstract float GetTradePenaltyFactor(MobileParty party);
	}
}
