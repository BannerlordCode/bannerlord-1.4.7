using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B3 RID: 435
	public abstract class BanditDensityModel : MBGameModel<BanditDensityModel>
	{
		// Token: 0x06001D67 RID: 7527
		public abstract int GetMaxSupportedNumberOfLootersForClan(Clan clan);

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001D68 RID: 7528
		public abstract int NumberOfMinimumBanditPartiesInAHideoutToInfestIt { get; }

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001D69 RID: 7529
		public abstract int NumberOfMaximumBanditPartiesInEachHideout { get; }

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06001D6A RID: 7530
		public abstract int NumberOfMaximumBanditPartiesAroundEachHideout { get; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06001D6B RID: 7531
		public abstract int NumberOfMaximumHideoutsAtEachBanditFaction { get; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06001D6C RID: 7532
		public abstract int NumberOfInitialHideoutsAtEachBanditFaction { get; }

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001D6D RID: 7533
		public abstract int NumberOfMinimumBanditTroopsInHideoutMission { get; }

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001D6E RID: 7534
		public abstract int NumberOfMaximumTroopCountForFirstFightInHideout { get; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06001D6F RID: 7535
		public abstract int NumberOfMaximumTroopCountForBossFightInHideout { get; }

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06001D70 RID: 7536
		public abstract float SpawnPercentageForFirstFightInHideoutMission { get; }

		// Token: 0x06001D71 RID: 7537
		public abstract int GetMinimumTroopCountForHideoutMission(MobileParty party, bool isAssault);

		// Token: 0x06001D72 RID: 7538
		public abstract int GetMaximumTroopCountForHideoutMission(MobileParty party, bool isAssault);

		// Token: 0x06001D73 RID: 7539
		public abstract bool IsPositionInsideNavalSafeZone(CampaignVec2 position);
	}
}
