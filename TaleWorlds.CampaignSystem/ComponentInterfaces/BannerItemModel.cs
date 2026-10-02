using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F3 RID: 499
	public abstract class BannerItemModel : MBGameModel<BannerItemModel>
	{
		// Token: 0x06001F57 RID: 8023
		public abstract IEnumerable<ItemObject> GetPossibleRewardBannerItems();

		// Token: 0x06001F58 RID: 8024
		public abstract IEnumerable<ItemObject> GetPossibleRewardBannerItemsForHero(Hero hero);

		// Token: 0x06001F59 RID: 8025
		public abstract int GetBannerItemLevelForHero(Hero hero);

		// Token: 0x06001F5A RID: 8026
		public abstract bool CanBannerBeUpdated(ItemObject item);
	}
}
