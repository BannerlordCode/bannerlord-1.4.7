using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F3 RID: 243
	public class DefaultBannerItemModel : BannerItemModel
	{
		// Token: 0x0600165F RID: 5727 RVA: 0x000671D3 File Offset: 0x000653D3
		public override IEnumerable<ItemObject> GetPossibleRewardBannerItems()
		{
			return Items.All.WhereQ<ItemObject>((ItemObject i) => i.IsBannerItem && i.StringId != "campaign_banner_small");
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00067200 File Offset: 0x00065400
		public override IEnumerable<ItemObject> GetPossibleRewardBannerItemsForHero(Hero hero)
		{
			IEnumerable<ItemObject> possibleRewardBannerItems = this.GetPossibleRewardBannerItems();
			int bannerItemLevelForHero = this.GetBannerItemLevelForHero(hero);
			List<ItemObject> list = new List<ItemObject>();
			foreach (ItemObject itemObject in possibleRewardBannerItems)
			{
				if ((itemObject.Culture == null || itemObject.Culture == hero.Culture) && (itemObject.ItemComponent as BannerComponent).BannerLevel == bannerItemLevelForHero)
				{
					list.Add(itemObject);
				}
			}
			return list;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00067288 File Offset: 0x00065488
		public override int GetBannerItemLevelForHero(Hero hero)
		{
			if (hero.Clan == null || hero.Clan.Leader != hero)
			{
				return 1;
			}
			if (hero.MapFaction.IsKingdomFaction && hero.Clan.Kingdom.RulingClan == hero.Clan)
			{
				return 3;
			}
			return 2;
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x000672D5 File Offset: 0x000654D5
		public override bool CanBannerBeUpdated(ItemObject item)
		{
			return true;
		}

		// Token: 0x04000780 RID: 1920
		public const int BannerLevel1 = 1;

		// Token: 0x04000781 RID: 1921
		public const int BannerLevel2 = 2;

		// Token: 0x04000782 RID: 1922
		public const int BannerLevel3 = 3;

		// Token: 0x04000783 RID: 1923
		private const string MapBannerId = "campaign_banner_small";
	}
}
