using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000163 RID: 355
	public class DefaultVassalRewardsModel : VassalRewardsModel
	{
		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x0008C001 File Offset: 0x0008A201
		public override int RelationRewardWithLeader
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001B08 RID: 6920 RVA: 0x0008C005 File Offset: 0x0008A205
		public override float InfluenceReward
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0008C00C File Offset: 0x0008A20C
		public override ItemRoster GetEquipmentRewardsForJoiningKingdom(Kingdom kingdom)
		{
			ItemRoster itemRoster = new ItemRoster();
			foreach (ItemObject itemObject in kingdom.Culture.VassalRewardItems)
			{
				itemRoster.AddToCounts(itemObject, 1);
			}
			ItemObject randomBannerAtLevel = this.GetRandomBannerAtLevel(2, kingdom.Culture);
			if (randomBannerAtLevel != null)
			{
				itemRoster.AddToCounts(randomBannerAtLevel, 1);
			}
			return itemRoster;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0008C088 File Offset: 0x0008A288
		private ItemObject GetRandomBannerAtLevel(int bannerLevel, CultureObject culture = null)
		{
			MBList<ItemObject> mblist = Campaign.Current.Models.BannerItemModel.GetPossibleRewardBannerItems().ToMBList<ItemObject>();
			if (culture == null)
			{
				return mblist.GetRandomElementWithPredicate<ItemObject>((ItemObject i) => (i.ItemComponent as BannerComponent).BannerLevel == bannerLevel);
			}
			return mblist.GetRandomElementWithPredicate<ItemObject>((ItemObject i) => (i.ItemComponent as BannerComponent).BannerLevel == bannerLevel && i.Culture == culture);
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x0008C0F0 File Offset: 0x0008A2F0
		public override TroopRoster GetTroopRewardsForJoiningKingdom(Kingdom kingdom)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (PartyTemplateStack partyTemplateStack in kingdom.Culture.VassalRewardTroopsPartyTemplate.Stacks)
			{
				troopRoster.AddToCounts(partyTemplateStack.Character, partyTemplateStack.MaxValue, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x0400091F RID: 2335
		private const int VassalRewardBannerLevel = 2;
	}
}
