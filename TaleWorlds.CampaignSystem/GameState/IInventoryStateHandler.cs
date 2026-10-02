using System;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000396 RID: 918
	public interface IInventoryStateHandler
	{
		// Token: 0x0600351E RID: 13598
		void ExecuteLootingScript();

		// Token: 0x0600351F RID: 13599
		void ExecuteSellAllLoot();

		// Token: 0x06003520 RID: 13600
		void ExecuteBuyConsumableItem();
	}
}
