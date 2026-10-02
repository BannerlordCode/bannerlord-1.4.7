using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D1 RID: 465
	public abstract class VassalRewardsModel : MBGameModel<VassalRewardsModel>
	{
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06001E62 RID: 7778
		public abstract float InfluenceReward { get; }

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06001E63 RID: 7779
		public abstract int RelationRewardWithLeader { get; }

		// Token: 0x06001E64 RID: 7780
		public abstract TroopRoster GetTroopRewardsForJoiningKingdom(Kingdom kingdom);

		// Token: 0x06001E65 RID: 7781
		public abstract ItemRoster GetEquipmentRewardsForJoiningKingdom(Kingdom kingdom);
	}
}
