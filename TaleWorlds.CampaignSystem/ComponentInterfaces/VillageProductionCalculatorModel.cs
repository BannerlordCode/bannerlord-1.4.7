using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AD RID: 429
	public abstract class VillageProductionCalculatorModel : MBGameModel<VillageProductionCalculatorModel>
	{
		// Token: 0x06001D35 RID: 7477
		public abstract float CalculateProductionSpeedOfItemCategory(ItemCategory item);

		// Token: 0x06001D36 RID: 7478
		public abstract ExplainedNumber CalculateDailyProductionAmount(Village village, ItemObject item);

		// Token: 0x06001D37 RID: 7479
		public abstract float CalculateDailyFoodProductionAmount(Village village);
	}
}
