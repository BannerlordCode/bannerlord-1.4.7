using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AE RID: 430
	public abstract class TargetScoreCalculatingModel : MBGameModel<TargetScoreCalculatingModel>
	{
		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001D39 RID: 7481
		public abstract float TravelingToAssignmentFactor { get; }

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06001D3A RID: 7482
		public abstract float BesiegingFactor { get; }

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06001D3B RID: 7483
		public abstract float AssaultingTownFactor { get; }

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001D3C RID: 7484
		public abstract float RaidingFactor { get; }

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06001D3D RID: 7485
		public abstract float DefendingFactor { get; }

		// Token: 0x06001D3E RID: 7486
		public abstract float GetDefensivePatrollingFactor(bool isNavalPatrolling);

		// Token: 0x06001D3F RID: 7487
		public abstract float GetOffensivePatrollingFactor(bool isNavalPatrolling);

		// Token: 0x06001D40 RID: 7488
		public abstract float GetTargetScoreForFaction(Settlement targetSettlement, Army.ArmyTypes missionType, MobileParty mobileParty, float ourStrength);

		// Token: 0x06001D41 RID: 7489
		public abstract float CalculateDefensivePatrollingScoreForSettlement(Settlement settlement, bool isTargetingPort, MobileParty mobileParty);

		// Token: 0x06001D42 RID: 7490
		public abstract float CalculateOffensivePatrollingScoreForSettlement(Settlement settlement, bool isTargetingPort, MobileParty mobileParty);

		// Token: 0x06001D43 RID: 7491
		public abstract float CurrentObjectiveValue(MobileParty mobileParty);
	}
}
