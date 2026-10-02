using System;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004B4 RID: 1204
	public static class GatherArmyAction
	{
		// Token: 0x06004AA4 RID: 19108 RVA: 0x00179720 File Offset: 0x00177920
		private static void ApplyInternal(MobileParty leaderParty, IMapPoint gatheringPoint, float playerInvolvement = 0f)
		{
			Army army = leaderParty.Army;
			CampaignEventDispatcher.Instance.OnArmyGathered(army, gatheringPoint);
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x00179740 File Offset: 0x00177940
		public static void Apply(MobileParty leaderParty, IMapPoint gatheringPoint)
		{
			GatherArmyAction.ApplyInternal(leaderParty, gatheringPoint, (leaderParty == MobileParty.MainParty) ? 1f : 0f);
		}
	}
}
