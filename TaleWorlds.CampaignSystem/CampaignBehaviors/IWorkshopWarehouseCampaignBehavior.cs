using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000412 RID: 1042
	public interface IWorkshopWarehouseCampaignBehavior
	{
		// Token: 0x0600417F RID: 16767
		bool IsGettingInputsFromWarehouse(Workshop workshop);

		// Token: 0x06004180 RID: 16768
		void SetIsGettingInputsFromWarehouse(Workshop workshop, bool isActive);

		// Token: 0x06004181 RID: 16769
		float GetStockProductionInWarehouseRatio(Workshop workshop);

		// Token: 0x06004182 RID: 16770
		void SetStockProductionInWarehouseRatio(Workshop workshop, float percentage);

		// Token: 0x06004183 RID: 16771
		float GetWarehouseItemRosterWeight(Settlement settlement);

		// Token: 0x06004184 RID: 16772
		bool IsRawMaterialsSufficientInTownMarket(Workshop workshop);

		// Token: 0x06004185 RID: 16773
		int GetInputCount(Workshop workshop);

		// Token: 0x06004186 RID: 16774
		int GetOutputCount(Workshop workshop);

		// Token: 0x06004187 RID: 16775
		ExplainedNumber GetInputDailyChange(Workshop workshop);

		// Token: 0x06004188 RID: 16776
		ExplainedNumber GetOutputDailyChange(Workshop workshop);
	}
}
