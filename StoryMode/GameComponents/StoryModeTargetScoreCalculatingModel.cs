using System;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace StoryMode.GameComponents
{
	// Token: 0x0200004A RID: 74
	public class StoryModeTargetScoreCalculatingModel : TargetScoreCalculatingModel
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x0001953C File Offset: 0x0001773C
		public override float TravelingToAssignmentFactor
		{
			get
			{
				return base.BaseModel.TravelingToAssignmentFactor;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x00019549 File Offset: 0x00017749
		public override float BesiegingFactor
		{
			get
			{
				return base.BaseModel.BesiegingFactor;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x00019556 File Offset: 0x00017756
		public override float AssaultingTownFactor
		{
			get
			{
				return base.BaseModel.AssaultingTownFactor;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00019563 File Offset: 0x00017763
		public override float RaidingFactor
		{
			get
			{
				return base.BaseModel.RaidingFactor;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x00019570 File Offset: 0x00017770
		public override float DefendingFactor
		{
			get
			{
				return base.BaseModel.DefendingFactor;
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001957D File Offset: 0x0001777D
		public override float GetDefensivePatrollingFactor(bool isNavalPatrolling)
		{
			return base.BaseModel.GetDefensivePatrollingFactor(isNavalPatrolling);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0001958B File Offset: 0x0001778B
		public override float GetOffensivePatrollingFactor(bool isNavalPatrolling)
		{
			return base.BaseModel.GetOffensivePatrollingFactor(isNavalPatrolling);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00019599 File Offset: 0x00017799
		public override float CalculateDefensivePatrollingScoreForSettlement(Settlement settlement, bool isTargetingPort, MobileParty mobileParty)
		{
			return base.BaseModel.CalculateDefensivePatrollingScoreForSettlement(settlement, isTargetingPort, mobileParty);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000195A9 File Offset: 0x000177A9
		public override float CalculateOffensivePatrollingScoreForSettlement(Settlement settlement, bool isTargetingPort, MobileParty mobileParty)
		{
			return base.BaseModel.CalculateOffensivePatrollingScoreForSettlement(settlement, isTargetingPort, mobileParty);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000195B9 File Offset: 0x000177B9
		public override float CurrentObjectiveValue(MobileParty mobileParty)
		{
			return base.BaseModel.CurrentObjectiveValue(mobileParty);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000195C8 File Offset: 0x000177C8
		public override float GetTargetScoreForFaction(Settlement targetSettlement, Army.ArmyTypes missionType, MobileParty mobileParty, float ourStrength)
		{
			if (missionType == Army.ArmyTypes.Raider && targetSettlement != null && targetSettlement.StringId == "village_ES3_2" && TutorialPhase.Instance != null && !TutorialPhase.Instance.IsCompleted)
			{
				return 0f;
			}
			return base.BaseModel.GetTargetScoreForFaction(targetSettlement, missionType, mobileParty, ourStrength);
		}
	}
}
