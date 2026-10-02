using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018B RID: 395
	public abstract class ItemDiscardModel : MBGameModel<ItemDiscardModel>
	{
		// Token: 0x06001C20 RID: 7200
		public abstract int GetXpBonusForDiscardingItems(ItemRoster itemRoster);

		// Token: 0x06001C21 RID: 7201
		public abstract int GetXpBonusForDiscardingItem(ItemObject item, int amount = 1);

		// Token: 0x06001C22 RID: 7202
		public abstract bool PlayerCanDonateItem(ItemObject item);
	}
}
