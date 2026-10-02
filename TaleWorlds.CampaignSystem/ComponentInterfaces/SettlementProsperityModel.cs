using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CC RID: 460
	public abstract class SettlementProsperityModel : MBGameModel<SettlementProsperityModel>
	{
		// Token: 0x06001E49 RID: 7753
		public abstract ExplainedNumber CalculateProsperityChange(Town fortification, bool includeDescriptions = false);

		// Token: 0x06001E4A RID: 7754
		public abstract ExplainedNumber CalculateHearthChange(Village village, bool includeDescriptions = false);
	}
}
