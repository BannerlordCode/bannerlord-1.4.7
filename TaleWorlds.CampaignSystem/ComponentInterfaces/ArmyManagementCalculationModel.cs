using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B5 RID: 437
	public abstract class ArmyManagementCalculationModel : MBGameModel<ArmyManagementCalculationModel>
	{
		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001D77 RID: 7543
		public abstract float AIMobilePartySizeRatioToCallToArmy { get; }

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06001D78 RID: 7544
		public abstract float PlayerMobilePartySizeRatioToCallToArmy { get; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06001D79 RID: 7545
		public abstract float MinimumNeededFoodInDaysToCallToArmy { get; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001D7A RID: 7546
		public abstract float MaximumDistanceToCallToArmy { get; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001D7B RID: 7547
		public abstract int InfluenceValuePerGold { get; }

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001D7C RID: 7548
		public abstract int AverageCallToArmyCost { get; }

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06001D7D RID: 7549
		public abstract int CohesionThresholdForDispersion { get; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06001D7E RID: 7550
		public abstract float MaximumWaitTime { get; }

		// Token: 0x06001D7F RID: 7551
		public abstract bool CanPlayerCreateArmy(out TextObject disabledReason);

		// Token: 0x06001D80 RID: 7552
		public abstract int CalculatePartyInfluenceCost(MobileParty armyLeaderParty, MobileParty party);

		// Token: 0x06001D81 RID: 7553
		public abstract float DailyBeingAtArmyInfluenceAward(MobileParty armyMemberParty);

		// Token: 0x06001D82 RID: 7554
		public abstract bool CanLordCreateArmy(MobileParty leaderParty, out MBList<MobileParty> possibleArmyMembers);

		// Token: 0x06001D83 RID: 7555
		public abstract int CalculateTotalInfluenceCost(Army army, float percentage);

		// Token: 0x06001D84 RID: 7556
		public abstract float GetPartySizeScore(MobileParty party);

		// Token: 0x06001D85 RID: 7557
		public abstract bool CheckPartyEligibility(MobileParty party, out TextObject explanation);

		// Token: 0x06001D86 RID: 7558
		public abstract int GetPartyRelation(Hero hero);

		// Token: 0x06001D87 RID: 7559
		public abstract ExplainedNumber CalculateDailyCohesionChange(Army army, bool includeDescriptions = false);

		// Token: 0x06001D88 RID: 7560
		public abstract int CalculateNewCohesion(Army army, PartyBase newParty, int calculatedCohesion, int sign);

		// Token: 0x06001D89 RID: 7561
		public abstract int GetCohesionBoostInfluenceCost(Army army, int percentageToBoost = 100);
	}
}
