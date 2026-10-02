using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000D5 RID: 213
	public interface IPlayerTradeBehavior
	{
		// Token: 0x0600147C RID: 5244
		int GetProjectedProfit(ItemRosterElement itemRosterElement, int itemCost);
	}
}
