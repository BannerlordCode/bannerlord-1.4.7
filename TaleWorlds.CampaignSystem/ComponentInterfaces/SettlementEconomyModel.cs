using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C5 RID: 453
	public abstract class SettlementEconomyModel : MBGameModel<SettlementEconomyModel>
	{
		// Token: 0x06001DFD RID: 7677
		public abstract float GetEstimatedDemandForCategory(Town town, ItemData itemData, ItemCategory category);

		// Token: 0x06001DFE RID: 7678
		public abstract float GetDailyDemandForCategory(Town town, ItemCategory category, int extraProsperity = 0);

		// Token: 0x06001DFF RID: 7679
		public abstract float GetDemandChangeFromValue(float purchaseValue);

		// Token: 0x06001E00 RID: 7680
		public abstract ValueTuple<float, float> GetSupplyDemandForCategory(Town town, ItemCategory category, float dailySupply, float dailyDemand, float oldSupply, float oldDemand);

		// Token: 0x06001E01 RID: 7681
		public abstract int GetTownGoldChange(Town town);

		// Token: 0x06001E02 RID: 7682
		public abstract float CalculateDailySettlementBudgetForItemCategory(Town town, float demand, ItemCategory category);
	}
}
