using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x02000471 RID: 1137
	internal class AIMoveToNearestLandBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004896 RID: 18582 RVA: 0x0016F2FB File Offset: 0x0016D4FB
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x0016F314 File Offset: 0x0016D514
		private void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			if (mobileParty.IsCurrentlyAtSea && mobileParty.CurrentSettlement == null)
			{
				float estimatedSafeSailDuration = Campaign.Current.Models.CampaignShipDamageModel.GetEstimatedSafeSailDuration(mobileParty);
				Settlement settlement = null;
				if (mobileParty.HasLandNavigationCapability)
				{
					int[] invalidTerrainTypesForNavigationType = Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(MobileParty.NavigationType.All);
					CampaignVec2 nearestFaceCenterForPositionWithPath = Campaign.Current.MapSceneWrapper.GetNearestFaceCenterForPositionWithPath(mobileParty.CurrentNavigationFace, true, Campaign.MapDiagonal / 2f, invalidTerrainTypesForNavigationType);
					float num2;
					float num = DistanceHelper.FindClosestDistanceFromMobilePartyToPoint(mobileParty, nearestFaceCenterForPositionWithPath, MobileParty.NavigationType.All, out num2);
					if (num > 0f && num < Campaign.MapDiagonal)
					{
						float num3 = (mobileParty.IsLordParty ? Campaign.Current.EstimatedAverageLordPartyNavalSpeed : (mobileParty.IsCaravan ? Campaign.Current.EstimatedAverageCaravanPartyNavalSpeed : (mobileParty.IsBandit ? Campaign.Current.EstimatedAverageBanditPartyNavalSpeed : (mobileParty.IsVillager ? Campaign.Current.EstimatedAverageVillagerPartyNavalSpeed : (Campaign.Current.EstimatedMaximumLordPartySpeedExceptPlayer * 0.5f)))));
						float num4 = num / num3 / estimatedSafeSailDuration;
						if (num4 > 0.75f)
						{
							float num5 = 2f * num4;
							if (settlement != null && mobileParty.DefaultBehavior == AiBehavior.MoveToNearestLandOrPort && mobileParty.TargetSettlement == settlement)
							{
								num5 *= 1.2f;
							}
							ValueTuple<AIBehaviorData, float> valueTuple = new ValueTuple<AIBehaviorData, float>(new AIBehaviorData(settlement, AiBehavior.MoveToNearestLandOrPort, MobileParty.NavigationType.All, false, false, false), num5);
							p.AddBehaviorScore(in valueTuple);
						}
					}
				}
			}
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x0016F476 File Offset: 0x0016D676
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0400140E RID: 5134
		private const int MoveToNearestLandMaximumScore = 2;

		// Token: 0x0400140F RID: 5135
		private const float RatioThreshold = 0.75f;
	}
}
