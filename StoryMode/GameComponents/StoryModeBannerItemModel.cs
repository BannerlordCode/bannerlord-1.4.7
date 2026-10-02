using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace StoryMode.GameComponents
{
	// Token: 0x0200003D RID: 61
	public class StoryModeBannerItemModel : BannerItemModel
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x00018BE3 File Offset: 0x00016DE3
		public override IEnumerable<ItemObject> GetPossibleRewardBannerItems()
		{
			if (!StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted)
			{
				return new List<ItemObject>();
			}
			return base.BaseModel.GetPossibleRewardBannerItems().WhereQ<ItemObject>((ItemObject i) => !this.IsItemDragonBanner(i));
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00018C1D File Offset: 0x00016E1D
		public override bool CanBannerBeUpdated(ItemObject item)
		{
			return !this.IsItemDragonBanner(item) && base.BaseModel.CanBannerBeUpdated(item);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00018C38 File Offset: 0x00016E38
		private bool IsItemDragonBanner(ItemObject item)
		{
			return item.StringId == "dragon_banner" || item.StringId == "dragon_banner_center" || item.StringId == "dragon_banner_dragonhead" || item.StringId == "dragon_banner_handle";
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00018C8D File Offset: 0x00016E8D
		public override IEnumerable<ItemObject> GetPossibleRewardBannerItemsForHero(Hero hero)
		{
			return base.BaseModel.GetPossibleRewardBannerItemsForHero(hero).WhereQ<ItemObject>((ItemObject b) => !this.IsItemDragonBanner(b));
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00018CAC File Offset: 0x00016EAC
		public override int GetBannerItemLevelForHero(Hero hero)
		{
			return base.BaseModel.GetBannerItemLevelForHero(hero);
		}
	}
}
