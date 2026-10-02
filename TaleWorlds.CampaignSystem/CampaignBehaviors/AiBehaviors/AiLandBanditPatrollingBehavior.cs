using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x0200046F RID: 1135
	public class AiLandBanditPatrollingBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004881 RID: 18561 RVA: 0x0016DD14 File Offset: 0x0016BF14
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x0016DD2D File Offset: 0x0016BF2D
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x0016DD30 File Offset: 0x0016BF30
		public void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			if (!mobileParty.IsBandit)
			{
				return;
			}
			if (mobileParty.IsBanditBossParty)
			{
				return;
			}
			if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsHideout)
			{
				if (mobileParty.CurrentSettlement.Parties.CountQ<MobileParty>((MobileParty x) => x.IsBandit && !x.IsBanditBossParty) <= Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt + 1)
				{
					return;
				}
			}
			MobileParty.NavigationType navigationType = MobileParty.NavigationType.Default;
			if (!mobileParty.HasLandNavigationCapability)
			{
				return;
			}
			AIBehaviorData aibehaviorData = new AIBehaviorData(mobileParty.HomeSettlement, AiBehavior.PatrolAroundPoint, navigationType, false, false, false);
			float num = 1f;
			if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsHideout && (mobileParty.CurrentSettlement.MapFaction == mobileParty.MapFaction || mobileParty.CurrentSettlement.Hideout.IsInfested))
			{
				float num2 = (float)mobileParty.CurrentSettlement.Parties.CountQ<MobileParty>((MobileParty x) => x.IsBandit && !x.IsBanditBossParty);
				int numberOfMinimumBanditPartiesInAHideoutToInfestIt = Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;
				int numberOfMaximumBanditPartiesInEachHideout = Campaign.Current.Models.BanditDensityModel.NumberOfMaximumBanditPartiesInEachHideout;
				num = (num2 - (float)numberOfMinimumBanditPartiesInAHideoutToInfestIt) / (float)(numberOfMaximumBanditPartiesInEachHideout - numberOfMinimumBanditPartiesInAHideoutToInfestIt);
			}
			float num3 = ((mobileParty.CurrentSettlement != null) ? (MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat) : 0.5f);
			float num4 = 0.5f * num * num3;
			if (num > 0f)
			{
				ValueTuple<AIBehaviorData, float> valueTuple = new ValueTuple<AIBehaviorData, float>(aibehaviorData, num4);
				p.AddBehaviorScore(in valueTuple);
			}
		}
	}
}
