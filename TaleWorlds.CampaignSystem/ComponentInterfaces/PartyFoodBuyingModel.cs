using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AB RID: 427
	public abstract class PartyFoodBuyingModel : MBGameModel<PartyFoodBuyingModel>
	{
		// Token: 0x06001D2B RID: 7467
		public abstract void FindItemToBuy(MobileParty mobileParty, Settlement settlement, out ItemRosterElement itemRosterElement, out float itemElementsPrice);

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06001D2C RID: 7468
		public abstract float MinimumDaysFoodToLastWhileBuyingFoodFromTown { get; }

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06001D2D RID: 7469
		public abstract float MinimumDaysFoodToLastWhileBuyingFoodFromVillage { get; }

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06001D2E RID: 7470
		public abstract float LowCostFoodPriceAverage { get; }
	}
}
