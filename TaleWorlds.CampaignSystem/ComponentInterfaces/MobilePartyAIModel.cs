using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B1 RID: 433
	public abstract class MobilePartyAIModel : MBGameModel<MobilePartyAIModel>
	{
		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001D4E RID: 7502
		public abstract float AiCheckInterval { get; }

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001D4F RID: 7503
		public abstract float FleeToNearbyPartyRadius { get; }

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06001D50 RID: 7504
		public abstract float FleeToNearbySettlementRadius { get; }

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06001D51 RID: 7505
		public abstract float HideoutPatrolDistanceAsDays { get; }

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06001D52 RID: 7506
		public abstract float FortificationPatrolDistanceAsDays { get; }

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001D53 RID: 7507
		public abstract float FortificationPortPatrolDistanceAsDays { get; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001D54 RID: 7508
		public abstract float VillagePatrolDistanceAsDays { get; }

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001D55 RID: 7509
		public abstract float SettlementDefendingNearbyPartyCheckRadius { get; }

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001D56 RID: 7510
		public abstract float SettlementDefendingWaitingPositionRadius { get; }

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06001D57 RID: 7511
		public abstract float NeededFoodsInDaysThresholdForSiege { get; }

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06001D58 RID: 7512
		public abstract float NeededFoodsInDaysThresholdForRaid { get; }

		// Token: 0x06001D59 RID: 7513
		public abstract bool ShouldConsiderAvoiding(MobileParty party, MobileParty targetParty);

		// Token: 0x06001D5A RID: 7514
		public abstract bool ShouldConsiderAttacking(MobileParty party, MobileParty targetParty);

		// Token: 0x06001D5B RID: 7515
		public abstract float GetPatrolRadius(MobileParty mobileParty, CampaignVec2 patrolPoint);

		// Token: 0x06001D5C RID: 7516
		public abstract float GetSettlementNearbyThreatAndAllyCheckRadius(Settlement settlement, bool isPort);

		// Token: 0x06001D5D RID: 7517
		public abstract bool ShouldPartyCheckInitiativeBehavior(MobileParty mobileParty);

		// Token: 0x06001D5E RID: 7518
		public abstract void GetBestInitiativeBehavior(MobileParty mobileParty, out AiBehavior bestInitiativeBehavior, out MobileParty bestInitiativeTargetParty, out float bestInitiativeBehaviorScore, out Vec2 averageEnemyVec);
	}
}
