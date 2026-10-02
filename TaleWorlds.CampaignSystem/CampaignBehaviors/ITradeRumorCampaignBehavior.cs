using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000410 RID: 1040
	public interface ITradeRumorCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x0600417D RID: 16765
		IEnumerable<TradeRumor> TradeRumors { get; }
	}
}
